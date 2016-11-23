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
    /// Interaction logic for TempReport.xaml
    /// </summary>
    public partial class TempReport : UserControl
    {
        public TempReport()
        {
            InitializeComponent();
            var fs = from f in Family.GetFamilyAll() where f.FamilyID < 20 select f;
            this.DataContext = fs;
        }
    }
}
