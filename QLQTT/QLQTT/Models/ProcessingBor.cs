using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQTT.Models
{
    internal class ProcessingBor
    {
        public string MaSv { get; set; }

        public string MaQtt { get; set; }
        public string TenQtt { get; set; }
        public string TenKc { get; set; }
        public DateTime NgayDk { get; set; }
        public string Loai { get; set; }
        public ProcessingBor() { }

        public ProcessingBor(string masv, string maqtt, string tenQtt, string tenKc, DateTime ngayDk, string loai)
        {
            MaSv = masv;
            MaQtt = maqtt;
            TenQtt = tenQtt;
            TenKc = tenKc;
            NgayDk = ngayDk;
            Loai = loai;
        }
    }
}
