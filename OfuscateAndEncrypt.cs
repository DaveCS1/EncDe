using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EncryptDecryptAndRandomize
{
    public class OfuscateAndEncrypt
    {      
        static Dictionary<string, string> nameMapping = new Dictionary<string, string>();
        static Random random = new Random();
        static byte[] key = new byte[32]; // 256 bits
        static byte[] iv = new byte[16]; // 128 bits
        List<string> filesToEncryp = new List<string>();
        public string EncryptionKey { get; private set; }

        public OfuscateAndEncrypt(List<string> filesToEncrypt)
        {
            filesToEncryp.Clear();
            random.NextBytes(key);
            random.NextBytes(iv);
            EncryptionKey = Convert.ToBase64String(key);
            foreach (string file in filesToEncrypt.ToList())
{
    string code = File.ReadAllText(file);
    string obfuscatedCode = ObfuscateCode(code);
    string encryptedCode = EncryptString(obfuscatedCode, key, iv);
    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file) + "_" + DateTime.Now.ToString("MMddyy_HHmmss");
   // string encryptedFileName = fileNameWithoutExtension + ".txt";
    string encryptedFileName = fileNameWithoutExtension + "_encrypted.txt";
    File.WriteAllText(encryptedFileName, encryptedCode);
    filesToEncrypt.Remove(file);
    SaveMappingToFile(fileNameWithoutExtension + "_mapping.txt");
    
    // Save the encryption key to a file with a similar name
   
}
        string keyFileName = "encryption_key_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
        File.WriteAllText(keyFileName, EncryptionKey);
        }

        #region Encrypt
        static string ObfuscateCode(string code)
        {
            string pattern = @"\b([a-zA-Z_][a-zA-Z0-9_]*)\b";
            return Regex.Replace(code, pattern, match =>
            {
                string originalName = match.Value;
                if (!nameMapping.ContainsKey(originalName))
                {
                    string obfuscatedName = GenerateRandomName();
                    nameMapping[originalName] = obfuscatedName;
                }
                return nameMapping[originalName];
            });
        }
        static string GenerateRandomName()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void SaveMappingToFile(string fileName)
        {
            File.WriteAllLines(fileName, nameMapping.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }

        static void LoadMappingFromFile(string fileName)
        {
            nameMapping = File.ReadLines(fileName)
                .Select(line => line.Split('='))
                .ToDictionary(parts => parts[0], parts => parts[1]);
        }

        static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        } 
        #endregion
        #region Decrypt
        public string DecryptAndDeobfuscate(string encryptedFilePath, string keyFilePath, string mappingFilePath)
        {
            // Read the encrypted code from the file
            string encryptedCode = File.ReadAllText(encryptedFilePath);

            // Read the encryption key from the file
            string keyString = File.ReadAllText(keyFilePath);
            byte[] key = Convert.FromBase64String(keyString);

            // Decrypt the code
            string decryptedCode = DecryptString(encryptedCode, key, iv);

            // Load the name mapping from the file
            LoadMappingFromFile(mappingFilePath);

            // Deobfuscate the code using the name mapping
            string deobfuscatedCode = DeobfuscateCode(decryptedCode);

            return deobfuscatedCode;
        }

        static string DecryptString(string encryptedText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        static string DeobfuscateCode(string obfuscatedCode)
        {
            string pattern = @"\b([A-Z0-9]{5})\b";
            return Regex.Replace(obfuscatedCode, pattern, match =>
            {
                string obfuscatedName = match.Value;
                return nameMapping.FirstOrDefault(x => x.Value == obfuscatedName).Key;
            });
        } 
        #endregion
//public List<string> GetEncryptedFiles()
//{
//            string[] encryptedFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_encrypted.txt");
//            return encryptedFiles.Select(Path.GetFileName).ToList();
//}
        public List<string> GetEncryptedFiles()
        {
            var encryptedFiles = new List<string>();
            var allFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory ,"*_encrypted.txt", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                encryptedFiles.Add(Path.GetFullPath(file));
            }

            return encryptedFiles;
        }

        //
    }
}
