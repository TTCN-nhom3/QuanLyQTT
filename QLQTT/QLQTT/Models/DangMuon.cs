using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class DangMuon
    {
        public string MaSv { get; set; }
        public string MaQtt { get; set; }
        public string MaKc { get; set; }

        public virtual KichCo MaKcNavigation { get; set; }
        public virtual QuanTuTrang MaQttNavigation { get; set; }
        public virtual SinhVien MaSvNavigation { get; set; }

        public DangMuon(string maSv, string maQtt, string maKc)
        {
            MaSv = maSv;
            MaQtt = maQtt;
            MaKc = maKc;
        }
    }
}
