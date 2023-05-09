using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QLQTT.Models;
using System.IO;

namespace QLQTT
{
    /// <summary>
    /// Interaction logic for MainStudent.xaml
    /// </summary>
    public partial class MainStudent : Window
    {
        QLQTTContext db = new QLQTTContext();
        SinhVien sv = new SinhVien();
        public BitmapFrame convert(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return BitmapFrame.Create(ms, BitmapCreateOptions.None,
                    BitmapCacheOption.OnLoad);
            }
        }
        public MainStudent(string id)
        {
            InitializeComponent();
            IEnumerable<SinhVien> q = db.SinhVien.Where(e => e.MaSv.Equals(id));
            this.sv = q.First();
        }

        //public void showDK()
        //{

        //}

        //public void showDS(string id)
        //{
        //    var list = from d in db.DangMuon
        //               join q in db.QuanTuTrang on d.MaQtt equals q.MaQtt
        //               join k in db.KichCo on d.MaKc equals k.MaKc
        //               where d.MaSv == id
        //               let ttm = (d.TrangThaiMat) ? "Đang mất" : "Đang mượn"
        //               select new
        //               {
        //                   q.MaQtt,
        //                   q.TenQtt,
        //                   k.MaKc,
        //                   k.TenKc,
        //                   ttm
        //               };
        //    dtgDS.ItemsSource = list.ToList();
        //}

        //public void showHD(string id)
        //{
        //    var hd = from m in db.HoaDonMuon
        //             join q in db.QuanTuTrang on m.MaQtt equals q.MaQtt
        //             join k in db.KichCo on m.MaKc equals k.MaKc
        //             where m.MaSv == id
        //             select new
        //             {
        //                 GiaoDich = "Mượn",
        //                 q.MaQtt,
        //                 q.TenQtt,
        //                 k.MaKc,
        //                 k.TenKc,
        //                 NgayGiaoDich = m.NgayMuon
        //             };
        //    var _hd = from d in db.HoaDonDoi
        //              join q in db.QuanTuTrang on d.MaQtt equals q.MaQtt
        //              join k in db.KichCo on d.MaKc equals k.MaKc
        //              where d.MaSv == id
        //              select new
        //              {
        //                  GiaoDich = "Đổi",
        //                  q.MaQtt,
        //                  q.TenQtt,
        //                  k.MaKc,
        //                  k.TenKc,
        //                  NgayGiaoDich = d.NgayDoi
        //              };
        //    var list = hd.ToList();
        //    foreach (var i in _hd)
        //    {
        //        list.Add(i);
        //    }
        //    dtgHD.ItemsSource = list;
        //}

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var logIn = new LogIn();
            logIn.Show();
            this.Close();
        }

        private void dtgDS_Loaded(object sender, RoutedEventArgs e)
        {
            var list = from d in db.DangMuon
                       join q in db.QuanTuTrang on d.MaQtt equals q.MaQtt
                       join k in db.KichCo on d.MaKc equals k.MaKc
                       where d.MaSv == sv.MaSv
                       let ttm = (d.TrangThaiMat) ? "Đang mất" : "Đang mượn"
                       select new
                       {
                           q.MaQtt,
                           q.TenQtt,
                           k.MaKc,
                           k.TenKc,
                           ttm
                       };
            dtgDS.ItemsSource = list.ToList();
        }

        private void dtgHD_Loaded(object sender, RoutedEventArgs e)
        {
            var hd = from m in db.HoaDonMuon
                     join q in db.QuanTuTrang on m.MaQtt equals q.MaQtt
                     join k in db.KichCo on m.MaKc equals k.MaKc
                     where m.MaSv == sv.MaSv
                     select new
                     {
                         GiaoDich = "Mượn",
                         q.MaQtt,
                         q.TenQtt,
                         k.MaKc,
                         k.TenKc,
                         NgayGiaoDich = m.NgayMuon
                     };
            var _hd = from d in db.HoaDonDoi
                      join q in db.QuanTuTrang on d.MaQtt equals q.MaQtt
                      join k in db.KichCo on d.MaKc equals k.MaKc
                      where d.MaSv == sv.MaSv
                      select new
                      {
                          GiaoDich = "Đổi",
                          q.MaQtt,
                          q.TenQtt,
                          k.MaKc,
                          k.TenKc,
                          NgayGiaoDich = d.NgayDoi
                      };
            var list = hd.ToList();
            foreach (var i in _hd)
            {
                list.Add(i);
            }
            dtgHD.ItemsSource = list;
        }

        private void MainStudent_Loaded(object sender, RoutedEventArgs e)
        {
            lblId.Content = sv.MaSv;
            lblName.Content = sv.TenSv;
            byte[] byteArray = sv.Anh.ToArray();
            img.Source = convert(byteArray);
            IEnumerable<KhoanNo> m = db.KhoanNo.Where(ele => ele.MaSv.Equals(sv.MaSv));
            if (m.Count() == 0)
            {
                lblMoney.Content = "Nợ: 0 đồng";
            }
            else
            {
                string str = m.First().TienNo.ToString();
                lblMoney.Content = "Nợ: " + str.Substring(0, str.Length - 1) + " đồng";
            }
        }

        private void lvwDK_Loaded(object sender, RoutedEventArgs e)
        {
            var kc = from k in db.KichCo
                     select k;
            List<KichCo> listKc = new List<KichCo>();
            foreach (KichCo i in kc)
            {
                if (!i.MoTa.Equals("all"))
                    listKc.Add(i);
            }
            lvwDK.ItemsSource = listKc;
        }

        private void cbbKC_Loaded(object sender, RoutedEventArgs e)
        {
            var kc = from k in db.KichCo
                     select k;
            List<KichCo> listKc = new List<KichCo>();
            foreach (KichCo i in kc)
            {
                if (!i.MoTa.Equals("all"))
                    listKc.Add(i);
            }
            cbbKC.ItemsSource = listKc;
            cbbKC.SelectedIndex = 0;
            cbbKC.DisplayMemberPath = "TenKc";
            cbbKC.SelectedValuePath = "MaKc";
        }

        //private void dtgDK_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var kc = from k in db.KichCo
        //             select k;
        //    List<KichCo> listKc = new List<KichCo>();
        //    foreach (KichCo i in kc)
        //    {
        //        if (!i.MoTa.Equals("all"))
        //            listKc.Add(i);
        //    }
        //    dtgDK.ItemsSource = listKc;
        //}
    }
}
