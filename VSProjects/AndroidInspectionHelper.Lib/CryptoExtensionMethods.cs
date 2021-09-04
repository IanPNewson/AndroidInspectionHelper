using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Android.Lib
{
    public static class CryptoExtensionMethods
    {

        public static string ExportPublicKeyFile(this RSA rsa)
        {
            var privateKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            var builder = new StringBuilder("-----BEGIN PUBLIC KEY-----");

            var base64PrivateKeyString = Convert.ToBase64String(privateKeyBytes);
            const int LINE_LENGTH = 64;

            for (var i = 0; i < base64PrivateKeyString.Length; ++i)
            {
                if (i % (LINE_LENGTH - 1) == 0)
                    builder.Append("\n");
                builder.Append(base64PrivateKeyString[i]);
            }
            if (builder[builder.Length-1] != '\n')
                builder.Append("\n");

            builder.AppendLine("-----END PUBLIC KEY-----");
            return builder.ToString();
        }

    }
}
