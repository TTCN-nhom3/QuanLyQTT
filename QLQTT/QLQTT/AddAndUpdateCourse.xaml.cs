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
    /// Form thêm mới hoặc cập nhật Khóa học
    /// </summary>
    public partial class AddAndUpdateCourse : Window
    {
        QLQTTContext db = new QLQTTContext();
        string act, id;
        DateTime startDate, finishDate;
        public AddAndUpdateCourse(string act, string id)
        {
            InitializeComponent();
            this.act = act;
            this.id = id;
        }
        public AddAndUpdateCourse(string act, string id, DateTime startDate, DateTime finishDate)
        {
            InitializeComponent();
            this.act = act;
            this.id = id;
            this.startDate = startDate;
            this.finishDate = finishDate;
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
                dateStart.SelectedDate = startDate;
                dateFinish.SelectedDate = finishDate;
            }
        }
        // Check dữ liệu nhập vào
        public bool checkData(string act)
        {
            string mes = "";
            bool s = true, e = true;
            if (dateStart.SelectedDate == null || dateStart.Text == "")
            {
                mes += "\nBạn cần chọn Ngày bắt đầu!";
                s = false;
            }
            else
            {
                if (act.Contains("Thêm"))
                {
                    DateTime f = db.KhoaHoc.OrderBy(k => k.NgayKt).First().NgayKt;
                    if (dateStart.SelectedDate <= f)
                    {
                        mes += "\nNgày bắt đầu phải sau ngày '" + f.Date.ToString() + "'!";
                    }
                }
            }
            if (dateFinish.SelectedDate == null || dateFinish.Text == "")
            {
                mes += "\nBạn cần chọn Ngày kết thúc!";
                e = false;
            }
            if (s && e)
            {
                if (dateStart.SelectedDate >= dateFinish.SelectedDate)
                {
                    mes += "\nNgày kết thúc phải sau Ngày bắt đầu!";
                }
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
                if (checkData(act))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn thêm mới Khóa học '"
                        + id + "'?", "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        KhoaHoc kh = new KhoaHoc();
                        kh.MaKh = txtId.Text;
                        kh.NgayBd = (DateTime)dateStart.SelectedDate;
                        kh.NgayKt = (DateTime)dateFinish.SelectedDate;
                        db.KhoaHoc.Add(kh);
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Thêm mới Khóa học '" 
                            + id + "' thành công!", "Thông báo");
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
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn cập nhật thông tin Khóa học '"
                        + id + "'?", "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var kh = db.KhoaHoc.SingleOrDefault(k => k.MaKh.Equals(id));
                        kh.NgayBd = (DateTime)dateStart.SelectedDate;
                        kh.NgayKt = (DateTime)dateFinish.SelectedDate;
                        db.SaveChanges();
                        this.Close();
                        MessageBox.Show("Cập nhật thông tin Khóa học '"
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
    }
}