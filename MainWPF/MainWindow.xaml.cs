using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => UserForMessage.AllUsers = UserForMessage.GetAllUsers());
            Task.Run(() => DBMain.UpdateAllPregnant());

            this.MaxHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            btns = new List<Button>() { btnSearch, btnAddFamily, btnOrphans, btnSystem, btnInventory, btnSettings };
            btnUser.Content = BaseDataBase.CurrentUser.Name;
            txtClock.Text = DateTime.Now.ToString("t") + "  " + DateTime.Now.ToString("d");
            DispatcherTimer dt = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(1), IsEnabled = true };
            dt.Tick += async delegate
            {
                txtClock.Text = DateTime.Now.ToString("t") + "  " + DateTime.Now.ToString("d");
                int num = await UserForMessage.GetUnReadedMessages();
                txtMessage.Text = num.ToString();
                brdrMessage.Visibility = num > 0 ? Visibility.Visible : Visibility.Collapsed;
            };

            //sign out when application ilde 10 minutes
            mIdle = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(10), IsEnabled = true };
            mIdle.Tick += delegate
            {
                mIdle.Stop();
                if (!SignInUser.IsActivated)
                {
                    SignInUser w = new SignInUser(false);
                    w.ShowDialog();
                    if (w.IsAccepted)
                        mIdle.Start();
                }
                else mIdle.Start();
            };
            InputManager.Current.PreProcessInput += OnActivity;

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.Space)
            {
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        public void btn_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)e.Source;
            if (b.Content.ToString() != "بيانات العوائل")
            {
                if (!BaseDataBase.CurrentUser.IsAdmin)
                {
                    if (b.Content.ToString() == "إحصائيات وبحث" && !BaseDataBase.CurrentUser.CanMakeStatistics)
                    {
                        MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                        return;
                    }
                    if (b.Content.ToString() == "بيانات النظام" && !BaseDataBase.CurrentUser.PointAdmin)
                    {
                        MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                        return;
                    }
                    if (b.Content.ToString() == "متابعة الأيتام" && !BaseDataBase.CurrentUser.CanUpdateFamily)
                    {
                        MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                        return;
                    }
                }
            }
            if (CheckTabControl(b.Content.ToString()))
            {
                TabItem ti = new TabItem();
                ti.Header = b.Content.ToString();
                switch (b.Content.ToString())
                {
                    case "إحصائيات وبحث":
                        ti.Content = new StatisticsControl();
                        break;
                    case "متابعة الأيتام":
                        ti.Content = new OrphanMainControl();
                        break;
                    case "بيانات العوائل":
                        ti.Content = new FamilyMainControl();
                        break;
                    case "بيانات النظام":
                        ti.Content = new SystemDataControl();
                        break;
                    case "المواد والمستودعات":
                        ti.Content = new InventoryBaseControl();
                        break;
                    case "تصدير إلى إكسل":
                        btnExitAboutControl_Click(btnExitSettings, null);
                        ti.Content = new StaHilal();
                        break;
                    case "استيراد من إكسل":
                        btnExitAboutControl_Click(btnExitSettings, null);
                        ti.Content = new ImportDataControl();
                        break;
                }
                tcMain.Items.Add(ti);

                if (tcMain.Items.Count == 1)
                    HideMainControls();
                tcMain.SelectedIndex = tcMain.Items.Count - 1;
            }
        }

        public void SendTabItem(TabItem TabItem)
        {
            if (CheckTabControl(TabItem.Header.ToString()))
            {
                tcMain.Items.Add(TabItem);
                tcMain.SelectedIndex = tcMain.Items.Count - 1;
                if (tcMain.Items.Count == 1)
                    HideMainControls();
                tcMain.SelectedIndex = tcMain.Items.Count - 1;
            }
        }
        public bool CheckTabControl(string msg)
        {
            foreach (var item in tcMain.Items)
            {
                if (((TabItem)item).Header.ToString() == msg)
                {
                    tcMain.SelectedItem = item;
                    return false;
                }
            }
            return true;
        }

        List<Button> btns;
        DispatcherTimer mIdle;
        private Point _inactiveMousePosition = new Point(0, 0);
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int num = await UserForMessage.GetUnReadedMessages();
            txtMessage.Text = num.ToString();
            brdrMessage.Visibility = num > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        void OnActivity(object sender, PreProcessInputEventArgs e)
        {
            InputEventArgs inputEventArgs = e.StagingItem.Input;

            if (inputEventArgs is MouseEventArgs || inputEventArgs is KeyboardEventArgs)
            {
                if (e.StagingItem.Input is MouseEventArgs)
                {
                    MouseEventArgs mouseEventArgs = (MouseEventArgs)e.StagingItem.Input;

                    // no button is pressed and the position is still the same as the application became inactive
                    if (mouseEventArgs.LeftButton == MouseButtonState.Released &&
                        mouseEventArgs.RightButton == MouseButtonState.Released &&
                        mouseEventArgs.MiddleButton == MouseButtonState.Released &&
                        mouseEventArgs.XButton1 == MouseButtonState.Released &&
                        mouseEventArgs.XButton2 == MouseButtonState.Released &&
                        _inactiveMousePosition == mouseEventArgs.GetPosition(grdContainer))
                        return;
                }

                mIdle.Stop();
                mIdle.Start();
            }
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            foreach (var item in btns)
            {
                item.IsEnabled = true;
            }
            if (grdBtns.ColumnDefinitions.Count > 0)
            {
                grdBtns.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                grdBtns.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                grdBtns.Width = 145;
                grdBtns.ClearValue(Grid.HeightProperty);
                grdBtns.ColumnDefinitions.Clear();
                for (int i = 0; i < 4; i++)
                {
                    RowDefinition r = new RowDefinition();
                    r.Height = new GridLength(170);
                    grdBtns.RowDefinitions.Add(r);
                    Grid.SetRow(btns[i], i);
                    Grid.SetColumn(btns[i], 0);
                }
                btns[5].Visibility = btns[4].Visibility = System.Windows.Visibility.Hidden;
                ((Storyboard)this.FindResource("sbShowButtons")).Begin(this);
            }
            else
            {
                grdBtns.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                grdBtns.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                grdBtns.Height = 200;
                grdBtns.ClearValue(Grid.WidthProperty);
                grdBtns.RowDefinitions.Clear();
                for (int i = 0; i < 6; i++)
                {
                    ColumnDefinition c = new ColumnDefinition();
                    c.Width = new GridLength(172);
                    grdBtns.ColumnDefinitions.Add(c);
                    Grid.SetRow(btns[i], 0);
                    Grid.SetColumn(btns[i], i);
                }
                btns[5].Visibility = btns[4].Visibility = System.Windows.Visibility.Visible;
                ((Storyboard)this.FindResource("sbShowButtons")).Begin(this);
            }
        }
        private void SbStartup_Completed(object sender, EventArgs e)
        {
            foreach (var item in btns)
            {
                item.IsEnabled = true;
            }
        }

        private void MyMainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tcMain.Items.Count > 0)
            {
                if (MyMessageBox.Show("هل تريد اغلاق كافة النوافذ؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    ShowMainControls();
                e.Cancel = true;
                return;
            }
            if (BaseDataBase.ZFP != null)
            {
                BaseDataBase.ZFP.EndEngine();
            }
        }

        private void btnExitAboutControl_Click(object sender, RoutedEventArgs e)
        {
            Grid g = (sender as Button).Parent as Grid;
            grdBtns.Visibility = System.Windows.Visibility.Visible;
            Storyboard sb = new Storyboard();
            DoubleAnimation da1 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.6)));
            DoubleAnimation da2 = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.1)));

            Storyboard.SetTargetName(da1, "grdBtns");
            Storyboard.SetTargetProperty(da1, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTargetName(da2, g.Name);
            Storyboard.SetTargetProperty(da2, new PropertyPath(Grid.OpacityProperty));

            sb.Children.Add(da1);
            sb.Children.Add(da2);

            grdSettings.IsEnabled = false;
            sb.Completed += delegate
            {
                foreach (var item in btns)
                {
                    item.IsEnabled = true;
                }
                g.Visibility = System.Windows.Visibility.Hidden;
            };

            sb.Begin(this);
        }
        private void btnAboutProject_Click(object sender, RoutedEventArgs e)
        {
            grdAboutProject.Visibility = System.Windows.Visibility.Visible;
            Storyboard sb = new Storyboard();
            DoubleAnimation da1 = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.1)));
            DoubleAnimation da2 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.6)));

            Storyboard.SetTargetName(da1, "grdBtns");
            Storyboard.SetTargetProperty(da1, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTargetName(da2, "grdAboutProject");
            Storyboard.SetTargetProperty(da2, new PropertyPath(Grid.OpacityProperty));

            sb.Children.Add(da1);
            sb.Children.Add(da2);

            sb.Completed += delegate
            {
                grdBtns.Visibility = System.Windows.Visibility.Hidden;
            };

            sb.Begin(this);
        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in btns)
            {
                item.IsEnabled = false;
            }

            grdSettings.Visibility = System.Windows.Visibility.Visible;
            Storyboard sb = new Storyboard();
            DoubleAnimation da1 = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.1)));
            DoubleAnimation da2 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.6)));

            Storyboard.SetTargetName(da1, "grdBtns");
            Storyboard.SetTargetProperty(da1, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTargetName(da2, "grdSettings");
            Storyboard.SetTargetProperty(da2, new PropertyPath(Grid.OpacityProperty));

            sb.Children.Add(da1);
            sb.Children.Add(da2);
            grdSettings.IsEnabled = false;

            sb.Completed += delegate
            {
                grdBtns.Visibility = System.Windows.Visibility.Hidden;
                grdSettings.IsEnabled = true;
            };

            sb.Begin(this);

        }
        private void btnAboutProg_Click(object sender, RoutedEventArgs e)
        {
            AboutProgramWindow w = new AboutProgramWindow();
            w.ShowDialog();
        }

        private void CommandBindingClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (tcMain.SelectedIndex != -1)
            {
                if (BaseDataBase.CurrentUser.IsAdmin || !BaseDataBase.CurrentUser.CanPresent || MyMessageBox.Show("تنبيه .. هل تريد الخروج", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    TabItem x = (tcMain.SelectedItem as TabItem);
                    if (x != null)
                    {
                        tcMain.Items.Remove(x);
                        if (tcMain.Items.Count == 0)
                        {
                            foreach (var item in btns)
                            {
                                item.IsEnabled = false;
                            }
                            ((Storyboard)this.Resources["sbHideButtons"]).Begin(this);
                            ((Storyboard)this.Resources["sbSlideDown"]).Begin(this);
                        }
                    }
                }
            }
        }
        private void btnTabClose_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem item in tcMain.Items)
            {
                if (item.Header.ToString() == "رسائل المستخدمين")
                    (item.Content as UserMessageControl).StopDispatcher();
            }
            if (BaseDataBase.CurrentUser.IsAdmin || !BaseDataBase.CurrentUser.CanPresent || MyMessageBox.Show("تنبيه .. هل تريد الخروج", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                TabItem x = (TabItem)((Button)e.OriginalSource).TemplatedParent;
                tcMain.Items.Remove(x);
                if (tcMain.Items.Count == 0)
                    ShowMainControls();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hl = sender as Hyperlink;
            if (hl != null)
            {
                Process.Start(hl.NavigateUri.AbsoluteUri);
            }
        }

        public void ShowMainControls()
        {
            foreach (var item in btns)
                item.IsEnabled = false;
            tcMain.Items.Clear();
            ((Storyboard)this.Resources["sbHideButtons"]).Begin(this);
            ((Storyboard)this.Resources["sbSlideDown"]).Begin(this);
        }
        public void HideMainControls()
        {
            foreach (var item in btns)
                item.IsEnabled = false;
            ((Storyboard)this.FindResource("sbHideButtons")).Begin(this);
            ((Storyboard)this.FindResource("sbSlideUp")).Begin(this);
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow w = new MainWPF.ChangePasswordWindow();
            w.ShowDialog();
        }

        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {
            SendTabItem(new TabItem() { Header = "رسائل المستخدمين", Content = new UserMessageControl() });
        }

        private void tcMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcMain.SelectedItem != null)
            {
                var ti = tcMain.SelectedItem as TabItem;
                if (ti.Header.ToString() == "رسائل المستخدمين")
                    (ti.Content as UserMessageControl).StartDispatcher();
                else
                {
                    foreach (TabItem item in tcMain.Items)
                    {
                        if (item.Header.ToString() == "رسائل المستخدمين")
                        {
                            (item.Content as UserMessageControl).StopDispatcher();
                            break;
                        }
                    }
                }
            }
        }
    }
}
