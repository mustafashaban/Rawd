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
using System.Windows.Media.Animation;

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
                }
            }
            catch { }
        }


        private void dgTraining_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnUpdateTraining_Click(null, null);
        }

        private void btnAddTraining_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "تدريب جديد", Content = new TrainingControl(new Training()) });
        }

        private void btnUpdateTraining_Click(object sender, RoutedEventArgs e)
        {
            if (dgTraining.SelectedIndex != -1)
            {
                var t = dgTraining.SelectedItem as Training;
                t.GetTrainees();
                var main = App.Current.MainWindow as MainWindow;
                main.SendTabItem(new TabItem() { Header = t.Name, Content = new TrainingControl(t) });
            }
        }

        bool isWorking = false;
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking)
            {
                isWorking = true;
                Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                sb.SetValue(Storyboard.TargetProperty, sender);
                sb.Begin();
                dgTraining.ItemsSource = Training.GetAllTraining();
                Control_Changed(null, null);
                sb.Pause();
                isWorking = false;
            }
        }

        private void btnDelTraining_Click(object sender, RoutedEventArgs e)
        {
            var tr = dgTraining.SelectedItem as Training;
            if (tr != null)
            {
                if (!BaseDataBase.CurrentUser.IsAdmin)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else if (Training.DeleteData(tr))
                {
                    (dgTraining.ItemsSource as List<Training>).Remove(tr);
                    dgTraining.Items.Refresh();
                    MyMessage.DeleteMessage();
                }
            }
        }
    }
}
