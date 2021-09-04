using Android.Lib.Adb;
using Android.Lib.AndroidHelper;
using INHelpers.Drawing;
using INHelpers.ExtensionMethods;
using SharpAdbClient;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewTreeVisualiser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            await LoadImage();
        }

        private WebService Service
        {
            get; set;
        }

        private ClientDevice _Cd= null;
        private ClientDevice Cd
        {
            get
            {
                if (null == _Cd)
                {
                    var client = new AdbClient();
                    _Cd = new ClientDevice(client, client.GetDevices().First());
                }
                return _Cd;
            }
        }

        private static readonly byte[] HEADER_PNG = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
        private static readonly byte[] HEADER_ADB_OKAY = new byte[] { (byte)'O', (byte)'K', (byte)'A', (byte)'Y' };

        private async Task LoadImage()
        {
            if (null == Service)
            {
                Service = await Cd.TryGetWebServiceAsync();
            }
            var views = await Service.GetViewsAsync();
            if (null == views?.ViewResponses) return;

            /*Bitmap screenBmp = null;
                        using (var socket = new AdbSocket(Cd.Client.EndPoint))
                        {
                            socket.SetDevice(Cd.Device);
                            socket.SendAdbRequest("shell:screencap -p");
                            using (var stream = new MemoryStream())
                            {
                                var outputStream = socket.GetShellStream();
                                var buffer = new byte[32 * 1024];
                                var lastRead = 0;
                                while ((lastRead = outputStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    stream.Write(buffer, 0, lastRead);
                                }

                                stream.Seek(0, SeekOrigin.Begin);

                                var header = new byte[HEADER_ADB_OKAY.Length];
                                stream.Read(header, 0, HEADER_ADB_OKAY.Length);
                                if (!header.SequenceEqual(HEADER_ADB_OKAY))
                                    throw new InvalidOperationException($"Invalid response from ADB: {Encoding.ASCII.GetString(header)}");

                                header = new byte[HEADER_PNG.Length];
                                stream.Read(header, 0, HEADER_PNG.Length);

                                if (!header.SequenceEqual(HEADER_PNG))
                                {
                                    var msg = Encoding.ASCII.GetString(stream.ToArray());
                                    throw new InvalidOperationException($"Stream does not represnt a PNG as it has an invalid header: {msg}");
                                }

                                stream.Seek(HEADER_ADB_OKAY.Length, SeekOrigin.Begin);

                                try
                                {
                                    screenBmp = new Bitmap(stream);
                                }
                                catch (Exception ex)
                                {
                                    var msg = Encoding.ASCII.GetString(stream.ToArray());
                                    throw new InvalidOperationException($"An error occurred reading screencap: {msg}", ex);
                                }
                            }
                        }*/
            var dir = new FileInfo(typeof(Form1).Assembly.Location)
                .Directory
                .Subdir("Temp")
                .EnsureExists();

            var filepath = $"/sdcard/screen.{DateTime.Now.Ticks}.png";

            await Cd.GetRemoteCommandResultAsync($"screencap -p > {filepath}");

            var outFile = dir.File($"screen.{DateTime.Now.Ticks}.png");

            Cd.Pull(filepath, outFile);
            await Cd.GetRemoteCommandResultAsync($"rm {filepath}");

            var screenshot = Bitmap.FromFile(outFile.FullName);

            var marginPct = 0.2;
            var margin = new Size((int)(screenshot.Width * marginPct), (int)(screenshot.Height * marginPct));

            var bmp = new Bitmap(screenshot.Size.Width + margin.Width*2, screenshot.Size.Height + margin.Height*2   );

            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(screenshot, (Point)margin);

                foreach (var root in views.ViewResponses.Values)
                    DrawView(g, root, margin);
            }
            this.pictureBox1.Image = bmp;
        }

        public void DrawView(Graphics g, ViewResponse view, Size offset)
        {
            Rectangle rect = view.BoundsInScreen;
            rect = new Rectangle(rect.Location + offset, rect.Size);
            g.FillRectangle(new SolidBrush(Color.FromArgb(50, ColorHelpers.RandomColor())), rect);

            if (!string.IsNullOrWhiteSpace(view.Text))
            {
                var font = new Font("Arial", 16);
                var size = g.MeasureString(view.Text, font);
                g.DrawString(view.Text, font, Brushes.Black, ((Rectangle)view.BoundsInScreen).Location + offset);
            }

            foreach (var child in view.Children)
                DrawView(g, child, offset);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                case Keys.R:
                    LoadImage();
                    break;
            }
        }
    }

    public static class Ext
    {
        public static T With<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }

}