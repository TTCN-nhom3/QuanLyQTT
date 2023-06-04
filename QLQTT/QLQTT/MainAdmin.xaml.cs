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
using CheckBox = System.Windows.Controls.CheckBox;
//using MessageBox = System.Windows.Forms.MessageBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using MessageBox = System.Windows.MessageBox;
using Binding = System.Windows.Data.Binding;

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
    "Quân tư trang đã quá hạn trả",
    "Khoản nợ của sinh viên"
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
            string selectedValue = myComboBox.SelectedItem as string;
            //IQueryable<QuanTuTrang> listItem = null;
            if (selectedValue == "Quân tư trang còn trong kho")
            {
                var result = from a in db.QuanTuTrang
                             join b in db.ChiTietQtt on a.MaQtt equals b.MaQtt
                             join c in db.KichCo on b.MaKc equals c.MaKc
                             select new
                             {
                                 MaQtt = a.MaQtt,
                                 TenQtt = a.TenQtt,
                                 TenKc = c.TenKc,
                                 SoLuongCT = b.SoLuongCt
                             };
                var listItem = result.Select(x => x);
                myDataGrid.ItemsSource = listItem.ToList();
            }
            else if (selectedValue == "Quân tư trang đang được mượn")
            {
                var result = from a in db.QuanTuTrang
                             join b in db.ChiTietQtt on a.MaQtt equals b.MaQtt
                             join c in db.DangMuon on  new {b.MaQtt, b.MaKc } equals new {c.MaQtt, c.MaKc }
                             join d in db.SinhVien on c.MaSv equals d.MaSv
                             join f in db.KichCo on b.MaKc equals f.MaKc
                             select new
                             {
                                 TenSv = d.TenSv,
                                 MaQtt = a.MaQtt,
                                 TenQtt = a.TenQtt,
                                 TenKc = f.TenKc,
                             };
                var listItem = result.Select(x => x);
                myDataGrid.ItemsSource = listItem.ToList();
            }
            else if (selectedValue == "Quân tư trang đã quá hạn trả")
            {
                var result = from a in db.QuanTuTrang
                             join b in db.ChiTietQtt on a.MaQtt equals b.MaQtt
                             join c in db.DangMuon on new { b.MaQtt, b.MaKc } equals new { c.MaQtt, c.MaKc }
                             join d in db.SinhVien on c.MaSv equals d.MaSv
                             join f in db.KichCo on b.MaKc equals f.MaKc
                             join g in db.KhoaHoc on d.MaKh equals g.MaKh
                             where g.NgayKt < DateTime.Now
                             select new
                             {
                                 TenSv = d.TenSv,
                                 MaQtt = a.MaQtt,
                                 TenQtt = a.TenQtt,
                                 TenKc = f.TenKc,
                                 NgayKt = g.NgayKt
                             };
                var listItem = result.Select(x => x);
                myDataGrid.ItemsSource = listItem.ToList();
            } else if( selectedValue == "Khoản nợ của sinh viên")
            {
                var result = from a in db.SinhVien
                            join b in db.CongNo on a.MaSv equals b.MaSv
                             select new
                             {
                                 MaSv = a.MaSv,
                                 TenSv = a.TenSv,
                                 SoTien = b.SoTien,
                                 HanTra = b.HanTra
                             };
                var listItem = result.Select(x => x);
                myDataGrid.ItemsSource = listItem.ToList();
            }
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

        private void search_Click(object sender, RoutedEventArgs e)
        {
            var searchMaSV = searchBox.Text;
            var DMQTT = db.QuanTuTrang
                        .Join(db.DangMuon, qtt => qtt.MaQtt, dmqtt => dmqtt.MaQtt, (qtt, dmqtt) => new { QuanTuTrang = qtt, DangMuon = dmqtt })
                        .Where(qtt => qtt.DangMuon.MaQtt == qtt.QuanTuTrang.MaQtt && qtt.DangMuon.MaSv == searchMaSV)
                        .Select(x => x.DangMuon);
            myDataGrid1.ItemsSource = DMQTT.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = new List<DataGridRow>();
            var searchMaSV = searchBox.Text;
           
            foreach (var item in myDataGrid1.Items)
            {
                var row = myDataGrid1.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                var checkBox = FindVisualChild<CheckBox>(row);

                if (checkBox.IsChecked == true)
                {
                    selectedRows.Add(row);
                }
            }
            if(selectedRows.Count() > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                       "Xác nhận trả quân tư trang này", "Xác nhận", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {

                    foreach (var row in selectedRows)
                    {
                        DangMuon a = row.Item as DangMuon;
                        var DMQTT = db.DangMuon
                            .SingleOrDefault(x => x.MaQtt == a.MaQtt && x.MaSv == searchMaSV && x.MaKc == a.MaKc);
                        if (DMQTT != null)
                        {
                            db.DangMuon.Remove(DMQTT);
                            db.SaveChanges();
                        }
                        var ct = db.ChiTietQtt.SingleOrDefault(x => x.MaQtt == a.MaQtt && x.MaKc == a.MaKc);
                        if (ct != null)
                        {
                            ct.SoLuongCt += 1;
                            var qtt = db.QuanTuTrang.SingleOrDefault(x => x.MaQtt == ct.MaQtt);
                            qtt.SoLuong += 1;
                            db.SaveChanges();
                        }
                    }
                    MessageBox.Show("Trả quân tư trang thành công", "Thông báo");
                }
                myDataGrid1.ItemsSource = db.DangMuon.Where(x => x.MaSv == searchMaSV).Select(x => x).ToList();
            }
        }
        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T)
                    return (T)child;

                T result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }

}

