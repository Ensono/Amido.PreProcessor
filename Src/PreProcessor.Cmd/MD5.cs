using System;
using System.Security.Cryptography;
using System.Text;

namespace Amido.PreProcessor.Cmd
{
    internal class MD5
    {
        private readonly string passPhrase;

        internal MD5(string passPhrase)
        {
            if (string.IsNullOrWhiteSpace(passPhrase))
            {
                throw new ArgumentException("No pass-phrase provided.", "passPhrase");
            }

            this.passPhrase = passPhrase;
        }

        internal string Decrypt(string value)
        {
            MD5CryptoServiceProvider hashProvider = null;
            TripleDESCryptoServiceProvider provider = null;

            try
            {
                hashProvider = new MD5CryptoServiceProvider();
                var hashPassPhrase = hashProvider.ComputeHash(Encoding.UTF8.GetBytes(passPhrase));

                provider = new TripleDESCryptoServiceProvider();
                provider.Key = hashPassPhrase;
                provider.Mode = CipherMode.ECB;
                provider.Padding = PaddingMode.PKCS7;

                var dataToEncrypt = Convert.FromBase64String(value);
                var decryptor = provider.CreateDecryptor();
                var results = decryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                return Encoding.UTF8.GetString(results);

            }
            finally
            {
                if (provider != null) provider.Clear();
                if (hashProvider != null) hashProvider.Clear();
            }
        }
    }
}
