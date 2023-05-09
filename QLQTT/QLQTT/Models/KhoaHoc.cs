using System;
using System.Collections.Generic;

namespace QLQTT.Models
{
    public partial class KhoaHoc
    {
        public KhoaHoc()
        {
            SinhVien = new HashSet<SinhVien>();
        }

        public string MaKh { get; set; }
        public DateTime NgayBd { get; set; }
        public DateTime NgayKt { get; set; }
        public string MoTa { get; set; }

        public virtual ICollection<SinhVien> SinhVien { get; set; }
    }
}
