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
    /// Interaction logic for ActionDetail.xaml
    /// </summary>
    public partial class ActionDetail : Window
    {
        string actionType, actionId, sId, sName, cStart, cFinish, 
            qttId, qttName, kId, kName, createDate, actionDate, actionState;

        public ActionDetail(string actionType, string actionId, string sId, string sName,
            string cStart, string cFinish, string qttId, string qttName, string kId,
            string kName, string createDate, string actionDate, string actionState)
        {
            InitializeComponent();
            this.actionType = actionType;
            this.actionId = actionId;
            this.sId = sId;
            this.sName = sName;
            this.cStart = cStart;
            this.cFinish = cFinish;
            this.qttId = qttId;
            this.qttName = qttName;
            this.kId = kId;
            this.kName = kName;
            this.createDate = createDate;
            this.actionDate = actionDate;
            this.actionState = actionState;
        }

        private void start_Loaded(object sender, RoutedEventArgs e)
        {
            lblType.Content = "Đơn đăng ký " + actionType.ToLower();
            lblIdTitle.Content = "Mã " + actionType.ToLower();
            lblId.Content = actionId;
            lblSName.Content = sName;
            lblSId.Content = sId;
            lblCStart.Content = cStart;
            lblCFinish.Content = cFinish;
            lblQttName.Content = qttName;
            lblQttId.Content = qttId;
            lblkName.Content = kName;
            lblkId.Content = kId;
            lblcreateDate.Content = createDate;
            lblactionDateTitle.Content = "Ngày " + actionType.ToLower();
            lblactionDate.Content = actionDate;
            lblStatus.Content = actionState;
        }
    }
}
