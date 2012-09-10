using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Amido.SystemEx.Contracts;

namespace Amido.SystemEx.IO
{
    public static class EncryptionUtil
    {
        private const string SaltValue = @"_7=L#4_]_64|:Bm/h|;-5Iu0/u-iIDi\yPd]-zFk0Ds2V;@8Q";
        private const int Iterations = 5000;
        private static Random random = new Random();
        
        public static byte[] GetHash(Stream stream)
        {
            return new MD5CryptoServiceProvider().ComputeHash(stream);
        }

        public static string DecryptString(string cipherText, string passPhrase)
        {
            return DecryptString(cipherText, passPhrase, SaltValue, Iterations, null, 256);
        }

        public static string DecryptString(string cipherText, string passPhrase, string saltValue)
        {
            return DecryptString(cipherText, passPhrase, saltValue, Iterations, null, 256);
        }

        public static string EncryptString(string plainText, string passPhrase)
        {
            return EncryptString(plainText, passPhrase, SaltValue);
        }

        public static string EncryptString(string plainText, string passPhrase, string saltValue)
        {
            return EncryptString(plainText, passPhrase, saltValue, Iterations, null, 256);
        }

        public static string EncryptString(
            string plainText,
            string passPhrase,
            string saltValue,
            int passwordIterations,
            string initVector,
            int keySize)
        {

            byte[] initVectorBytes = initVector == null ? new byte[16] : Encoding.ASCII.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = GetKeyBytes(passPhrase, saltValue, passwordIterations, keySize);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            byte[] cipherTextBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                }
            }
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        public static byte[] GetKeyBytes(string passPhrase, string saltValue, int passwordIterations, int keySize)
        {
            Args.NotNull(saltValue, "saltValue");
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations);
            return deriveBytes.GetBytes(keySize / 8);
        }

        public static string DecryptString(
            string cipherText,
            string passPhrase,
            string saltValue,
            int passwordIterations,
            string initVector,
            int keySize)
        {
            byte[] initVectorBytes = initVector == null ? new byte[16] : Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);


            byte[] keyBytes = GetKeyBytes(passPhrase, saltValue, passwordIterations, keySize);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            byte[] plainTextBytes;
            int decryptedByteCount;
            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    plainTextBytes = new byte[cipherTextBytes.Length];
                    decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                }
            }
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return plainText;
        }

        public static string RandomString(int size)
        {
            var data = new byte[size];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)random.Next(32, 126);
            }
            var encoding = new ASCIIEncoding();
            return encoding.GetString(data);
        }
    }
}