using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace EncryptDecryptAndRandomize
{
    public class FileEncryptor
    {
        public static void EncryptFile(string inputFile, string outputFile, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = DeriveIV(key);
                aes.Padding = PaddingMode.PKCS7;

                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (CryptoStream csEncrypt = new CryptoStream(fsOutput, encryptor, CryptoStreamMode.Write))
                {
                    fsInput.CopyTo(csEncrypt);
                }
            }
        }

        public static void DecryptFile(string inputFile, string outputFile, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = DeriveIV(key);
                aes.Padding = PaddingMode.PKCS7;

                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream csDecrypt = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                {
                    csDecrypt.CopyTo(fsOutput);
                }
            }
        }

        private static byte[] DeriveIV(byte[] key)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(key).Take(16).ToArray();
            }
        }

        public static void WriteKeyToFile(string keyFile, byte[] key)
        {
            File.WriteAllBytes(keyFile, key);
        }

        public static byte[] ReadKeyFromFile(string keyFile)
        {
            return File.ReadAllBytes(keyFile);
        }
        public void process()
        {
         string inputFile = "path/to/input/file.txt";
string encryptedFile = "path/to/encrypted/file.encrypted";
string decryptedFile = "path/to/decrypted/file.txt";
string keyFile = "path/to/key/file.key";

// Generate a random key
byte[] key = new byte[32];
using (var rng = new RNGCryptoServiceProvider())
{
    rng.GetBytes(key);
}

// Write the key to a file
FileEncryptor.WriteKeyToFile(keyFile, key);

// Encrypt the file
FileEncryptor.EncryptFile(inputFile, encryptedFile, key);   
        }
        
    }
}
   //    public void process(){
      //      string inputFile = "path/to/input/file.txt";
/// <summary>
/// string encryptedFile = "path/to/encrypted/file.encrypted";
/// </summary>
//string decryptedFile = "path/to/decrypted/file.txt";
//byte[] key = // Provide the encryption key
//FileEncryptor.EncryptFile(inputFile, encryptedFile, key);
//FileEncryptor.DecryptFile(encryptedFile, decryptedFile, key);
      //  }