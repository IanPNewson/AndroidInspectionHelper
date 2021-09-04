using Android.Lib.Adb.Dumpsys;
using INHelpers.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Android.Lib.Adb
{
    /// <summary>
    /// Gets information about an app from the APK and adb output
    /// </summary>
    public class ApkPackageInfo : IPackageInfo
    {

        public ApkPackageInfo(ClientDevice cd, string packageId)
        {
            ClientDevice = cd ?? throw new ArgumentNullException(nameof(cd));
            PackageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
        }

        public ClientDevice ClientDevice { get; }
        public string PackageId { get; }

        public string MainActivityIdentifier
        {
            get
            {
                var schemes = Dumpsys["Activity Resolver Table:"]?["Schemes:"];
                var activityNode = schemes
                    ?.Properties
                    ?.Select(scheme =>
                        scheme.Value.Properties.FirstOrDefault(
                            activityNode => activityNode.Value.Properties.Any(
                                actionNode =>
                                    actionNode.Key == "Action: \"android.intent.action.VIEW\"" &&
                                    actionNode.Value.Values.Contains("Category: \"android.intent.category.DEFAULT\"")))
                        .Key)
                    ?.SingleOrDefault(activityNode => activityNode != null);

                if (string.IsNullOrWhiteSpace(activityNode)) return null;

                var parts = activityNode.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                    return parts[1];

                return null;
            }
        }

        public string Version
        {
            get
            {
                const string _VERSION_NAME_PREFIX = @"versionName=";
                var versionName = DumpsysValues.FirstOrDefault(val => val.StartsWith(_VERSION_NAME_PREFIX));
                if (null == versionName)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        System.Diagnostics.Debugger.Break();
                    return null;
                }
                return versionName.Substring(_VERSION_NAME_PREFIX.Length);
            }
        }

        /// <summary>
        /// Attempts to get a nice icon to display. This isn't exact, it atttempts to get a nice icon based on filenames
        /// and making sure they're square and not one of the default android icons.
        /// </summary>
        /// <returns>FileInfo repesenting where the icon is stored. May be null, and may not exist.</returns>
        public FileInfo GetIconPath(DirectoryInfo workingDirectory)
        {
            var iconFile = this.GetDefaultSubdir(workingDirectory).File(PackageId + ".icon.fromapk.png");
            if (!iconFile.Exists)
            {

                var apk = GetLocalApk(workingDirectory);

                using (var zip = ZipFile.OpenRead(apk.FullName))
                {

                    IEnumerable<ZipArchiveEntry> allImageEntries = zip.Entries
                                        .Where(entry =>
                                            entry.FullName.StartsWith("res/", StringComparison.OrdinalIgnoreCase) &&

                                            (
                                            entry.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                            entry.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                            entry.Name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                            entry.Name.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)
                                            ));
                    IEnumerable<IGrouping<string, ZipArchiveEntry>> imageEntriesByName = allImageEntries
                        .GroupBy(entry => entry.Name)
                        .OrderByDescending(entry => entry.Sum(x => x.Length));

                    if (!imageEntriesByName.Any()) return null;

                    //Look for common file names
                    var preferredFilenames = new[] {
                        "launcher.png",
                        "launcher_icon.png",

                        "icon.png",
                        "app_icon.png",

                        "ic_launcher.png",
                        "ic_launcher",

                        "launcher",
                        "app_icon",
                        "icon",
                        "app"
                    };

                    imageEntriesByName = imageEntriesByName
                        .OrderBy(x =>
                        {
                            for (var i = 0; i < preferredFilenames.Length; ++i)
                            {
                                if (x.Key.Equals(preferredFilenames[i], StringComparison.OrdinalIgnoreCase))
                                    return i * 2;
                                if (x.Key.Contains(preferredFilenames[i], StringComparison.OrdinalIgnoreCase))
                                    return i * 2 + 1;
                            }
                            return int.MaxValue;
                        })
                        .ThenByDescending(x => x.Count());

                    if (null != imageEntriesByName && imageEntriesByName.Any())
                    {
                        ZipArchiveEntry iconEntry = null;
                        var iconIndex = 0;

                        while (false && !iconFile.Exists && iconIndex < imageEntriesByName.Count())
                        {
                            if (!iconFile.Exists)
                            {
                                iconEntry = imageEntriesByName
                                    .ElementAt(iconIndex)
                                    .OrderByDescending(x => x.Length)
                                    .First();
                                iconEntry.ExtractToFile(iconFile.FullName);
                                iconFile.Refresh();
                            }

                            try
                            {


                                //Test file dimensions, it should be square
                                using (var icon = (Bitmap)Image.FromFile(iconFile.FullName))
                                {
                                    Action discardIcon = () =>
                                    {
                                        icon.Dispose();

                                        iconEntry = null;
                                        iconFile.Delete();
                                    };

                                    //Some icons are a few pixels out from being square, this gives
                                    //some allowance for that
                                    var minHeight = icon.Width * 0.95;
                                    var maxHeight = icon.Width * 1.05;
                                    if (icon.Height < minHeight || icon.Height > maxHeight)
                                    {
                                        discardIcon();
                                        continue;
                                    }


                                    var defaultIconHashes = Resources.DefaultIcons.Select(bmp => bmp.GetSimilarityHash());
                                    var iconHash = icon.GetSimilarityHash();

                                    if (defaultIconHashes.Any(hash => hash.SequenceEqual(iconHash)))
                                    {
                                        discardIcon();
                                        continue;
                                    }

                                }

                            }
                            finally
                            {
                                ++iconIndex;
                            }
                        }

                        //If we still don't have an icon, extract all images and find the largest with correct dimensions
                        if (!iconFile.Exists)
                        {
                            int[] _ICON_SIZES = new[] { 512, 192, 144, 96, 72, 48 };

                            var imageIndex = 0;
                            var tempImageDirectory = this.GetDefaultSubdir(workingDirectory)
                                .Subdir("ImagesTemp");

                            if (tempImageDirectory.Exists) tempImageDirectory.Delete(true);
                            tempImageDirectory.EnsureExists();

                            foreach (var entry in allImageEntries)
                            {
                                var tempImage = tempImageDirectory.File(imageIndex++ + ".png");
                                entry.ExtractToFile(tempImage.FullName);
                            }

                            var defaultIconHashes = Resources.DefaultIcons.Select(bmp => bmp.GetSimilarityHash());

                            foreach (var imageFile in tempImageDirectory.GetFiles()
                                .OrderByDescending(f => f.Length))
                            {
                                try
                                {
                                    using (var bmp = new Bitmap(imageFile.FullName))
                                    {
                                        if (bmp.Width != bmp.Height) continue;

                                        var iconHash = bmp.GetSimilarityHash();
                                        if (defaultIconHashes.Any(hash => hash.SequenceEqual(iconHash))) continue;

                                        if (_ICON_SIZES.Contains(bmp.Height))
                                        {
                                            imageFile.CopyTo(iconFile.FullName);
                                            iconFile.Refresh();
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    //Protect against bad images
                                    if (System.Diagnostics.Debugger.IsAttached)
                                        System.Diagnostics.Debugger.Break();
                                }
                            }
                        }
                    }

                    //if (!iconFile.Exists || pkg.PackageId == "bbc.mobile.news.uk")//Extracts all images for debugging purposes
                    if (false)
                    {
                        var imagesDir = this.GetDefaultSubdir(workingDirectory).Subdir("Images").EnsureExists();
                        foreach (var grp in imageEntriesByName)
                        {
                            try
                            {
                                var outFile = imagesDir.File(grp.Key);
                                if (!outFile.Exists)
                                    grp.OrderByDescending(x => x.Length).First().ExtractToFile(outFile.FullName);
                            }
                            catch { }
                        }
                    }
                }
            }

            return iconFile;
        }

        #region Manifest

        public FileInfo GetManifestXmlFile(DirectoryInfo apkDirectory, IAxmlDecoder axmlDecoder, bool overwriteExistingFile = false)
        {
            if (apkDirectory is null)
                throw new ArgumentNullException(nameof(apkDirectory));

            var file = apkDirectory.File("AndroidManifest.xml");
            return GetManifestXmlFile(apkDirectory, file, axmlDecoder, overwriteExistingFile);
        }

        public FileInfo GetManifestXmlFile(DirectoryInfo apkDirectory, FileInfo file, IAxmlDecoder axmlDecoder, bool overwriteExistingFile = false)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));
            if (axmlDecoder is null)
                throw new ArgumentNullException(nameof(axmlDecoder));
            if (file.Exists && !overwriteExistingFile)
                return file;

            var localApk = GetLocalApk(apkDirectory);
            FileInfo bxmlFile;

            using (var zip = ZipFile.Open(localApk.FullName, ZipArchiveMode.Read))
            {
                var manifestEntry = zip.GetEntry("AndroidManifest.xml");
                bxmlFile = apkDirectory.File("AndroidManifest.bxml");

                if (bxmlFile.Exists && overwriteExistingFile)
                    bxmlFile.Delete();
                if (!bxmlFile.Exists)
                {
                    manifestEntry.ExtractToFile(bxmlFile.FullName);
                    bxmlFile.Refresh();
                }
            }

            axmlDecoder.Decode(bxmlFile, file);
            return file;
        }

        #endregion

        #region APK

        public string ApkPath { get => ApkPaths?.FirstOrDefault(); }

        public IEnumerable<string> ApkPaths
        {
            get
            {
                var strPaths = ClientDevice.GetRemoteCommandResultAsync($"pm path {PackageId}").Result;
                if (null == strPaths)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        System.Diagnostics.Debugger.Break();
                    return null;
                }

                var paths = strPaths.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.Substring("package:".Length))
                                    .ToArray();
                return paths;
            }
        }

        public FileInfo GetLocalApk(DirectoryInfo directory)
        {
            if (directory is null)
                throw new ArgumentNullException(nameof(directory));
            var dir = this.GetDefaultSubdir(directory);

            var file = dir.File(PackageId + ".apk");
            if (file.Exists) return file;

            PullLocalApk(file);
            return file;
        }

        public FileInfo GetLocalApk(FileInfo file, bool overwriteExisting = false)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));
            if (file.Exists && !overwriteExisting)
                throw new ArgumentException($"File '{file.FullName}' already exists");

            PullLocalApk(file);
            return file;
        }

        public FileInfo PullLocalApk(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.Exists)
                return file;

            ClientDevice.Pull(ApkPath, file);
            file.Refresh();

            return file;
        }

        #endregion

        #region dumpsys

        public IEnumerable<string> DumpsysValues
        {
            get
            {
                var values = new List<string>();
                AddAllValues(Dumpsys);
                return values;

                void AddAllValues(PropertyList propertyList)
                {
                    values.AddRange(propertyList.Values);
                    foreach (var subList in propertyList.Properties.Values)
                    {
                        AddAllValues(subList);
                    }
                }
            }
        }

        private PropertyList _Dumpsys = null;
        public PropertyList Dumpsys
        {
            get
            {
                return _Dumpsys ?? (_Dumpsys = GetDumpsys().Result);
            }
        }

        public string DisplayName => PackageId;

        public async Task<PropertyList> GetDumpsys()
        {
            PropertyList dumpsys = null;
            await ClientDevice.ExecuteRemoteCommandAsync($"dumpsys package {PackageId}", new StringReceiver(str =>
            {
                dumpsys = PropertyList.Parse(str, "  ");
            }));
            return dumpsys;
        }

        #endregion

        public override string ToString()
        {
            return PackageId;
        }

    }
}
