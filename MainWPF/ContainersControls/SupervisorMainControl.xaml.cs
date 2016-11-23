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
    /// Interaction logic for SupervisorMainControl.xaml
    /// </summary>
    public partial class SupervisorMainControl : UserControl
    {
        public SupervisorMainControl()
        {
            InitializeComponent();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                BindingListCollectionView view =
                    CollectionViewSource.GetDefaultView(dgSupervisor.ItemsSource)
                    as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = string.Format("FirstName like '%{0}%'", txtFirstName.Text);
                    if (!string.IsNullOrEmpty(txtLastName.Text))
                        view.CustomFilter += string.Format(" and LastName like '%{0}%'", txtLastName.Text);
                    if (cmboGender.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Gender like '{0}'", (cmboGender.Items[cmboGender.SelectedIndex] as ComboBoxItem).Content);
                }
            }
            catch { }
        }

        private void cmboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control_Changed(null, null);
        }

        private void btnAddNewSupervisor_Click(object sender, RoutedEventArgs e)
        {
            SupervisorControl w = new SupervisorControl();
            if (w.ShowDialog() == true)
            {
                dgSupervisor.ItemsSource = Supervisor.GetAllSupervisorTable;
            }
        }

        private void dgSupervisor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateSupervisor_Click(null, null);
        }

        private void btnUpdateSupervisor_Click(object sender, RoutedEventArgs e)
        {
            if (dgSupervisor.SelectedIndex >= 0)
            {
                SupervisorControl w = new SupervisorControl(Supervisor.GetSupervisorByID((int)(dgSupervisor.Items[dgSupervisor.SelectedIndex] as DataRowView)[0]));
                if (w.ShowDialog() == true)
                {
                    dgSupervisor.ItemsSource = Supervisor.GetAllSupervisorTable;
                }
            }
        }

        private void btnSupervisorTrainings_Click(object sender, RoutedEventArgs e)
        {
            if (dgSupervisor.SelectedIndex >= 0)
            {
                TrainingTrainedDetails w = new TrainingTrainedDetails((dgSupervisor.SelectedItem as Supervisor).SupervisorID, Trained.TrainedEnumType.مشرف);
                w.ShowDialog();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgSupervisor.ItemsSource = Supervisor.GetAllSupervisorTable;
            Control_Changed(null, null);
        }

        private void btnDelLister_Click(object sender, RoutedEventArgs e)
        {
            if (dgSupervisor.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    Supervisor s = Supervisor.GetSupervisorByID((int)(dgSupervisor.Items[dgSupervisor.SelectedIndex] as DataRowView)[0]);
                    if (int.Parse(BaseDataBase._Scalar("select dbo.fn_SupervisorOrphansCount(" + s.SupervisorID + ")")) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف المشرف \nبسبب وجود بيانات إشراف سابقة له");
                    }
                    else if (int.Parse(BaseDataBase._Scalar("select COUNT(ID) from Trained where TrainedType = 3 and TrainedID = " + s.SupervisorID)) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف المشرف \nبسبب وجود بيانات تدريب سابقة له");
                    }
                    else
                    {
                        if (s.DeleteSupervisorData())
                        {
                            dgSupervisor.ItemsSource = Supervisor.GetAllSupervisorTable;
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }
    }
}
