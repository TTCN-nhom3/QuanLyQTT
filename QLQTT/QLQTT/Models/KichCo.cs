using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class KichCo
    {
        public KichCo()
        {
            ChiTietQtt = new HashSet<ChiTietQtt>();
            DangMuon = new HashSet<DangMuon>();
            HoaDonDoi = new HashSet<HoaDonDoi>();
            HoaDonMuon = new HashSet<HoaDonMuon>();
        }

        public string MaKc { get; set; }
        public string TenKc { get; set; }
        public string MoTa { get; set; }

        public virtual ICollection<ChiTietQtt> ChiTietQtt { get; set; }
        public virtual ICollection<DangMuon> DangMuon { get; set; }
        public virtual ICollection<HoaDonDoi> HoaDonDoi { get; set; }
        public virtual ICollection<HoaDonMuon> HoaDonMuon { get; set; }
    }
}
