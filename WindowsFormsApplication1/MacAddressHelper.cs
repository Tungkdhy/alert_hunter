using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace WindowsFormsApplication1
{
    public static class MacAddressHelper
    {
        /// <summary>
        /// Normalize MAC address: lowercase và chỉ giữ lại các ký tự hex (0-9, a-f)
        /// </summary>
        private static string NormalizeMacForKey(string mac)
        {
            if (string.IsNullOrEmpty(mac))
                return string.Empty;

            mac = mac.ToLower();
            return new string(mac.Where(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f')).ToArray());
        }

        /// <summary>
        /// Lấy MAC address để dùng cho key generation
        /// Logic giống Python: lấy tất cả MAC từ các interface đang up, normalize, lấy max, nếu > 6 ký tự thì lấy 6 ký tự cuối
        /// </summary>
        public static string GetMacForKey()
        {
            try
            {
                List<string> macs = new List<string>();

                // Lấy tất cả network interfaces
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface ni in interfaces)
                {
                    // Chỉ lấy các interface đang up (IsUp)
                    if (ni.OperationalStatus != OperationalStatus.Up)
                        continue;

                    // Lấy physical address (MAC address)
                    PhysicalAddress physicalAddress = ni.GetPhysicalAddress();
                    if (physicalAddress == null)
                        continue;

                    string macAddress = physicalAddress.ToString();
                    string normalizedMac = NormalizeMacForKey(macAddress);

                    // Bỏ qua MAC rỗng hoặc "000000000000"
                    if (!string.IsNullOrEmpty(normalizedMac) && normalizedMac != "000000000000")
                    {
                        macs.Add(normalizedMac);
                    }
                }

                if (macs.Count == 0)
                {
                    throw new Exception("Không tìm được MAC nào dùng cho key (NetworkInterface).");
                }

                // Lấy MAC lớn nhất (theo thứ tự string)
                string maxMac = macs.OrderByDescending(m => m).First();

                // Nếu MAC dài hơn 6 ký tự, lấy 6 ký tự cuối
                if (maxMac.Length > 6)
                {
                    maxMac = maxMac.Substring(maxMac.Length - 6);
                }

                return maxMac;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy MAC address: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo key từ key suffix và MAC address (giống Python get_config_key)
        /// </summary>
        public static byte[] GetConfigKey(string keySuffix)
        {
            try
            {
                string macPart = GetMacForKey();
                string keyMaterial = keySuffix + macPart;
                byte[] keyMaterialBytes = Encoding.UTF8.GetBytes(keyMaterial);

                using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    return sha256.ComputeHash(keyMaterialBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo config key: {ex.Message}");
            }
        }
    }
}


