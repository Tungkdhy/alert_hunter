using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

public static class ApiService
{
    private static readonly HttpClient http = new HttpClient();
    private static Config _config;
    private static readonly string ConfigPath;

    // ✅ Static constructor — chỉ chạy 1 lần
    static ApiService()
    {
        try
        {
            // Dùng Application.StartupPath an toàn hơn trong WinForms
            ConfigPath = Path.Combine(Application.StartupPath, "config.json");

            if (!File.Exists(ConfigPath))
            {
                // nếu chưa có, tạo file mặc định
                var defaultConfig = new Config
                {
                    BaseUrl = "http://192.168.17.132:8000/api/alerts",
                    UserToken = "your_secret_user_token_here"
                };
                string jsonDefault = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                File.WriteAllText(ConfigPath, jsonDefault);

                _config = defaultConfig;
                MessageBox.Show(
                    "⚙️ File cấu hình chưa tồn tại — hệ thống đã tạo mới:\n\n" + ConfigPath +
                    "\n\nHãy sửa thông tin BaseUrl và UserToken rồi khởi động lại chương trình.",
                    "Config Created",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                string json = File.ReadAllText(ConfigPath);
                _config = JsonConvert.DeserializeObject<Config>(json);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Lỗi khi đọc file config.json:\n" + ex.Message,
                "Lỗi cấu hình",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );

            _config = new Config
            {
                BaseUrl = "http://localhost:8000/api/alerts",
                UserToken = "CHANGE_ME_USER_TOKEN"
            };
        }
    }

    // 🔍 Hàm gọi API đọc dữ liệu cảnh báo
    public static async Task<List<Alert>> GetAlertsAsync(string macFilter)
    {
        try
        {
            if (_config == null)
                throw new InvalidOperationException("Config chưa được load.");

            string url = _config.BaseUrl.TrimEnd('/') + $"?mac={macFilter}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-User-Token", _config.UserToken);

            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var wrapper = JsonConvert.DeserializeObject<AlertResponse>(json);
            return wrapper != null && wrapper.items != null ? wrapper.items : new List<Alert>();
        }
        catch (Exception ex)
        {
            MessageBox.Show("⚠ Lỗi tải dữ liệu từ API:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return new List<Alert>();
        }
    }
}

// ===================== CLASS MODELS =========================

public class Config
{
    public string BaseUrl { get; set; }
    public string UserToken { get; set; }
}

public class AlertResponse
{
    public List<Alert> items { get; set; }
}

public class Alert
{
    public string agent_id { get; set; }
    public HostInfo host { get; set; }
    public string path { get; set; }
    public string reason { get; set; }
    public List<Rule> rules { get; set; }
    public string timestamp { get; set; }
    public string received_at { get; set; }
}

public class HostInfo
{
    public string hostname { get; set; }
    public List<string> macs { get; set; }
    public List<string> ipv4 { get; set; }
}

public class Rule
{
    public string name { get; set; }
    public Meta meta { get; set; }
}

public class Meta
{
    public double? score { get; set; }
}
