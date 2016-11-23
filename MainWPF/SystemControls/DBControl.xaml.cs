using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace MainWPF.SystemControls
{
    /// <summary>
    /// Interaction logic for DBControl.xaml
    /// </summary>
    public partial class DBControl : Window
    {
        LoadingWindow w;
        bool canCancel = false;
        string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments, Environment.SpecialFolderOption.Create);

        public DBControl()
        {
            InitializeComponent();
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                w = new LoadingWindow();
                w.Closing += w_Closing;

                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.DoWork += bwBackup_DoWork;

                System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog();
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (File.Exists(fd.SelectedPath + "\\" + BaseDataBase.DateNow.ToString("DB_Backup_ddMMyyyy") + ".bak"))
                    {
                        if (MyMessageBox.Show("الموقع الذي تم اختياره يحوي ملف احتياطي بنفس التاريخ\nهل تريد استبداله؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            File.Delete(fd.SelectedPath + "\\" + BaseDataBase.DateNow.ToString("DB_Backup_ddMMyyyy") + ".bak");
                        }
                        else { bw.CancelAsync(); return; }
                    }
                    bw.RunWorkerAsync(fd.SelectedPath);
                    w.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message + "\n\nحدث خطأ أثناء عملية أخذ نسخة احتياطية\nيرجى المحاولة مرة أخرى عبر تغيير موقع الحفظ\n\nفي حال استمرار ظهور المشكلة يرجى مراجعة فريق الدعم");
            }
        }
        void bwBackup_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (File.Exists(tempPath + "\\" + "DB_Full.bak"))
                    File.Delete(tempPath + "\\" + "DB_Full.bak");


                int x = BaseDataBase._NonQuery(string.Format("BACKUP DATABASE [Ma3an] TO DISK = '{0}\\DB_Full.bak' WITH NOFORMAT, NOINIT, SKIP, NOREWIND, NOUNLOAD,  STATS = 10", tempPath, BaseDataBase.ConnectionStringMaster));
                if (x == -1)
                {

                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                    {
                        zip.Password = "RawdPassXx";
                        string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Data");
                        zip.AddFiles(files, "Data");
                        zip.AddFile(tempPath + "\\" + "DB_Full.bak", "");
                        zip.Save(e.Argument + "\\" + BaseDataBase.DateNow.ToString("DB_Backup_ddMMyyyy") + ".bak");
                    }

                    //ZipFile.CreateFromDirectory("Data", e.Argument + "\\" + BaseDataBase.DateNow.ToString("DB_Backup_ddMMyyyy") + ".bak", CompressionLevel.NoCompression, true);
                    //using (ZipArchive zip = ZipFile.Open(e.Argument + "\\" + BaseDataBase.DateNow.ToString("DB_Backup_ddMMyyyy") + ".bak", ZipArchiveMode.Update))
                    //{
                    //    zip.CreateEntryFromFile(tempPath + "\\" + "DB_Full.bak", "DB_Full.bak");
                    //}
                }
                else
                    e.Cancel = true;
            }
            catch (Exception ex) { e.Cancel = true; }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            w = new LoadingWindow();
            w.Closing += w_Closing;

            BackgroundWorker bw = new BackgroundWorker();
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += bwRestore_DoWork;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "إختر ملف الإستعادة";
            ofd.Filter = "Database Backup Files(*.bak)|*.bak";
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == true && ofd.CheckFileExists)
            {
                bw.RunWorkerAsync(ofd.FileName);
                w.ShowDialog();
            }
        }
        void bwRestore_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Directory.Exists("Data"))
                    Directory.Delete("Data", true);
                if (Directory.Exists(tempPath + "\\Data"))
                    Directory.Delete(tempPath + "\\Data", true);
                if (File.Exists("DB_Full.bak"))
                    File.Delete("DB_Full.bak");
                if (File.Exists(tempPath + "\\DB_Full.bak"))
                    File.Delete(tempPath + "\\DB_Full.bak");

                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(e.Argument.ToString()))
                {
                    zip.Password = "RawdPassXx";
                    zip.ExtractAll(Environment.CurrentDirectory, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                    zip.ExtractAll(tempPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }
                //ZipFile.ExtractToDirectory(e.Argument.ToString(), Environment.CurrentDirectory);
                //ZipFile.ExtractToDirectory(e.Argument.ToString(), tempPath);

                BaseDataBase._NonQuery(@"ALTER DATABASE [Ma3an] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", BaseDataBase.ConnectionStringMaster);
                int x = BaseDataBase._NonQuery(string.Format(@"RESTORE DATABASE [Ma3an] FROM DISK = '{0}' WITH  REPLACE;", tempPath + "\\DB_Full.bak"), BaseDataBase.ConnectionStringMaster);
                BaseDataBase._NonQuery(@"ALTER DATABASE [Ma3an] SET MULTI_USER", BaseDataBase.ConnectionStringMaster);
                if (x != -1)
                { e.Cancel = true; }
            }
            catch (Exception ex)
            { e.Cancel = true; }
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            canCancel = true;
            w.Close();
            canCancel = false;
            if (!e.Cancelled)
                MyMessageBox.Show("تمت العملية بنجاح");
            else MyMessageBox.Show("حدثت مشكلة إثناء المعالجة");

            try
            {
                if (Directory.Exists(tempPath + "\\Data"))
                    Directory.Delete(tempPath + "\\Data", true);
                if (File.Exists("DB_Full.bak"))
                    File.Delete("DB_Full.bak");
                if (File.Exists(tempPath + "\\DB_Full.bak"))
                    File.Delete(tempPath + "\\DB_Full.bak");
            }
            catch { }
        }

        void w_Closing(object sender, CancelEventArgs e)
        {
            if (!canCancel)
            {
                e.Cancel = true;
            }
        }
    }
}
