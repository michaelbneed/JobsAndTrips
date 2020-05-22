
#region Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace RaskTrip.Utility.Security
{
    public class Cryptographer
    {
        // A random password generator
        // http://www.pctools.com/guides/password/?length=12&phonetic=on&alpha=on&mixedcase=on&numeric=on&punctuation=on&nosimilar=on&quantity=1&generate=true

        private const string DEFAULT_ENCRYPTION_KEY = "wrudeQE_asP+";
        private const string DEFAULT_ENCRYPTION_KEY16 = "gTJPVBgm3aV7r35Z91ZLCQChAiycvhIPLXjb73rXj8k="; // "e9en3jffUEL098Z+";
        private const string DEFAULT_IV = "NYL/NSev88bTZD3XNN9zyw==";
        private const int AES_BLOCK_SIZE = 16;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="reversible"></param>
        /// <returns></returns>
        //public byte[] EncryptString(byte[] plainBytes, string encryptionKey, bool reversible)
        //{
        //    return AesEncryptString(plainBytes, encryptionKey);
        //}

        private byte[] SHA512ManagedEncryptString(byte[] plainMessage)
        {
            return (new SHA512Managed()).ComputeHash(plainMessage);
        }

        //private byte[] AesEncryptString(byte[] plainBytes, string encryptionKey)
        //{
        //    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        //    // aes.IV = new byte[AES_BLOCK_SIZE];
        //    aes.IV = Convert.FromBase64String(DEFAULT_IV);
        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(encryptionKey, new byte[0]);
        //    aes.Key = pdb.CryptDeriveKey("AES", "SHA1", 256, new byte[AES_BLOCK_SIZE]);
        //    MemoryStream ms = new MemoryStream(plainBytes.Length);
        //    CryptoStream encStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
        //    encStream.Write(plainBytes, 0, plainBytes.Length);
        //    encStream.FlushFinalBlock();
        //    byte[] encryptedBytes = new byte[ms.Length];
        //    ms.Position = 0;
        //    ms.Read(encryptedBytes, 0, (int)ms.Length);
        //    encStream.Close();
        //    return encryptedBytes;
        //}

        private byte[] AesEncryptString(byte[] plainBytes, string encryptionKey, string iv)
        {
            byte[] encryptedBytes;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Convert.FromBase64String(encryptionKey);
                aesAlg.IV = Convert.FromBase64String(iv);
                aesAlg.Padding = PaddingMode.Zeros;
                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all the data to be encrypted to the stream
                            swEncrypt.Write(Encoding.Unicode.GetString(plainBytes));
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }
            }
            return encryptedBytes;
        }

        /// <summary>
        /// Encrypt using the default key.
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <param name="messgaeLength"
        /// <param name="reversible"></param>
        /// <returns></returns>
        public byte[] EncryptString(byte[] plainBytes, bool reversible)
        {
            if (reversible)
                return AesEncryptString(plainBytes, DEFAULT_ENCRYPTION_KEY16, DEFAULT_IV);
            else
                return SHA512ManagedEncryptString(plainBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedBase64"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        //public byte[] DecryptString(byte[] encryptedBytes, string encryptionKey)
        //{
        //    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        //    aes.IV = new byte[AES_BLOCK_SIZE];
        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(encryptionKey, new byte[0]);
        //    aes.Key = pdb.CryptDeriveKey("AES", "SHA1", 256, new byte[AES_BLOCK_SIZE]);
        //    MemoryStream ms = new MemoryStream(encryptedBytes.Length);
        //    CryptoStream decStream = new CryptoStream(ms,
        //                                              aes.CreateDecryptor(),
        //                                              CryptoStreamMode.Write);
        //    decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
        //    decStream.FlushFinalBlock();
        //    byte[] plainBytes = new byte[ms.Length];
        //    ms.Position = 0;
        //    ms.Read(plainBytes, 0, (int)ms.Length);
        //    decStream.Close();
        //    return plainBytes;
        //}

        public byte[] DecryptString(byte[] encryptedBytes, string encryptionKey, string iv)
        {
            string plainText = String.Empty;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Convert.FromBase64String(encryptionKey);
                aesAlg.IV = Convert.FromBase64String(iv);
                aesAlg.Padding = PaddingMode.Zeros;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return Encoding.Unicode.GetBytes(plainText.TrimEnd((new char[] {'\0' })));
        }

        /// <summary>
        /// Decrypt using the default key.
        /// </summary>
        /// <param name="encryptedBase64"></param>
        /// <returns></returns>
        public byte[] DecryptString(byte[] encrypted)
        {

            return DecryptString(encrypted, DEFAULT_ENCRYPTION_KEY16, DEFAULT_IV);
        }


        public bool VerifyEncryption(byte[] input, string encryptedValue)
        {
            string encryptedInput = Convert.ToBase64String(EncryptString(input, false));

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(encryptedInput, encryptedValue);
        }
    }
}
