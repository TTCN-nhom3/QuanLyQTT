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
        }
        //Hiển thị danh sách các Chi tiết Quân tư trang
        private void dtgQTTDetail_Load(object sender, RoutedEventArgs e)
        {
            var listCTQTT = from c in db.ChiTietQtt
                            join q in db.QuanTuTrang on c.MaQtt equals q.MaQtt
                            join k in db.KichCo on c.MaKc equals k.MaKc
                            select new
                            {
                                c,
                                qtt = q.TenQtt,
                                kc = k.TenKc,
                            };
            dtgQTTDetail.ItemsSource = listCTQTT.ToList();
        }
        // Nhấn nút "Thêm mới"
        private void btnAddQTTDetail_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateQTTDetail addAndUpdateQTTDetail = new AddAndUpdateQTTDetail(
                "Thêm mới Chi tiết Quân tư trang");
            this.Hide();
            addAndUpdateQTTDetail.ShowDialog();
            dtgQTTDetail_Load(sender, e);
            this.ShowDialog();
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
            this.Hide();
            addAndUpdateQTTDetail.ShowDialog();
            dtgQTTDetail_Load(sender, e);
            this.ShowDialog();
        }
        // Nhấn nút "Thoát"
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
