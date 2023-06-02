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
    /// Interaction logic for SelectSizeForChangeAction.xaml
    /// </summary>
    public partial class SelectSizeForChangeAction : Window
    {
        QLQTTContext db = new QLQTTContext();
        public KichCo kc;
        string name;
        public static SelectSizeForChangeAction instance;
        public SelectSizeForChangeAction(string id, string name)
        {
            InitializeComponent();
            instance = this;
            kc = db.KichCo.SingleOrDefault(k => k.MaKc.Equals(id));
            this.name = name;
        }
        // Hiển thị danh sách Kích cỡ
        private void lvwDK_Loaded(object sender, RoutedEventArgs e)
        {
            var kc = from k in db.KichCo
                     where k.TenKc != "None"
                     select k;
            List<KichCo> listKc = new List<KichCo>();
            foreach (KichCo i in kc)
            {
                listKc.Add(i);
            }
            lvwDK.ItemsSource = listKc;
        }
        // Load dữ liệu combobox Kích cỡ
        private void cbbKC_Loaded(object sender, RoutedEventArgs e)
        {
            var kc = from k in db.KichCo
                     where k.TenKc != "None"
                     select k;
            List<KichCo> listKc = new List<KichCo>();
            foreach (KichCo i in kc)
            {
                listKc.Add(i);
            }
            cbbKC.ItemsSource = listKc;
            cbbKC.SelectedIndex = 0;
            cbbKC.DisplayMemberPath = "TenKc";
            cbbKC.SelectedValuePath = "MaKc";
        }
        // Nhấn nút "Chọn"
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbbKC.SelectedItem != null)
            {
                KichCo selected = (KichCo)cbbKC.SelectedItem;
                MessageBoxResult message = MessageBox.Show("Bạn có chắc chắn chọn '"
                    + selected.TenKc + "'?", "Xác nhận", MessageBoxButton.YesNo);
                if (message == MessageBoxResult.Yes)
                {
                    MainStudent.instance.id = selected.MaKc;
                    MainStudent.instance.chosen = true;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn Kích cỡ nào!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("Bạn chắc chắn chọn " + kc.TenKc + "?",
                   "Confirmation Box", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                MainStudent.instance.id = kc.MaKc;
                this.Close();
                MainStudent.instance.chosen = true;
            }
        }
        // Hiển thị tên Kích cỡ cũ
        private void start_Load(object sender, RoutedEventArgs e)
        {
            lblOldID.Content = name;
        }
    }
}
