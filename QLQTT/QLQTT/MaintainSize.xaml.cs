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
    /// Form bảo trì Kích cỡ
    /// </summary>
    public partial class MaintainSize : Window
    {
        QLQTTContext db = new QLQTTContext();
        public MaintainSize()
        {
            InitializeComponent();
            lblExitWarning.Content = "Nếu bạn nhất nút 'X' màu đỏ, ứng dụng sẽ tắt ngay lập tức!";
        }
        // Sinh mã Kích cỡ mới
        public string newKC()
        {
            string id = db.KichCo.Max(c => c.MaKc);
            if (id == null)
            {
                return "KCO0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "KCO" + new_id.ToString().PadLeft(7, '0');
        }
        //Hiển thị danh sách các Kích cỡ
        private void dtgKC_Load(object sender, RoutedEventArgs e)
        {
            var listKC = db.KichCo.Select(k => k);
            dtgKC.ItemsSource = listKC.ToList();
        }
        // Nhấn nút "Thêm mới"
        private void btnAddKC_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateSize addAndUpdateSize = new AddAndUpdateSize(
                "Thêm mới Kích cỡ", newKC());
            this.Hide();
            addAndUpdateSize.Show();
        }
        // Nhấn nút "Xóa"
        private void btnDeleteKC_Click(object sender, RoutedEventArgs e)
        {
            KichCo k = (KichCo)dtgKC.SelectedItem;
            var muon = db.Muon.SingleOrDefault(m => m.MaKc.Equals(k.MaKc));
            var doi = db.Doi.SingleOrDefault(d => d.MaKc.Equals(k.MaKc));
            var mat = db.Mat.SingleOrDefault(m => m.MaKc.Equals(k.MaKc));
            var dangmuon = db.DangMuon.SingleOrDefault(d => d.MaKc.Equals(k.MaKc));
            if (muon != null || doi != null || mat != null || dangmuon != null)
            {
                MessageBox.Show("Không thể xóa Kích cỡ '" + k.MaKc
                    + "'. Hãy đọc kĩ hướng dẫn", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa Kích cỡ '" + k.MaKc
                    + "'?", "Xác nhận", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var ctqtt = db.ChiTietQtt.Where(c => c.MaKc.Equals(k.MaKc));
                    List<ChiTietQtt> listChiTietQtt = ctqtt.ToList();
                    foreach (ChiTietQtt i in listChiTietQtt)
                    {
                        db.ChiTietQtt.Remove(i);
                        var qtt = db.QuanTuTrang.SingleOrDefault(q => q.MaQtt.Equals(i.MaQtt));
                        qtt.SoLuong -= i.SoLuongCt;
                        db.SaveChanges();
                    }
                    db.KichCo.Remove(k);
                    db.SaveChanges();
                    MessageBox.Show("Xóa Kích cỡ '" + k.MaKc + "' thành công!", "Thông báo");
                }
            }
            dtgKC_Load(sender, e);
        }
        // Nhấn nút "Cập nhật"
        private void btnUpdateKC_Click(object sender, RoutedEventArgs e)
        {
            KichCo k = (KichCo)dtgKC.SelectedItem;
            AddAndUpdateSize addAndUpdateSize = new AddAndUpdateSize(
                "Cập nhật thông tin Quân tư trang", k.MaKc, k.TenKc, k.MoTa);
            this.Hide();
            addAndUpdateSize.Show();
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
            if (txtTenKC.Text == "" || txtTenKC.Text == null)
            {
                dtgKC_Load(sender, e);
            }
            else
            {
                var l = db.KichCo.Where(q => q.TenKc.Contains(txtTenKC.Text));
                dtgKC.ItemsSource = l.ToList();
            }
        }
        // Nhấn nút "Tải lại"
        private void btnCancelApply_Click(object sender, RoutedEventArgs e)
        {
            txtTenKC.Text = "";
            dtgKC_Load(sender, e);
        }
    }
}
