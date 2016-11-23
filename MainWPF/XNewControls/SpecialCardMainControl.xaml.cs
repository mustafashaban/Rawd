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

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for SpecialCardMainControl.xaml
    /// </summary>
    public partial class SpecialCardMainControl : UserControl
    {
        public SpecialCardMainControl()
        {
            InitializeComponent();
        }

        private void dgCenter_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var c = e.Row.DataContext as SpecialCardCenter;
                if (c != null && c.IsValidate())
                {
                    if (!c.Id.HasValue)
                    {
                        if (SpecialCardCenter.InsertData(c))
                            MyMessage.InsertMessage();
                        else
                            e.Cancel = true;
                    }
                    else
                    {
                        if (SpecialCardCenter.UpdateData(c))
                            MyMessage.UpdateMessage();
                        else
                            e.Cancel = true;
                    }
                }
            }
        }

        private void dgCard_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var c = e.Row.DataContext as SpecialCard;
                if (c != null && c.IsValidate())
                {
                    if (!c.ID.HasValue)
                    {
                        if (SpecialCard.InsertData(c))
                            MyMessage.InsertMessage();
                        else
                            e.Cancel = true;
                    }
                    else
                    {
                        if (SpecialCard.UpdateData(c))
                            MyMessage.UpdateMessage();
                        else
                            e.Cancel = true;
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgCard.ItemsSource = SpecialCard.GetAllSpecialCard();
            dgCenter.ItemsSource = SpecialCardCenter.GetAllSpecialCardCenter();
        }

        private void btnDeleteCard_Click(object sender, RoutedEventArgs e)
        {
            var c = dgCard.SelectedItem as SpecialCard;
            if (c != null)
            {
                if (c.CanRemove())
                {
                    if (SpecialCard.DeleteData(c))
                    {
                        dgCard.ItemsSource = null;
                        dgCard.ItemsSource = SpecialCard.GetAllSpecialCard();
                        MyMessage.DeleteMessage();
                    }
                }
                else MyMessageBox.Show("لايمكن حذف البطاقة الخاصة بسبب وجود بطاقات مرتبطة بالعوائل او الافراد");
            }
        }

        private void btnDeleteCenter_Click(object sender, RoutedEventArgs e)
        {
            var c = dgCenter.SelectedItem as SpecialCardCenter;
            if (c != null)
            {
                if (c.CanRemove())
                {
                    if (SpecialCardCenter.DeleteData(c))
                    {
                        dgCenter.ItemsSource = null;
                        dgCenter.ItemsSource = SpecialCardCenter.GetAllSpecialCardCenter();
                        MyMessage.DeleteMessage();
                    }
                }
                else MyMessageBox.Show("لايمكن حذف المركز بسبب وجود بطاقات مرتبطة بالعوائل او الافراد");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "احصائيات البطاقات الخاصة", Content = new SpecialCardStaControl() });
        }
    }
}
