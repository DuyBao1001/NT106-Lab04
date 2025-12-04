using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq; 
using System.Net; 

namespace Bai5
{
    public partial class Bai5 : Form
    {
        public Bai5()
        {
            InitializeComponent();
            // Gán sự kiện Click cho nút LOGIN 
            this.button1.Click += new System.EventHandler(this.btnLogin_Click);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            string username = maskedTextBox1.Text;
            string password = maskedTextBox2.Text;
            RichTextBox rtbResult = richTextBox1;

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ URL, Username và Password.", "Lỗi");
                return;
            }

            // Khởi tạo HttpClient
            using (var client = new HttpClient())
            {
                try
                {
                    // 1. Chuẩn bị dữ liệu form-data
                    var content = new MultipartFormDataContent
                    {
                        // Truyền username và password dưới dạng form-data
                        { new StringContent(username), "username" },
                        { new StringContent(password), "password" }
                    };

                    // 2. Thực hiện HTTP POST
                    rtbResult.Text = "Đang gửi yêu cầu đăng nhập tới API...";
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    // 3. Xử lý phản hồi JSON
                    JObject responseObject = JObject.Parse(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        string tokenType = responseObject["token_type"]?.ToString() ?? "N/A";
                        string accessToken = responseObject["access_token"]?.ToString() ?? "Token not found";

                        rtbResult.Text =
                            tokenType + "\n" +
                            accessToken + "\n\n" +
                            "Đăng nhập thành công \n";
                    }
                    else
                    {
                        string detail = responseObject["detail"]?.ToString() ?? response.ReasonPhrase;

                        rtbResult.Text =
                            "Đăng nhập thất bại (Mã: " + (int)response.StatusCode + ")\n\n" +
                            "Chi tiết lỗi (Detail):\n" + detail;
                    }
                }
                catch (HttpRequestException ex)
                {
                    rtbResult.Text = "Lỗi kết nối mạng: Không thể kết nối tới máy chủ.\nChi tiết: " + ex.Message;
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    rtbResult.Text = "Lỗi: Phản hồi từ server không phải định dạng JSON hợp lệ.";
                }
                catch (Exception ex)
                {
                    rtbResult.Text = "Đã xảy ra lỗi không xác định: " + ex.Message;
                }
            }
        }
    }
}