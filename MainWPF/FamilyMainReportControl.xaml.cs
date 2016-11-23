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
    /// Interaction logic for FamilyMainReportControl.xaml
    /// </summary>
    public partial class FamilyMainReportControl : UserControl
    {
        public FamilyMainReportControl()
        {
            InitializeComponent();
            var fs = from f in Family.GetFamilyAll() where f.FamilyID < 1000 select f;
            this.DataContext = fs;
        }
    }
}
