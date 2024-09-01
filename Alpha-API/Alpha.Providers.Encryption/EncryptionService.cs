using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Integrations.Encryption.DataTransferObjects;
using Alpha.Integrations.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Alpha.Providers.Encryption.Configurations;

namespace Alpha.Providers.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private readonly CryptoConfiguration _cryptoConfiguration;

        public EncryptionService(CryptoConfiguration cryptoConfiguration)
        {
            _cryptoConfiguration = cryptoConfiguration;
        }

        public string Encrypt(string message)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(_cryptoConfiguration.InitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(_cryptoConfiguration.SaltValue);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(message);

            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                _cryptoConfiguration.PassPhrase,
                saltValueBytes,
                _cryptoConfiguration.HashAlgorithm,
                _cryptoConfiguration.PasswordIterations
            );

            byte[] keyBytes = password.GetBytes(_cryptoConfiguration.KeySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor
            (
                keyBytes,
                initVectorBytes
            );

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                encryptor,
                CryptoStreamMode.Write
            );

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        public string Decrypt(string cipher)
        {
            if (cipher.IsNullOrEmpty()) return string.Empty;

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(_cryptoConfiguration.InitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(_cryptoConfiguration.SaltValue);

            byte[] cipherTextBytes = Convert.FromBase64String(cipher);

            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                _cryptoConfiguration.PassPhrase,
                saltValueBytes,
                _cryptoConfiguration.HashAlgorithm,
                _cryptoConfiguration.PasswordIterations
            );

            byte[] keyBytes = password.GetBytes(_cryptoConfiguration.KeySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor
            (
                keyBytes,
                initVectorBytes
            );

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                decryptor,
                CryptoStreamMode.Read
            );

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read
            (
                plainTextBytes,
                0,
                plainTextBytes.Length
            );

            memoryStream.Close();
            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString
            (
                plainTextBytes,
                0,
                decryptedByteCount
            );

            return plainText;
        }

        public GeneratePasswordResponse GenerateEncryptedPassword(string cipher)
        {
            var response = new GeneratePasswordResponse();

            var keybytes = Encoding.UTF8.GetBytes(_cryptoConfiguration.CryptoJsIv);
            var iv = Encoding.UTF8.GetBytes(_cryptoConfiguration.CryptoJsIv);

            var encrypted = Convert.FromBase64String(cipher);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            var plainPassword = string.Format(decriptedFromJavascript);

            response.IsSuccess = plainPassword != "keyError";

            if (response.IsSuccess)
            {
                response.Password = Encrypt(plainPassword);
            }

            return response;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            string plaintext = null;

            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;

                rijAlg.Key = key;
                rijAlg.IV = iv;
 
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
    }
}