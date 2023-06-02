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

namespace QLQTT
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        QLQTTContext db = new QLQTTContext();
        public LogIn()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SinhVien.SingleOrDefault(t => t.MaSv.Equals(txtName.Text));
            if (query == null && txtName.Text != "ad")
            {
                MessageBox.Show("Username không tồn tại!");
            }
            else if (txtName.Text == "ad")
            {
                if (txtPW.Text != "123")
                {
                    MessageBox.Show("Sai mật khẩu!");
                }
                else
                {
                    var a = new MainAdmin();
                    a.Show();
                    this.Close();
                    MessageBox.Show("Đăng nhập thành công!");
                }
            }
            else
            {
                var sv = db.SinhVien.SingleOrDefault(t => t.MaSv.Equals(txtName.Text) &&
                t.MatKhau.Equals(txtPW.Text));
                if (sv != null)
                {
                    var s = new MainStudent(txtName.Text);
                    s.Show();
                    this.Close();
                    MessageBox.Show("Đăng nhập thành công!");
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu!");
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            string id = "QTT0000007";
            var muon = db.Muon.SingleOrDefault(m => m.MaQtt.Equals(id));
            var doi = db.Doi.SingleOrDefault(d => d.MaQtt.Equals(id));
            var mat = db.Mat.SingleOrDefault(m => m.MaQtt.Equals(id));
            var dangmuon = db.DangMuon.SingleOrDefault(d => d.MaQtt.Equals(id));
            if (muon != null || doi != null || mat != null || dangmuon != null)
            {
                txtTest.Text = "yes";
            }
            else
            {
                txtTest.Text = "no";
            }
        }
    }
}
