using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class QuanTuTrang
    {
        public QuanTuTrang()
        {
            ChiTietQtt = new HashSet<ChiTietQtt>();
            DangMuon = new HashSet<DangMuon>();
            HoaDonDoi = new HashSet<HoaDonDoi>();
            HoaDonMuon = new HashSet<HoaDonMuon>();
        }

        public string MaQtt { get; set; }
        public string TenQtt { get; set; }
        public decimal GiaTien { get; set; }
        public int SoLuong { get; set; }
        public string MoTa { get; set; }

        public virtual ICollection<ChiTietQtt> ChiTietQtt { get; set; }
        public virtual ICollection<DangMuon> DangMuon { get; set; }
        public virtual ICollection<HoaDonDoi> HoaDonDoi { get; set; }
        public virtual ICollection<HoaDonMuon> HoaDonMuon { get; set; }
    }
}
