using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CafeManager.WPF.Stores
{
    public class EncryptionHelper
    {
        private readonly byte[] _key;

        public EncryptionHelper(IServiceProvider provider)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            string keyString = configuration["Encryption_Key"];

            // Băm chuỗi khóa từ cấu hình thành 32 byte để đảm bảo kích thước hợp lệ
            using (SHA256 sha256 = SHA256.Create())
            {
                _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyString));
            }
        }

        public string EncryptAES(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.GenerateIV(); // Tạo vector khởi tạo (IV) ngẫu nhiên
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // Ghi IV vào đầu dữ liệu mã hóa
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string DecryptAES(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(fullCipher, iv, iv.Length);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}