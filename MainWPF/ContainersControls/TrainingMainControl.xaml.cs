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
using System.Data;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for TrainingMainControl.xaml
    /// </summary>
    public partial class TrainingMainControl : UserControl
    {
        public TrainingMainControl()
        {
            InitializeComponent();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                BindingListCollectionView view =
                    CollectionViewSource.GetDefaultView(dgTraining.ItemsSource)
                    as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = string.Format("TrainingName like '%{0}%'", txtName.Text);
                    if (!string.IsNullOrEmpty(txtPrivateSide.Text))
                        view.CustomFilter += string.Format(" and PrivateSide like '%{0}%'", txtPrivateSide.Text);
                }
            }
            catch { }
        }


        private void dgTraining_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateTraining_Click(null, null);
        }

        private void btnAddTraining_Click(object sender, RoutedEventArgs e)
        {
            TrainingControl w = new TrainingControl();
            if (w.ShowDialog() == true)
                dgTraining.ItemsSource = Training.GetAllTrainingsTable;
        }

        private void btnUpdateTraining_Click(object sender, RoutedEventArgs e)
        {
            if (dgTraining.SelectedIndex != -1)
            {
                TrainingControl w = new TrainingControl(Training.GetTrainingByID(((int)(dgTraining.Items[dgTraining.SelectedIndex] as DataRowView)[0])));
                if (w.ShowDialog() == true)
                    dgTraining.ItemsSource = Training.GetAllTrainingsTable;
            }
        }

        private void btnTrainers_Click(object sender, RoutedEventArgs e)
        {
            if (dgTraining.SelectedIndex != -1)
            {
                TrainerControl w = new TrainerControl();
                w.TraininigID = (int)(dgTraining.Items[dgTraining.SelectedIndex] as DataRowView)[0];
                w.ShowDialog();
                dgTraining.ItemsSource = Training.GetAllTrainingsTable;
            }
        }

        private void btnTrained_Click(object sender, RoutedEventArgs e)
        {
            if (dgTraining.SelectedIndex != -1)
            {
                TrainedControl w = new TrainedControl((int?)(dgTraining.Items[dgTraining.SelectedIndex] as DataRowView)[0]);
                w.ShowDialog();
                dgTraining.ItemsSource = Training.GetAllTrainingsTable;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgTraining.ItemsSource = Training.GetAllTrainingsTable;
            Control_Changed(null, null);
        }

        private void btnDelLister_Click(object sender, RoutedEventArgs e)
        {
            if (dgTraining.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    if (((int)(dgTraining.Items[dgTraining.SelectedIndex] as DataRowView)[2]) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف التدريب \nبسبب وجود بيانات متدربين ضمنه\nيجب حذف المتدربين أولاً");
                    }
                    else
                    {
                        Training t = Training.GetTrainingByID((int)(dgTraining.Items[dgTraining.SelectedIndex] as DataRowView)[0]);
                        if (t.DeleteTrainingData())
                        {
                            dgTraining.ItemsSource = Training.GetAllTrainingsTable;
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }
    }
}
