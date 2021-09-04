using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Android.Lib.AndroidHelper
{
    public class WebService
    {
        public const string PACKAGEID = @"com.androidteststudio.helper";

        public WebService(IPAddress ip, RSA responseEncryptionKey, string encryptionKeyDevicePath, Action restartService = null)
        {
            IP = ip ?? throw new ArgumentNullException(nameof(ip));
            ResponseEncryptionKey = responseEncryptionKey ?? throw new ArgumentNullException(nameof(responseEncryptionKey));
            EncryptionKeyDevicePath = encryptionKeyDevicePath ?? throw new ArgumentNullException(nameof(encryptionKeyDevicePath));
            RestartService = restartService;
        }

        public IPAddress IP { get; }
        public RSA ResponseEncryptionKey { get; }
        public string EncryptionKeyDevicePath { get; }
        public Action RestartService { get; }
        private string BaseUrl { get => $"http://{IP}:10023/"; }

        public async Task<PackageResponse[]> GetPackagesAsync()
        {
            var json = await new WebClient().DownloadStringTaskAsync(BaseUrl + "getpackages");
            var list = JsonConvert.DeserializeObject<PackageResponse[]>(json);
            return list;
        }

        public async Task<Bitmap> GetPackageIconAsync(string packageId)
        {
            //return await TryAsync(async () =>
            //{
            var data = await new WebClient().DownloadDataTaskAsync(BaseUrl + "getpackageicon?packageId=" + packageId);
            using (var stream = new MemoryStream(data))
            {
                return (Bitmap)Bitmap.FromStream(stream);
            }
            //});
        }

        public async Task<bool> IsAliveAsync()
        {
            try
            {
                var result = await new WebClient().DownloadStringTaskAsync(BaseUrl + "isalive");
                return bool.Parse(result);
            }
            catch
            {
                return false;
            }
        }

        public async Task<ViewChangeResult> GetViewsAsync()
        {
            try
            {
                var jsonBytes = await new WebClient().DownloadDataTaskAsync($"{BaseUrl}getviews?encryptionKeyPath={EncryptionKeyDevicePath}");

                var json =
                    Encoding.UTF8.GetString(
                        ResponseEncryptionKey.Decrypt(jsonBytes, RSAEncryptionPadding.OaepSHA1)
                    );
                var list = JsonConvert.DeserializeObject<ViewChangeResult>(json);
                return list;
            }
            catch (WebException ex)
            {
                throw;
            }
        }

        private async Task<T> TryAsync<T>(Func<Task<T>> action, int depth = 0)
        {
            try
            {
                return await action();
            }
            catch (WebException ex)
                 when (ex.InnerException?.InnerException is SocketException)
            {
                if (depth >= 10 || null == RestartService) throw;
                Thread.Sleep(1000);
                Console.WriteLine("Restarting helper");
                RestartService.Invoke();
                Thread.Sleep(1000);
                return await TryAsync(action, depth + 1);
            }
        }

    }

}
