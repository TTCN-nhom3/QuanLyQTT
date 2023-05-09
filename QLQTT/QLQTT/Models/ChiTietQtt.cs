using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class ChiTietQtt
    {
        public string MaQtt { get; set; }
        public string MaKc { get; set; }
        public int SoLuongCt { get; set; }

        public virtual KichCo MaKcNavigation { get; set; }
        public virtual QuanTuTrang MaQttNavigation { get; set; }
    }
}
