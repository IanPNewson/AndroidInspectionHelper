using Android.Lib.Adb.UI;
using Android.Lib.AndroidHelper;
using INHelpers.ExtensionMethods;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Android.Lib.Adb
{
    public static class ExtensionMethods
    {



        #region Screen size

        private static Regex _RgxScreenSize = new Regex(@"Physical size: (?<Width>[0-9]*)x(?<Height>[0-9]*)");

        private class ScreenResolution
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public static async Task<Size> GetScreenSize(this ClientDevice cd)
        {
            Size? size = null;
            await cd.ExecuteRemoteCommandAsync("wm size", new TypedReceiver<ScreenResolution>(_RgxScreenSize, res =>
            {
                size = new Size(res.Width, res.Height);
            }));
            return size.Value;
        }

        #endregion

        #region Input

        public static async Task KeyEvent(this ClientDevice cd, KeyCodes keyCode)
        {
            await cd.ExecuteRemoteCommandAsync($"input keyevent {((int)keyCode)}", new StringReceiver(str =>
            {
            }));
        }

        public static async Task InputText(this ClientDevice cd, string text)
        {
            await cd.ExecuteRemoteCommandAsync($"input text {text}", new StringReceiver(str =>
            {
            }));
        }

        public static async Task Swipe(this ClientDevice cd, PointF start, PointF end, TimeSpan? duration = null)
        {
            duration = duration ?? TimeSpan.FromMilliseconds(500);

            string cmd = $"input touchscreen swipe {(int)start.X} {(int)start.Y} {(int)end.X} {(int)end.Y} {duration.Value.Milliseconds}";
            Console.WriteLine(cmd);
            await cd.ExecuteRemoteCommandAsync(cmd, new StringReceiver(str =>
            {
                Console.WriteLine(str);
            }));
        }

        public static async Task Tap(this ClientDevice cd, PointF pt)
        {
            await cd.ExecuteRemoteCommandAsync($"input tap {pt.X} {pt.Y}", new StringReceiver(str =>
            {
                Console.WriteLine(str);
            }));
        }

        #endregion

        #region Orientations

        private static Regex _RgxOrientation = new Regex(@"SurfaceOrientation: (?<Value>[0-9]*)");

        public static async Task<Orientations> GetOrientation(this ClientDevice clientDevice)
        {
            Orientations? value = null;
            await clientDevice.ExecuteRemoteCommandAsync(
                "dumpsys input",
                new TypedReceiver<Orientation>(
                    _RgxOrientation,
                    orientation =>
                    {
                        value = (Orientations)orientation.Value;
                    }));
            return value.Value;
        }

        public class Orientation
        {
            public int Value { get; set; }

        }

        #endregion

        #region UiAutomataor

        public static async Task<Hierarchy> UiHierarchy(this ClientDevice cd)
        {
            var filename = $"/data/local/tmp/ui.{DateTime.Now.Ticks}.xml";
            string result = null;
            await cd.ExecuteRemoteCommandAsync($"uiautomator dump {filename} > /dev/null && cat {filename} && rm {filename}", new StringReceiver(str =>
            {
                result = str;
            }));

            //result = result.Substring(0, result.LastIndexOf('>')+1);
            try
            {
                var doc = XDocument.Parse(result);
                return new Hierarchy(doc.Root);
            }
            catch (Exception ex)
            {
                //TODO: log somewhere
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
                return null;
            }
        }

        #endregion

        #region packages

        public static async Task<IPackageInfo[]> ListPackages(this ClientDevice cd, bool thirdPartyOnly = true, bool fromAdbOnly = false)
        {
            if (!fromAdbOnly)
            {
                var service = await cd.TryGetWebServiceAsync();
                if (null != service)
                    return (await service.GetPackagesAsync()).Select(pkg => new WebServicePackageInfo(service, pkg))
                        .ToArray();
            }

            //Fall back to APK method
            var packages = new List<IPackageInfo>();
            await cd.ExecuteRemoteCommandAsync("pm list packages" + (thirdPartyOnly ? " -3" : ""), new StringReceiver(str =>
            {
                using (var reader = new StringReader(str))
                {
                    string strLine;
                    while (null != (strLine = reader.ReadLine()))
                    {
                        strLine = strLine.Substring("package:".Length);
                        if (!string.IsNullOrWhiteSpace(strLine))
                            packages.Add(new ApkPackageInfo(cd, strLine));
                    }
                }
            }));
            return packages.ToArray();
        }

        public static async Task<string> Start(this ClientDevice cd, string identifier, bool wait = true)
        {
            string result = null;
            await cd.ExecuteRemoteCommandAsync(wait ? $"am start -W {identifier}" : $"am start {identifier}", new StringReceiver(str =>
            {
                result = str;
            }));
            return result;
        }

        public static async Task<string> StartService(this ClientDevice cd, string identifier)
        {
            string result = null;
            await cd.ExecuteRemoteCommandAsync($"am start-service {identifier}", new StringReceiver(str =>
            {
                result = str;
            }));
            return result;
        }

        public static async Task<string> Stop(this ClientDevice cd, string packageName)
        {
            string result = null;
            await cd.ExecuteRemoteCommandAsync($"am force-stop {packageName}", new StringReceiver(str =>
            {
                result = str;
            }));
            return result;
        }

        public static async Task Uninstall(this ClientDevice cd, string packageName)
        {
            await cd.GetRemoteCommandResultAsync("pm uninstall " + packageName);
        }

        #endregion

        #region Version

        public static Task<int> AndroidVersion(this ClientDevice cd)
        {
            return cd.GetRemoteCommandResultAsync<int>(@"getprop ro.build.version.release");
        }

        #endregion

        #region Network

        /// <summary>
        /// Returns a list of IP addresses for the device, except loopback
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static async Task<IPAddress[]> GetIPAddresses(this ClientDevice cd)
        {
            var pattern = @"[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}";

            var output = await cd.GetRemoteCommandResultAsync($"ip address | egrep '{pattern}'");

            var rgx = new Regex(pattern.Replace(".", "\\."));
            return rgx.Matches(output).Cast<Match>().Select(math => IPAddress.Parse(math.Value))
                .Except(new[] { IPAddress.Loopback, new IPAddress(new byte[] { 0, 0, 0, 0 }) })
                .Distinct()
                .ToArray();
        }

        #endregion
        
        public static async Task<WebService> TryGetWebServiceAsync(this ClientDevice cd)
        {
            await StartHelper(cd);

            var ips = await cd.GetIPAddresses();
            if (!ips.Any()) return null;

            //Ensure there's a public key there
            var rsa = RSA.Create();
            var keyText = rsa.ExportPublicKeyFile();
            var remoteFileName = $"/data/local/tmp/{Guid.NewGuid()}";

            cd.Push(Encoding.ASCII.GetBytes(keyText), remoteFileName);

            var service = new WebService(
                ips.First(),
                rsa,
                remoteFileName,
                async () => await cd.StartHelper());

            if (await service.IsAliveAsync()) return service;
            return null;
        }

        private static async Task StartHelper(this ClientDevice cd)
        {
            await cd.EnsureHelperApkIsInstalledAsync();
            await cd.Start(WebService.PACKAGEID + "/.MainActivity", wait: false);
        }

        public static void Pull(this ClientDevice clientDevice, string pullFromPath, FileInfo destination, bool overwriteExistingFile = false)
        {
            if (clientDevice is null)
            {
                throw new ArgumentNullException(nameof(clientDevice));
            }

            if (pullFromPath is null)
            {
                throw new ArgumentNullException(nameof(pullFromPath));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (destination.Exists && !overwriteExistingFile)
                throw new ArgumentException($"File path '{destination.FullName}' already exists and parameter {nameof(overwriteExistingFile)} indicates not to overwrite it");

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), clientDevice.Device))
            using (Stream stream = File.OpenWrite(destination.FullName))
            {
                service.Pull(pullFromPath, stream, null, CancellationToken.None);
            }
        }

        #region Execute helpers

        public static async Task ExecuteRemoteCommandAsync(this ClientDevice cd, string command, IShellOutputReceiver receiver, CancellationToken? cancellationToken = null)
        {
            cancellationToken = cancellationToken ?? CancellationToken.None;
            await cd.Client.ExecuteRemoteCommandAsync(command, cd.Device, receiver, cancellationToken.Value);
        }

        public static async Task<T> GetRemoteCommandResultAsync<T>(this ClientDevice cd, string cmd)
            where T : struct
        {
            T value = default(T);
            await cd.ExecuteRemoteCommandAsync(cmd, new PrimitiveTypeReceiver<T>(val => value = val));
            return value;
        }

        public static async Task<string> GetRemoteCommandResultAsync(this ClientDevice cd, string cmd)
        {
            string value = null;
            await cd.ExecuteRemoteCommandAsync(cmd, new StringReceiver(val => value = val));
            return value;
        }

        #endregion

    }
}
