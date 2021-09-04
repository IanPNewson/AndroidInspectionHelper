using Android.Lib;
using INHelpers.Diagnostics;
using INHelpers.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxmlDecoder
{
    public class AxmlDecoder : IAxmlDecoder
    {

        private static FileInfo _Exe;

        static AxmlDecoder()
        {
            _Exe = new FileInfo(typeof(AxmlDecoder).Assembly.Location)
                .Directory
                .File("axmldec.exe");
        }

        public void Decode(FileInfo @in, FileInfo @out)
        {
            if (@in is null) throw new ArgumentNullException(nameof(@in));
            if (@out is null) throw new ArgumentNullException(nameof(@out));
            if (!@in.Exists) throw new ArgumentException($"Input file '{@in.FullName}' does not exist", nameof(@in));

            var cmd = new CommandLineRunner();
            cmd.Run(".", _Exe.FullName, $"-i \"{@in.FullName}\" -o \"{@out.FullName}\"");
        }
    }
}
