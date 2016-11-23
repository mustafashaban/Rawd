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
using System.Data;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for TrainingTrainedDetails.xaml
    /// </summary>
    public partial class TrainingTrainedDetails : Window
    {
        public TrainingTrainedDetails(int? TrainedID, Trained.TrainedEnumType TrainedType)
        {
            InitializeComponent();
            dgTrainingDetails.ItemsSource = BaseDataBase._Tabling("exec sp_GetTrainingTraiedDetails " + TrainedID + "," + (int)TrainedType).DefaultView;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
