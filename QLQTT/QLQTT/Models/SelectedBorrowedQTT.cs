using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQTT.Models
{
    class SelectedBorrowedQTT : SelectedQTT
    {
        public KichCo kc { get; set; }
        public string tt { get; set; }
        public SelectedBorrowedQTT(QuanTuTrang qtt, bool isChecked, string tt, KichCo kc) : base(qtt, isChecked)
        {
            this.tt = tt;
            this.kc = kc;
        }
    }
}
