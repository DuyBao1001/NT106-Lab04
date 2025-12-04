using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai4
{
    public class Movie // Nhớ thêm từ khóa public
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string DetailUrl { get; set; } // Sửa LinkURL thành DetailUrl để khớp với ListFilm.cs

        //Các thông tin chi tiết
        public string Director { get; set; }
        public string Actor { get; set; }
        public string Genre { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
    }
}