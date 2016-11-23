using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace MainWPF.MainControls
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            if (!(BaseDataBase.CurrentUser.IT || BaseDataBase.CurrentUser.PointAdmin))
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            SystemControls.DBControl w = new SystemControls.DBControl();
            w.ShowDialog();
        }


        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            UsersControl w = new UsersControl();
            w.ShowDialog();
        }

        private void btnExcelImport_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            var w = App.Current.MainWindow as MainWindow;
            w.btn_Click(null, e);
        }

        private void btnExcelExport_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            var w = App.Current.MainWindow as MainWindow;
            w.btn_Click(null, e);
        }

        private void btnCodes_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            SectorWindow w = new SectorWindow();
            w.ShowDialog();
        }

        private void btnIEData_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IT)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            OutDataImportExport w = new OutDataImportExport();
            w.ShowDialog();
        }

        private void btnCriteria_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            try
            {
                ItemCriteriaControl w = new ItemCriteriaControl();
                w.ShowDialog();
            }
            catch { }
        }

        private void btnFingerPrintActivator_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IT)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            FingerPrintActivatorWindow w = new FingerPrintActivatorWindow();
            w.ShowDialog();
        }

        private void btnHeaderControl_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            ReportHeaderWindow w = new ReportHeaderWindow();
            w.ShowDialog();
        }

        private void btnFixData_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            FixingDataWindow w = new FixingDataWindow();
            w.ShowDialog();
        }

        private void btnServerConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IT)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            ServerWindow w = new ServerWindow();
            w.ShowDialog();
        }

        private void btnVoucherType_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.IsAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                return;
            }
            VoucherTypeControl w = new VoucherTypeControl();
            w.ShowDialog();
        }
    }
}
