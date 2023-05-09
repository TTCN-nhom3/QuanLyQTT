﻿using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class KhoanNo
    {
        public string MaKn { get; set; }
        public decimal? TienNo { get; set; }
        public DateTime HanTra { get; set; }
        public string MoTa { get; set; }
        public string MaSv { get; set; }

        public virtual SinhVien MaSvNavigation { get; set; }
    }
}
