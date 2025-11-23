using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace WindowsFormsApplication1
{
    public static class DbConfigHelper
    {
        private static string Version()
        {
            return "wsdetector V1.1.4 ";
        }

        private static string DecryptConfig(string cipherText)
        {
            try
            {
                // Key derivation giống Python: SHA256(key_suffix + mac_part)
                string keySuffix = Version();
                byte[] keyBytes = MacAddressHelper.GetConfigKey(keySuffix);
                
                byte[] fullCipher = Convert.FromBase64String(cipherText);

                if (fullCipher.Length < 28)
                {
                    throw new Exception("Dữ liệu mã hóa không hợp lệ");
                }

                // Format: nonce (12 bytes) + ciphertext + tag (16 bytes)
                byte[] nonce = new byte[12];
                Array.Copy(fullCipher, 0, nonce, 0, 12);

                byte[] tag = new byte[16];
                Array.Copy(fullCipher, fullCipher.Length - 16, tag, 0, 16);

                byte[] ciphertext = new byte[fullCipher.Length - 12 - 16];
                Array.Copy(fullCipher, 12, ciphertext, 0, ciphertext.Length);

                // Combine ciphertext + tag cho GCM
                byte[] ciphertextWithTag = new byte[ciphertext.Length + 16];
                Array.Copy(ciphertext, 0, ciphertextWithTag, 0, ciphertext.Length);
                Array.Copy(tag, 0, ciphertextWithTag, ciphertext.Length, 16);
                
                AesEngine aesEngine = new AesEngine();
                GcmBlockCipher gcm = new GcmBlockCipher(aesEngine);
                KeyParameter keyParam = new KeyParameter(keyBytes);
                AeadParameters parameters = new AeadParameters(keyParam, 128, nonce);
                
                gcm.Init(false, parameters);
                
                byte[] decryptedData = new byte[gcm.GetOutputSize(ciphertextWithTag.Length)];
                int len = gcm.ProcessBytes(ciphertextWithTag, 0, ciphertextWithTag.Length, decryptedData, 0);
                int finalLen = gcm.DoFinal(decryptedData, len);

                // Chỉ lấy dữ liệu thực tế (loại bỏ null bytes ở cuối)
                int totalLen = len + finalLen;
                if (totalLen < decryptedData.Length)
                {
                    byte[] actualData = new byte[totalLen];
                    Array.Copy(decryptedData, 0, actualData, 0, totalLen);
                    decryptedData = actualData;
                }

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi giải mã: " + ex.Message);
            }
        }

        public static string GetDatabasePath()
        {
            try
            {
                // Thử load db_config.json trước
                string configPath = Path.Combine(Application.StartupPath, "db_config.json");
                if (!File.Exists(configPath))
                {
                    // Thử load db_config.dat
                    configPath = Path.Combine(Application.StartupPath, "db_config.dat");
                    if (!File.Exists(configPath))
                    {
                        return null; // Không tìm thấy config
                    }
                }

                // Đọc file
                byte[] fileBytes = File.ReadAllBytes(configPath);
                
                string textContent = "";
                
                // Kiểm tra xem có phải file mã hóa không
                if (fileBytes.Length >= 28)
                {
                    try
                    {
                        // Thử giải mã như file mã hóa
                        string base64Content = Convert.ToBase64String(fileBytes);
                        textContent = DecryptConfig(base64Content);
                    }
                    catch
                    {
                        // Không phải file mã hóa, xử lý như text
                        textContent = Encoding.UTF8.GetString(fileBytes);
                    }
                }
                else
                {
                    // Xử lý như file text (JSON)
                    textContent = Encoding.UTF8.GetString(fileBytes);
                }

                // Loại bỏ BOM nếu có
                if (textContent.Length > 0 && textContent[0] == '\uFEFF')
                {
                    textContent = textContent.Substring(1);
                }
                
                textContent = textContent.Trim();
                
                // Thử giải mã nếu là base64 (chỉ thử nếu có vẻ như base64)
                if (!string.IsNullOrEmpty(textContent) && textContent.Length > 50 && 
                    !textContent.TrimStart().StartsWith("{"))
                {
                    try
                    {
                        textContent = DecryptConfig(textContent);
                    }
                    catch
                    {
                        // File chưa được mã hóa, giữ nguyên
                    }
                }

                // Parse JSON
                JObject config = JObject.Parse(textContent);
                string dbPath = config["database_path"]?.ToString();
                
                return string.IsNullOrEmpty(dbPath) ? null : dbPath;
            }
            catch
            {
                return null; // Lỗi đọc config, trả về null
            }
        }
    }
}

