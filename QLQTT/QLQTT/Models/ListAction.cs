using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQTT.Models
{
    class ListAction
    {
        public ListAction(string maGD, string tenGD, string maSV, string qtt,
            string kc, string tt, DateTime ngayDK, DateTime? ngayGD)
        {
            this.maGD = maGD;
            this.tenGD = tenGD;
            this.maSV = maSV;
            this.qtt = qtt;
            this.kc = kc;
            this.tt = tt;
            this.ngayDK = ngayDK;
            this.ngayGD = ngayGD;
        }
        public string maGD { get; set; }
        public string tenGD { get; set; }
        public string maSV { get; set; }
        public string qtt { get; set; }
        public string kc { get; set; }
        public string tt { get; set; }
        public DateTime ngayDK { get; set; }
        public DateTime? ngayGD { get; set; }
    }
}
