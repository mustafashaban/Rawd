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
    public partial class ItemCriteriaControl : Window
    {
        public ItemCriteriaControl()
        {
            InitializeComponent();

            cmboItems.ItemsSource = (from x in Item.AllItems orderby x.Name select x).ToList();
            this.DataContext = new ItemCriteria();
            dgMain.ItemsSource = ItemCriteria.GetAllItemCriteria();

        }

        private void cmboCriteriaType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmboCriteriaType.SelectedItem != null)
            {
                if (cmboCriteriaType.SelectedItem.ToString() == "بدون معيار")
                {
                    spAgeCriteria.Visibility = System.Windows.Visibility.Collapsed;
                    spPersonCount.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    if (cmboCriteriaType.SelectedItem.ToString() == "عمر الشخص")
                    {
                        spAgeCriteria.Visibility = System.Windows.Visibility.Visible;
                        spPersonCount.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        spAgeCriteria.Visibility = System.Windows.Visibility.Collapsed;
                        spPersonCount.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }


        private void radAdd_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                btnExecute.Content = radAdd.Content;
                this.DataContext = new ItemCriteria();
            }
        }
        private void radUpdate_Checked(object sender, RoutedEventArgs e)
        {
            btnExecute.Content = radUpdate.Content;
            Binding b = new Binding();
            b.ElementName = "dgMain";
            b.Path = new PropertyPath(DataGrid.SelectedItemProperty);
            this.SetBinding(Window.DataContextProperty, b);

            dgMain_SelectionChanged(null, null);
        }
        private void radDelete_Checked(object sender, RoutedEventArgs e)
        {
            btnExecute.Content = radDelete.Content;
            Binding b = new Binding();
            b.ElementName = "dgMain";
            b.Path = new PropertyPath(DataGrid.SelectedItemProperty);
            this.SetBinding(Window.DataContextProperty, b);
           
            dgMain_SelectionChanged(null, null);
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as ItemCriteria;
            if (x != null)
            {
                x.CriteriaItem = cmboItems.SelectedItem as Item;
                if (radAdd.IsChecked == true)
                {
                    if (isValidateByControls(x) && x.IsValidate() && ItemCriteria.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        dgMain.ItemsSource = ItemCriteria.GetAllItemCriteria();
                    }
                }
                else if (radUpdate.IsChecked == true)
                {
                    if (isValidateByControls(x) && x.IsValidate() && ItemCriteria.UpdateData(x))
                    {
                        MyMessage.UpdateMessage();
                        dgMain.ItemsSource = ItemCriteria.GetAllItemCriteria();
                    }
                }
                else if (radDelete.IsChecked == true)
                {
                    if (MyMessageBox.Show("هل تريد تأكيد حذف المعيار", MessageBoxButton.YesNo) == MessageBoxResult.Yes && ItemCriteria.DeleteData(x))
                    {
                        MyMessage.DeleteMessage();
                        dgMain.ItemsSource = ItemCriteria.GetAllItemCriteria();
                    }
                }
            }
        }

        private bool isValidateByControls(ItemCriteria x)
        {
            if (cmboItems.SelectedItem == null)
            {
                MyMessageBox.Show("يجب اختيار المادة");
                return false;
            }

            if (x.CriteriaType == "عمر الشخص")
            {
                if (!nudAgeEnd.Value.HasValue || !nudAgeStart.Value.HasValue)
                {
                    MyMessageBox.Show("يجب ادخال مجال العمر");
                    return false;
                }
                switch (cmboSpan1.SelectedItem.ToString())
                {
                    case "يوم":
                        x.StartCriteria = (int)nudAgeStart.Value.Value;
                        break;
                    case "شهر":
                        x.StartCriteria = (int)nudAgeStart.Value.Value * 30;
                        break;
                    case "سنة":
                        x.StartCriteria = (int)nudAgeStart.Value.Value * 30 * 12;
                        break;
                }

                switch (cmboSpan2.SelectedItem.ToString())
                {
                    case "يوم":
                        x.EndCriteria = (int)nudAgeEnd.Value.Value;
                        break;
                    case "شهر":
                        x.EndCriteria = (int)nudAgeEnd.Value.Value * 30 - 1;
                        break;
                    case "سنة":
                        x.EndCriteria = (int)nudAgeEnd.Value.Value * 30 * 12 - 1;
                        break;
                }
            }
            else if (x.CriteriaType == "بدون معيار")
            {
                x.StartCriteria = null;
                x.EndCriteria = null;
            }
            else if (x.CriteriaType == "عدد الافراد")
            {
                x.StartCriteria = (int)nudPerosnCountFrom.Value.Value;
                x.EndCriteria = (int)nudPerosnCountTo.Value.Value;
                if (!nudPerosnCountFrom.Value.HasValue || !nudPerosnCountTo.Value.HasValue)
                {
                    MyMessageBox.Show("يجب ادخال مجال عدد الأفراد");
                    return false;
                }
            }
            return true;
        }

        private void dgMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = this.DataContext as ItemCriteria;
            if (x != null)
            {
                if (x.CriteriaType == "عمر الشخص")
                {
                    if (x.StartCriteria.Value < 31)
                    {
                        cmboSpan1.SelectedIndex = 0;
                        nudAgeStart.Value = x.StartCriteria;
                    }
                    else if (x.StartCriteria.Value < 361)
                    {
                        cmboSpan1.SelectedIndex = 1;
                        nudAgeStart.Value = x.StartCriteria / 31;
                    }
                    else
                    {
                        cmboSpan1.SelectedIndex = 2;
                        nudAgeStart.Value = x.StartCriteria / 31 / 12;
                    }
                    /////////////////////////////////
                    if (x.EndCriteria.Value < 31)
                    {
                        cmboSpan2.SelectedIndex = 0;
                        nudAgeEnd.Value = x.EndCriteria;
                    }
                    else if (x.EndCriteria.Value < 361)
                    {
                        cmboSpan2.SelectedIndex = 1;
                        nudAgeEnd.Value = x.EndCriteria / 31;
                    }
                    else
                    {
                        cmboSpan2.SelectedIndex = 2;
                        nudAgeEnd.Value = x.EndCriteria / 31 / 12;
                    }
                }
                else if (x.CriteriaType == "عدد الافراد")
                {
                    nudPerosnCountFrom.Value = x.StartCriteria;
                    nudPerosnCountTo.Value = x.EndCriteria;
                }
            }
        }
    }
}