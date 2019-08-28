using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace FileEncryption
{
    /// <summary>
    /// Does encryption and decrytion of files using the RijndaelManaged class
    /// </summary>
    public class EncryptDecrypt
    {
        // TODO: Write files in encrypt / decrypt functions.
        // TODO: Get rid of password and just store the key and iv in plaintext as originalfilename_key.txt.
        // TODO: Get rid of the password textbox and add a file selector for the key.txt.

        private string inputfile;

        public EncryptDecrypt(string inputfile)
        {
            this.inputfile = inputfile;
        }

        public void Encrypt()
        {
            byte[] input = File.ReadAllBytes(this.inputfile);

            using (var aes = new AesManaged())
            {
                // Generate a new key and IV.
                aes.GenerateKey();
                aes.GenerateIV();
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Write the key and IV to a file.
                List<byte> key_and_iv = new List<byte>();
                key_and_iv.AddRange(aes.Key);
                key_and_iv.AddRange(aes.IV);
                File.WriteAllBytes(this.inputfile + ".encrypted.key", key_and_iv.ToArray());

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor();

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write the input to the encryting stream.
                            swEncrypt.Write(input);
                        }
                    }

                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Read))
                    {
                        using (var srEncrypt = new StreamReader(csEncrypt))
                        {
                            // Read the encrypted bytes from the encrypting stream.
                            //List<byte> encrypted = new List<byte>(Encoding.ASCII.GetBytes(srEncrypt.ReadToEnd()));

                            // Adding padding bytes.
                            //for (byte p = 0; encrypted.Count < 16 || encrypted.Count % 16 != 0; ++p) encrypted.Add(p);

                            // And finally write the output to a file.
                            //File.WriteAllBytes(this.inputfile + ".encrypted", encrypted.ToArray());

                            byte[] encrypted = Encoding.ASCII.GetBytes(srEncrypt.ReadToEnd());
                            File.WriteAllBytes(this.inputfile + ".encrypted", encrypted);
                        }
                    }
                }
            }
        }

        public void Decrypt()
        {
            List<byte> input = new List<byte>(File.ReadAllBytes(this.inputfile));
            List<byte> key_and_iv = new List<byte>(File.ReadAllBytes(this.inputfile + ".key"));

            using (var aes = new AesManaged())
            {
                // Use the key and IV from the key file.
                aes.Key = key_and_iv.GetRange(0, 32).ToArray();
                aes.IV = key_and_iv.GetRange(32, 16).ToArray();

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aes.CreateDecryptor();

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(input.ToArray()))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream.
                            byte[] decrypted = Encoding.ASCII.GetBytes(srDecrypt.ReadToEnd());

                            // And finally write the output to a file.
                            File.WriteAllBytes(this.inputfile + ".decrypted", decrypted);
                        }
                    }
                }
            }
        }
    }
}
