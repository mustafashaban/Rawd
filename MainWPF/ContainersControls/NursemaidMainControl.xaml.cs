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
    /// Interaction logic for NursemaidMainControl.xaml
    /// </summary>
    public partial class NursemaidMainControl : UserControl
    {
        public NursemaidMainControl()
        {
            InitializeComponent();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                BindingListCollectionView view =
                    CollectionViewSource.GetDefaultView(dgGuardians.ItemsSource)
                    as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = string.Format("FirstName like '%{0}%'", txtFirstName.Text);
                    if (!string.IsNullOrEmpty(txtLastName.Text))
                        view.CustomFilter += string.Format(" and LastName like '%{0}%'", txtLastName.Text);
                }
            }
            catch { }
        }


        private void btnAddNewGuardian_Click(object sender, RoutedEventArgs e)
        {
            GuardianControl w = new GuardianControl("أنثى");
            if (w.ShowDialog() == true)
            {
                dgGuardians.ItemsSource = Guardian.GetAllNursemaidTable;
            }
        }

        private void dgGuardian_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateGuardian_Click(null, null);
        }

        private void btnUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            if (dgGuardians.SelectedIndex >= 0)
            {
                GuardianControl w = new GuardianControl(Guardian.GetGuardianByID((int)(dgGuardians.Items[dgGuardians.SelectedIndex] as DataRowView)[0]));
                if (w.ShowDialog() == true)
                {
                    dgGuardians.ItemsSource = Guardian.GetAllNursemaidTable;
                }
            }
        }

        private void dgGuardians_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateGuardian_Click(null, null);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgGuardians.ItemsSource = Guardian.GetAllNursemaidTable;
            Control_Changed(null, null);
        }

        private void btnNusemaidTrainings_Click(object sender, RoutedEventArgs e)
        {
            if (dgGuardians.SelectedIndex != -1)
            {
                TrainingTrainedDetails w = new TrainingTrainedDetails((int)(dgGuardians.Items[dgGuardians.SelectedIndex] as DataRowView)[0], Trained.TrainedEnumType.حاضنة);
                w.ShowDialog();
            }
        }

        private void btnDelNursemaid_Click(object sender, RoutedEventArgs e)
        {
            if (dgGuardians.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    Guardian g = Guardian.GetGuardianByID((int)(dgGuardians.Items[dgGuardians.SelectedIndex] as DataRowView)[0]);
                    if (int.Parse(BaseDataBase._Scalar("select dbo.GetOrphanCountByGuardianID(" + g.GuardianID + ")")) > 0 || int.Parse(BaseDataBase._Scalar("select dbo.GetFamilyCountByGuardianID(" + g.GuardianID + ")")) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف الحاضنة \nبسبب وجود بيانات حضانة سابقة لها");
                    }
                    else if (int.Parse(BaseDataBase._Scalar("select COUNT(ID) from Trained where TrainedType = 4 and TrainedID = " + g.GuardianID)) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف الحاضنة \nبسبب وجود بيانات تدريب سابقة لها");
                    }
                    else
                    {
                        if (g.DeleteGuardianData())
                        {
                            dgGuardians.ItemsSource = Guardian.GetAllNursemaidTable;
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }
    }
}
