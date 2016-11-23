using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for StaHilal.xaml
    /// </summary>
    public partial class StaHilal : UserControl
    {
        public StaHilal()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        private void btnFill_Click(object sender, RoutedEventArgs e)
        {
            if (cLoadingShape.Visibility != System.Windows.Visibility.Visible)
            {
                dgExcelFile.ItemsSource = null;
                cLoadingShape.Visibility = System.Windows.Visibility.Visible;
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.DoWork += bw_DoWork;

                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_CreateHilalTable", con);
                com.CommandType = CommandType.StoredProcedure;
                bw.RunWorkerAsync(com);
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            dt.Clear();
            var com = e.Argument as SqlCommand;
            com.CommandTimeout = 500;
            var con = com.Connection;
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
            }
            finally { con.Close(); }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                cLoadingShape.Visibility = System.Windows.Visibility.Hidden;
                dgExcelFile.ItemsSource = dt.DefaultView;
            }
            else
            {
                cLoadingShape.Visibility = System.Windows.Visibility.Hidden;
            }
        }


        private void btnExport2Excel_Click(object sender, RoutedEventArgs e)
        {
            if (dgExcelFile.ItemsSource != null)
            {
                var dt = (dgExcelFile.ItemsSource as DataView).Table;
                for (int i = 0; i < dgExcelFile.Columns.Count; i++)
                {
                    dt.Columns[i].Caption = dgExcelFile.Columns[i].Header.ToString();
                }

                ExportToExcel.ExportDataTableToExcel x = new ExportToExcel.ExportDataTableToExcel(dt);
                x.GenerateReport();
            }
        }
    }
}
