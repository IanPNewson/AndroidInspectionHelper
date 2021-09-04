using System.IO;

namespace Android.Lib.Adb
{
    public interface IPackageInfo
    {
        string PackageId { get; }
        string Version { get; }
        string DisplayName { get; }

        FileInfo GetIconPath(DirectoryInfo workingDirectory);

        string MainActivityIdentifier { get; }
    }
}
