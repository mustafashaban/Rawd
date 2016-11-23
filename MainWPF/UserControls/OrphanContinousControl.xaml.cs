using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for OrphanContinousControl.xaml
    /// </summary>
    public partial class OrphanContinousControl : UserControl
    {
        public OrphanContinousControl()
        {
            InitializeComponent();
        }

        private int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;

                DataTable dt = BaseDataBase._TablingStoredProcedure("sp_GetSpecialtyAll");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    grd.RowDefinitions.Add(new RowDefinition());
                    MyContinuingOrphanControl x = new MyContinuingOrphanControl();
                    x.SpecialtyID = (int?)dt.Rows[i].ItemArray[0];
                    x.OrphanID = OrphanID;
                    x.txtMain.Text = dt.Rows[i].ItemArray[1].ToString();
                    grd.Children.Add(x);
                    Grid.SetRow(x, grd.RowDefinitions.Count - 1);

                    RowDefinition r = new RowDefinition();
                    r.Height = new GridLength(15);
                    grd.RowDefinitions.Add(r);
                    Border b = new Border();
                    b.Background = Brushes.DarkGreen;
                    b.Margin = new Thickness(20, 2, 20, 5);
                    grd.Children.Add(b);
                    Grid.SetRow(b, grd.RowDefinitions.Count - 1);
                }
            }
        }
    }
}
