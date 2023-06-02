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

namespace QLQTT
{
    /// <summary>
    /// Giao diện Sinh viên
    /// </summary>
    public partial class MainStudent : Window
    {
        QLQTTContext db = new QLQTTContext();
        SinhVien sv = new SinhVien(); // Sinh viên đang đăng nhập
        KhoaHoc kh = new KhoaHoc();
        public static MainStudent instance;
        public string id;
        public bool chosen = false;
        public MainStudent(string id)
        {
            InitializeComponent();
            instance = this;
            this.sv = db.SinhVien.SingleOrDefault(s => s.MaSv.Equals(id));
            this.kh = db.KhoaHoc.SingleOrDefault(k => k.MaKh.Equals(sv.MaKh));
        }
        // Chuyển đổi dữ liệu ảnh
        public BitmapFrame convert(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return BitmapFrame.Create(ms, BitmapCreateOptions.None,
                    BitmapCacheOption.OnLoad);
            }
        }
        private void MainStudent_Loaded(object sender, RoutedEventArgs e)
        {
            lblId.Content = sv.MaSv;
            lblName.Content = sv.TenSv;
            byte[] byteArray = sv.Anh.ToArray();
            img.Source = convert(byteArray);
            IEnumerable<CongNo> m = db.CongNo.Where(ele => ele.MaSv.Equals(sv.MaSv));
            if (m.Count() == 0)
            {
                lblMoney.Content = "Nợ: 0 đồng";
            }
            else
            {
                string str = m.First().SoTien.ToString();
                lblMoney.Content = "Nợ: " + str.Substring(0, str.Length - 1) + " đồng";
            }
        }
        /* Đăng xuất */
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var logIn = new LogIn();
            logIn.Show();
            this.Close();
        }
        /* Đăng ký qtt */
        private void lvwDK_Loaded(object sender, RoutedEventArgs e)
        {
            var kc = from k in db.KichCo
                     select k;
            List<KichCo> listKc = new List<KichCo>();
            foreach (KichCo i in kc)
            {
                if (!i.MoTa.Equals("all"))
                    listKc.Add(i);
            }
            lvwDK.ItemsSource = listKc;
        }
        private void cbbKC_Loaded(object sender, RoutedEventArgs e)
        {
            var kc = from k in db.KichCo
                     where k.TenKc != "None"
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
        /* Danh sách qtt */
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
            foreach (SelectedBorrowedQTT i in dtgBorrowing.ItemsSource)
            {
                if (i.isChecked)
                {
                    if (i.changing)
                    {
                        err += i.qtt.TenQtt;
                    }
                }
            }
            if (err.Equals(""))
            {
                txtWarning.Text = "";
            }
            else
            {
                txtWarning.Text = "Bạn đang chờ để đổi " + err;
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
                            cn.HanTra = kh.NgayKt;// Cần fix hạn trả là 1 tuần sau khi kết thúc khóa học
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
        /* Giao dịch */
        private void dtgHD_Loaded(object sender, RoutedEventArgs e)
        {
            var borrowList = from borrow in db.Muon
                             join qtt in db.QuanTuTrang on borrow.MaQtt equals qtt.MaQtt
                             join size in db.KichCo on borrow.MaKc equals size.MaKc
                             where borrow.MaSv == sv.MaSv && borrow.TrangThai == "Hoàn thành"
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
                             where change.MaSv == sv.MaSv && change.TrangThai == "Hoàn thành"
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
                qttId, qttName, kId, kName, createDate, actionDate;
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
                qttId = borrow.ToList().First().q.MaQtt;
                qttName = borrow.ToList().First().q.TenQtt;
                kId = borrow.ToList().First().k.MaKc;
                kName = borrow.ToList().First().k.TenKc;
                createDate = borrow.ToList().First().b.NgayDk.Date.ToString("dd/MM/yyyy");
                actionDate = borrow.ToList().First().b.NgayMuon.GetValueOrDefault().ToString("dd/MM/yyyy");

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
                qttId = change.ToList().First().q.MaQtt;
                qttName = change.ToList().First().q.TenQtt;
                kId = change.ToList().First().k.MaKc;
                kName = change.ToList().First().k.TenKc;
                createDate = change.ToList().First().c.NgayDk.Date.ToString("dd/MM/yyyy");
                actionDate = change.ToList().First().c.NgayDoi.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
            ActionDetail actionDetail = new ActionDetail(actionType, actionId, sId, sName, cStart, cFinish,
                qttId, qttName, kId, kName, createDate, actionDate);
            actionDetail.Show();
            dtgLoss_Load(sender, e);
        }
    }
}
