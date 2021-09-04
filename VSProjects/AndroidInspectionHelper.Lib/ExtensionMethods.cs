using System;
using System.Drawing;

namespace Android.Lib
{
    public static class ExtensionMethods
    {
        public static bool[] GetSimilarityHash(this Bitmap img)
        {
            if (null == img) throw new ArgumentNullException(nameof(img));

            var hash = new bool[16 * 16];
            var resized = new Bitmap(img, new Size(16, 16));

            for (var x = 0; x < resized.Width; ++x)
                for (var y = 0; y < resized.Height; ++y)
                {
                    hash[x * 16 + y] = resized.GetPixel(x, y).GetBrightness() > 0.5 ? true : false;
                }

            return hash;
        }
    }

}
