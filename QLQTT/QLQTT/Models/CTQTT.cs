using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQTT.Models
{
    class CTQTT
    {
        public QuanTuTrang qtt { get; set; }
        public KichCo kc { get; set; }
        public int SoLuongCt { get; set; }

        public CTQTT(QuanTuTrang qtt, KichCo kc, int soLuongCt)
        {
            this.qtt = qtt;
            this.kc = kc;
            SoLuongCt = soLuongCt;
        }
    }
}
