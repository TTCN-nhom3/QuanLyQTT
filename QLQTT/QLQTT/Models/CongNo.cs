using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class CongNo
    {
        public string MaCn { get; set; }
        public string MaSv { get; set; }
        public decimal SoTien { get; set; }
        public DateTime HanTra { get; set; }
        public string MoTa { get; set; }

        public virtual SinhVien MaSvNavigation { get; set; }
    }
}
