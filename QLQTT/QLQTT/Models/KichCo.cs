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
            Doi = new HashSet<Doi>();
            Mat = new HashSet<Mat>();
            Muon = new HashSet<Muon>();
        }

        public string MaKc { get; set; }
        public string TenKc { get; set; }
        public string MoTa { get; set; }

        public virtual ICollection<ChiTietQtt> ChiTietQtt { get; set; }
        public virtual ICollection<DangMuon> DangMuon { get; set; }
        public virtual ICollection<Doi> Doi { get; set; }
        public virtual ICollection<Mat> Mat { get; set; }
        public virtual ICollection<Muon> Muon { get; set; }
    }
}
