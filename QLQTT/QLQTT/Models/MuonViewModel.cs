using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQTT.Models
{
    public class MuonViewModel
    {
        public string MaMuon { get; set; }
        public string MaSv { get; set; }
        public string TenQtt { get; set; }
        public string TenKC { get; set; }
        public DateTime NgayDk { get; set; }
        public DateTime? NgayMuon { get; set; }
        public string TrangThai { get; set; }
        public string MoTa { get; set; }

        public MuonViewModel()
        {

        }
        public MuonViewModel(string maMuon, string maSv, string tenQtt, string tenKC, DateTime ngayDk, DateTime? ngayMuon, string trangThai, string moTa)
        {
            MaMuon = maMuon;
            MaSv = maSv;
            TenQtt = tenQtt;
            TenKC = tenKC;
            NgayDk = ngayDk;
            NgayMuon = ngayMuon;
            TrangThai = trangThai;
            MoTa = moTa;
        }
    }

}
