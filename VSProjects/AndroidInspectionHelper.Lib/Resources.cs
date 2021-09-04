using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android.Lib
{
    public static class Resources
    {

        public static Bitmap[] DefaultIcons
        {
            get
            {
                return new[] { Android.DefaultLauncherIcon2, Android.DefaultLauncherIcon1, Android.DefaultLauncherIcon3 };
            }
        }

    }
}
