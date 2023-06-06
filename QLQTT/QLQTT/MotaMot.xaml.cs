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
    /// Interaction logic for MotaMot.xaml
    /// </summary>
    public partial class MotaMot : Window
    {
        public string mamuon;
        QLQTTContext db = new QLQTTContext();
        public MotaMot()
        {
            InitializeComponent();
        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            var row = (from m in db.Muon
                       where m.MaMuon == mamuon
                       select m).FirstOrDefault();
            row.MoTa = tbMoTa.Text;
            row.NgayMuon = DateTime.Today;
            row.TrangThai = "Hoàn thành";
            db.SaveChanges();
            this.Close();
        }
    }
}
