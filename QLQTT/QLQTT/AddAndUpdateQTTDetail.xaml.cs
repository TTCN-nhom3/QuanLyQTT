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
using System.Text.RegularExpressions;
using QLQTT.Models;

namespace QLQTT
{
    /// <summary>
    /// Form thêm mới hoặc cập nhật Chi tiết Quân tư trang
    /// </summary>
    public partial class AddAndUpdateQTTDetail : Window
    {
        QLQTTContext db = new QLQTTContext();
        string act, qttId, kcId;
        int sl;
        public AddAndUpdateQTTDetail(string act)
        {
            InitializeComponent();
            this.act = act;
        }
        public AddAndUpdateQTTDetail(string act, string qttId, string kcId, int sl)
        {
            InitializeComponent();
            this.act = act;
            this.qttId = qttId;
            this.kcId = kcId;
            this.sl = sl;
        }
        // Load dữ liệu
        private void start_Loaded(object sender, RoutedEventArgs e)
        {
            lblTitle.Content = act;
            if (act.Contains("Cập nhật"))
            {
                string qtt = db.QuanTuTrang.SingleOrDefault(q => q.MaQtt.Equals(qttId)).TenQtt;
                cbbQTT.Text = qtt;
                cbbQTT.IsEnabled = false;
                string kc = db.KichCo.SingleOrDefault(q => q.MaKc.Equals(kcId)).TenKc;
                cbbKC.Text = kc;
                cbbKC.IsEnabled = false;
                txtQuantity.Text = sl.ToString();
            }
            else
            {
                var qSource = db.QuanTuTrang.Select(q => q);
                cbbQTT.ItemsSource = qSource.ToList();
                cbbQTT.DisplayMemberPath = "TenQtt";
                cbbQTT.SelectedValuePath = "MaQtt";
                var kSource = db.KichCo.Select(q => q);
                cbbKC.ItemsSource = kSource.ToList();
                cbbKC.DisplayMemberPath = "TenKc";
                cbbKC.SelectedValuePath = "MaQtt";
                txtQuantity.Text = "0";
            }
        }
        // Check dữ liệu nhập vào
        public bool checkData(string act)
        {
            string mes = "";
            if (act.Contains("Thêm"))
            {
                bool q = true, k = true;
                if (cbbQTT.Text == "" || cbbQTT.SelectedItem == null)
                {
                    mes += "\nBạn cần chọn Quân tư trang!";
                    q = false;
                }
                if (cbbKC.Text == "" || cbbKC.SelectedItem == null)
                {
                    mes += "\nBạn cần chọn Kích cỡ!";
                    k = false;
                }
                if (q && k)
                {
                    var ct = db.ChiTietQtt.SingleOrDefault(
                        c => c.MaQtt.Equals(cbbQTT.SelectedValue.ToString())
                        && c.MaKc.Equals(cbbKC.SelectedValue.ToString()));
                    if (ct != null)
                    {
                        mes += "\n Quân tư trang '" + cbbQTT.Text
                            + "' đã có Kích cỡ '" + cbbKC.Text + "'!";
                    }
                }
            }
            if (txtQuantity.Text == "" || txtQuantity.Text == null)
            {
                mes += "\nBạn cần nhập Số lượng!";
            }
            else if (!Regex.IsMatch(txtQuantity.Text, @"\d+"))
            {
                mes += "\nSố lượng nhập vào phải là số!";
            }
            else if (int.Parse(txtQuantity.Text) < 0)
            {
                mes += "\nSố lượng nhập vào phải >= 0!";
            }
            if (mes != "")
            {
                MessageBox.Show(mes, "Dữ liệu không hợp lệ",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        // Nhấn vào nút "Thêm"
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkData(act))
                {
                    string q = cbbQTT.Text;
                    string k = cbbKC.Text;
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn thêm Kích cỡ '" + k + "' cho "
                        + " Quân tư trang '" + q + "' hay không?",
                        "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        ChiTietQtt c = new ChiTietQtt();
                        c.MaQtt = cbbQTT.SelectedValue.ToString();
                        c.MaKc = cbbKC.SelectedValue.ToString();
                        db.ChiTietQtt.Add(c);
                        QuanTuTrang qtt = db.QuanTuTrang.SingleOrDefault(
                            x => x.MaQtt.Equals(cbbQTT.SelectedValue.ToString()));
                        qtt.SoLuong += int.Parse(txtQuantity.Text);
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Thêm Kích cỡ '" + k + "' cho Quân tư trang '"
                            + q + "' thành công", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm: " + ex.Message + "!", "Thông báo");
            }
        }
        // Nhấn nút "Cập nhật"
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkData(act))
                {
                    string q = cbbQTT.Text;
                    string k = cbbKC.Text;
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn cập nhật thông tin Quân tư trang '"
                        + k + "' với " + " Kích cỡ '" + k + "' hay không?",
                        "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var ct = db.ChiTietQtt.SingleOrDefault(
                            c => c.MaQtt.Equals(cbbQTT.SelectedValue.ToString())
                            && c.MaKc.Equals(cbbKC.SelectedValue.ToString()));
                        ct.SoLuongCt = int.Parse(txtQuantity.Text);
                        // Thay đổi số lượng của Quân tư trang tương ứng
                        var qtt = db.QuanTuTrang.SingleOrDefault(
                            x => x.MaQtt.Equals(cbbQTT.SelectedValue.ToString()));
                        qtt.SoLuong += int.Parse(txtQuantity.Text) - sl;
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Cập nhật thông tin Quân tư trang '" + q
                            + "' với Kích cỡ '" + k + "' thành công", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi cập nhật: " + ex.Message + "!", "Thông báo");
            }
        }
        // Nhất nút "Hủy bỏ"
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            string mes = "";
            if (act.Contains("Thêm"))
            {
                mes = "thêm mới?";
            }
            else
            {
                mes = "cập nhật?";
            }
            MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn hủy việc " + mes,
                        "Xác nhận", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        // Hiển thị MaintainQTTDetail khi đóng của sổ này
        private void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainTainQTTDetail mainTainQTTDetail = new MainTainQTTDetail();
            mainTainQTTDetail.Show();
        }

    }
}