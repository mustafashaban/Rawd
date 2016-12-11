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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class OrphanBaseControl : UserControl
    {
        public OrphanBaseControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        
        private void btnSta_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTraining_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "التدريبات", Content = new TrainingMainControl() });
        }

        private void btnFinance_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSponsor_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "بيانات الكفلاء", Content = new SponsorMainControl() });
        }

        private void btnOrphan_Click(object sender, RoutedEventArgs e)
        {
            //if (!BaseDataBase.CurrentUser.ReportCreator)
            //{
            //    MyMessageBox.Show("ليس لديك صلاحية دخول");
            //    return;
            //}
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "بيانات الايتام/طلاب العلم", Content = new OrphanMainControl() });
        }
    }
}
