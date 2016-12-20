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
    /// Interaction logic for TrainedControl.xaml
    /// </summary>
    public partial class TrainedControl : Window
    {
        int? TrainingID;
        public TrainedControl(int? TrainingID)
        {
            InitializeComponent();
            this.TrainingID = TrainingID;
            dgTrained.ItemsSource = Trained.GetTrainedAllByTrainingID(TrainingID);
        }

        private void cmboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control_Changed(null, null);
        }
        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            if (dgTemp.ItemsSource != null)
            {
                ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(dgTemp.ItemsSource);
                view.Filter = delegate(object item)
                {
                    Person p = (Person)item;
                    if (txtFirstName.Text != "" && !p.FirstName.Contains(txtFirstName.Text))
                    {
                        return false;
                    }
                    if (txtLastName.Text != "" && !p.LastName.Contains(txtLastName.Text))
                    {
                        return false;
                    }
                    if (cmboGender.SelectedIndex > 0)
                    {
                        return (cmboGender.SelectedIndex == 1) ? (p.Gender == "ذكر") : (p.Gender == "أنثى");
                    }
                    return true;
                };
                view.Refresh();
            }
        }

        private void btnAddTrained_Click(object sender, RoutedEventArgs e)
        {
            if (dgTemp.SelectedIndex != -1)
            {
                Trained t = new Trained();

                if (dgTemp.SelectedItem is Orphan)
                {
                    var x = dgTemp.SelectedItem as Orphan;
                    t.TrainedID = x.OrphanID;
                    t.TrainedType = Trained.TrainedEnumType.يتيم;
                }
                else if (dgTemp.SelectedItem is Parent)
                {
                    var x = dgTemp.SelectedItem as Parent;
                    t.TrainedID = x.ParentrID;
                    t.TrainedType = Trained.TrainedEnumType.والدة;
                }
                else if (dgTemp.SelectedItem is Lister)
                {
                    var x = dgTemp.SelectedItem as Lister;
                    t.TrainedID = x.ListerID;
                    t.TrainedType = Trained.TrainedEnumType.مقيّم;
                }
                else if (dgTemp.SelectedItem is Supervisor)
                {
                    var x = dgTemp.SelectedItem as Supervisor;
                    t.TrainedID = x.SupervisorID;
                    t.TrainedType = Trained.TrainedEnumType.مشرف;
                }
                else 
                {
                    var x = dgTemp.SelectedItem as Guardian;
                    t.TrainedID = x.GuardianID;
                    t.TrainedType = Trained.TrainedEnumType.حاضنة;
                }
                
                t.TrainingID = this.TrainingID;
                bool CanAdd = true;
                foreach (var item in dgTrained.ItemsSource as List<Trained>)
                {
                    if (item.TrainedID == t.TrainedID && t.TrainedType == item.TrainedType)
                    {
                        CanAdd = false;
                        break;
                    }
                }
                if (CanAdd)
                {
                    t.IsAbiding = true;
                    t.IsAttended = true;
                    t.InsertTrainedData();

                    var ts = dgTrained.ItemsSource as List<Trained>;
                    ts.Add(t);
                    dgTrained.Items.Refresh();
                }
            }
        }

        private void Svae_Click(object sender, RoutedEventArgs e)
        {
            var ts = dgTrained.ItemsSource as List<Trained>;
            foreach (var item in ts)
            {
                item.UpdateTrainedData();
            }
            MyMessage.UpdateMessage();
        }

        private void btnDelTrained_Click(object sender, RoutedEventArgs e)
        {
            if (dgTrained.SelectedIndex!= -1)
            {
                var t = dgTrained.SelectedItem as Trained; 
                t.DeleteTrainedData();
                (dgTrained.ItemsSource as List<Trained>).Remove(t);
                dgTrained.Items.Refresh();
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var r = sender as RadioButton;
            if (r.Content == null)
            {
                dgTemp.ItemsSource = Orphan.GetAllOrphan();
                return;
            }
            switch (r.Content.ToString())
            {
                case "أيتام":
                    dgTemp.ItemsSource = Orphan.GetAllOrphan();
                    Control_Changed(null, null);
                    break;
                case "أمهات":
                    dgTemp.ItemsSource = Mother.GetAllParentsFemale();
                    Control_Changed(null, null);
                    break;
                case "حاضنات":
                    dgTemp.ItemsSource = Guardian.GetAllGuardianFeMale;
                    Control_Changed(null, null);
                    break;
                case "مقيّمين":
                    dgTemp.ItemsSource = Lister.GetAllListers;
                    Control_Changed(null, null);
                    break;
                case "مشرفين":
                    dgTemp.ItemsSource = Supervisor.GetAllSupervisors;
                    Control_Changed(null, null);
                    break;
                default:
                    break;
            }
        }

        private void dgTemp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAddTrained_Click(null, null);
        }
    }
}
