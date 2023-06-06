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
    /// Form thêm mới hoặc cập nhật Kích cỡ
    /// </summary>
    public partial class AddAndUpdateSize : Window
    {
        QLQTTContext db = new QLQTTContext();
        string act, id, name, describe;
        public AddAndUpdateSize(string act, string id)
        {
            InitializeComponent();
            this.act = act;
            this.id = id;
        }
        public AddAndUpdateSize(string act, string id,
            string name, string describe)
        {
            InitializeComponent();
            this.act = act;
            this.id = id;
            this.name = name;
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
                txtDescribe.Text = describe;
            }
        }
        // Check dữ liệu nhập vào
        public bool checkData()
        {
            string mes = "";
            if (txtName.Text == null || txtName.Text == "")
            {
                mes += "\nBạn cần nhập tên Kích cỡ!";
            }
            if (txtDescribe.Text == null || txtDescribe.Text == "")
            {
                mes += "\nBạn cần nhập Mô tả!";
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
                        "Bạn có chắc chắn muốn thêm mới Kích cỡ '"
                        + id + "'?", "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        KichCo kc = new KichCo();
                        kc.MaKc = txtId.Text;
                        kc.TenKc = txtName.Text;
                        kc.MoTa = txtDescribe.Text;
                        db.KichCo.Add(kc);
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Thêm mới Kích cỡ '" + id + "' thành công!", "Thông báo");
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
                        "Bạn có chắc chắn muốn cập nhật thông tin Kích cỡ '"
                        + id + "'?", "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var kc = db.KichCo.SingleOrDefault(q => q.MaKc.Equals(id));
                        kc.TenKc = txtName.Text;
                        kc.MoTa = txtDescribe.Text;
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Cập nhật thông tin Kích cỡ '" + id
                            + "' thành công!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi cập nhật: " + ex.Message + "!", "Thông báo");
            }
        }
        //Nhấn nút "Hủy bỏ"
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
        // Hiển thị MaintainSize khi đóng cửa sổ này
        private void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MaintainSize maintainSize = new MaintainSize();
            maintainSize.Show();
        }

    }
}