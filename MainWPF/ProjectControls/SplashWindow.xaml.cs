using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
using System.Windows.Media.Imaging;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        enum ConnectionState { OK, NoSQL, NoDB };

        public SplashWindow()
        {
            InitializeComponent();
            Process currentProcess = Process.GetCurrentProcess();
            var runningProcess = (from process in Process.GetProcesses()
                                  where process.Id != currentProcess.Id && process.ProcessName.Equals(currentProcess.ProcessName, StringComparison.Ordinal)
                                  select process).FirstOrDefault();
            if (runningProcess != null)
            {
                ShowWindow(runningProcess.MainWindowHandle, 3);
                this.Close();
                return;
            }

            var ci = new System.Globalization.CultureInfo("en-us");
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
        }
        private ConnectionState CheckConnection()
        {
            string err;
            if (string.IsNullOrEmpty(Properties.Settings.Default.ConnectionString))
            {
                if (BaseDataBase.CheckConnection(@"Data Source=(local)\SQLEXPRESS;Integrated Security=True", out err))
                {
                    Properties.Settings.Default.ConnectionString = @"Data Source=(local)\SQLEXPRESS;Integrated Security=True";
                    Properties.Settings.Default.Save();
                    MyMessageBox.Show("تم إعداد قاعدة البيانات على المخدم المحلي للجهاز\nاذا كنت تستخدم مخدم شبكي فيرجى تغيير إعدادت الاتصال بالمخدم بالدخول الى الاعدادات");
                    System.Threading.Thread.Sleep(2000);
                    return ConnectionState.OK;
                }
                else
                {
                    ServerWindow w = new ServerWindow();
                    if (w.ShowDialog() != true)
                    {
                        MyMessageBox.Show("لم يتم اعداد مخدم قاعدة البيانات");
                        return ConnectionState.NoSQL;
                    }
                    else return ConnectionState.OK;
                }
            }
            else
            {
                if (BaseDataBase.CheckConnection(Properties.Settings.Default.ConnectionString, out err))
                {
                    switch (BaseDataBase.IsDataBaseExists("Ma3an"))
                    {
                        case true: return ConnectionState.OK;
                        case false: return ConnectionState.NoDB;
                        default: return ConnectionState.NoSQL;
                    }
                }
                else return ConnectionState.NoSQL;
            }
        }


        bool canCancel = false;
        LoadingWindow w;
        private void ChekStudySide()
        {
            int i;
            string s = BaseDataBase._Scalar("Select Count(IsLocal) from MergedStudySide where IsLocal = 1");
            if (string.IsNullOrEmpty(s))
            {
                MyMessageBox.Show("حدثت مشكلة في قاعدة البيانات يرجى مراجعة فريق الدعم");
                Environment.Exit(Environment.ExitCode);
            }
            else
            {
                i = int.Parse(s);
                if (i == 0)
                {
                    StudySideInsertControl w = new StudySideInsertControl();
                    if (!w.ShowDialog() == true)
                    {
                        Environment.Exit(Environment.ExitCode);
                    }
                }
                else if (i > 1)
                {
                    MyMessageBox.Show("حدثت مشكلة في قاعدة البيانات يرجى مراجعة فريق الدعم");
                    Environment.Exit(Environment.ExitCode);
                }
            }
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            var r = CheckConnection();
            if (r == ConnectionState.OK)
            {
                SignInUser w = new SignInUser(true);
                App.Current.MainWindow = w;
                w.Show();
                this.Close();
            }
            else if (r == ConnectionState.NoDB)
            {
                //NoDB 
                string s = "";
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionStringMaster);
                SqlCommand com = new SqlCommand(@"select SUBSTRING(physical_name, 1, CHARINDEX(N'master.mdf',Lower(physical_name))- 1) 
			                                    from [master].sys.master_files
			                                    where database_id = 1 and file_id = 1", con);
                try
                {
                    con.Open();
                    s = com.ExecuteScalar().ToString();
                }
                catch
                {
                    MyMessageBox.Show("حدث خطأ عند انشاء قاعدة البيانات\nيرجى مراجعة فريق الدعم");
                    return;
                }
                finally
                {
                    con.Close();
                }

                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.DoWork += bwBackup_DoWork;
                bw.RunWorkerAsync(s);
                w = new LoadingWindow("اعداد قاعدة البيانات");
                w.Closing += w_Closing;
                w.ShowDialog();
            }
            else
            {
                //NoSql
                MyMessageBox.Show("خطأ في الاتصال بقاعدة البيانات\nيجب تشغيل مخدم قاعدة البيانات\nأو تنصيبه من جديد");
                this.Close();
            }
        }
        private void bwBackup_DoWork(object sender, DoWorkEventArgs e)
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments, Environment.SpecialFolderOption.Create);
            if (File.Exists(tempPath + "\\db"))
                File.Delete(tempPath + "\\db");
            ZipFile.ExtractToDirectory(Path.Combine(Environment.CurrentDirectory, "datadb"), tempPath);
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionStringMaster);
            string ss = string.Format(@"
                                RESTORE DATABASE [Ma3an] FROM  DISK = N'{0}' WITH  FILE = 1,  KEEP_REPLICATION,  NOUNLOAD,  REPLACE,  STATS = 5,
                                    MOVE 'Ma3anDB' TO '{1}Ma3an.mdf', MOVE 'Ma3anDB_log' TO '{1}Ma3an_log.ldf';
                                    ", tempPath + "\\db", e.Argument.ToString());
            SqlCommand com = new SqlCommand(ss, con);
            try
            {
                con.Open();
                int x = com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                e.Cancel = true;
            }
            finally
            {
                con.Close();
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            canCancel = true;
            w.Close();
            if (e.Cancelled)
            {
                MyMessageBox.Show("حدث خطأ عند انشاء قاعدة البيانات\nيرجى مراجعة فريق الدعم");
                this.Close();
            }
            else
            {
                SignInUser siu = new SignInUser(true);
                App.Current.MainWindow = siu;
                siu.Show();
                this.Close();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
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
        void w_Closing(object sender, CancelEventArgs e)
        {
            if (!canCancel)
                e.Cancel = true;
        }
    }
}
