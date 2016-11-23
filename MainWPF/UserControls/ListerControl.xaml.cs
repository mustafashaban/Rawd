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
    /// <summary>
    /// Interaction logic for Lister.xaml
    /// </summary>
    public partial class ListerControl : Window
    {
        public ListerControl()
        {
            InitializeComponent();
            this.DataContext = new Lister();
        }
        public ListerControl(int ListerID)
        {
            InitializeComponent();
            this.DataContext = Lister.GetListerByID(ListerID);

            btnExecute.Visibility = System.Windows.Visibility.Hidden;
            btnUpdate.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            Lister x = (Lister)this.DataContext;
            if (x.IsValidate())
            {
                if (DBMain.InsertData(x))
                {
                    MyMessage.InsertMessage();
                    DialogResult = true;
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Lister x = (Lister)this.DataContext;
            if (x.IsValidate())
            {
                if (DBMain.UpdateData(x))
                {
                    MyMessage.UpdateMessage();
                    DialogResult = true;
                }
            }
        }
    }
}
