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
    /// Interaction logic for SupervisorControl.xaml
    /// </summary>
    public partial class SupervisorControl : Window
    {
        public SupervisorControl()
        {
            InitializeComponent();
            this.DataContext = new Supervisor();
        }
        public SupervisorControl(Supervisor s)
        {
            InitializeComponent();
            this.DataContext = s;
            btnExecute.Content = "تعديل";
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            Supervisor x = (Supervisor)this.DataContext;
            if (x.IsValidate())
            {
                if (x.SupervisorID == null)
                {
                    if (x.InsertSupervisorData())
                    {
                        MyMessage.InsertMessage();
                        DialogResult = true;
                    }
                }
                else
                {
                    if (x.UpdateSupervisorData())
                    {
                        MyMessage.UpdateMessage();
                        DialogResult = true;
                    }
                }
            }
        }
    }
}
