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
    /// Form bảo trì Khóa học
    /// </summary>
    public partial class MaintainCourse : Window
    {
        QLQTTContext db = new QLQTTContext();
        public MaintainCourse()
        {
            InitializeComponent();
            lblExitWarning.Content = "Nếu bạn nhất nút 'X' màu đỏ, ứng dụng sẽ tắt ngay lập tức!";
        }
        // Sinh mã Khóa học mới
        public string newKH()
        {
            string id = db.KhoaHoc.Max(c => c.MaKh);
            if (id == null)
            {
                return "SEM0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "SEM" + new_id.ToString().PadLeft(7, '0');
        }
        //Hiển thị danh sách các Khóa học
        private void dtgKH_Load(object sender, RoutedEventArgs e)
        {
            var listKH = db.KhoaHoc.Select(k => k);
            dtgKH.ItemsSource = listKH.ToList();
        }
        // Nhấn nút "Thêm mới"
        private void btnAddKH_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateCourse addAndUpdateCourse = new AddAndUpdateCourse(
                "Thêm mới Khóa học", newKH());
            this.Close();
            addAndUpdateCourse.Show();
        }
        // Nhấn nút "Xóa"
        private void btnDeleteKH_Click(object sender, RoutedEventArgs e)
        {
            KhoaHoc k = (KhoaHoc)dtgKH.SelectedItem;
            var sv = db.SinhVien.SingleOrDefault(s => s.MaKh.Equals(k.MaKh));
            if (k.NgayKt.Year >= DateTime.Now.Year || sv != null)
            {
                MessageBox.Show("Không thể xóa Khóa học '" + k.MaKh
                    + "'!\nHãy đọc kĩ hướng dẫn", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa Khóa học '" + k.MaKh
                    + "'?", "Xác nhận", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    db.KhoaHoc.Remove(k);
                    db.SaveChanges();
                    MessageBox.Show("Xóa Khóa học '" + k.MaKh + "' thành công!", "Thông báo");
                }
            }
        }
        // Nhấn nút "Cập nhật"
        private void btnUpdateKH_Click(object sender, RoutedEventArgs e)
        {
            KhoaHoc k = (KhoaHoc)dtgKH.SelectedItem;
            AddAndUpdateCourse addAndUpdateCourse = new AddAndUpdateCourse(
                "Cập nhật thông tin Quân tư trang", k.MaKh, k.NgayBd, k.NgayKt);
            this.Close();
            addAndUpdateCourse.Show();
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
            bool id = false, d = false;
            List<KhoaHoc> lId = new List<KhoaHoc>();
            List<KhoaHoc> lDate = new List<KhoaHoc>();
            if (txtMaKH.Text != "" || txtMaKH.Text != null)
            {
                var l = db.KhoaHoc.Where(k => k.MaKh.Contains(txtMaKH.Text));
                foreach (KhoaHoc i in l)
                {
                    lId.Add(i);
                }
                id = true;
            }
            if (txtDate.SelectedDate != null)
            {
                var l = db.KhoaHoc.Where(k =>
                    DateTime.Compare((DateTime)txtDate.SelectedDate, k.NgayBd) >= 0
                    && DateTime.Compare((DateTime)txtDate.SelectedDate, k.NgayKt) <= 0);
                foreach (KhoaHoc i in l)
                {
                    lDate.Add(i);
                }
                d = true;
            }
            if(id && d)
            {
                var l = lId.Select(k => new
                {
                    k.MaKh,
                    k.NgayBd,
                    k.NgayKt,
                    k.MoTa
                }).Intersect(lDate.Select(k => new
                {
                    k.MaKh,
                    k.NgayBd,
                    k.NgayKt,
                    k.MoTa
                }));
                dtgKH.ItemsSource = l.ToList();
            }
            else if (id)
            {
                dtgKH.ItemsSource = lId;
            }
            else if (d)
            {
                dtgKH.ItemsSource = lDate;
            }
            else
            {
                dtgKH_Load(sender, e);
            }
        }
        //Nhấn nút "Tải lại"
        private void btnCancelApply_Click(object sender, RoutedEventArgs e)
        {
            txtMaKH.Text = "";
            txtDate.SelectedDate = null;
            dtgKH_Load(sender, e);
        }
    }
}
