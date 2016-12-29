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

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for OrphanWindow.xaml
    /// </summary>
    public partial class OrphanWindow : Window
    {
        public OrphanWindow(Orphan o)
        {
            InitializeComponent();
            this.DataContext = o;
            cOrphan.btnGoToFamilyDetails.Visibility = Visibility.Collapsed;
            cOrphan.btnSave.Click -= cOrphan.btnSave_Click;
            cOrphan.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Orphan;
            if (x.IsValidate())
            {
                if (x.OrphanID.HasValue)
                {
                    if (Orphan.UpdateData(x))
                    {
                        MyMessage.UpdateMessage();
                        DialogResult = true;
                    }
                }
                else
                {
                    DialogResult = true;
                }
            }
        }
    }
}
