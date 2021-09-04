using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using Android.Lib.Manifest;

namespace AndroidLib.Test.Manifest
{
    public class PermissionTest
    {
        [Fact] public void Parse()
        {
            var manifest = Android.Lib.Manifest.Manifest.FromXml(TestData.AndroidManifest_xml);

            Assert.Equal("android.permission.ACCESS_WIFI_STATE", manifest.Permissions.ElementAt(0).Name);
            Assert.Equal("android.permission.CHANGE_WIFI_STATE", manifest.Permissions.ElementAt(1).Name);
        }

    }
}
