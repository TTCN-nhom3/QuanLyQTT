using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQTT.Models
{
    class SelectedQTT
    {
        public QuanTuTrang qtt { get; set; }
        public bool isChecked { get; set; }
        public SelectedQTT()
        {
        }
        public SelectedQTT(QuanTuTrang qtt, bool isChecked)
        {
            this.qtt = qtt;
            this.isChecked = isChecked;
        }
    }
}
