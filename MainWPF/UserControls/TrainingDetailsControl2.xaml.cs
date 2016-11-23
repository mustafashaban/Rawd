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
    /// Interaction logic for TrainingDetailsControl2.xaml
    /// </summary>
    public partial class TrainingDetailsControl2 : UserControl
    {
        public TrainingDetailsControl2()
        {
            InitializeComponent();
        }

        public int? OrphanID
        {
            set
            {
                dgTrainingDetails.ItemsSource =
                    BaseDataBase._Tabling("exec sp_GetTrainingTraiedDetails " + value + "," + (int)Trained.TrainedEnumType.يتيم).DefaultView;
                if (dgTrainingDetails.Items.Count == 0)
                    BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                else
                    BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
            }
        }
    }
}
