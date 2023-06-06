using QLQTT.Models;
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

namespace QLQTT
{
    /// <summary>
    /// Interaction logic for MoTaNhieu.xaml
    /// </summary>
    public partial class MoTaNhieu_Doi : Window
    {
        QLQTTContext db = new QLQTTContext();
        public List<DoiViewModel> listd = new List<DoiViewModel>();
        public List<string> madois = new List<string>();
        public MoTaNhieu_Doi()
        {
            InitializeComponent();
        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in listd)
            {
                var a = (from m in db.Doi
                         where m.MaDoi == i.MaDoi
                         select m).FirstOrDefault();
                a.MoTa = i.MoTa;

                db.SaveChanges();
            }
            this.Close();
        }

        private void dtgMuon1_Loaded(object sender, RoutedEventArgs e)
        {

            dtgMuon1.ItemsSource = listd;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
