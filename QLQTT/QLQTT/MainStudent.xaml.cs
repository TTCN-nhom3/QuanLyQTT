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
using System.IO;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace QLQTT
{
    /// <summary>
    /// Giao diện Sinh viên
    /// </summary>
    public partial class MainStudent : Window
    {
        QLQTTContext db = new QLQTTContext();
        SinhVien sv = new SinhVien(); // Sinh viên đang đăng nhập
        KhoaHoc kh = new KhoaHoc(); // Khóa học mà sinh viên đang đăng nhập học
        public static MainStudent instance;
        public string id;
        public bool chosen;
        public MainStudent(string id)
        {
            InitializeComponent();
            instance = this;
            this.sv = db.SinhVien.SingleOrDefault(s => s.MaSv.Equals(id));
            this.kh = db.KhoaHoc.SingleOrDefault(k => k.MaKh.Equals(sv.MaKh));
            lblExitWarning.Content = "Nếu bạn nhất nút 'X' màu đỏ, ứng dụng sẽ tắt ngay lập tức!";
        }

        /*CHUYỂN ĐỔI DỮ LIỆU ẢNH*/
        public BitmapFrame convert(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return BitmapFrame.Create(ms, BitmapCreateOptions.None,
                    BitmapCacheOption.OnLoad);
            }
        }

        /*CẬP NHẬT SỐ TIỀN NỢ*/
        public void UpdateDebt()
        {
            string str;
            CongNo d = db.CongNo.SingleOrDefault(ele => ele.MaSv.Equals(sv.MaSv));
            if (d == null)
            {
                lblMoney.Content = "Nợ: 0 đồng";
                str = "0";
            }
            else
            {
                str = d.SoTien.ToString();
                lblMoney.Content = "Nợ: " + str.Substring(0, str.Length - 1) + " đồng";
                lblDate.Content = "Hạn trả: " + d.HanTra.ToString("dd/MM/yyyy");
            }
            var tt = db.HoaDonThanhToan.Where(ele => ele.MaSv.Equals(sv.MaSv));
            decimal value = 0;
            foreach (var i in tt)
            {
                value += (decimal)i.SoTien;
            }
            lblTotalPay.Content = "Tổng số tiền đã thanh toán: "
                + value.ToString().Substring(0, str.Length - 1) + " đồng";
            value += decimal.Parse(str);
            str = value.ToString();
            lblTotalValue.Content = "Tổng giá trị đã làm mất: "
                + str.Substring(0, str.Length - 1) + " đồng";
        }

        /*LOAD DỮ LIỆU BAN ĐẦU*/
        private void MainStudent_Loaded(object sender, RoutedEventArgs e)
        {
            lblId.Content = sv.MaSv;
            lblName.Content = sv.TenSv;
            byte[] byteArray = sv.Anh.ToArray();
            img.Source = convert(byteArray);
            UpdateDebt();
        }

        /*ĐĂNG XUẤT*/
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var logIn = new LogIn();
            logIn.Show();
            this.Close();
        }

        /*===================ĐĂNG KÝ QUÂN TƯ TRANG===================*/

        // Load combobox
        private void cbbKC_Loaded(object sender, RoutedEventArgs e)
        {
            var kc = from k in db.KichCo
                     select k;
            List<KichCo> listKc = new List<KichCo>();
            foreach (KichCo i in kc)
            {
                if (!i.MoTa.Equals("all"))
                    listKc.Add(i);
            }
            cbbKC.ItemsSource = listKc;
            cbbKC.SelectedIndex = 0;
            cbbKC.DisplayMemberPath = "TenKc";
            cbbKC.SelectedValuePath = "MaKc";
        }

        // Sinh mã mượn mới
        public string newMuon()
        {
            string id = db.Muon.Max(d => d.MaMuon);
            if (id == null)
            {
                return "BOR0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "BOR" + new_id.ToString().PadLeft(7, '0');
        }

        // Kích nút "Đăng ký"
        private void btnDK_Click(object sender, RoutedEventArgs e)
        {
            var checkData = false; //biến check dữ liệu trống
            var checkAdded = false; // biến check xem thêm thành công chưa 
            //Lay ma kc 
            var makc = "";
            KichCo kc = (KichCo)cbbKC.SelectedItem;
            if (kc != null)
            {
                makc = db.KichCo.SingleOrDefault(x => x.MaKc.Equals(kc.MaKc)).MaKc;
            }
            else
            {
                MessageBox.Show("Bạn cần chọn quân tư trang và kích cỡ!");
                return;
            }

            //Lấy danh sách đang mượn của thằng sv đăng nhập
            var dm = from d in db.DangMuon
                     where d.MaSv == sv.MaSv
                     select d;
            //Lấy danh sách chi tiet qtt theo makc
            List<DangMuon> listDm = new List<DangMuon>();
            var qtt = from f in db.ChiTietQtt
                      where f.MaKc == makc
                      select f;
            List<ChiTietQtt> listct = new List<ChiTietQtt>();
            //Lấy danh sách masv co status la "dang xu ly" trong bang Muon 
            var m = from h in db.Muon
                    where h.MaSv == sv.MaSv && h.TrangThai == "Đang xử lý"
                    select h;
            List<Muon> listMuon = new List<Muon>();
            foreach (DangMuon i in dm)
            {
                listDm.Add(i);
            }
            foreach (ChiTietQtt j in qtt)
            {
                listct.Add(j);
            }
            foreach (Muon k in m)
            {
                listMuon.Add(k);
            }
            //Duyệt các dòng của DataGrid 
            foreach (SelectedQTT item in dtgDK.ItemsSource)
            {
                // Kiểm tra xem có trường nào để trống không 
                if (item.isChecked && cbbKC.SelectedItem != null)
                {
                    checkData = true; break;
                }

            }
            if (checkData)
            {
                var checkHaveError = false;
                var checkNotExistQTT = true;
                var checkHaveSize = true;
                var checkNotExistOnGoingStatus = true;

                foreach (SelectedQTT item in dtgDK.ItemsSource)
                {
                    //check xem dòng nào có checkBox được chọn           
                    if (item.isChecked)
                    {
                        //Check xem mã QTT của dòng đó có nằm trong list đang mượn đã lấy ở trên không 
                        if (listDm.Any(d => d.MaQtt == item.qtt.MaQtt))
                        {
                            checkNotExistQTT = false;
                            checkHaveError = true;

                        }
                        if (!listct.Any(d => d.MaQtt == item.qtt.MaQtt))
                        {
                            checkHaveSize = false;
                            checkHaveError = true;

                        }
                        if (listMuon.Any(d => d.MaQtt == item.qtt.MaQtt))
                        {
                            checkNotExistOnGoingStatus = false;
                            checkHaveError = true;

                        }
                        if (checkHaveError)
                        { break; }

                    }

                }
                foreach (SelectedQTT item in dtgDK.ItemsSource)
                {

                    // Kiểm tra giá trị của thuộc tính IsChecked của từng đối tượng trong danh sách
                    if (item.isChecked)
                    {
                        //Nếu chưa tồn tại mã QTT trong list Đang mượn 
                        if (checkNotExistQTT)
                        {
                            if (checkHaveSize)
                            {
                                if (checkNotExistOnGoingStatus)
                                {
                                    //Code thêm vào bảng mượn 

                                    var mamuon = newMuon();
                                    DateTime today = DateTime.Today;

                                    string maqtt = item.qtt.MaQtt;

                                    //var kichco = (KichCo)cbbKC.SelectedItem;
                                    //string makichco = kc.MaKc;
                                    Muon hdm = new Muon(mamuon, sv.MaSv, maqtt, makc, today, default, null);
                                    //Tạo bản sao của collection ban đầu, không có là lỗi, dhs 
                                    List<EntityEntry<Muon>> entriesCopy = db.ChangeTracker.Entries<Muon>().ToList();

                                    // Ngừng theo dõi các thực thể Muon hiện tại trong Context (nếu có)
                                    foreach (var entry in entriesCopy)
                                    {
                                        if (entry.Entity.MaMuon == hdm.MaMuon)
                                        {
                                            entry.State = EntityState.Detached;
                                        }
                                    }

                                    db.Muon.Add(hdm);
                                    db.SaveChanges();
                                    checkAdded = true;
                                    // Cập nhật lại danh sách listMuon
                                    listMuon = (from h in db.Muon
                                                where h.MaSv == sv.MaSv && h.TrangThai == "Đang xử lý"
                                                select h).ToList();
                                }
                                else
                                {
                                    MessageBox.Show("Đăng ký mượn không thành công, không được mượn đồ có đơn đang xử lý");
                                    break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Đăng ký mượn không thành công, chọn sai kích cỡ");
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Đăng ký mượn không thành công, không được đăng ký mượn đồ đã có, nếu bị mất, hãy báo mất! ");
                            break;
                        }


                    }

                }
                //Nếu đã mượn thành công 
                if (checkAdded)
                {
                    MessageBox.Show("Đăng ký mượn thành công! ");
                }
            }
            else
            {
                MessageBox.Show("Bạn cần chọn quân tư trang và kích cỡ!");
            }
            dtg_processing_Loaded(sender, e);
        }

        // Chọn 1 kích cỡ trong combobox
        private void cbbKC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            KichCo kc = (KichCo)cbbKC.SelectedItem;

            if (kc != null)
            {
                txbl_mota.Text = db.KichCo.SingleOrDefault(x => x.MaKc.Equals(kc.MaKc)).MoTa;
            }
        }

        // Load dữ liệu bảng quân tư trang
        private void dtgDK_Loaded(object sender, RoutedEventArgs e)
        {
            var qtt = from k in db.QuanTuTrang
                      select k;

            List<SelectedQTT> listqtt = new List<SelectedQTT>();
            List<QuanTuTrang> l = new List<QuanTuTrang>();
            foreach (var i in qtt)
            {

                SelectedQTT q = new SelectedQTT(i, false);
                listqtt.Add(q);
            }
            dtgDK.ItemsSource = listqtt;
        }

        // Kích nút "Xóa hết"
        private void delAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn xóa toàn bộ yêu cầu mượn? ", "Confirmation Box", MessageBoxButton.YesNo);
            if (messageChange == MessageBoxResult.Yes)
            {
                using (var dbContext = new QLQTTContext())
                {
                    var entities = dbContext.Set<Muon>().ToList();
                    dbContext.Set<Muon>().RemoveRange(entities);
                    dbContext.SaveChanges();
                }
                dtg_processing_Loaded(sender, e);
            }
            else
            {
                return;
            }

        }

        // Kích nút "Xóa mục đã chọn"
        private void delOp_Click(object sender, RoutedEventArgs e)
        {

            if (dtg_processing.SelectedItem != null)
            {
                MessageBoxResult messageChange = MessageBox.Show("Bạn có chắc chắn muốn xóa yêu cầu mượn này? ", "Confirmation Box", MessageBoxButton.YesNo);
                if (messageChange == MessageBoxResult.Yes)
                {
                    ProcessingBor selectedRow = dtg_processing.SelectedItem as ProcessingBor;
                    string maqtt = selectedRow.MaQtt;
                    string masv = selectedRow.MaSv;
                    var rowToDelete = (from m in db.Muon
                                       where m.MaSv == sv.MaSv && m.MaQtt == maqtt
                                       select m).FirstOrDefault();
                    if (rowToDelete != null)
                    {
                        // Xóa dòng từ nguồn dữ liệu
                        db.Muon.Remove(rowToDelete);
                        db.SaveChanges();

                        // Cập nhật lại DataGrid (nếu cần)

                        dtg_processing_Loaded(sender, e);
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

        // Load dữ liệu bảng đang xử lý
        private void dtg_processing_Loaded(object sender, RoutedEventArgs e)
        {



            List<Muon> muon = (from m in db.Muon
                               where m.MaSv == sv.MaSv && m.TrangThai != "Hoàn thành"
                               select m).ToList();

            List<Doi> doi = (from m in db.Doi
                             where m.MaSv == sv.MaSv && m.TrangThai != "Hoàn thành"
                             select m).ToList();
            List<ProcessingBor> listPB = new List<ProcessingBor>();

            foreach (var i in muon)
            {
                var tenqtt = (from t in db.QuanTuTrang
                              where t.MaQtt == i.MaQtt
                              select t.TenQtt).FirstOrDefault();

                var tenkc = (from t in db.KichCo
                             where t.MaKc == i.MaKc
                             select t.TenKc).FirstOrDefault();

                ProcessingBor q = new ProcessingBor(sv.MaSv, i.MaQtt, tenqtt, tenkc, i.NgayDk, "Mượn");
                listPB.Add(q);
            }
            foreach (var i in doi)
            {
                var tenqtt = (from t in db.QuanTuTrang
                              where t.MaQtt == i.MaQtt
                              select t.TenQtt).FirstOrDefault();

                var tenkc = (from t in db.KichCo
                             where t.MaKc == i.MaKc
                             select t.TenKc).FirstOrDefault();

                ProcessingBor q = new ProcessingBor(sv.MaSv, i.MaQtt, tenqtt, tenkc, i.NgayDk, "Đổi");
                listPB.Add(q);
            }
            listPB = listPB.OrderByDescending(q => q.NgayDk).ToList();
            dtg_processing.ItemsSource = listPB;

        }

        /*===================DANH SÁCH QUÂN TƯ TRANG===================*/

        // Các quân tư trang đang mượn
        private void dtgBorrowing_Loaded(object sender, RoutedEventArgs e)
        {
            var list = from borrow in db.DangMuon
                       join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                       join size in db.KichCo on borrow.MaKc equals size.MaKc
                       where borrow.MaSv == sv.MaSv
                       select new
                       {
                           qtt,
                           size
                       };
            List<SelectedBorrowedQTT> listqtt = new List<SelectedBorrowedQTT>();
            foreach (var i in list)
            {
                listqtt.Add(new SelectedBorrowedQTT(i.qtt, false, i.size, false));
            }
            // Viết 2 vòng lặp vì nếu hàm isChanging xuất hiện trong vòng lặp trên
            // sẽ xuất hiện lỗi do có 2 truy vấn đang thực hiện cùng lúc dẫn đến lỗi
            // (Ngôn ngữ yêu cầu phải close 1 trong 2 truy vấn)
            foreach (SelectedBorrowedQTT i in listqtt)
            {
                if (isChanging(i.qtt.MaQtt))
                {
                    i.changing = true;
                }
            }
            dtgBorrowing.ItemsSource = listqtt;
        }

        // Kiểm tra xem quân tư trang này có đang chờ đổi hay không
        public bool isChanging(string maQtt)
        {
            var change = db.Doi.SingleOrDefault(c => c.MaSv.Equals(sv.MaSv) && c.MaQtt.Equals(maQtt)
            && !c.TrangThai.Equals("Hoàn thành"));
            if (change == null)
            {
                return false;
            }
            return true;
        }

        // Kiểm tra quân tư trang chờ đổi, hiện cảnh báo nếu có
        public void checkError()
        {
            string err = "";
            int c = 0;
            foreach (SelectedBorrowedQTT i in dtgBorrowing.ItemsSource)
            {
                if (i.isChecked)
                {
                    if (i.changing)
                    {
                        if (c > 0)
                        {
                            err += ", ";
                        }
                        err += i.qtt.TenQtt;
                        c++;
                    }
                }
            }
            if (err.Equals(""))
            {
                txtWarning.Text = "";
            }
            else
            {
                txtWarning.Text = "Bạn đang chờ để đổi: " + err + ".";
            }
        }

        // Chọn hoặc bỏ chọn 1 quân tư trang
        private void cbxBorrowing_Click(object sender, RoutedEventArgs e)
        {
            checkError();
        }

        // Sinh mã Công nợ mới
        public string newCN()
        {
            string id = db.CongNo.Max(c => c.MaCn);
            if (id == null)
            {
                return "DEB0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "DEB" + new_id.ToString().PadLeft(7, '0');
        }

        // Sinh mã Mất mới
        public string newMat()
        {
            string id = db.Mat.Max(m => m.MaMat);
            if (id == null)
            {
                return "LOS0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "LOS" + new_id.ToString().PadLeft(7, '0');
        }

        // Sinh mã Đổi mới
        public string newDoi()
        {
            string id = db.Doi.Max(d => d.MaDoi);
            if (id == null)
            {
                return "CHA0000001";
            }
            int new_id = int.Parse(id.Substring(3)) + 1;
            return "CHA" + new_id.ToString().PadLeft(7, '0');
        }

        // Nhấn nút Báo mất
        private void btnBM_Click(object sender, RoutedEventArgs e)
        {
            // Danh sách quân tư trang đã chọn
            List<SelectedBorrowedQTT> listChecked = new List<SelectedBorrowedQTT>();
            foreach (SelectedBorrowedQTT i in dtgBorrowing.ItemsSource)
            {
                if (i.isChecked)
                {
                    listChecked.Add(i);
                }
            }
            if (listChecked.Count() == 0)
            {
                MessageBox.Show("Bạn chưa chọn Quân tư trang nào");
            }
            else
            {
                foreach (SelectedBorrowedQTT i in listChecked)
                {
                    string mes = "Bạn có chắc chắn muốn báo mất '" + i.qtt.TenQtt + "'?";
                    if (isChanging(i.qtt.MaQtt))
                    {
                        mes += "\nBạn đang chờ để đổi '" + i.qtt.TenQtt +
                            "'. Nếu báo mất, việc đăng ký đổi " + i.qtt.TenQtt + " sẽ bị hủy bỏ.\n";
                    }
                    MessageBoxResult messageLoss = MessageBox.Show(mes +
                        "\nHành động này không thể được thu hồi", "Xác nhận",
                        MessageBoxButton.YesNo);
                    if (messageLoss == MessageBoxResult.Yes)
                    {
                        // Cộng nợ cho sv
                        CongNo kn = db.CongNo.SingleOrDefault(t => t.MaSv.Equals(sv.MaSv));
                        if (kn == null)
                        {
                            CongNo cn = new CongNo();
                            cn.MaCn = newCN();
                            cn.MaSv = sv.MaSv;
                            cn.SoTien = i.qtt.GiaTien;
                            cn.HanTra = kh.NgayKt.AddDays(7);
                            db.CongNo.Add(cn);
                        }
                        else
                        {
                            kn.SoTien += i.qtt.GiaTien;
                        }
                        // Thêm bản ghi vào bảng Mat
                        Mat mat = new Mat();
                        mat.MaMat = newMat();
                        mat.MaSv = sv.MaSv;
                        mat.MaQtt = i.qtt.MaQtt;
                        mat.MaKc = i.kc.MaKc;
                        mat.NgayMat = DateTime.Now;
                        mat.SoTien = i.qtt.GiaTien;
                        db.Mat.Add(mat);
                        // Xóa bản ghi tương ứng khỏi bảng DangMuon
                        DangMuon del_borrowing = db.DangMuon.SingleOrDefault(d => d.MaSv.Equals(sv.MaSv)
                        && d.MaQtt.Equals(i.qtt.MaQtt));
                        db.DangMuon.Remove(del_borrowing);
                        // Nếu quân tư trang này đang chờ đổi thì xóa bản ghi tương ứng khỏi bảng Doi
                        Doi del_change = db.Doi.SingleOrDefault(c => c.MaSv.Equals(sv.MaSv) &&
                        c.MaQtt.Equals(i.qtt.MaQtt) && !c.TrangThai.Equals("Hoàn thành"));
                        if (del_change != null)
                        {
                            db.Doi.Remove(del_change);
                        }
                        db.SaveChanges();
                        MessageBox.Show("Báo mất '" + i.qtt.TenQtt + "' thành công!");
                    }
                }
                dtgBorrowing_Loaded(sender, e);
                dtgLoss_Load(sender, e);
                UpdateDebt();
            }
        }

        // Nhấn nút Đổi
        private void btnDoi_Click(object sender, RoutedEventArgs e)
        {
            List<SelectedBorrowedQTT> listCheck = new List<SelectedBorrowedQTT>();
            foreach (SelectedBorrowedQTT i in dtgBorrowing.ItemsSource)
            {
                if (i.isChecked)
                {
                    listCheck.Add(i);
                }
            }
            if (listCheck.Count() == 0)
            {
                MessageBox.Show("Bạn chưa chọn Quân tư trang nào");
            }
            else
            {
                bool checkChanging = true;
                foreach (SelectedBorrowedQTT i in listCheck)
                {
                    if (isChanging(i.qtt.MaQtt))
                    {
                        MessageBox.Show("Không thể đăng ký đổi bởi vì bạn đã đăng ký đổi '" + i.qtt.TenQtt
                            + "' trước đó!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        checkChanging = false;
                        break;
                    }
                }
                if (checkChanging)
                {
                    foreach (SelectedBorrowedQTT i in listCheck)
                    {
                        chosen = false;
                        MessageBoxResult messageChange = MessageBox.Show(
                            "Bạn có chắc chắn muốn đăng ký đổi '" +
                            i.qtt.TenQtt + "'?", "Xác nhận", MessageBoxButton.YesNo);
                        if (messageChange == MessageBoxResult.Yes)
                        {
                            if (!i.kc.TenKc.Equals("None"))
                            {
                                SelectSizeForChangeAction change = new
                                    SelectSizeForChangeAction(i.kc.MaKc, i.kc.TenKc);
                                this.Hide();
                                change.ShowDialog();
                                this.Show();
                            }
                            else
                            {
                                id = i.kc.MaKc;
                                chosen = true;
                            }
                            if (chosen)
                            {
                                Doi doi = new Doi();
                                doi.MaDoi = newDoi();
                                doi.MaSv = sv.MaSv;
                                doi.MaQtt = i.qtt.MaQtt;
                                doi.MaKc = id;
                                doi.NgayDk = DateTime.Now;
                                db.Doi.Add(doi);
                                db.SaveChanges();
                                string tenKc = db.KichCo.SingleOrDefault(k => k.MaKc.Equals(id)).TenKc;
                                MessageBox.Show("Đăng ký đổi '" + i.qtt.TenQtt + "' thành công!"
                                    + "\n" + i.kc.TenKc + " -> " + tenKc);
                            }
                            else
                            {
                                MessageBox.Show("Đăng ký đổi " + i.qtt.TenQtt + " KHÔNG thành công");
                            }
                        }
			else
			{
				MessageBox.Show("Đăng ký đổi " + i.qtt.TenQtt + " KHÔNG thành công");
			}
                    }
                    dtgBorrowing_Loaded(sender, e);
                }
            }
        }

        // Nhấn nút "Chọn tất cả"
        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            List<SelectedBorrowedQTT> listAll = new List<SelectedBorrowedQTT>();
            foreach (SelectedBorrowedQTT i in dtgBorrowing.ItemsSource)
            {
                if (!i.isChecked)
                {
                    i.isChecked = true;
                }
                listAll.Add(i);
            }
            dtgBorrowing.ItemsSource = listAll;
            checkError();
        }

        // Nhấn nút "Bỏ chọn"
        private void btnNone_Click(object sender, RoutedEventArgs e)
        {
            List<SelectedBorrowedQTT> listAll = new List<SelectedBorrowedQTT>();
            foreach (SelectedBorrowedQTT i in dtgBorrowing.ItemsSource)
            {
                if (i.isChecked)
                {
                    i.isChecked = false;
                }
                listAll.Add(i);
            }
            dtgBorrowing.ItemsSource = listAll;
            txtWarning.Text = "";
        }

        // Phần cảnh báo biến mất khi chuyển tab
        private void tabDS_LostFocus(object sender, RoutedEventArgs e)
        {
            txtWarning.Text = "";
        }

        // Các quân tư trang đã mất
        private void dtgLoss_Load(object sender, RoutedEventArgs e)
        {
            var loss = from q in db.QuanTuTrang
                       join m in db.Mat on q.MaQtt equals m.MaQtt
                       join k in db.KichCo on m.MaKc equals k.MaKc
                       where m.MaSv == sv.MaSv
                       select new
                       {
                           qtt = q.TenQtt,
                           kc = k.TenKc,
                           date = m.NgayMat,
                           value = m.SoTien
                       };
            dtgLoss.ItemsSource = loss.ToList();
        }

        /*===================CÁC ĐƠN ĐĂNG KÝ ĐÃ XỬ LÝ===================*/

        // Danh sách các đươn đăng ký đã xưt lý
        private void dtgHD_Loaded(object sender, RoutedEventArgs e)
        {
            var borrowList = from borrow in db.Muon
                             join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                             join size in db.KichCo on borrow.MaKc equals size.MaKc
                             where borrow.MaSv == sv.MaSv && borrow.TrangThai != "Đang xử lý"
                             select new
                             {
                                 id = borrow.MaMuon,
                                 GiaoDich = "Mượn",
                                 qtt.TenQtt,
                                 size.TenKc,
                                 NgayGiaoDich = borrow.NgayMuon
                             };
            var changeList = from change in db.Doi
                             join qtt in db.QuanTuTrang on change.MaQtt equals qtt.MaQtt
                             join size in db.KichCo on change.MaKc equals size.MaKc
                             where change.MaSv == sv.MaSv && change.TrangThai != "Đang xử lý"
                             select new
                             {
                                 id = change.MaDoi,
                                 GiaoDich = "Đổi",
                                 qtt.TenQtt,
                                 size.TenKc,
                                 NgayGiaoDich = change.NgayDoi
                             };
            var listAction = borrowList.ToList();
            foreach (var i in changeList)
            {
                listAction.Add(i);
            }
            dtgHD.ItemsSource = listAction;
        }

        // Nhấn nút "Xem chi tiết"
        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            Type t = dtgHD.SelectedItem.GetType();
            PropertyInfo[] p = t.GetProperties();
            string actionType, actionId, sId, sName, cStart, cFinish,
                qttId, qttName, kId, kName, createDate, actionDate, actionState;
            actionType = p[1].GetValue(dtgHD.SelectedValue).ToString();
            actionId = p[0].GetValue(dtgHD.SelectedValue).ToString();
            sId = sv.MaSv;
            sName = sv.TenSv;
            cStart = kh.NgayBd.Date.ToString("dd/MM/yyyy");
            cFinish = kh.NgayKt.Date.ToString("dd/MM/yyyy");
            if (actionType.Equals("Mượn"))
            {
                var borrow = from b in db.Muon
                             join k in db.KichCo on b.MaKc equals k.MaKc
                             join q in db.QuanTuTrang on b.MaQtt equals q.MaQtt
                             where b.MaMuon == actionId
                             select new
                             {
                                 q,
                                 k,
                                 b
                             };
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
                             where c.MaDoi == actionId
                             select new
                             {
                                 q,
                                 k,
                                 c
                             };
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

        /*===================CÁC HÓA ĐƠN THANH TOÁN===================*/

        // Danh sách hóa đơn thanh toán
        private void dtgHDTT_Loaded(object sender, RoutedEventArgs e)
        {
            var l = db.HoaDonThanhToan.Where(ele => ele.MaSv.Equals(sv.MaSv));
            dtgHDTT.ItemsSource = l.ToList();
        }
    }
}
