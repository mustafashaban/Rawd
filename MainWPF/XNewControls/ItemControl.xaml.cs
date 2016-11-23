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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class ItemControl : UserControl
    {
        public ItemControl()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (dg.ItemsSource == null)
                dg.ItemsSource = (await Item.GetAllItemsTalbe()).DefaultView;
        }

        private void btnNewItem_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.ReportCreator)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "اضافة مادة", Content = new ItemDetailsControl(new Item()) });
        }

        private void btnNewBasket_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.ReportCreator)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            if (!BaseDataBase.CurrentUser.ReportCreator)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "اضافة مادة متفرعة", Content = new ItemDetailsControl(new Item(true)) });
        }

        private void btnNewFormed_Click(object sender, RoutedEventArgs e)
        {
            if (!(BaseDataBase.CurrentUser.PointAdmin))
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            TabItem ti = new TabItem() { Header = "تشكيلة مواد", Content = new FormedBasketControl() };
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(ti);
        }
        private void btnUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.ReportCreator)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            if (dg.SelectedItem != null)
            {
                Item i = Item.GetItemByID((int)(dg.SelectedItem as DataRowView)[0]);
                if (i.IsBasket)
                    i.GetBasketItems();
                TabItem ti = new TabItem() { Header = i.Name, Content = new ItemDetailsControl(i) };
                var main = App.Current.MainWindow as MainWindow;
                main.SendTabItem(ti);
            }
        }

        private void dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnUpdateItem_Click(null, null);
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dv = dg.ItemsSource as DataView;
            try
            {
                if (dv != null)
                {
                    if (txtCode.Text.Trim() != "")
                    {
                        dv.RowFilter = "Id = " + txtCode.Text;
                        if (txtName.Text.Trim() != "")
                            dv.RowFilter += $" and Name like '*{txtName.Text.Trim()}*' ";
                    }
                    else if (txtName.Text.Trim() != "")
                        dv.RowFilter = $" Name like '*{txtName.Text.Trim()}*' ";
                    else
                        dv.RowFilter = "";
                }
            }
            catch { dv.RowFilter = ""; }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
            sb.SetValue(Storyboard.TargetProperty, sender);
            sb.Begin();
            DataTable dt = await Item.GetAllItemsTalbe();
            sb.Pause();
            dg.ItemsSource = dt.DefaultView;
        }
    }
}
