using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQTT.Models
{
    public class DoiViewModel
    {
        public string MaDoi { get; set; }
        public string MaSv { get; set; }
        public string TenQtt { get; set; }
        public string TenKC { get; set; }
        public DateTime NgayDk { get; set; }
        public DateTime? NgayDoi { get; set; }
        public string TrangThai { get; set; }
        public string MoTa { get; set; }

        public DoiViewModel()
        {

        }
        public DoiViewModel(string maDoi, string maSv, string tenQtt, string tenKC, DateTime ngayDk, DateTime? ngayDoi, string trangThai, string moTa)
        {
            MaDoi = maDoi;
            MaSv = maSv;
            TenQtt = tenQtt;
            TenKC = tenKC;
            NgayDk = ngayDk;
            NgayDoi = ngayDoi;
            TrangThai = trangThai;
            MoTa = moTa;
        }
    }

}
