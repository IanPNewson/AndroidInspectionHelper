using Android.Lib.AndroidHelper;
using INHelpers.ExtensionMethods;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace Android.Lib.Adb
{
    /// <summary>
    /// Gets inforation about an app from the installed web service
    /// </summary>
    public class WebServicePackageInfo : IPackageInfo
    {

        public WebServicePackageInfo(WebService service, PackageResponse package)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
            Package = package ?? throw new ArgumentNullException(nameof(package));
        }

        public string PackageId => Package.Id;

        public string Version => Package.Version;
        
        public WebService Service { get; }
        public PackageResponse Package { get; }

        public string DisplayName => Package.Name;

        public string MainActivityIdentifier => $"{PackageId}/{Package.DefaultActivity.Name}";

        public FileInfo GetIconPath(DirectoryInfo workingDirectory)
        {
            if (workingDirectory is null) throw new ArgumentNullException(nameof(workingDirectory));

            var file = this.GetDefaultSubdir(workingDirectory)
                .File(PackageId + ".icon.fromwebservice.png");

            if (file.Exists) return file;

            var bmp = Service.GetPackageIconAsync(PackageId).Result;
            bmp.Save(file.FullName, ImageFormat.Png);
            bmp.Dispose();
            return file;
        }
    }
}
