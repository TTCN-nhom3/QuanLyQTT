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
using System.Reflection;
using QLQTT.Models;

namespace QLQTT
{
    /// <summary>
    /// Form bảo trì Chi tiết Quân tư trang
    /// </summary>
    public partial class MainTainQTTDetail : Window
    {
        QLQTTContext db = new QLQTTContext();
        public MainTainQTTDetail()
        {
            InitializeComponent();
            lblExitWarning.Content = "Nếu bạn nhất nút 'X' màu đỏ, ứng dụng sẽ tắt ngay lập tức!";
        }
        //Hiển thị danh sách các Chi tiết Quân tư trang
        private void dtgQTTDetail_Load(object sender, RoutedEventArgs e)
        {
            var listCTQTT = from c in db.ChiTietQtt
                            join q in db.QuanTuTrang on c.MaQtt equals q.MaQtt
                            join k in db.KichCo on c.MaKc equals k.MaKc
                            select new
                            {
                                MaQtt = c.MaQtt,
                                MaKc = c.MaKc,
                                qtt = q.TenQtt,
                                kc = k.TenKc,
                                c.SoLuongCt
                            };
            dtgQTTDetail.ItemsSource = listCTQTT.ToList();
        }
        // Nhấn nút "Thêm mới"
        private void btnAddQTTDetail_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateQTTDetail addAndUpdateQTTDetail = new AddAndUpdateQTTDetail(
                "Thêm mới Chi tiết Quân tư trang");
            this.Close();
            addAndUpdateQTTDetail.Show();
        }
        // Nhấn nút "Xóa"
        private void btnDeleteQTTDetail_Click(object sender, RoutedEventArgs e)
        {
            Type t = dtgQTTDetail.SelectedItem.GetType();
            PropertyInfo[] p = t.GetProperties();
            ChiTietQtt ct = (ChiTietQtt)p[0].GetValue(dtgQTTDetail.SelectedValue);
            string q = p[1].GetValue(dtgQTTDetail.SelectedValue).ToString();
            string k = p[2].GetValue(dtgQTTDetail.SelectedValue).ToString();
            var muon = db.Muon.SingleOrDefault(m => m.MaQtt.Equals(ct.MaQtt) && m.MaKc.Equals(ct.MaKc));
            var doi = db.Doi.SingleOrDefault(d => d.MaQtt.Equals(ct.MaQtt) && d.MaKc.Equals(ct.MaKc));
            var mat = db.Mat.SingleOrDefault(m => m.MaQtt.Equals(ct.MaQtt) && m.MaKc.Equals(ct.MaKc));
            var dangmuon = db.DangMuon.SingleOrDefault(d => d.MaQtt.Equals(ct.MaQtt) && d.MaKc.Equals(ct.MaKc));
            if (muon != null || doi != null || mat != null || dangmuon != null)
            {
                MessageBox.Show("Không thể loại bỏ Kích cỡ '" + k + "' đối với Quân tư trang '"
                    + q + "'!\nHãy đọc kĩ hướng dẫn.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn loại bỏ Kích cỡ '" + k +
                    "' đối với Quân tư trang '" + q + "'?", "Xác nhận",
                    MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    db.ChiTietQtt.Remove(ct);
                    var qtt = db.QuanTuTrang.SingleOrDefault(qt => qt.MaQtt.Equals(ct.MaQtt));
                    qtt.SoLuong -= ct.SoLuongCt;
                    db.SaveChanges();
                    MessageBox.Show("Loại bỏ Kích cỡ '" + k + "' đối với Quân tư trang '"
                        + q + "' thành công!", "Thông báo");
                }
            }
            dtgQTTDetail_Load(sender, e);
        }
        // Nhấn nút "Cập nhật"
        private void btnUpdateQTTDetail_Click(object sender, RoutedEventArgs e)
        {
            ChiTietQtt c = (ChiTietQtt)dtgQTTDetail.SelectedItem;
            AddAndUpdateQTTDetail addAndUpdateQTTDetail = new AddAndUpdateQTTDetail(
                "Cập nhật thông tin Chi tiết Quân tư trang", c.MaQtt, c.MaKc, c.SoLuongCt);
            this.Close();
            addAndUpdateQTTDetail.Show();
        }
        // Nhấn nút "Thoát"
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainAdmin mainAdmin = new MainAdmin();
            this.Close();
            mainAdmin.Show();
        }
        // Nhấn nút "Tìm kiếm"
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            bool q = false, k = false;
            List<CTQTT> lQTT = new List<CTQTT>();
            List<CTQTT> lKC = new List<CTQTT>();
            if (txtTenQTT.Text != "" || txtTenQTT.Text != null)
            {
                var l = from c in db.ChiTietQtt
                        join qtt in db.QuanTuTrang on c.MaQtt equals qtt.MaQtt
                        join kc in db.KichCo on c.MaKc equals kc.MaKc
                        where qtt.TenQtt.Contains(txtTenQTT.Text)
                        select new
                        {
                            qtt,
                            kc,
                            c.SoLuongCt
                        };
                foreach (var i in l)
                {
                    lQTT.Add(new CTQTT(i.qtt, i.kc, i.SoLuongCt));
                }
                q = true;
            }
            if (txtTenKC.Text != "" || txtTenKC.Text != null)
            {
                var l = from c in db.ChiTietQtt
                        join qtt in db.QuanTuTrang on c.MaQtt equals qtt.MaQtt
                        join kc in db.KichCo on c.MaKc equals kc.MaKc
                        where kc.TenKc.Contains(txtTenKC.Text)
                        select new
                        {
                            qtt,
                            kc,
                            c.SoLuongCt
                        };
                foreach (var i in l)
                {
                    lKC.Add(new CTQTT(i.qtt, i.kc, i.SoLuongCt));
                }
                k = true;
            }
            if (q && k)
            {
                var l = lQTT.Select(x => new
                {
                    x.qtt.MaQtt,
                    qtt = x.qtt.TenQtt,
                    x.kc.MaKc,
                    kc = x.kc.TenKc,
                    x.SoLuongCt

                }).Intersect(lKC.Select(x => new
                {
                    x.qtt.MaQtt,
                    qtt = x.qtt.TenQtt,
                    x.kc.MaKc,
                    kc = x.kc.TenKc,
                    x.SoLuongCt
                }));
                dtgQTTDetail.ItemsSource = l.ToList();
            }
            else if (q)
            {
                dtgQTTDetail.ItemsSource = lQTT;
            }
            else if (k)
            {
                dtgQTTDetail.ItemsSource = lKC;
            }
            else
            {
                dtgQTTDetail_Load(sender, e);
            }
        }
        // Nhấn nút "Tải lại"
        private void btnCancelApply_Click(object sender, RoutedEventArgs e)
        {
            txtTenQTT.Text = "";
            txtTenKC.Text = "";
            dtgQTTDetail_Load(sender, e);
        }
    }
}
