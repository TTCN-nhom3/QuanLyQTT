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
    /// Form bảo trì Quân tư trang
    /// </summary>
    public partial class MaintainQTT : Window
    {
        QLQTTContext db = new QLQTTContext();
        public MaintainQTT()
        {
            InitializeComponent();
            lblExitWarning.Content = "Nếu bạn nhất nút 'X' màu đỏ, ứng dụng sẽ tắt ngay lập tức!";
        }
        // Sinh mã Quân tư trang mới
        public string newQTT()
        {
            string id = db.QuanTuTrang.Max(c => c.MaQtt);
            if (id == null)
            {
                return "QTT0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "QTT" + new_id.ToString().PadLeft(7, '0');
        }
        //Hiển thị danh sách các Quân tư trang
        private void dtgQTT_Load(object sender, RoutedEventArgs e)
        {
            var listQTT = db.QuanTuTrang.Select(q => q);
            dtgQTT.ItemsSource = listQTT.ToList();
        }
        // Nhấn nút "Thêm mới"
        private void btnAddQTT_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateQTT addAndUpdateQTT = new AddAndUpdateQTT(
                "Thêm mới Quân tư trang", newQTT());
            this.Close();
            addAndUpdateQTT.Show();
        }
        // Nhấn nút "Xóa"
        private void btnDeleteQTT_Click(object sender, RoutedEventArgs e)
        {
            QuanTuTrang q = (QuanTuTrang)dtgQTT.SelectedItem;
            var muon = db.Muon.SingleOrDefault(m => m.MaQtt.Equals(q.MaQtt));
            var doi = db.Doi.SingleOrDefault(d => d.MaQtt.Equals(q.MaQtt));
            var mat = db.Mat.SingleOrDefault(m => m.MaQtt.Equals(q.MaQtt));
            var dangmuon = db.DangMuon.SingleOrDefault(d => d.MaQtt.Equals(q.MaQtt));
            if (muon != null || doi != null || mat != null || dangmuon != null)
            {
                MessageBox.Show("Không thể xóa Quân tư trang '" + q.MaQtt
                    + "'!\nHãy đọc kĩ hướng dẫn", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa Quân tư trang '" + q.MaQtt
                    + "'?", "Xác nhận", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var ctqtt = db.ChiTietQtt.Where(c => c.MaQtt.Equals(q.MaQtt));
                    List<ChiTietQtt> listChiTietQtt = ctqtt.ToList();
                    foreach (ChiTietQtt i in listChiTietQtt)
                    {
                        db.ChiTietQtt.Remove(i);
                    }
                    db.QuanTuTrang.Remove(q);
                    db.SaveChanges();
                    MessageBox.Show("Xóa Quân tư trang '" + q.MaQtt + "' thành công!", "Thông báo");
                }
            }
            dtgQTT_Load(sender, e);
        }
        // Nhấn nút "Cập nhật"
        private void btnUpdateQTT_Click(object sender, RoutedEventArgs e)
        {
            QuanTuTrang q = (QuanTuTrang)dtgQTT.SelectedItem;
            AddAndUpdateQTT addAndUpdateQTT = new AddAndUpdateQTT(
                "Cập nhật thông tin Quân tư trang", q.MaQtt, q.TenQtt, q.GiaTien, q.MoTa);
            this.Close();
            addAndUpdateQTT.Show();
        }
        // Nhấn nút "Thoát"
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainAdmin mainAdmin = new MainAdmin();
            this.Close();
            mainAdmin.Show();
        }
        // Nhán nút "Tìm kiếm"
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if(txtTenQTT.Text == "" || txtTenQTT.Text == null)
            {
                dtgQTT_Load(sender, e);
            }
            else
            {
                var l = db.QuanTuTrang.Where(q => q.TenQtt.Contains(txtTenQTT.Text));
                dtgQTT.ItemsSource = l.ToList();
            }
        }
        // Nhấn nút "Tải lại"
        private void btnCancelApply_Click(object sender, RoutedEventArgs e)
        {
            txtTenQTT.Text = "";
            dtgQTT_Load(sender, e);
        }
    }
}
