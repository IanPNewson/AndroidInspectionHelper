using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Android.Lib.Adb
{
    public class ClientDevice
    {
        public ClientDevice(AdbClient client, DeviceData device)
        {
            Client = client;
            Device = device;
        }

        private Size? _DeviceSize = null;

        public AdbClient Client { get; }
        public DeviceData Device { get; }

        public async Task<Size> GetVisibleScreenSize(bool adjustForOrientation = true)
        {
            if (null == _DeviceSize)
            {
                _DeviceSize = await this.GetScreenSize();
            }

            Size value = _DeviceSize.Value;

            if (adjustForOrientation)
            {
                var orientation = await this.GetOrientation();
                switch (orientation)
                {
                    case Orientations.Degrees90:
                    case Orientations.Degrees270:
                        value = new Size(value.Height, value.Width);
                        break;
                }
            }
            
            return value;
        }

        #region getprops

        private Dictionary<string, string> _Props = null;

        public async Task<Dictionary<string, string>> GetProps()
        {
            if (null != _Props) return _Props;
            var dic = new Dictionary<string, string>();
            var props = await this.GetRemoteCommandResultAsync("getprop");
            var rgx = new Regex(@"\[(?<Name>[^\]]+)\]: \[(?<Value>[^\]]+)\]");
            foreach (Match match in rgx.Matches(props))
            {
                dic.Add(match.Groups["Name"].Value, match.Groups["Value"].Value);
            }
            _Props = dic;
            return dic;
        }

        public async Task<string> Manufacturer()
        {
            return (await GetProps()).GetValueOrDefault("ro.product.manufacturer");
        }

        public async Task<string> Model()
        {
            return (await GetProps()).GetValueOrDefault("ro.product.model");
        }

        public async Task<string> ProductName()
        {
            return
                (await GetProps()).GetValueOrDefault("ro.display.series") ??
                (await GetProps()).GetValueOrDefault("ro.product.name");
        }

        #endregion

        public async Task<string> Start(Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            return await this.GetRemoteCommandResultAsync($" am start -a android.intent.action.VIEW -d {uri}");
        }

        public void Push(byte[] data, string remoteLocation)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            using (var stream = new MemoryStream(data))
                Push(stream, remoteLocation);
        }

        public void Push(FileInfo file, string remoteLocation)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));
            if (!file.Exists)
                throw new ArgumentException($"File'{file.FullName}' does not exist", nameof(file));
            using (var stream = file.OpenRead())
                Push(stream, remoteLocation);
        }

        public void Push(Stream stream, string remoteLocation)
        {
            var service = new SyncService(Client, Device);
            service.Push(stream, remoteLocation,555, DateTimeOffset.Now, null, CancellationToken.None);
        }

        public static implicit operator ClientDevice((AdbClient, DeviceData) tuple)
        {
            return new ClientDevice(tuple.Item1, tuple.Item2)
;        }
    }
}
