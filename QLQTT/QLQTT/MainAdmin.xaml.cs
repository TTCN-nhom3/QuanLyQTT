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
    /// Interaction logic for MainAdmin.xaml
    /// </summary>
    public partial class MainAdmin : Window
    {
        QLQTTContext db = new QLQTTContext();
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
    }
}
