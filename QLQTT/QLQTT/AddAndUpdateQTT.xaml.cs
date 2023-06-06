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
    /// Form thêm mới hoặc cập nhật Quân tư trang
    /// </summary>
    public partial class AddAndUpdateQTT : Window
    {
        QLQTTContext db = new QLQTTContext();
        string act, id, name, describe;
        decimal price;
        public AddAndUpdateQTT(string act, string id)
        {
            InitializeComponent();
            this.act = act;
            this.id = id;
        }
        public AddAndUpdateQTT(string act, string id, string name, decimal price, string describe)
        {
            InitializeComponent();
            this.act = act;
            this.id = id;
            this.name = name;
            this.price = price;
            this.describe = describe;
        }
        // Load dữ liệu
        private void start_Loaded(object sender, RoutedEventArgs e)
        {
            lblTitle.Content = act;
            txtId.Text = id;
            if (act.Contains("Thêm"))
            {
                btnUpdate.IsEnabled = false;
            }
            else
            {
                btnAdd.IsEnabled = false;
                txtName.Text = name;
                txtPrice.Text = price.ToString().Split('.').First();
                txtDescribe.Text = describe;
            }
        }
        // Check dữ liệu nhập vào
        public bool checkData()
        {
            string mes = "";
            if (txtName.Text == null || txtName.Text == "")
            {
                mes += "\nBạn cần nhập tên Quân tư trang!";
            }
            if (txtPrice.Text == null || txtPrice.Text == "")
            {
                mes += "\nBạn cần nhập giá tiền!";
            }
            else if (!Regex.IsMatch(txtPrice.Text, @"\d+"))
            {
                mes += "\nGiá tiền nhập vào phải là số!";
            }
            else if (int.Parse(txtPrice.Text) <= 0)
            {
                mes += "\nGiá tiền nhập vào phải lớn hơn 0!";
            }
            if (mes != "")
            {
                MessageBox.Show(mes, "Dữ liệu không hợp lệ",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        // Nhấn nút "Thêm"
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkData())
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn thêm mới Quân tư trang '"
                        + id + "'?", "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        QuanTuTrang qtt = new QuanTuTrang();
                        qtt.MaQtt = txtId.Text;
                        qtt.TenQtt = txtName.Text;
                        qtt.GiaTien = decimal.Parse(txtPrice.Text);
                        qtt.MoTa = txtDescribe.Text;
                        db.QuanTuTrang.Add(qtt);
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Thêm mới Quân tư trang '" + id
                            + "' thành công!", "Thông báo");
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
                if (checkData())
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn cập nhật thông tin Quân tư trang '"
                        + id + "'?", "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var qtt = db.QuanTuTrang.SingleOrDefault(q => q.MaQtt.Equals(id));
                        qtt.TenQtt = txtName.Text;
                        qtt.GiaTien = decimal.Parse(txtPrice.Text);
                        qtt.MoTa = txtDescribe.Text;
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Cập nhật thông tin Quân tư trang '"
                            + id + "' thành công!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi cập nhật: " + ex.Message + "!", "Thông báo");
            }
        }
        // Nhấn nút "Hủy bỏ"
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
        // Hiển thị MaintainQTT khi đóng cửa sổ này
        private void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MaintainQTT maintainQTT = new MaintainQTT();
            maintainQTT.Show();
        }

    }
}