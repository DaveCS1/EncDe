using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Security.Cryptography;
namespace EncryptDecryptAndRandomize
{
   public class test
    {
        private const string TestInputFile = "test_input.txt";
        private const string TestEncryptedFile = "test_encrypted.txt";
        private const string TestDecryptedFile = "test_decrypted.txt";
        private const string TestKeyFile = "test_key.key";
        public test()
        {
            File.WriteAllText(TestInputFile, "This is a test file.");
           
        }
        public void tes()
        {
            //FileEncryptor.WriteKeyToFile(TestKeyFile, key);

            //// Encrypt the file
            //FileEncryptor.EncryptFile(TestInputFile, TestEncryptedFile, key);
        }
        public void EncryptDecrypt_ValidFile_Success()
        {
            // Generate a random key
            byte[] key = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }

            // Write the key to a file
            FileEncryptor.WriteKeyToFile(TestKeyFile, key);

            // Encrypt the file
            FileEncryptor.EncryptFile(TestInputFile, TestEncryptedFile, key);

            // Decrypt the file
            FileEncryptor.DecryptFile(TestEncryptedFile, TestDecryptedFile, key);

            // Assert that the decrypted file matches the original input file
            string decryptedContent = File.ReadAllText(TestDecryptedFile);
            Console.WriteLine(decryptedContent);
        }
    }
}
