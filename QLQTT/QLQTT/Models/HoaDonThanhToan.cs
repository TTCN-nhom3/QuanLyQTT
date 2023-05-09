using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class HoaDonThanhToan
    {
        public string MaHdtt { get; set; }
        public decimal? SoTien { get; set; }
        public DateTime NgayTra { get; set; }
        public string MoTa { get; set; }
        public string MaSv { get; set; }

        public virtual SinhVien MaSvNavigation { get; set; }
    }
}
