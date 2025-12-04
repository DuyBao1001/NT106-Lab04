using System;
using System.Windows.Forms;

namespace Bai4
{
    public partial class BookingForm : Form
    {
        private Movie _movie;

        public BookingForm(Movie movie)
        {
            InitializeComponent();
            _movie = movie;

            lblMovieTitle.Text = "Phim: " + _movie.Title;

            txtPrice.Text = "";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //Lấy dữ liệu và loại bỏ khoảng trắng thừa
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string priceString = txtPrice.Text.Trim();

            //Kiểm tra nhập trống tên và sđt
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên và SĐT khách hàng!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!phone.StartsWith("0") || phone.Length != 10 || !long.TryParse(phone, out _))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(priceString, out decimal ticketPrice) || ticketPrice < 0)
            {
                MessageBox.Show("Giá vé không hợp lệ!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Tính tiền 
            int quantity = (int)nudQty.Value;
            decimal totalAmount = quantity * ticketPrice;

            string message =
                $"    ĐẶT VÉ THÀNH CÔNG    \n\n" +
                $"Phim: {_movie.Title}\n" +
                $"Khách hàng: {name}\n" +
                $"SĐT: {phone}\n" +
                $"Giá vé: {ticketPrice:N0} VNĐ/vé\n" +
                $"Số lượng: {quantity}\n" +
                $"---------------------------\n" +
                $"TỔNG TIỀN: {totalAmount:N0} VNĐ";

            MessageBox.Show(message, "Hệ thống quản lý vé", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}