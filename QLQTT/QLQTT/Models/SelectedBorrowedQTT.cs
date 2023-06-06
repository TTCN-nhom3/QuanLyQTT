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
        public bool changing { get; set; }
        public SelectedBorrowedQTT(QuanTuTrang qtt, bool isChecked, KichCo kc, bool changing) : base(qtt, isChecked)
        {
            this.kc = kc;
            this.changing = changing;
        }
    }
}
