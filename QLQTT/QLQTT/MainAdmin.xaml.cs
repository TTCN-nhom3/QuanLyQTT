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
using System.Text.RegularExpressions;

namespace QLQTT
{
    /// <summary>
    /// Interaction logic for MainAdmin.xaml
    /// </summary>
    public partial class MainAdmin : Window
    {
        QLQTTContext db = new QLQTTContext();
        static bool isSearched;
        static string temp_id;
        static decimal temp_value;
        List<string> comboboxThongKeItems = new List<string> {
            "Quân tư trang còn trong kho",
            "Quân tư trang đang được mượn",
            "Quân tư trang đã quá hạn trả",
            "Khoản nợ của sinh viên"
        };
        List<DoiViewModel> listd = new List<DoiViewModel>();
        List<MuonViewModel> listm = new List<MuonViewModel>();
        public static MainAdmin instance;
        public MainAdmin()
        {
            InitializeComponent();
            notification();
            lblExitWarning.Content = "Nếu bạn nhất nút 'X' màu đỏ, ứng dụng sẽ tắt ngay lập tức!";
        }
        /*ĐĂNG XUẤT*/
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var logIn = new LogIn();
            this.Close();
            logIn.Show();
        }

        /*THÔNG BÁO*/
        public void notification()
        {
            int borrow_count = db.Muon.Where(m => m.TrangThai.Equals("Đang xử lý")).Count();
            int change_count = db.Doi.Where(d => d.TrangThai.Equals("Đang xử lý")).Count();
            lblNeededVerify.Content = "Có " + (borrow_count + change_count)
                + " đăng ký đang CHỜ ĐƯỢC XÁC NHẬN (Mượn: " + borrow_count
                + ", Đổi: " + change_count + ")";
            borrow_count = db.Muon.Where(m => m.TrangThai.Equals("Xác nhận")).Count();
            change_count = db.Doi.Where(d => d.TrangThai.Equals("Xác nhận")).Count();
            lblAlreadyVerify.Content = "Có " + (borrow_count + change_count)
                + " đăng ký ĐÃ ĐƯỢC XÁC NHẬN (Mượn: " + borrow_count
                + ", Đổi: " + change_count + ")";
        }

