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
    public partial class MotaMot_Doi : Window
    {
        public string madoi;
        QLQTTContext db = new QLQTTContext();
        public MotaMot_Doi()
        {
            InitializeComponent();
        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            var row = (from m in db.Doi
                       where m.MaDoi == madoi
                       select m).FirstOrDefault();
            row.MoTa = tbMoTa.Text;
            row.NgayDoi = DateTime.Today;
            row.TrangThai = "Hoàn thành";
            db.SaveChanges();
            this.Close();
        }
    }
}
