using System;
using System.IO;
using System.Security.Cryptography;

// ReSharper disable UnusedMember.Global

namespace AutomationEngine
{
    public static class Encryption
    {
        private static readonly byte[] Key = { 4, 102, 111, 23, 28, 111, 232, 117 };
        private static readonly byte[] Vector = { 106, 177, 252, 221, 119, 94, 77, 71 };

        public static string Encrypt(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            using (var cryptoProvider = new DESCryptoServiceProvider())
            using (var cryptoTransform = cryptoProvider.CreateEncryptor(Key, Vector))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cryptoStream))
            {
                writer.Write(text);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        public static string Decrypt(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            using (var cryptoProvider = new DESCryptoServiceProvider())
            using (var cryptoTransform = cryptoProvider.CreateDecryptor(Key, Vector))
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(text)))
            using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cryptoStream))
            {
                return reader.ReadToEnd();
            }
        }
    }

}