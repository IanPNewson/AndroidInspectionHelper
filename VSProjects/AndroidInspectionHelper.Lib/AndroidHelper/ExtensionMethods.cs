using Android.Lib.Adb;
using INHelpers.ExtensionMethods;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Android.Lib.AndroidHelper
{
    public static class ExtensionMethods
    {

        public static async Task EnsureHelperApkIsInstalledAsync(this ClientDevice cd)
        {
            if ((await cd.ListPackages(fromAdbOnly:true)).Any(pkg => pkg.PackageId == WebService.PACKAGEID)) return;

            var apkFile = new FileInfo(typeof(ExtensionMethods).Assembly.Location).Directory.Subdir("AndroidHelper")
                .File("AndroidTestStudioHelper.apk");

            if (!apkFile.Exists) throw new FileNotFoundException("No AndroidTestStudioHelper.apk found", apkFile.FullName);

            using (var apkStream = File.OpenRead(apkFile.FullName))
            {
                cd.Client.Install(cd.Device, apkStream);
            }
        }

    }
}
