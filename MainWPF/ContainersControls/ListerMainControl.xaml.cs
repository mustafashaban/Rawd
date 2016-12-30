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
    /// Interaction logic for ListerMainControl.xaml
    /// </summary>
    public partial class ListerMainControl : UserControl
    {
        public ListerMainControl()
        {
            InitializeComponent();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                BindingListCollectionView view =
                    CollectionViewSource.GetDefaultView(dgListers.ItemsSource)
                    as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = string.Format("FirstName like '%{0}%' or LastName like '%{0}%'", txtName.Text);
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

        private void btnAddNewLister_Click(object sender, RoutedEventArgs e)
        {
            ListerControl w = new ListerControl();
            if (w.ShowDialog() == true)
            {
                dgListers.ItemsSource = Lister.GetAllListersTable;
            }
        }

        private void dgListers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateLister_Click(null, null);
        }

        private void btnUpdateLister_Click(object sender, RoutedEventArgs e)
        {
            if (dgListers.SelectedIndex >= 0)
            {
                ListerControl w = new ListerControl((int)(dgListers.Items[dgListers.SelectedIndex] as DataRowView)[0]);
                if (w.ShowDialog() == true)
                {
                    dgListers.ItemsSource = Lister.GetAllListersTable;
                }
            }
        }

        private void btnListerTrainings_Click(object sender, RoutedEventArgs e)
        {
            if (dgListers.SelectedIndex != -1)
            {
                //TrainingTrainedDetails w = new TrainingTrainedDetails((int)(dgListers.Items[dgListers.SelectedIndex] as DataRowView)[0], Trained.TrainedEnumType.مقيّم);
                //w.ShowDialog();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgListers.ItemsSource = Lister.GetAllListersTable;
            Control_Changed(null, null);
        }

        private void btnDelLister_Click(object sender, RoutedEventArgs e)
        {
            if (dgListers.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    Lister l = Lister.GetListerByID((int)(dgListers.Items[dgListers.SelectedIndex] as DataRowView)[0]);
                    if (int.Parse(BaseDataBase._Scalar("select dbo.GetGroupCountByListerID(" + l.ListerID + ")")) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف المقيّم \nبسبب وجود بيانات تقييم سابقة له ضمن فرق التقييم");
                    }
                    else if (int.Parse(BaseDataBase._Scalar("select COUNT(ID) from Trained where TrainedType = 2 and TrainedID = " + l.ListerID)) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف المقيّم \nبسبب وجود بيانات تدريب سابقة له");
                    }
                    else if (DBMain.DeleteData(l))
                    {
                        dgListers.ItemsSource = Lister.GetAllListersTable;
                        MyMessage.DeleteMessage();
                    }
                }
            }
        }
    }
}