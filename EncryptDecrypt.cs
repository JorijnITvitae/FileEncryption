using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace FileEncryption
{
    /// <summary>
    /// Does encryption and decrytion of files using the RijndaelManaged class
    /// </summary>
    public class EncryptDecrypt
    {
        private string fileName;
        private string original;
        private RijndaelManaged rijndael;
        private string encrypted;
        private string decrypted;

        public string FileName
        {
            get => this.fileName;
        }

        public string Original
        {
            get => this.original;
        }
        
        public string Encrypted
        {
            get => this.encrypted;
        }

        public string Decrypted
        {
            get => this.decrypted;
        }

        public byte[] Key
        {
            get => this.rijndael.Key;
        }

        public byte[] IV
        {
            get => this.rijndael.IV;
        }

        public EncryptDecrypt(string fileName)
        {
            this.fileName = fileName;
            this.original = File.ReadAllText(fileName);
            this.rijndael = new RijndaelManaged();
            this.rijndael.GenerateKey();
            this.rijndael.GenerateIV();
        }

        public void Encrypt()
        {
            // Check variables.
            if (this.Original == null || this.Original.Length <= 0)
                throw new ArgumentNullException("Input Text");

            if (this.Key == null || this.Key.Length <= 0)
                throw new ArgumentNullException("Key");

            if (this.IV == null || this.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = this.rijndael.CreateEncryptor(this.Key, this.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(this.Original);
                    }

                    // Read the encrypted bytes from the encrypting stream and place them in a string.
                    this.encrypted = msEncrypt.ToString();
                }
            }
        }

        public void Decrypt()
        {
            // Check variables.
            if (this.Original == null || this.Original.Length <= 0)
                throw new ArgumentNullException("Input Text");

            if (this.Key == null || this.Key.Length <= 0)
                throw new ArgumentNullException("Key");

            if (this.IV == null || this.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = this.rijndael.CreateDecryptor(this.Key, this.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(Encoding.ASCII.GetBytes(this.Original)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream and place them in a string.
                        this.decrypted = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
