using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class HoaDonMuon
    {
        public string MaHdm { get; set; }
        public DateTime NgayTaoHd { get; set; }
        public DateTime? NgayMuon { get; set; }
        public string TrangThai { get; set; }
        public string MoTa { get; set; }
        public string MaSv { get; set; }
        public string MaQtt { get; set; }
        public string MaKc { get; set; }

        public virtual KichCo MaKcNavigation { get; set; }
        public virtual QuanTuTrang MaQttNavigation { get; set; }
        public virtual SinhVien MaSvNavigation { get; set; }
    }
}