        /*NHỮNG THAY ĐỔI KHI CHUYỂN TAB*/
        private void tabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabPayment.IsSelected == true)
            {
                txtValue.Text = "";
                txtSearchDebt.Text = "";
                isSearched = false;
            }
            //else if (tabHDTT.IsSelected == true)
            //{
            //    txtSearchHD.Text = "";
            //}
            //else if (tabDSGD.IsSelected == true)
            //{
            //    tabDSGD_Loaded(sender, e);
            //}
        }

        /*=====================BẢO TRÌ=====================*/

        // Nhấn nút "Bảo trì Quân tư trang"
        private void btnQTTMaintain_Click(object sender, RoutedEventArgs e)
        {
            MaintainQTT maintainQTT = new MaintainQTT();
            this.Close();
            maintainQTT.Show();
        }

        // Nhấn nút "Bảo trì Kích cỡ"
        private void btnKCMaintain_Click(object sender, RoutedEventArgs e)
        {
            MaintainSize maintainSize = new MaintainSize();
            this.Close();
            maintainSize.Show();
        }

        // Nhấn nút "Bảo trì Chi tiết Quân tư trang"
        private void btnCTQTTMaintain_Click(object sender, RoutedEventArgs e)
        {
            MainTainQTTDetail mainTainQTTDetail = new MainTainQTTDetail();
            this.Close();
            mainTainQTTDetail.Show();
        }

        // Nhấn nút "Bảo trì Khóa học"
        private void btnKHMaintain_Click(object sender, RoutedEventArgs e)
        {
            MaintainCourse maintainCourse = new MaintainCourse();
            this.Close();
            maintainCourse.Show();
        }

        /*=====================QUẢN LÝ MƯỢN=====================*/

        // Danh sách các đơn mượn chưa xác nhận
        private void dtgVerifyBorrow_Loaded(object sender, RoutedEventArgs e)
        {
            var borrow = from muon in db.Muon
                         join qtt in db.QuanTuTrang on muon.MaQtt equals qtt.MaQtt
                         join kc in db.KichCo on muon.MaKc equals kc.MaKc
                         where muon.TrangThai.Equals("Đang xử lý")
                         select new
                         {
                             muon,
                             TenQTT = qtt.TenQtt,
                             TenKC = kc.TenKc
                         };
            dtgVerifyChange.ItemsSource = borrow.ToList();
        }

        // Nhấn nút "Xác nhận tất cả đơn mượn"
        private void btnVerifyAllM_Click(object sender, RoutedEventArgs e)
        {
            var count = from m in db.Muon
                        join q in db.QuanTuTrang on m.MaQtt equals q.MaQtt
                        join k in db.KichCo on m.MaKc equals k.MaKc
                        where m.TrangThai.Equals("Đang xử lý")
                        group m by new
                        {
                            q.MaQtt,
                            q.TenQtt,
                            k.MaKc,
                            k.TenKc
                        } into g
                        select new
                        {
                            MaQtt = g.Key.MaQtt,
                            TenQtt = g.Key.TenQtt,
                            MaKc = g.Key.MaKc,
                            TenKc = g.Key.TenKc,
                            c = g.Count()
                        };
            List<VerifyAll> verifyAlls = new List<VerifyAll>();
            foreach (var i in count.ToList())
            {
                verifyAlls.Add(new VerifyAll(i.MaQtt, i.TenQtt, i.MaKc, i.TenKc, i.c));
            }
            foreach (var i in verifyAlls)
            {
                int having = db.ChiTietQtt.SingleOrDefault(c => c.MaQtt.Equals(i.MaQtt)
                    && c.MaKc.Equals(i.MaKc)).SoLuongCt;
                int waiting = db.Doi.Where(d => d.MaQtt.Equals(i.MaQtt)
                    && d.MaKc.Equals(i.MaKc) && d.TrangThai.Equals("Xác nhận")).Count();
                waiting += db.Muon.Where(m => m.MaQtt.Equals(i.MaQtt)
                    && m.MaKc.Equals(i.MaKc) && m.TrangThai.Equals("Xác nhận")).Count();
                if (i.c + waiting > having)
                {
                    int l = i.c + waiting - having;
                    string mes = "Số " + i.TenQtt + " " + i.TenKc + " đang có: " + having +
                    "\nSố sinh viên đang chờ lấy " + i.TenQtt + " " + i.TenKc + ": "
                    + waiting + "\nSố đơn mượn " + i.TenQtt + " " + i.TenKc +
                    " bạn đang muốn xác nhận: " + i.c + "\nSố " + i.TenQtt + " " + i.TenKc +
                    " có thể không đủ(Thiếu " + l + ")." + " Bạn có chắc muốn xác nhận tất cả?";
                    MessageBoxResult messageBoxResult = MessageBox.Show(mes, "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var id = db.Muon.Where(d => d.MaQtt.Equals(i.MaQtt) && d.MaKc.Equals(i.MaKc)
                            && d.TrangThai.Equals("Đang xử lý")).Select(d => d.MaMuon);
                        List<string> ids = new List<string>();
                        foreach (string str in id.ToList())
                        {
                            ids.Add(str);
                        }
                        foreach (string str in ids)
                        {
                            Muon muon = db.Muon.SingleOrDefault(d => d.MaMuon.Equals(str));
                            muon.TrangThai = "Xác nhận";
                            db.SaveChanges();
                        }
                        MessageBox.Show("Xác nhận " + i.c + " đơn mượn "
                            + i.TenQtt + " " + i.TenKc + " thành công", "Thông báo");
                    }
                    else if (waiting < having)
                    {
                        MessageBoxResult messageBoxResult1 = MessageBox.Show(
                            "Bạn có thể xác nhận " + (having - waiting) + " đơn mượn " + i.TenQtt
                            + i.TenKc + " mà vẫn đảm bảo cung cấp được cho tất cả sinh viên đang chờ"
                            + "\nBạn có muốn xác nhận " + (having - waiting) + " đơn mượn "
                            + i.TenQtt + i.TenKc + " đầu tiên?", "Xác nhận", MessageBoxButton.YesNo);
                        if (messageBoxResult1 == MessageBoxResult.Yes)
                        {
                            var id = db.Muon.Where(d => d.MaQtt.Equals(i.MaQtt) && d.MaKc.Equals(i.MaKc)
                                && d.TrangThai.Equals("Đang xử lý")).Select(d => d.MaMuon);
                            List<string> ids = new List<string>();
                            foreach (string str in id.ToList().Take(having - waiting))
                            {
                                ids.Add(str);
                            }
                            foreach (string str in ids)
                            {
                                Muon muon = db.Muon.SingleOrDefault(d => d.MaMuon.Equals(str));
                                muon.TrangThai = "Xác nhận";
                                db.SaveChanges();
                            }
                            MessageBox.Show("Xác nhận " + (having - waiting) + " đơn mượn "
                            + i.TenQtt + " " + i.TenKc + " không thành công", "Thông báo");
                        }
                        else
                        {
                            MessageBox.Show("Xác nhận " + i.c + " đơn mượn "
                                + i.TenQtt + " " + i.TenKc + " thành công", "Thông báo");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Xác nhận " + i.c + " đơn mượn "
                            + i.TenQtt + " " + i.TenKc + " thành công", "Thông báo");
                    }
                }
                else
                {
                    var id = db.Muon.Where(d => d.MaQtt.Equals(i.MaQtt) && d.MaKc.Equals(i.MaKc)
                            && d.TrangThai.Equals("Đang xử lý")).Select(d => d.MaMuon);
                    List<string> ids = new List<string>();
                    foreach (string str in id.ToList())
                    {
                        ids.Add(str);
                    }
                    foreach (string str in ids)
                    {
                        Muon muon = db.Muon.SingleOrDefault(d => d.MaMuon.Equals(str));
                        muon.TrangThai = "Xác nhận";
                        db.SaveChanges();
                    }
                    MessageBox.Show("Xác nhận " + i.c + " đơn mượn "
                        + i.TenQtt + " " + i.TenKc + " thành công", "Thông báo");
                }
            }
            dtgVerifyBorrow_Loaded(sender, e);
            notification();
        }

        private void dtgMuon_Loaded(object sender, RoutedEventArgs e)
        {
            var m = from muon in db.Muon
                    join kc in db.KichCo on muon.MaKc equals kc.MaKc
                    join qtt in db.QuanTuTrang on muon.MaQtt equals qtt.MaQtt
                    where muon.TrangThai == "Xác nhận"
                    select new MuonViewModel
                    (
                        muon.MaMuon,
                        muon.MaSv,
                        qtt.TenQtt,
                        kc.TenKc,
                        muon.NgayDk,
                        muon.NgayMuon,
                        muon.TrangThai,
                        muon.MoTa
                    );
            List<MuonViewModel> listm = new List<MuonViewModel>();
            foreach (MuonViewModel i in m)
            {
                listm.Add(i);
            }
            dtgMuon.ItemsSource = listm;

            btnChoMuonAll.Visibility = Visibility.Hidden;
        }

        private void btnTimKiem_Muon_Click(object sender, RoutedEventArgs e)
        {
            if (txbMSV_Muon.Text != "")
            {
                var m = from muon in db.Muon
                        join kc in db.KichCo on muon.MaKc equals kc.MaKc
                        join qtt in db.QuanTuTrang on muon.MaQtt equals qtt.MaQtt
                        where muon.TrangThai == "Xác nhận" && muon.MaSv == txbMSV_Muon.Text
                        select new MuonViewModel
                        (
                            muon.MaMuon,
                            muon.MaSv,
                            qtt.TenQtt,
                            kc.TenKc,
                            muon.NgayDk,
                            muon.NgayMuon,
                            muon.TrangThai,
                            muon.MoTa
                        );

                if (m.Any())
                {
                    foreach (MuonViewModel i in m)
                    {
                        listm.Add(i);
                    }
                    dtgMuon.ItemsSource = listm;

                    btnChoMuonAll.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Không có kết quả tìm kiếm nào");
                }

            }
            else
            {
                MessageBox.Show("Bạn hãy nhập mã sinh viên muốn tìm");
            }
        }

        private void btnXemAll_Muon_Click(object sender, RoutedEventArgs e)
        {

            dtgMuon_Loaded(sender, e);
            listm.Clear();
        }

        private void btnChoMuon_Click(object sender, RoutedEventArgs e)
        {
            if (dtgMuon.SelectedItem != null)
            {
                MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn cho sinh viên mượn đồ đã chọn?", "Confirmation Box", MessageBoxButton.YesNo);
                if (messageChange == MessageBoxResult.Yes)
                {
                    MuonViewModel selectedRow = dtgMuon.SelectedItem as MuonViewModel;
                    string mamuon = selectedRow.MaMuon;
                    string masv = selectedRow.MaSv;
                    string tenqtt = selectedRow.TenQtt;
                    string tenkc = selectedRow.TenKC;
                    var maqtt = (from m in db.QuanTuTrang
                                 where m.TenQtt == tenqtt
                                 select m.MaQtt).FirstOrDefault();
                    var makc = (from m in db.KichCo
                                where m.TenKc == tenkc
                                select m.MaKc).FirstOrDefault();
                    ChiTietQtt chitietqtt = (from m in db.ChiTietQtt
                                             where m.MaQtt == maqtt && m.MaKc == makc
                                             select m).FirstOrDefault();

                    var row = (from m in db.Muon
                               where m.MaMuon == mamuon
                               select m).FirstOrDefault();
                    var dm = new DangMuon(masv, maqtt, makc);
                    if (row != null)
                    {

                        row.NgayMuon = DateTime.Today;
                        row.TrangThai = "Hoàn thành";
                        chitietqtt.SoLuongCt -= 1;
                        db.Add(dm);
                        db.SaveChanges();

                        MotaMot mota1 = new MotaMot();
                        mota1.mamuon = mamuon;
                        mota1.ShowDialog();
                        dtgMuon_Loaded(sender, e);
                        listm.Clear();
                    }

                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào");
            }
        }

        private void btnChoMuonAll_Click(object sender, RoutedEventArgs e)
        {
            if (dtgMuon != null)
            {
                MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn cho mượn toàn bộ?", "Confirmation Box", MessageBoxButton.YesNo);
                if (messageChange == MessageBoxResult.Yes)
                {
                    List<string> mamuons = new List<string>();
                    foreach (var i in listm)
                    {
                        if (listm != null)
                        {
                            var row = (from m in db.Muon
                                       where m.MaMuon == i.MaMuon
                                       select m).FirstOrDefault();

                            var maqtt = (from m in db.QuanTuTrang
                                         where m.MaQtt == row.MaQtt
                                         select m.MaQtt).FirstOrDefault();
                            var makc = (from m in db.KichCo
                                        where m.MaKc == row.MaKc
                                        select m.MaKc).FirstOrDefault();
                            var masv = (from m in db.SinhVien
                                        where m.MaSv == row.MaSv
                                        select m.MaSv).FirstOrDefault();
                            ChiTietQtt chitietqtt = (from m in db.ChiTietQtt
                                                     where m.MaQtt == maqtt && m.MaKc == makc
                                                     select m).FirstOrDefault();
                            var dm = new DangMuon(masv, maqtt, makc);
                            row.NgayMuon = DateTime.Today;
                            row.TrangThai = "Hoàn thành";
                            chitietqtt.SoLuongCt -= 1;
                            db.Add(dm);
                            db.SaveChanges();
                            //string mamuon = (from m in db.Muon
                            //               where m.MaMuon == i.MaMuon
                            //               select m.MaMuon).FirstOrDefault();
                            //mamuons.Add(mamuon);
                        }
                    }
                    MoTaNhieu motaN = new MoTaNhieu();
                    motaN.listm = listm;
                    motaN.ShowDialog();
                    dtgMuon_Loaded(sender, e);
                    listm.Clear();
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Danh sách trống!");
            }
        }

        private void btnXoa_Muon_Click(object sender, RoutedEventArgs e)
        {
            if (dtgMuon.SelectedItem != null)
            {
                MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn xóa yêu cầu mượn này? ", "Confirmation Box", MessageBoxButton.YesNo);
                if (messageChange == MessageBoxResult.Yes)
                {
                    MuonViewModel selectedRow = dtgMuon.SelectedItem as MuonViewModel;

                    string tenqtt = selectedRow.TenQtt;
                    var maqtt = (from m in db.QuanTuTrang
                                 where m.TenQtt == tenqtt
                                 select m.MaQtt).FirstOrDefault();
                    string masv = selectedRow.MaSv;
                    var rowToDelete = (from m in db.Muon
                                       where m.MaSv == masv && m.MaQtt == maqtt
                                       select m).FirstOrDefault();
                    if (rowToDelete != null)
                    {
                        // Xóa dòng từ nguồn dữ liệu
                        db.Muon.Remove(rowToDelete);
                        db.SaveChanges();
                        // Cập nhật lại DataGrid (nếu cần)
                        dtgMuon_Loaded(sender, e);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn mục cần xóa");
            }
        }

        /*=====================QUẢN LÝ ĐỔI=====================*/

        private void dtgDoi_Loaded(object sender, RoutedEventArgs e)
        {
            var d = from doi in db.Doi
                    join kc in db.KichCo on doi.MaKc equals kc.MaKc
                    join qtt in db.QuanTuTrang on doi.MaQtt equals qtt.MaQtt
                    where doi.TrangThai != "Hoàn Thành" //=="xac nhan"
                    select new DoiViewModel
                    (
                        doi.MaDoi,
                        doi.MaSv,
                        qtt.TenQtt,
                        kc.TenKc,
                        doi.NgayDk,
                        doi.NgayDoi,
                        doi.TrangThai,
                        doi.MoTa
                    );
            List<DoiViewModel> listd = new List<DoiViewModel>();
            foreach (DoiViewModel i in d)
            {
                listd.Add(i);
            }
            dtgDoi.ItemsSource = listm;

            btnChoDoiAll.Visibility = Visibility.Hidden;
        }

        private void btnTimKiem_Doi_Click(object sender, RoutedEventArgs e)
        {
            if (txbMSV_Doi.Text != "")
            {
                var d = from doi in db.Doi
                        join kc in db.KichCo on doi.MaKc equals kc.MaKc
                        join qtt in db.QuanTuTrang on doi.MaQtt equals qtt.MaQtt
                        where doi.TrangThai != "Hoàn Thành" && doi.MaSv == txbMSV_Doi.Text
                        select new DoiViewModel
                        (
                            doi.MaDoi,
                            doi.MaSv,
                            qtt.TenQtt,
                            kc.TenKc,
                            doi.NgayDk,
                            doi.NgayDoi,
                            doi.TrangThai,
                            doi.MoTa
                        );
                if (d.Any())
                {
                    foreach (DoiViewModel i in d)
                    {
                        listd.Add(i);
                    }
                    dtgDoi.ItemsSource = listd;

                    btnChoDoiAll.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Không có kết quả tìm kiếm nào");
                }
            }
            else
            {
                MessageBox.Show("Bạn hãy nhập mã sinh viên muốn tìm");
            }
        }

        private void btnXemAll_Doi_Click(object sender, RoutedEventArgs e)
        {
            dtgDoi_Loaded(sender, e);
            listd.Clear();
        }

        private void btnChoDoi_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDoi.SelectedItem != null)
            {
                MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn cho sinh viên đổi đồ đã chọn?", "Confirmation Box", MessageBoxButton.YesNo);
                if (messageChange == MessageBoxResult.Yes)
                {
                    DoiViewModel selectedRow = dtgDoi.SelectedItem as DoiViewModel;
                    string madoi = selectedRow.MaDoi;
                    string masv = selectedRow.MaSv;
                    string tenqtt = selectedRow.TenQtt;
                    string tenkc = selectedRow.TenKC;
                    var maqtt = (from m in db.QuanTuTrang
                                 where m.TenQtt == tenqtt
                                 select m.MaQtt).FirstOrDefault();
                    var makc = (from m in db.KichCo
                                where m.TenKc == tenkc
                                select m.MaKc).FirstOrDefault();
                    ChiTietQtt chitietqtt = (from m in db.ChiTietQtt
                                             where m.MaQtt == maqtt && m.MaKc == makc
                                             select m).FirstOrDefault();
                    //xóa bản ghi cũ để thêm bản ghi mới thay đổi kích cỡ
                    var rowToDelete = (from m in db.DangMuon
                                       where m.MaSv == masv && m.MaQtt == maqtt
                                       select m).FirstOrDefault();
                    ChiTietQtt chitietqttcu = (from m in db.ChiTietQtt
                                               where m.MaQtt == maqtt && m.MaKc == rowToDelete.MaKc
                                               select m).FirstOrDefault();
                    db.DangMuon.Remove(rowToDelete);
                    chitietqttcu.SoLuongCt += 1;
                    db.SaveChanges();
                    var row = (from m in db.Doi
                               where m.MaDoi == madoi
                               select m).FirstOrDefault();
                    var dm = new DangMuon(masv, maqtt, makc);
                    if (row != null)
                    {
                        row.NgayDoi = DateTime.Today;
                        row.TrangThai = "Hoàn thành";
                        chitietqtt.SoLuongCt -= 1;
                        db.Add(dm);
                        db.SaveChanges();
                        MotaMot_Doi mota1 = new MotaMot_Doi();
                        mota1.madoi = madoi;
                        mota1.ShowDialog();
                        dtgDoi_Loaded(sender, e);
                        listd.Clear();
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào");
            }
        }

        private void btnChoDoiAll_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDoi != null)
            {
                MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn cho đổi toàn bộ?", "Confirmation Box", MessageBoxButton.YesNo);
                if (messageChange == MessageBoxResult.Yes)
                {
                    List<string> madois = new List<string>();

                    foreach (var i in listd)
                    {

                        if (listd != null)
                        {
                            var row = (from m in db.Doi
                                       where m.MaDoi == i.MaDoi
                                       select m).FirstOrDefault();
                            var maqtt = (from m in db.QuanTuTrang
                                         where m.MaQtt == row.MaQtt
                                         select m.MaQtt).FirstOrDefault();
                            var makc = (from m in db.KichCo
                                        where m.MaKc == row.MaKc
                                        select m.MaKc).FirstOrDefault();
                            var masv = (from m in db.SinhVien
                                        where m.MaSv == row.MaSv
                                        select m.MaSv).FirstOrDefault();
                            ChiTietQtt chitietqtt = (from m in db.ChiTietQtt
                                                     where m.MaQtt == maqtt && m.MaKc == makc
                                                     select m).FirstOrDefault();
                            //xóa bản ghi cũ để thêm bản ghi mới thay đổi kích cỡ
                            var rowToDelete = (from m in db.DangMuon
                                               where m.MaSv == masv && m.MaQtt == maqtt
                                               select m).FirstOrDefault();
                            ChiTietQtt chitietqttcu = (from m in db.ChiTietQtt
                                                       where m.MaQtt == maqtt && m.MaKc == rowToDelete.MaKc
                                                       select m).FirstOrDefault();
                            db.DangMuon.Remove(rowToDelete);
                            chitietqttcu.SoLuongCt += 1;
                            db.SaveChanges();
                            var dm = new DangMuon(masv, maqtt, makc);
                            row.NgayDoi = DateTime.Today;
                            row.TrangThai = "Hoàn thành";
                            chitietqtt.SoLuongCt -= 1;
                            db.Add(dm);
                            db.SaveChanges();
                            //string mamuon = (from m in db.Muon
                            //               where m.MaMuon == i.MaMuon
                            //               select m.MaMuon).FirstOrDefault();
                            //mamuons.Add(mamuon);
                        }
                    }
                    MoTaNhieu_Doi motaN = new MoTaNhieu_Doi();
                    motaN.listd = listd;
                    motaN.ShowDialog();
                    dtgDoi_Loaded(sender, e);
                    listm.Clear();
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Danh sách trống!");
            }
        }

        private void btnXoa_Doi_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDoi.SelectedItem != null)
            {
                MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn xóa yêu cầu mượn này? ", "Confirmation Box", MessageBoxButton.YesNo);
                if (messageChange == MessageBoxResult.Yes)
                {
                    DoiViewModel selectedRow = dtgDoi.SelectedItem as DoiViewModel;
                    string tenqtt = selectedRow.TenQtt;
                    var maqtt = (from m in db.QuanTuTrang
                                 where m.TenQtt == tenqtt
                                 select m.MaQtt).FirstOrDefault();
                    string masv = selectedRow.MaSv;
                    var rowToDelete = (from m in db.Muon
                                       where m.MaSv == masv && m.MaQtt == maqtt
                                       select m).FirstOrDefault();
                    if (rowToDelete != null)
                    {
                        // Xóa dòng từ nguồn dữ liệu
                        db.Muon.Remove(rowToDelete);
                        db.SaveChanges();
                        // Cập nhật lại DataGrid (nếu cần)
                        dtgDoi_Loaded(sender, e);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn mục cần xóa");
            }
        }

        // Class được tạo ra phục vụ cho việc xác nhận tất cả
        internal class VerifyAll
        {
            public string MaQtt { get; set; }
            public string TenQtt { get; set; }
            public string MaKc { get; set; }
            public string TenKc { get; set; }
            public int c { get; set; }
            public VerifyAll()
            {

            }
            public VerifyAll(string MaQtt, string TenQtt, string MaKc, string TenKc, int c)
            {
                this.MaQtt = MaQtt;
                this.TenQtt = TenQtt;
                this.MaKc = MaKc;
                this.TenKc = TenKc;
                this.c = c;
            }
        }

        // Lấy danh sách các đơn đổi chưa đc xác nhận
        private void dtgVerifyChange_Loaded(object sender, RoutedEventArgs e)
        {
            var change = from doi in db.Doi
                         join qtt in db.QuanTuTrang on doi.MaQtt equals qtt.MaQtt
                         join kc in db.KichCo on doi.MaKc equals kc.MaKc
                         where doi.TrangThai.Equals("Đang xử lý")
                         select new
                         {
                             doi,
                             TenQTT = qtt.TenQtt,
                             TenKC = kc.TenKc
                         };
            dtgVerifyChange.ItemsSource = change.ToList();
        }

        // Nhấn nút "Xác nhận tất cả đơn đổi"
        private void btnVerifyAllD_Click(object sender, RoutedEventArgs e)
        {
            var count = from d in db.Doi
                        join q in db.QuanTuTrang on d.MaQtt equals q.MaQtt
                        join k in db.KichCo on d.MaKc equals k.MaKc
                        where d.TrangThai.Equals("Đang xử lý")
                        group d by new
                        {
                            q.MaQtt,
                            q.TenQtt,
                            k.MaKc,
                            k.TenKc
                        } into g
                        select new
                        {
                            MaQtt = g.Key.MaQtt,
                            TenQtt = g.Key.TenQtt,
                            MaKc = g.Key.MaKc,
                            TenKc = g.Key.TenKc,
                            c = g.Count()
                        };
            List<VerifyAll> verifyAlls = new List<VerifyAll>();
            foreach (var i in count.ToList())
            {
                verifyAlls.Add(new VerifyAll(i.MaQtt, i.TenQtt, i.MaKc, i.TenKc, i.c));
            }
            foreach (var i in verifyAlls)
            {
                int having = db.ChiTietQtt.SingleOrDefault(c => c.MaQtt.Equals(i.MaQtt)
                    && c.MaKc.Equals(i.MaKc)).SoLuongCt;
                int waiting = db.Doi.Where(d => d.MaQtt.Equals(i.MaQtt)
                    && d.MaKc.Equals(i.MaKc) && d.TrangThai.Equals("Xác nhận")).Count();
                waiting += db.Muon.Where(m => m.MaQtt.Equals(i.MaQtt)
                    && m.MaKc.Equals(i.MaKc) && m.TrangThai.Equals("Xác nhận")).Count();
                if (i.c + waiting > having)
                {
                    int l = i.c + waiting - having;
                    string mes = "Số " + i.TenQtt + " " + i.TenKc + " đang có: " + having +
                    "\nSố sinh viên đang chờ lấy " + i.TenQtt + " " + i.TenKc + ": "
                    + waiting + "\nSố đơn đổi " + i.TenQtt + " " + i.TenKc +
                    " bạn đang muốn xác nhận: " + i.c + "\nSố " + i.TenQtt + " " + i.TenKc +
                    " có thể không đủ(Thiếu " + l + ")." + " Bạn có chắc muốn xác nhận tất cả?";
                    MessageBoxResult messageBoxResult = MessageBox.Show(mes, "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var id = db.Doi.Where(d => d.MaQtt.Equals(i.MaQtt) && d.MaKc.Equals(i.MaKc)
                            && d.TrangThai.Equals("Đang xử lý")).Select(d => d.MaDoi);
                        List<string> ids = new List<string>();
                        foreach (string str in id.ToList())
                        {
                            ids.Add(str);
                        }
                        foreach (string str in ids)
                        {
                            Doi doi = db.Doi.SingleOrDefault(d => d.MaDoi.Equals(str));
                            doi.TrangThai = "Xác nhận";
                            db.SaveChanges();
                        }
                        MessageBox.Show("Xác nhận " + i.c + " đơn đổi "
                            + i.TenQtt + " " + i.TenKc + " thành công", "Thông báo");
                    }
                    else if (waiting < having)
                    {
                        MessageBoxResult messageBoxResult1 = MessageBox.Show(
                            "Bạn có thể xác nhận " + (having - waiting) + " đơn đổi " + i.TenQtt
                            + i.TenKc + " mà vẫn đảm bảo cung cấp được cho tất cả sinh viên đang chờ"
                            + "\nBạn có muốn xác nhận " + (having - waiting) + " đơn đổi "
                            + i.TenQtt + i.TenKc + " đầu tiên?", "Xác nhận", MessageBoxButton.YesNo);
                        if (messageBoxResult1 == MessageBoxResult.Yes)
                        {
                            var id = db.Doi.Where(d => d.MaQtt.Equals(i.MaQtt) && d.MaKc.Equals(i.MaKc)
                                && d.TrangThai.Equals("Đang xử lý")).Select(d => d.MaDoi);
                            List<string> ids = new List<string>();
                            foreach (string str in id.ToList().Take(having - waiting))
                            {
                                ids.Add(str);
                            }
                            foreach (string str in ids)
                            {
                                Doi doi = db.Doi.SingleOrDefault(d => d.MaDoi.Equals(str));
                                doi.TrangThai = "Xác nhận";
                                db.SaveChanges();
                            }
                            MessageBox.Show("Xác nhận " + (having - waiting) + " đơn đổi "
                            + i.TenQtt + " " + i.TenKc + " thành công", "Thông báo");
                        }
                    }
                }
                else
                {
                    var id = db.Doi.Where(d => d.MaQtt.Equals(i.MaQtt) && d.MaKc.Equals(i.MaKc)
                            && d.TrangThai.Equals("Đang xử lý")).Select(d => d.MaDoi);
                    List<string> ids = new List<string>();
                    foreach (string str in id.ToList())
                    {
                        ids.Add(str);
                    }
                    foreach (string str in ids)
                    {
                        Doi doi = db.Doi.SingleOrDefault(d => d.MaDoi.Equals(str));
                        doi.TrangThai = "Xác nhận";
                        db.SaveChanges();
                    }
                    MessageBox.Show("Xác nhận " + i.c + " đơn đổi "
                        + i.TenQtt + " " + i.TenKc + " thành công", "Thông báo");
                }
            }
            dtgVerifyChange_Loaded(sender, e);
            notification();
        }

        /*=====================QUẢN LÝ TRẢ=====================*/

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
            if (selectedRows.Count() > 0)
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

        /*=====================THANH TOÁN=====================*/

        // Lấy danh sách các công nợ
        private void dtgPayment_Loaded(object sender, RoutedEventArgs e)
        {
            var debt = from cn in db.CongNo
                       join s in db.SinhVien on cn.MaSv equals s.MaSv
                       let t = cn.SoTien.ToString()
                       select new
                       {
                           cn.MaCn,
                           s.MaSv,
                           s.TenSv,
                           SoTien = t + "0",
                           cn.HanTra
                       };
            dtgPayment.ItemsSource = debt.ToList();
        }

        // Nhấn nút "Tìm kiếm"
        private void btnSearchDebt_Click(object sender, RoutedEventArgs e)
        {
            isSearched = false;
            string err = "";
            if (txtSearchDebt.Text == null || txtSearchDebt.Text == "")
            {
                err = "\nBạn cần nhập Mã sinh viên";
            }
            else if (!Regex.IsMatch(txtSearchDebt.Text, @"\d{10}"))
            {
                err = "\nMã sinh viên phải là 10 kí tự số";
            }
            if (err != "")
            {
                MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string id = txtSearchDebt.Text;
                bool exist = db.SinhVien.Any(s => s.MaSv.Equals(id));
                if (!exist)
                {
                    err = "Không tồn tại mã sinh viên này";
                    MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    bool c = db.CongNo.Any(cn => cn.MaSv.Equals(id));
                    if (!c)
                    {
                        err = "Sinh viên '" + id + "' không có nợ";
                        MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        var debt = from cn in db.CongNo
                                   join s in db.SinhVien on cn.MaSv equals s.MaSv
                                   where cn.MaSv.Equals(id)
                                   let t = cn.SoTien.ToString()
                                   select new
                                   {
                                       cn.MaCn,
                                       s.MaSv,
                                       s.TenSv,
                                       SoTien = t + "0",
                                       cn.HanTra
                                   };
                        dtgPayment.ItemsSource = debt.ToList();
                        isSearched = true;
                        temp_id = id;
                        temp_value = db.CongNo.SingleOrDefault(cn => cn.MaSv.Equals(id)).SoTien;
                        string value = temp_value.ToString();
                        MessageBox.Show("Sinh viên '" + id + "' nợ " + value.Substring(0,
                            value.Length - 1) + " nghìn đồng", "Thông báo", MessageBoxButton.OK);
                    }
                }
            }
        }

        // Sinh mã hóa đơn thanh toán mới
        public string newHD()
        {
            string id = db.HoaDonThanhToan.Max(h => h.MaHdtt);
            if (id == null)
            {
                return "REC0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "REC" + new_id.ToString().PadLeft(7, '0');
        }

        // Nhấn nút "Thanh toán"
        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            string err = "";
            if (isSearched)
            {
                if (txtValue.Text == null || txtValue.Text == "")
                {
                    err = "Bạn cần nhập số tiền thanh toán";
                }
                else if (!Regex.IsMatch(txtValue.Text, @"\d+"))
                {
                    err = "Số tiền thanh toán nhập vào phải là số";
                }
                else if (int.Parse(txtValue.Text) <= 0)
                {
                    err = "Số tiền thanh toán phải dương";
                }
                else if (int.Parse(txtValue.Text) > temp_value)
                {
                    string str = temp_value.ToString();
                    err = "Số tiền thanh toán phải nhỏ hơn hoặc bằng số tiền nợ ("
                        + str.Substring(0, str.Length - 1) + ")";
                }
                if (err != "")
                {
                    MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc muốn thanh toán " + txtValue.Text + ".000 cho sinh viên '"
                        + temp_id + "'?", "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        // Sinh hóa đơn thanh toán mới
                        HoaDonThanhToan hd = new HoaDonThanhToan();
                        hd.MaHdtt = newHD();
                        hd.MaSv = temp_id;
                        hd.NgayTra = DateTime.Now;
                        hd.SoTien = decimal.Parse(txtValue.Text);
                        db.HoaDonThanhToan.Add(hd);
                        // Trừ nợ
                        CongNo cn = db.CongNo.SingleOrDefault(c => c.MaSv.Equals(temp_id));
                        if (cn.SoTien == decimal.Parse(txtValue.Text))
                        {
                            db.CongNo.Remove(cn);
                        }
                        else
                        {
                            cn.SoTien -= decimal.Parse(txtValue.Text);
                        }
                        db.SaveChanges();
                        isSearched = false;
                        MessageBox.Show("Thanh toán thành công " + txtValue.Text
                            + ".000 cho sinh viên '" + temp_id + "'", "Thông báo");
                        btnPayCancel_Click(sender, e);
                    }
                }
            }
            else
            {
                err = "Bạn chưa tìm kiếm sinh viên thanh toán";
                MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Nhấn nút "Hủy bỏ"
        private void btnPayCancel_Click(object sender, RoutedEventArgs e)
        {
            isSearched = false;
            txtSearchDebt.Text = "";
            txtValue.Text = "";
            dtgPayment_Loaded(sender, e);
        }

        /*=====================DANH SÁCH HÓA DƠN THANH TOÁN=====================*/

        // Danh sách toàn bộ hóa đơn thanh toán
        private void dtgHDTT_Loaded(object sender, RoutedEventArgs e)
        {
            var hd = from h in db.HoaDonThanhToan
                     join s in db.SinhVien on h.MaSv equals s.MaSv
                     let t = h.SoTien.ToString()
                     select new
                     {
                         MaHD = h.MaHdtt,
                         MaSV = h.MaSv,
                         TenSV = s.TenSv,
                         SoTien = t + "0",
                         NgayTra = h.NgayTra
                     };
            dtgHDTT.ItemsSource = hd.ToList();
        }

        // Nhấn nút "Tìm kiếm"
        private void btnSearchHD_Click(object sender, RoutedEventArgs e)
        {
            string err = "";
            if (txtSearchHD.Text == null || txtSearchHD.Text == "")
            {
                err = "\nBạn cần nhập Mã sinh viên";
            }
            else if (!Regex.IsMatch(txtSearchHD.Text, @"\d{10}"))
            {
                err = "\nMã sinh viên phải là 10 kí tự số";
            }
            if (err != "")
            {
                MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string id = txtSearchHD.Text;
                bool exist = db.SinhVien.Any(s => s.MaSv.Equals(id));
                if (!exist)
                {
                    err = "Không tồn tại mã sinh viên này";
                    MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    bool c = db.HoaDonThanhToan.Any(hd => hd.MaSv.Equals(id));
                    if (!c)
                    {
                        err = "Sinh viên '" + id + "' không có hóa đơn thanh toán nào";
                        MessageBox.Show(err, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        var hd = from h in db.HoaDonThanhToan
                                 join s in db.SinhVien on h.MaSv equals s.MaSv
                                 where h.MaSv.Equals(id)
                                 let t = h.SoTien.ToString()
                                 select new
                                 {
                                     MaHD = h.MaHdtt,
                                     MaSV = h.MaSv,
                                     TenSV = s.TenSv,
                                     SoTien = t + "0",
                                     NgayTra = h.NgayTra
                                 };
                        dtgHDTT.ItemsSource = hd.ToList();
                    }
                }
            }
        }

        // Nhấn nút "Tải lại"
        private void btnReloadHD_Click(object sender, RoutedEventArgs e)
        {
            txtSearchHD.Text = "";
            dtgHDTT_Loaded(sender, e);
        }

        /*=====================THỐNG KÊ=====================*/

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
                             join c in db.DangMuon on new { b.MaQtt, b.MaKc } equals new { c.MaQtt, c.MaKc }
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
            }
            else if (selectedValue == "Khoản nợ của sinh viên")
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

        /*=====================DANH SÁCH ĐƠN ĐĂNG KÝ=====================*/

        // Load các combobox
        private void tabDSGD_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaSVGD.Text = "";
            dateGD.SelectedDate = null;
            var qtt = db.QuanTuTrang.Select(q => q);
            cbbQTTGD.ItemsSource = qtt.ToList();
            cbbQTTGD.DisplayMemberPath = "TenQtt";
            cbbQTTGD.SelectedValuePath = "MaQtt";
            cbbQTTGD.SelectedItem = null;
            List<string> tt = new List<string>();
            tt.Add("Đang xử lý");
            tt.Add("Xác nhận");
            tt.Add("Hoàn thành");
            cbbTTGD.ItemsSource = tt;
            cbbTTGD.SelectedItem = null;
        }

        // Lấy danh sách toàn bộ mượn, đổi
        private void dtgGD_Loaded(object sender, RoutedEventArgs e)
        {
            var borrowList = from borrow in db.Muon
                             join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                             join size in db.KichCo on borrow.MaKc equals size.MaKc
                             select new
                             {
                                 borrow,
                                 qtt.TenQtt,
                                 size.TenKc
                             };
            var changeList = from change in db.Doi
                             join qtt in db.QuanTuTrang on change.MaQtt equals qtt.MaQtt
                             join size in db.KichCo on change.MaKc equals size.MaKc
                             select new
                             {
                                 change,
                                 qtt.TenQtt,
                                 size.TenKc
                             };
            List<ListAction> listAction = new List<ListAction>();
            foreach (var i in borrowList)
            {
                listAction.Add(new ListAction(i.borrow.MaMuon, "Mượn", i.borrow.MaSv
                    , i.TenQtt, i.TenKc, i.borrow.TrangThai, i.borrow.NgayDk, i.borrow.NgayMuon));
            }
            foreach (var i in changeList)
            {
                listAction.Add(new ListAction(i.change.MaDoi, "Đổi", i.change.MaSv
                    , i.TenQtt, i.TenKc, i.change.TrangThai, i.change.NgayDk, i.change.NgayDoi));
            }
            dtgGD.ItemsSource = listAction;
        }

        // Intersect 2 list các object
        static List<ListAction> intersect(List<ListAction> l1, List<ListAction> l2)
        {
            var list = l1.Select(s => new
            {
                s.maGD,
                s.tenGD,
                s.maSV,
                s.qtt,
                s.kc,
                s.tt,
                s.ngayDK,
                s.ngayGD
            }).Intersect(l2.Select(t => new
            {
                t.maGD,
                t.tenGD,
                t.maSV,
                t.qtt,
                t.kc,
                t.tt,
                t.ngayDK,
                t.ngayGD
            }));
            List<ListAction> l = new List<ListAction>();
            foreach (var i in list)
            {
                l.Add(new ListAction(i.maGD, i.tenGD, i.maSV, i.qtt, i.kc, i.tt, i.ngayDK, i.ngayGD));
            }
            return l;
        }

        // Nhấn nút "Áp dụng"
        private void btnApplyGD_Click(object sender, RoutedEventArgs e)
        {
            bool s = false, d = false, q = false, t = false;
            List<ListAction> listActSV = new List<ListAction>();
            List<ListAction> listActDate = new List<ListAction>();
            List<ListAction> listActQTT = new List<ListAction>();
            List<ListAction> listActTT = new List<ListAction>();
            // lấy danh sách giao dịch theo mã sinh viên
            if (txtMaSVGD.Text != "")
            {
                var borrowList = from borrow in db.Muon
                                 join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on borrow.MaKc equals size.MaKc
                                 where borrow.MaSv.Equals(txtMaSVGD.Text)
                                 select new
                                 {
                                     borrow,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                var changeList = from change in db.Doi
                                 join qtt in db.QuanTuTrang on change.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on change.MaKc equals size.MaKc
                                 where change.MaSv.Equals(txtMaSVGD.Text)
                                 select new
                                 {
                                     change,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                foreach (var i in borrowList)
                {
                    listActSV.Add(new ListAction(i.borrow.MaMuon, "Mượn", i.borrow.MaSv
                        , i.TenQtt, i.TenKc, i.borrow.TrangThai, i.borrow.NgayDk, i.borrow.NgayMuon));
                }
                foreach (var i in changeList)
                {
                    listActSV.Add(new ListAction(i.change.MaDoi, "Đổi", i.change.MaSv
                        , i.TenQtt, i.TenKc, i.change.TrangThai, i.change.NgayDk, i.change.NgayDoi));
                }
                s = true;
            }
            // lấy danh sách giao dịch theo ngày giao dịch
            if (dateGD.SelectedDate != null)
            {
                var borrowList = from borrow in db.Muon
                                 join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on borrow.MaKc equals size.MaKc
                                 where DateTime.Compare(borrow.NgayDk, (DateTime)dateGD.SelectedDate) == 0
                                 select new
                                 {
                                     borrow,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                var changeList = from change in db.Doi
                                 join qtt in db.QuanTuTrang on change.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on change.MaKc equals size.MaKc
                                 where DateTime.Compare(change.NgayDk, (DateTime)dateGD.SelectedDate) == 0
                                 select new
                                 {
                                     change,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                foreach (var i in borrowList)
                {
                    listActDate.Add(new ListAction(i.borrow.MaMuon, "Mượn", i.borrow.MaSv
                        , i.TenQtt, i.TenKc, i.borrow.TrangThai, i.borrow.NgayDk, i.borrow.NgayMuon));
                }
                foreach (var i in changeList)
                {
                    listActDate.Add(new ListAction(i.change.MaDoi, "Đổi", i.change.MaSv
                        , i.TenQtt, i.TenKc, i.change.TrangThai, i.change.NgayDk, i.change.NgayDoi));
                }
                d = true;
            }
            // lấy danh sách giao dịch theo quân tư trang
            if (cbbQTTGD.SelectedItem != null && cbbQTTGD.Text != "")
            {
                var borrowList = from borrow in db.Muon
                                 join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on borrow.MaKc equals size.MaKc
                                 where borrow.MaQtt.Equals(cbbQTTGD.SelectedValue.ToString())
                                 select new
                                 {
                                     borrow,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                var changeList = from change in db.Doi
                                 join qtt in db.QuanTuTrang on change.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on change.MaKc equals size.MaKc
                                 where change.MaQtt.Equals(cbbQTTGD.SelectedValue.ToString())
                                 select new
                                 {
                                     change,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                foreach (var i in borrowList)
                {
                    listActQTT.Add(new ListAction(i.borrow.MaMuon, "Mượn", i.borrow.MaSv
                        , i.TenQtt, i.TenKc, i.borrow.TrangThai, i.borrow.NgayDk, i.borrow.NgayMuon));
                }
                foreach (var i in changeList)
                {
                    listActQTT.Add(new ListAction(i.change.MaDoi, "Đổi", i.change.MaSv
                        , i.TenQtt, i.TenKc, i.change.TrangThai, i.change.NgayDk, i.change.NgayDoi));
                }
                q = true;
            }
            // lấy danh sách giao dịch theo trạng thái
            if (cbbTTGD.SelectedItem != null && cbbTTGD.SelectedValue.ToString() != "")
            {
                var borrowList = from borrow in db.Muon
                                 join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on borrow.MaKc equals size.MaKc
                                 where borrow.TrangThai.Equals(cbbTTGD.SelectedValue.ToString())
                                 select new
                                 {
                                     borrow,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                var changeList = from change in db.Doi
                                 join qtt in db.QuanTuTrang on change.MaQtt equals qtt.MaQtt
                                 join size in db.KichCo on change.MaKc equals size.MaKc
                                 where change.TrangThai.Equals(cbbTTGD.SelectedValue.ToString())
                                 select new
                                 {
                                     change,
                                     qtt.TenQtt,
                                     size.TenKc
                                 };
                foreach (var i in borrowList)
                {
                    listActTT.Add(new ListAction(i.borrow.MaMuon, "Mượn", i.borrow.MaSv
                        , i.TenQtt, i.TenKc, i.borrow.TrangThai, i.borrow.NgayDk, i.borrow.NgayMuon));
                }
                foreach (var i in changeList)
                {
                    listActTT.Add(new ListAction(i.change.MaDoi, "Đổi", i.change.MaSv
                        , i.TenQtt, i.TenKc, i.change.TrangThai, i.change.NgayDk, i.change.NgayDoi));
                }
                t = true;
            }
            // Bộ lọc :)))))
            if (s && d && q && t)
            {
                List<ListAction> list = intersect(listActSV, listActDate);
                list = intersect(list, listActQTT);
                list = intersect(list, listActTT);
                dtgGD.ItemsSource = list;
            }
            else if (s && d && q)
            {
                List<ListAction> list = intersect(listActSV, listActDate);
                list = intersect(list, listActQTT);
                dtgGD.ItemsSource = list;
            }
            else if (s && d && t)
            {
                List<ListAction> list = intersect(listActSV, listActDate);
                list = intersect(list, listActTT);
                dtgGD.ItemsSource = list;
            }
            else if (s && t && q)
            {
                List<ListAction> list = intersect(listActSV, listActQTT);
                list = intersect(list, listActTT);
                dtgGD.ItemsSource = list;
            }
            else if (t && d && q)
            {
                List<ListAction> list = intersect(listActQTT, listActDate);
                list = intersect(list, listActTT);
                dtgGD.ItemsSource = list;
            }
            else if (s && d)
            {
                List<ListAction> list = intersect(listActSV, listActDate);
                dtgGD.ItemsSource = list;
            }
            else if (s && q)
            {
                List<ListAction> list = intersect(listActSV, listActQTT);
                dtgGD.ItemsSource = list;
            }
            else if (s && t)
            {
                List<ListAction> list = intersect(listActSV, listActTT);
                dtgGD.ItemsSource = list;
            }
            else if (d && q)
            {
                List<ListAction> list = intersect(listActQTT, listActDate);
                dtgGD.ItemsSource = list;
            }
            else if (d && t)
            {
                List<ListAction> list = intersect(listActTT, listActDate);
                dtgGD.ItemsSource = list;
            }
            else if (q && t)
            {
                List<ListAction> list = intersect(listActQTT, listActTT);
                dtgGD.ItemsSource = list;
            }
            else if (s)
            {
                dtgGD.ItemsSource = listActSV.ToList();
            }
            else if (d)
            {
                dtgGD.ItemsSource = listActDate.ToList();
            }
            else if (q)
            {
                dtgGD.ItemsSource = listActQTT.ToList();
            }
            else if (t)
            {
                dtgGD.ItemsSource = listActTT.ToList();
            }
            else
            {
                dtgGD_Loaded(sender, e);
            }
        }

        // Nhấn nút "Hủy bỏ"
        private void btnRemoveAllDG_Click(object sender, RoutedEventArgs e)
        {
            tabDSGD_Loaded(sender, e);
            dtgGD_Loaded(sender, e);
        }

        // Nhấn nút "Xem"
        private void btnDetailHD_Click(object sender, RoutedEventArgs e)
        {
            Type t = dtgGD.SelectedItem.GetType();
            PropertyInfo[] p = t.GetProperties();
            string sId, sName, cStart, cFinish, qttId, qttName, kId, kName,
                createDate, actionDate, actionState;
            string actionType = p[1].GetValue(dtgGD.SelectedValue).ToString();
            string actionId = p[0].GetValue(dtgGD.SelectedValue).ToString();
            sId = p[2].GetValue(dtgGD.SelectedValue).ToString();
            if (actionType.Equals("Mượn"))
            {
                var borrow = from b in db.Muon
                             join k in db.KichCo on b.MaKc equals k.MaKc
                             join q in db.QuanTuTrang on b.MaQtt equals q.MaQtt
                             join s in db.SinhVien on b.MaSv equals s.MaSv
                             join kh in db.KhoaHoc on s.MaKh equals kh.MaKh
                             where b.MaMuon == actionId
                             select new
                             {
                                 q,
                                 k,
                                 b,
                                 s,
                                 kh
                             };
                sName = borrow.First().s.TenSv;
                cStart = borrow.First().kh.NgayBd.Date.ToString("dd/MM/yyyy");
                cFinish = borrow.First().kh.NgayKt.Date.ToString("dd/MM/yyyy");
                qttId = borrow.First().q.MaQtt;
                qttName = borrow.First().q.TenQtt;
                kId = borrow.First().k.MaKc;
                kName = borrow.First().k.TenKc;
                createDate = borrow.First().b.NgayDk.Date.ToString("dd/MM/yyyy");
                if (borrow.First().b.NgayMuon == null)
                {
                    actionDate = "";
                }
                else
                {
                    actionDate = borrow.First().b.NgayMuon.GetValueOrDefault().ToString("dd/MM/yyyy");
                }
                actionState = borrow.First().b.TrangThai;

            }
            else
            {
                var change = from c in db.Doi
                             join k in db.KichCo on c.MaKc equals k.MaKc
                             join q in db.QuanTuTrang on c.MaQtt equals q.MaQtt
                             join s in db.SinhVien on c.MaSv equals s.MaSv
                             join kh in db.KhoaHoc on s.MaKh equals kh.MaKh
                             where c.MaDoi == actionId
                             select new
                             {
                                 q,
                                 k,
                                 c,
                                 s,
                                 kh
                             };
                sName = change.First().s.TenSv;
                cStart = change.First().kh.NgayBd.Date.ToString("dd/MM/yyyy");
                cFinish = change.First().kh.NgayKt.Date.ToString("dd/MM/yyyy");
                qttId = change.First().q.MaQtt;
                qttName = change.First().q.TenQtt;
                kId = change.First().k.MaKc;
                kName = change.First().k.TenKc;
                createDate = change.First().c.NgayDk.Date.ToString("dd/MM/yyyy");
                if (change.First().c.NgayDoi == null)
                {
                    actionDate = "";
                }
                else
                {
                    actionDate = change.First().c.NgayDoi.GetValueOrDefault().ToString("dd/MM/yyyy");
                }
                actionState = change.First().c.TrangThai;
            }
            ActionDetail actionDetail = new ActionDetail(actionType, actionId, sId, sName, cStart, cFinish,
                qttId, qttName, kId, kName, createDate, actionDate, actionState);
            actionDetail.Show();
        }

        // Nhấn nút "Hủy"
        private void btnRemoveHD_Click(object sender, RoutedEventArgs e)
        {
            Type t = dtgGD.SelectedItem.GetType();
            PropertyInfo[] p = t.GetProperties();
            string status = p[5].GetValue(dtgGD.SelectedValue).ToString();
            if (status.Equals("Hoàn thành"))
            {
                MessageBox.Show("Không thể hủy đơn có trạng thái là '" + status
                    + "'", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string actionId = p[0].GetValue(dtgGD.SelectedValue).ToString();
                string actionType = p[1].GetValue(dtgGD.SelectedValue).ToString();
                if (actionType.Equals("Mượn"))
                {
                    Muon b = db.Muon.SingleOrDefault(m => m.MaMuon.Equals(actionId));
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn hủy đơn '" + actionId + "'",
                        "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        db.Muon.Remove(b);
                        db.SaveChanges();
                        MessageBox.Show("Hủy đơn '" + actionId + "' thành công",
                            "Thông báo", MessageBoxButton.OK);
                    }
                }
                else
                {
                    Doi c = db.Doi.SingleOrDefault(d => d.MaDoi.Equals(actionId));
                    MessageBoxResult messageBoxResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn hủy đơn '" + actionId + "'",
                        "Xác nhận", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        db.Doi.Remove(c);
                        db.SaveChanges();
                        MessageBox.Show("Hủy đơn '" + actionId + "' thành công",
                            "Thông báo", MessageBoxButton.OK);
                    }
                }
            }
        }
    }
}
