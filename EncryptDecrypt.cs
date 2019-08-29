using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace FileEncryption
{
    /// Does encryption and decrytion of files using the AesCryptoServiceProvider class.
    public class EncryptDecrypt
    {
        public enum mode
        {
            ENCRYPT,
            DECRYPT,
        }

        public EncryptDecrypt(string inputfile, mode cryptomode)
        {
            // Read the input file.
            byte[] input = File.ReadAllBytes(inputfile);

            // Create a new crypto service provider.
            var aes = new AesCryptoServiceProvider();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            if (cryptomode == mode.ENCRYPT)
            {
                // Generate a new key and IV.
                aes.GenerateKey();
                aes.GenerateIV();

                // Write the key and IV to a file.
                List<byte> key_and_iv = new List<byte>();
                key_and_iv.AddRange(aes.Key);
                key_and_iv.AddRange(aes.IV);
                File.WriteAllBytes(inputfile + ".encrypted.key", key_and_iv.ToArray());

                // Perform the encryption.
                var ms = new MemoryStream();
                var en = aes.CreateEncryptor();
                var cs = new CryptoStream(ms, en, CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                // Write the encrypted bytes to a file.
                File.WriteAllBytes(inputfile + ".encrypted", ms.ToArray());
            }
            else
            {
                // Read the key and IV from a file.
                List<byte> key_and_iv = new List<byte>(File.ReadAllBytes(inputfile + ".key"));
                aes.Key = key_and_iv.GetRange(0, 32).ToArray();
                aes.IV = key_and_iv.GetRange(32, 16).ToArray();

                // Perform the decryption.
                var ms = new MemoryStream();
                var de = aes.CreateDecryptor();
                var cs = new CryptoStream(ms, de, CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                // Write the decrypted bytes to a file.
                File.WriteAllBytes(inputfile + ".decrypted", ms.ToArray());
            }
        }
    }
}
