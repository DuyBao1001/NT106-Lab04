using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace Bai4
{
    public partial class FilmDetail : Form
    {
        private Movie _movie;

        public FilmDetail(Movie movie)
        {
            InitializeComponent();

            _movie = movie;

            this.Text = "Chi tiết phim: " + _movie.Title;
            lblTitle.Text = _movie.Title.ToUpper();

            //Bắt đầu tải trang web
            LoadWebPage();
        }

        private async void LoadWebPage()
        {
            try
            {
                await webView.EnsureCoreWebView2Async(null);

                // Điều hướng đến link chi tiết phim
                if (!string.IsNullOrEmpty(_movie.DetailUrl))
                {
                    webView.CoreWebView2.Navigate(_movie.DetailUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải trình duyệt: " + ex.Message + "\nĐảm bảo đã cài NuGet 'WebView2'");
            }
        }

        private void btnBookTicket_Click(object sender, EventArgs e)
        {
            BookingForm bookingForm = new BookingForm(_movie);
            bookingForm.ShowDialog();
        }
    }
}