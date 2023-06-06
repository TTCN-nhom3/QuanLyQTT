using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class Muon
    {
        public string MaMuon { get; set; }
        public string MaSv { get; set; }
        public string MaQtt { get; set; }
        public string MaKc { get; set; }
        public DateTime NgayDk { get; set; }
        public DateTime? NgayMuon { get; set; }
        public string TrangThai { get; set; }
        public string MoTa { get; set; }

        public virtual KichCo MaKcNavigation { get; set; }
        public virtual QuanTuTrang MaQttNavigation { get; set; }
        public virtual SinhVien MaSvNavigation { get; set; }
        public Muon(string maMuon, string maSv, string maQtt, string maKc, DateTime ngayDk, DateTime? ngayMuon, string moTa)
        {
            MaMuon = maMuon;
            MaSv = maSv;
            MaQtt = maQtt;
            MaKc = maKc;
            NgayDk = ngayDk;
            NgayMuon = ngayMuon;

            MoTa = moTa;

        }
    }
}
