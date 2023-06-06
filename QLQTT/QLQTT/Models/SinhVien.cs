using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class SinhVien
    {
        public SinhVien()
        {
            CongNo = new HashSet<CongNo>();
            DangMuon = new HashSet<DangMuon>();
            Doi = new HashSet<Doi>();
            HoaDonThanhToan = new HashSet<HoaDonThanhToan>();
            Mat = new HashSet<Mat>();
            Muon = new HashSet<Muon>();
        }

        public string MaSv { get; set; }
        public string TenSv { get; set; }
        public byte[] Anh { get; set; }
        public string MatKhau { get; set; }
        public string MaKh { get; set; }
        public string MoTa { get; set; }

        public virtual KhoaHoc MaKhNavigation { get; set; }
        public virtual ICollection<CongNo> CongNo { get; set; }
        public virtual ICollection<DangMuon> DangMuon { get; set; }
        public virtual ICollection<Doi> Doi { get; set; }
        public virtual ICollection<HoaDonThanhToan> HoaDonThanhToan { get; set; }
        public virtual ICollection<Mat> Mat { get; set; }
        public virtual ICollection<Muon> Muon { get; set; }
    }
}
