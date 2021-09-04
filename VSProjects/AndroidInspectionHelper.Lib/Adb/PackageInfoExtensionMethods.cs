using INHelpers.ExtensionMethods;
using System;
using System.IO;
using System.Text;

namespace Android.Lib.Adb
{

    public static class PackageInfoExtensionMethods
    {

        public static DirectoryInfo GetDefaultSubdir(this IPackageInfo pkg, DirectoryInfo directory)
        {
            if (pkg is null)
                throw new ArgumentNullException(nameof(pkg));
            if (directory is null)
                throw new ArgumentNullException(nameof(directory));

            return directory.Subdir(pkg.PackageId)
                            .Subdir(pkg.Version)
                            .EnsureExists();
        }

    }
}
