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
using System.Windows.Forms;

namespace QLQTT
{
    /// <summary>
    /// Interaction logic for MainAdmin.xaml
    /// </summary>
    public partial class MainAdmin : Window
    {
        QLQTTContext db = new QLQTTContext();
        List<string> comboboxThongKeItems = new List<string> {
    "Quân tư trang còn trong kho",
    "Quân tư trang đang được mượn",
    "Quân tư trang đã quá hạn trả"
};

        public MainAdmin()
        {
            InitializeComponent();
        }
        // Đăng xuất
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var logIn = new LogIn();
            logIn.Show();
            this.Close();
        }
        // Nhấn nút "Quản lý Quân tư trang"
        private void btnQTTMaintain_Click(object sender, RoutedEventArgs e)
        {
            MaintainQTT maintainQTT = new MaintainQTT();
            this.Hide();
            maintainQTT.ShowDialog();
            this.Show();
        }
        // Nhấn nút "Quản lý Kích cỡ"
        private void btnKCMaintain_Click(object sender, RoutedEventArgs e)
        {
            MaintainSize maintainSize = new MaintainSize();
            this.Hide();
            maintainSize.ShowDialog();
            this.Show();
        }
        // Nhấn nút "Quản lý Chi tiết Quân tư trang"
        private void btnCTQTTMaintain_Click(object sender, RoutedEventArgs e)
        {
            MainTainQTTDetail mainTainQTTDetail = new MainTainQTTDetail();
            this.Hide();
            mainTainQTTDetail.ShowDialog();
            this.Show();
        }
        // Nhấn nút "Quản lý Khóa học"
        private void btnKHMaintain_Click(object sender, RoutedEventArgs e)
        {
            MaintainCourse maintainCourse = new MaintainCourse();
            this.Hide();
            maintainCourse.ShowDialog();
            this.Show();
        }
        // Chọn thống kê
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            //myDataGrid.ItemsSource = null;
            // Kiểm tra giá trị đã chọn trong ComboBox
            string selectedValue = myComboBox.SelectedItem as string;
            IQueryable<QuanTuTrang> listItem = null;
            if (selectedValue == "Quân tư trang còn trong kho")
            {
                listItem = db.QuanTuTrang.Select(x => x);
            }
            else if (selectedValue == "Quân tư trang đang được mượn")
            {
                    listItem = db.QuanTuTrang
                        .Join(db.DangMuon, x => x.MaQtt, y => y.MaQtt, (x, y) => x);
            }
            else if (selectedValue == "Quân tư trang đã quá hạn trả")
            {
                listItem = db.QuanTuTrang
               .Join(db.Muon, qtt => qtt.MaQtt, muon => muon.MaQtt, (qtt, muon) => new { QuanTuTrang = qtt, Muon = muon })
               .Join(db.SinhVien, qm => qm.Muon.MaSv, sv => sv.MaSv, (qm, sv) => new { QuanTuTrang = qm.QuanTuTrang, SinhVien = sv })
               .Join(db.KhoaHoc, qms => qms.SinhVien.MaKh, kh => kh.MaKh, (qms, kh) => new { QuanTuTrang = qms.QuanTuTrang, KhoaHoc = kh })
               .Where(qms => qms.KhoaHoc.NgayKt < DateTime.Now)
               .Select(qms => qms.QuanTuTrang);
              
            }
            myDataGrid.ItemsSource = listItem.ToList();


        }

        private void ThongKe_Loaded(object sender, RoutedEventArgs e)
        {
            myComboBox.ItemsSource = comboboxThongKeItems;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
