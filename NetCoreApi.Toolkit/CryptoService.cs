using System;
using System.Security.Cryptography;
using System.Text;

namespace NetCoreApi.Toolkit
{
    public class CryptoService
    {
        private const int ITERATIONS = 1000;

        private readonly RijndaelManaged _Rijndael;
        private readonly byte[] _Salt;

        public CryptoService(string issuer, string iv, string salt)
        {
            _Salt = Encoding.UTF8.GetBytes(salt);

            _Rijndael = new RijndaelManaged
            {
                BlockSize = 128,
                KeySize = 128,
                IV = HexStringToByteArray(iv),
                Key = GenerateKey(issuer),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.ECB
            };
        }

        public string Encrypt(string strPlainText)
        {
            byte[] strText = Encoding.UTF8.GetBytes(strPlainText);
            ICryptoTransform transform = _Rijndael.CreateEncryptor();
            byte[] cipherText = transform.TransformFinalBlock(strText, 0, strText.Length);

            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptor = _Rijndael.CreateDecryptor(_Rijndael.Key, _Rijndael.IV);
            byte[] originalBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(originalBytes);
        }

        private static byte[] HexStringToByteArray(string strHex)
        {
            byte[] r = new byte[strHex.Length / 2];
            for (int i = 0; i <= strHex.Length - 1; i += 2)
            {
                r[i / 2] = Convert.ToByte(Convert.ToInt32(strHex.Substring(i, 2), 16));
            }

            return r;
        }

        private byte[] GenerateKey(string token)
        {
            using Rfc2898DeriveBytes rfc2898 = new(Encoding.UTF8.GetBytes(token), _Salt, ITERATIONS);

            return rfc2898.GetBytes(128 / 8);
        }
    }

}
