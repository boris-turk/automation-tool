using System;
using System.IO;
using System.Security.Cryptography;

namespace BTurk.Automation.Core.Credentials;

public class UserCredentials
{
    public string Identifier { get; set; }

    public string EncryptedCredentials { get; set; }

    public void EncryptCredentials(string username, string password, string encryptionKey)
    {
        using var aesAlg = Aes.Create();

        aesAlg.Key = DeriveKeyFromPassword(encryptionKey, aesAlg.KeySize / 8);
        aesAlg.IV = new byte[16];

        var encryption = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();

        using (var csEncrypt = new CryptoStream(msEncrypt, encryption, CryptoStreamMode.Write))
        {
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(username);
                swEncrypt.Write("\n");
                swEncrypt.Write(password);
            }
        }

        EncryptedCredentials = Convert.ToBase64String(msEncrypt.ToArray());
    }

    public (string Username, string Password) DecryptCredentials(string encryptionKey)
    {
        using var aesAlg = Aes.Create();

        aesAlg.Key = DeriveKeyFromPassword(encryptionKey, aesAlg.KeySize / 8);
        aesAlg.IV = new byte[16];

        var decryption = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(Convert.FromBase64String(EncryptedCredentials));
        using var csDecrypt = new CryptoStream(msDecrypt, decryption, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        var username = srDecrypt.ReadLine();
        var password = srDecrypt.ReadToEnd();

        return (username, password);
    }

    // Derive a key from the password using PBKDF2
    private byte[] DeriveKeyFromPassword(string password, int keySize)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, new byte[16], 10000, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(keySize);
    }
}