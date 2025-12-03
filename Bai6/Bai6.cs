using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Bai6 
{
    public partial class Bai6 : Form
    {
        public Bai6()
        {
            InitializeComponent();
        }

        private async void btnGetInfo_Click(object sender, EventArgs e)
        {
            string url = tbUrl.Text;
            string fullToken = tbToken.Text.Trim();

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(fullToken))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ URL và Token xác thực (từ Bài 5).", "Lỗi");
                return;
            }
            string tokenScheme = "Bearer";
            string accessToken;

            if (fullToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = fullToken.Substring("Bearer ".Length).Trim();
            }
            else
            {
                accessToken = fullToken;
            }

            using (var client = new HttpClient())
            {
                try
                {
                    // 1. Thêm Header Authorization 
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(tokenScheme, accessToken);

                    // 2. Thực hiện HTTP GET
                    rtbResult.Text = "Đang gửi yêu cầu GET thông tin người dùng...";
                    HttpResponseMessage response = await client.GetAsync(url);

                    // 3. Đọc phản hồi
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        // Lấy thông tin User và định dạng JSON
                        JToken userObject = JToken.Parse(responseString);

                        rtbResult.Text =
                            "Lấy thông tin thành công (HTTP 200 OK):\n\n" +
                            // Dùng định dạng JSON có indent để dễ đọc
                            userObject.ToString(Newtonsoft.Json.Formatting.Indented);
                    }
                    else
                    {
                        // Lỗi xác thực hoặc lỗi khác
                        rtbResult.Text =
                            "Lấy thông tin thất bại (Mã: " + (int)response.StatusCode + ")\n\n" +
                            "Chi tiết lỗi:\n" + responseString;
                    }
                }
                catch (Exception ex)
                {
                    rtbResult.Text = "Lỗi không xác định: " + ex.Message;
                }
            }
        }
    }
}