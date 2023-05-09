﻿using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class SinhVien
    {
        public SinhVien()
        {
            DangMuon = new HashSet<DangMuon>();
            HoaDonDoi = new HashSet<HoaDonDoi>();
            HoaDonMuon = new HashSet<HoaDonMuon>();
            HoaDonThanhToan = new HashSet<HoaDonThanhToan>();
            KhoanNo = new HashSet<KhoanNo>();
        }

        public string MaSv { get; set; }
        public string TenSv { get; set; }
        public byte[] Anh { get; set; }
        public string MatKhau { get; set; }
        public string MaKh { get; set; }
        public string MoTa { get; set; }

        public virtual KhoaHoc MaKhNavigation { get; set; }
        public virtual ICollection<DangMuon> DangMuon { get; set; }
        public virtual ICollection<HoaDonDoi> HoaDonDoi { get; set; }
        public virtual ICollection<HoaDonMuon> HoaDonMuon { get; set; }
        public virtual ICollection<HoaDonThanhToan> HoaDonThanhToan { get; set; }
        public virtual ICollection<KhoanNo> KhoanNo { get; set; }
    }
}
