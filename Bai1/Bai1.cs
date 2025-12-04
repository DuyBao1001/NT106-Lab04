using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Bai1
{
    public partial class Bai1 : Form
    {
        public Bai1()
        {
            InitializeComponent();
        }


        private string getHTML(string szUrl)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = "Lỗi: Không thể tải nội dung.";

            try
            {
                request = WebRequest.Create(szUrl);

                response = request.GetResponse();

                dataStream = response.GetResponseStream();

                reader = new StreamReader(dataStream, System.Text.Encoding.UTF8);

                responseFromServer = reader.ReadToEnd();
            }
            catch (WebException ex)
            {

                responseFromServer = $"Lỗi WebRequest: {ex.Message}\nURL: {szUrl}";
            }
            catch (Exception ex)
            {
                responseFromServer = $"Lỗi hệ thống: {ex.Message}";
            }
            finally
            {
                if (reader != null) reader.Close();
                if (dataStream != null) dataStream.Close();
                if (response != null) response.Close();
            }

            return responseFromServer;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                rtxtHtmlContent.Text = "Vui lòng nhập một địa chỉ URL.";
                return;
            }
            rtxtHtmlContent.Text = getHTML(url);
        }

        private void rtxtHtmlContent_TextChanged(object sender, EventArgs e)
        {

        }

        private void Bai1_Load(object sender, EventArgs e)
        {

        }
    }
}