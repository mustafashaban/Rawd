using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace MainWPF
{
    public partial class UserMessageControl : UserControl
    {
        public UserMessageControl()
        {
            InitializeComponent();
            lbUsers.ItemsSource = UserForMessage.AllUsers.OrderBy(x => x.UnreadedMessages);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtMessage.Text != "" && lbUsers.SelectedItem != null)
            {
                var u = lbUsers.SelectedItem as UserForMessage;
                u.AddMessages(new UserMessage { Message = txtMessage.Text });
                messagesItems.Items.Refresh();
                svMain.ScrollToEnd();
                txtMessage.Text = "";
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(null, null);
        }

        private void lbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            var u = lbUsers.SelectedItem as UserForMessage;
            if (u != null)
            {
                u.GetAllMessages();
                messagesItems.Items.Refresh();
                svMain.ScrollToEnd();
            }
        }

        bool isWorking = false;
        private async void btnLoadMoreItems_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoaded && lbUsers.SelectedItem != null)
            {
                if (!isWorking)
                {
                    isWorking = true;
                    double verticalOffset = svMain.VerticalOffset;
                    var u = lbUsers.SelectedItem as UserForMessage;
                    bool IsThereItems = false;
                    await Task.Run(() => IsThereItems = u.GetFirstMessages());
                    if (IsThereItems)
                    {
                        messagesItems.Items.Refresh();
                        //await Task.Delay(10);
                        //svMain.ScrollToVerticalOffset(verticalOffset + 400);
                    }
                    isWorking = false;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(lbUsers.ItemsSource);
            if (view != null)
                view.Filter = FilterPredicate;
        }

        private bool FilterPredicate(object obj)
        {
            if (txtSearch.Text == "")
                return true;
            var u = obj as UserForMessage;
            return u.Name.Contains(txtSearch.Text);
        }

        DispatcherTimer dt;
        public void StartDispatcher()
        {
            if (dt == null)
                dt = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(10), IsEnabled = true };
            dt.Tick += async delegate
            {
                var u = lbUsers.SelectedItem as UserForMessage;
                if (u != null)
                {
                    int num = await UserForMessage.GetUnReadedMessages();
                    if (u.UnreadedMessages > 0)
                    {
                        u.MakeMessagesReaded();
                        bool isAdded = await u.GetLastMessages();
                        if (isAdded)
                            messagesItems.Items.Refresh();
                    }
                    u.CheckMessages();
                }
                if (UserForMessage.AllUsers.Sum(x => x.UnreadedMessages) > 0)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(lbUsers.ItemsSource);
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("UnreadedMessages", ListSortDirection.Descending));
                    lbUsers.Items.Refresh();
                }
            };
            dt.Start();
        }
        public void StopDispatcher()
        {
            if (dt == null)
                return;
            dt.Stop();
        }
    }
}
