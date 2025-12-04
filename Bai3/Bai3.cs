using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Bai3
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeWebView();
        }

        async void InitializeWebView()
        {
            await webView.EnsureCoreWebView2Async(null);
        }

        //Load Website
        private void btnLoad_Click(object sender, EventArgs e)
        {
            string url = txtURL.Text.Trim();
            if (string.IsNullOrEmpty(url)) return;

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
                txtURL.Text = url;
            }

            try
            {
                if (webView != null && webView.CoreWebView2 != null)
                {
                    webView.CoreWebView2.Navigate(url);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi điều hướng: " + ex.Message);
            }
        }

        //Reload
        private void btnReload_Click(object sender, EventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
                webView.Reload();
        }

        //Download File HTML
        private void btnDownHTML_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtURL.Text;
                using (WebClient client = new WebClient())
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "HTML File|*.html";
                    saveFileDialog.FileName = "index.html";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        client.DownloadFile(url, saveFileDialog.FileName);
                        MessageBox.Show("Download HTML thành công!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi download: " + ex.Message);
            }
        }

        //Download Resources
        private void btnDownResources_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtURL.Text;
                string folderPath = "DownloadedImages"; // Thư mục lưu ảnh (trong bin/Debug)

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // tải HTML 
                HtmlWeb web = new HtmlWeb();
                var document = web.Load(url);

                // Lấy tất cả file có dạng <img>
                var imageNodes = document.DocumentNode.SelectNodes("//img");

                if (imageNodes == null)
                {
                    MessageBox.Show("Không tìm thấy hình ảnh nào.");
                    return;
                }

                int count = 0;
                using (WebClient client = new WebClient())
                {
                    foreach (var node in imageNodes)
                    {
                        string imgSrc = node.GetAttributeValue("src", "");
                        if (string.IsNullOrEmpty(imgSrc)) continue;

                        // Xử lý đường dẫn
                        Uri baseUri = new Uri(url);
                        Uri fullUri = new Uri(baseUri, imgSrc);

                        // Tạo tên
                        string fileName = Path.GetFileName(fullUri.LocalPath);
                        if (string.IsNullOrWhiteSpace(fileName)) fileName = $"image_{count}.jpg";

                        string savePath = Path.Combine(folderPath, fileName);

                        try
                        {
                            client.DownloadFile(fullUri, savePath);
                            count++;
                        }
                        catch 
                        { 
                            //Bỏ qua lỗi
                        }
                    }
                }
                MessageBox.Show($"Đã tải {count} ảnh vào thư mục: {Path.GetFullPath(folderPath)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi resources: " + ex.Message);
            }
        }

        // View Source
        private void btnViewSource_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtURL.Text;
                string htmlContent;
                using (WebClient client = new WebClient())
                {
                    htmlContent = client.DownloadString(url);
                }

                // Hiển thị source code
                SourceCodeForm frm = new SourceCodeForm(htmlContent);
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi View Source: " + ex.Message);
            }
        }
    }
}
