using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android.Lib
{
    /// <summary>
    /// Decodes binary XML into normal XML
    /// </summary>
    public interface IAxmlDecoder
    {
        void Decode(FileInfo @in, FileInfo @out);
    }
}
