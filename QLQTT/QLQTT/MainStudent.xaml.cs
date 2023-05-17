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
                       let ttm = d.TrangThai
                       select new
                       {
                           q,
                           k,
                           ttm
                       };
            List<SelectedBorrowedQTT> listqtt = new List<SelectedBorrowedQTT>();
            foreach (var i in list)
            {
                SelectedBorrowedQTT q = new SelectedBorrowedQTT(i.q, false, i.ttm, i.k);
                listqtt.Add(q);
            }
            dtgDS.ItemsSource = listqtt;
        }

        private void dtgHD_Loaded(object sender, RoutedEventArgs e)
        {
            var hdm = from m in db.HoaDonMuon
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
            var hdd = from d in db.HoaDonDoi
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
            var listHD = hdm.ToList();
            foreach (var i in hdd)
            {
                listHD.Add(i);
            }
            dtgHD.ItemsSource = listHD;
        }

        private void MainStudent_Loaded(object sender, RoutedEventArgs e)
        {
            lblId.Content = sv.MaSv;
            lblName.Content = sv.TenSv;
            byte[] byteArray = sv.Anh.ToArray();
            img.Source = convert(byteArray);
            IEnumerable<CongNo> m = db.CongNo.Where(ele => ele.MaSv.Equals(sv.MaSv));
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

        private void btnBM_Click(object sender, RoutedEventArgs e)
        {
            List<SelectedBorrowedQTT> listCheck = new List<SelectedBorrowedQTT>();
            foreach (SelectedBorrowedQTT i in dtgDS.ItemsSource)
            {
                if (i.isChecked)
                {
                    listCheck.Add(i);
                }
            }
            if (listCheck.Count() == 0)
            {
                MessageBox.Show("Bạn chưa chọn quân tư trang nào");
            }
            else
            {
                bool checkLost = true;
                foreach (SelectedBorrowedQTT i in listCheck)
                {
                    if (i.tt.Equals("Đang mất"))
                    {
                        MessageBox.Show(i.qtt.TenQtt+ " đã được báo mất!");
                        checkLost = false;
                        break;
                    }
                }
                if (checkLost)
                {
                    foreach (SelectedBorrowedQTT i in listCheck)
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("Bạn có chắc chắn muốn báo mất " +
                                                i.qtt.TenQtt + "?", "Confirmation Box", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            var q = db.DangMuon.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv)
                                && t.MaQtt.Equals(i.qtt.MaQtt));
                            q.TrangThai = "Đang mất";
                            var hdm = db.HoaDonMuon.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv)
                                                        && t.MaQtt.Equals(i.qtt.MaQtt) && !(t.TrangThai.Equals("Đã mất")
                                                    || t.TrangThai.Equals("Đã đổi")));
                            if (hdm != null)
                            {
                                hdm.TrangThai = "Đã mất";
                            }
                            else
                            {
                                var hdd = db.HoaDonDoi.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv)
                                                        && t.MaQtt.Equals(i.qtt.MaQtt) && !(t.TrangThai.Equals("Đã mất")
                                    || t.TrangThai.Equals("Đã đổi")));
                                hdd.TrangThai = "Đã mất";
                            }
                            var kn = db.CongNo.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv));
                            if (kn == null)
                            {
                                // Tạo khoản nợ mới
                                CongNo cn = new CongNo();
                                // cn.MaCn = newKN(); (Tạo mã mới)
                                cn.MaSv = sv.MaSv;
                                cn.TienNo = i.qtt.GiaTien;
                                // kn.HanTra = ; (Dựa vào thời gian kết thúc khóa học)
                                db.CongNo.Add(cn);
                            }
                            else
                            {
                                kn.TienNo += i.qtt.GiaTien;
                            }
                            db.SaveChanges();
                        }
                    }
                    dtgDS_Loaded(sender, e);
                }

            }
        }

        private void btnDoi_Click(object sender, RoutedEventArgs e)
        {
            List<SelectedBorrowedQTT> listCheck = new List<SelectedBorrowedQTT>();
            foreach (SelectedBorrowedQTT i in dtgDS.ItemsSource)
            {
                if (i.isChecked)
                {
                    listCheck.Add(i);
                }
            }
            if (listCheck.Count() == 0)
            {
                MessageBox.Show("Bạn chưa chọn quân tư trang nào");
            }
            else
            {
                bool checkLost = true;
                foreach (SelectedBorrowedQTT i in listCheck)
                {
                    if (i.tt.Equals("Đang mất"))
                    {
                        MessageBox.Show(i.qtt.TenQtt + " đã được báo mất!");
                        checkLost = false;
                        break;
                    }
                    if (i.tt.Equals("Chờ đổi"))
                    {
                        MessageBox.Show(i.qtt.TenQtt + " đang được chờ đổi!");
                        checkLost = false;
                        break;
                    }
                }
                if (checkLost)
                {
                    foreach (SelectedBorrowedQTT i in listCheck)
                    {
                        string id = i.kc.MaKc;
                        if (!i.kc.TenKc.Equals("none"))
                        {
                            bool check = false;
                            do
                            {
                                MessageBoxResult messageBoxResult1 = MessageBox.Show("Bạn có muốn chọn kích cỡ khác cho " +
                                            i.qtt.TenQtt + "?", "Confirmation Box", MessageBoxButton.YesNo);
                                if (messageBoxResult1 == MessageBoxResult.Yes)
                                {
                                    // Cửa sổ chọn kích cỡ
                                    // Nếu hủy việc chọn kích cỡ thì check = true
                                    // Nếu chọn được kích cỡ thì truyền MaKc vào id
                                }
                            } while (check);
                        }
                        MessageBoxResult messageBoxResult2 = MessageBox.Show("Bạn có chắc chắn muốn đổi " +
                            i.qtt.TenQtt + "?", "Confirmation Box", MessageBoxButton.YesNo);
                        if (messageBoxResult2 == MessageBoxResult.Yes)
                        {
                            var q = db.DangMuon.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv)
                                    && t.MaQtt.Equals(i.qtt.MaQtt));
                            q.TrangThai = "Chờ đổi";
                            var hdm = db.HoaDonMuon.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv)
                                && t.MaQtt.Equals(i.qtt.MaQtt) && !(t.TrangThai.Equals("Đã mất")
                                || t.TrangThai.Equals("Đã đổi")));
                            if (hdm != null)
                            {
                                hdm.TrangThai = "Đã đổi";
                            }
                            else
                            {
                                var hdd = db.HoaDonDoi.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv)
                                    && t.MaQtt.Equals(i.qtt.MaQtt) && !(t.TrangThai.Equals("Đã mất")
                                    || t.TrangThai.Equals("Đã đổi")));
                                hdd.TrangThai = "Đã đổi";
                            }
                            HoaDonDoi hd = new HoaDonDoi();
                            // hd.MaHdd = newHDD(); (Tạo mã mới)
                            hd.MaSv = sv.MaSv;
                            hd.MaQtt = i.qtt.MaQtt;
                            hd.MaKc = id;
                            // Truyền vào các thuộc tính khác
                            db.HoaDonDoi.Add(hd);
                            db.SaveChanges();
                        }
                    }
                    dtgDS_Loaded(sender, e);
                }
            }
        }
    }
}
