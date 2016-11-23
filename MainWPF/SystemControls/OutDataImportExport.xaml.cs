using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// <summary>
    /// Interaction logic for OutDataImportExport.xaml
    /// </summary>
    public partial class OutDataImportExport : Window
    {
        public OutDataImportExport()
        {
            InitializeComponent();
            cmboInventory.ItemsSource = Inventory.AllInventories.Where(x => x.IsActive).ToList();
        }

        private void btnExportPresentData_Click(object sender, RoutedEventArgs e)
        {
            var i = cmboInventory.SelectedItem as Inventory;
            if (i != null)
            {
                var dt1 = BaseDataBase._Tabling("select * from [Order] where InventoryID = " + i.ID);
                var dt2 = BaseDataBase._Tabling($"select * from Order_Item where OrderID in (select Id from [Order] where InventoryID = {i.ID})");
                var dt3 = BaseDataBase._Tabling($"select * from [dbo].[FamilyNeed_ListerGroup] where OrderID in (select Id from [Order] where InventoryID = {i.ID})");

                DataSet ds = new DataSet();
                ds.Tables.Add(dt1);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Rawd DataSet File|*.ds";
                if (sfd.ShowDialog() == true)
                {
                    ds.WriteXml(sfd.FileName);
                    ds.WriteXmlSchema(sfd.FileName.Substring(0, sfd.FileName.LastIndexOf('.')) + "_ds.ds");
                }
            }
            else MyMessageBox.Show("يجب اختيار المستودع المراد تصدير بيانات التسليم فيه");
        }

        private void btnImportPresentData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Rawd DataSet File|*.ds";
            if (ofd.ShowDialog() == true)
            {
                DataSet ds = new DataSet();
                try
                {
                    ds.ReadXmlSchema(ofd.FileName.Substring(0, ofd.FileName.LastIndexOf('.')) + "_ds.ds");
                    ds.ReadXml(ofd.FileName);
                }
                catch
                {
                    MyMessageBox.Show("خطأ في استيراد الملف");
                    return;
                }
                if (ds.Tables.Count != 3)
                {
                    MyMessageBox.Show("خطأ في بنية الملف المستورد");
                    return;
                }
                if (MyMessageBox.Show($"البيانات المستوردة خاصة بمستودع \"{Inventory.GetInventoryByID(ds.Tables[0].Rows[0].Field<int>("InventoryID")).Name}\"\nهل تريد تأكيد استيراد البيانات؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                    SqlCommand com = new SqlCommand("sp_ImportPresentData", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@dt1", ds.Tables[0]));
                    com.Parameters.Add(new SqlParameter("@dt2", ds.Tables[1]));
                    com.Parameters.Add(new SqlParameter("@dt3", ds.Tables[2]));
                    try
                    {
                        con.Open();
                        com.ExecuteNonQuery();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    finally
                    {
                        con.Close();
                    }
                }
                else MyMessageBox.Show("تم الغاء الامر");
            }
        }
    }
}