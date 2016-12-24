using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MainWPF
{
    class BaseDataBase
    {
        public static string ConnectionString
        {
            get
            {
               return Properties.Settings.Default.ConnectionString + ";Initial Catalog=Ma3an";
               //return @"Data Source=.\SQLEXPRESS;Integrated Security=True" + ";Initial Catalog=Ma3an";
            }
        }
        public static string ConnectionStringMaster
        {
            get { return Properties.Settings.Default.ConnectionString + ";Initial Catalog=master"; }
        }

        public static ZKFPEngXControl.ZKFPEngX ZFP;
        public async static Task<bool> InitializeFingerPrintDevice()
        {
            bool b = false;
            await Task.Run(() =>
            {
                if (ZFP == null)
                    ZFP = new ZKFPEngXControl.ZKFPEngX();
                if (ZFP.InitEngine() == 0)
                {
                    ZFP.FPEngineVersion = "10";
                    ZFP.CreateFPCacheDB();
                    ZFP.SensorIndex = 0;
                    b = true;
                }
                else { b = false; }
            });
            return b;
        }

        public static User CurrentUser;
        //============================Retern Server DateTime.Now=============================================================
        public static DateTime DateNow
        {
            get
            {
                string s = _Scalar("Select GetDate();");
                if (!string.IsNullOrEmpty(s))
                {
                    try
                    {
                        return DateTime.Parse(s);
                    }
                    catch { }
                }
                return DateTime.Now;
            }
        }

        //============================Retern Table From Select Query=============================================================
        public static DataTable _Tabling(string Query)
        {
            SqlConnection CN = new SqlConnection(ConnectionString);
            SqlCommand com = new SqlCommand(Query, CN);
            try
            {
                CN.Open();
                SqlDataReader rd = com.ExecuteReader();
                DataTable tempTable = new DataTable();
                if (rd.HasRows)
                {
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        tempTable.Columns.Add(rd.GetName(i), rd.GetFieldType(i));
                    }
                }
                while (rd.Read())
                {
                    object[] os;
                    os = new object[rd.FieldCount];
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        os[i] = rd[i];
                    }
                    tempTable.Rows.Add(os);
                }
                CN.Close();
                return tempTable;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
            finally { CN.Close(); }
        }
        public static async Task<DataTable> _TablingAsync(string Query)
        {
            SqlConnection CN = new SqlConnection(ConnectionString);
            SqlCommand com = new SqlCommand(Query, CN);
            try
            {
                CN.Open();
                SqlDataReader rd = com.ExecuteReader();
                DataTable tempTable = new DataTable();
                await Task.Run(() =>
                {
                    if (rd.HasRows)
                    {
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            tempTable.Columns.Add(rd.GetName(i), rd.GetFieldType(i));
                        }
                    }
                    while (rd.Read())
                    {
                        object[] os;
                        os = new object[rd.FieldCount];
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            os[i] = rd[i];
                        }
                        tempTable.Rows.Add(os);
                    }
                    CN.Close();
                });
                return tempTable;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
            finally { CN.Close(); }
        }
        //============================Retern Table From StoredProcedure=============================================================
        public static DataTable _TablingStoredProcedure(string StoredProcedure)
        {
            SqlConnection CN = new SqlConnection(ConnectionString);
            try
            {
                CN.Open();
                SqlCommand com = new SqlCommand(StoredProcedure, CN);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rd = com.ExecuteReader();
                DataTable tempTable = new DataTable();
                for (int i = 0; i < rd.FieldCount; i++)
                {
                    tempTable.Columns.Add(rd.GetName(i), rd.GetFieldType(i));
                }
                while (rd.Read())
                {
                    object[] os;
                    os = new object[rd.FieldCount];
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        os[i] = rd[i];
                    }
                    tempTable.Rows.Add(os);
                }
                CN.Close();
                return tempTable;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return new DataTable(); }
            finally { CN.Close(); }
        }
        public static async Task<DataTable> _TablingStoredProcedureAsync(string StoredProcedure, params SqlParameter[] prs)
        {
            DataTable tempTable = new DataTable();
            SqlConnection CN = new SqlConnection(ConnectionString);
            CN.Open();
            SqlCommand com = new SqlCommand(StoredProcedure, CN);
            com.CommandType = CommandType.StoredProcedure;
            foreach (var item in prs)
            {
                if (item.Value == null)
                    item.Value = DBNull.Value;
                com.Parameters.Add(item);
            }
            await Task.Run(() =>
            {
                try
                {
                    SqlDataReader rd = com.ExecuteReader();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        tempTable.Columns.Add(rd.GetName(i), rd.GetFieldType(i));
                    }
                    while (rd.Read())
                    {
                        object[] os;
                        os = new object[rd.FieldCount];
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            os[i] = rd[i];
                        }
                        tempTable.Rows.Add(os);
                    }
                    CN.Close();
                    return tempTable;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return new DataTable(); }
                finally { CN.Close(); }
            });
            return tempTable;
        }
        public static DataTable _TablingStoredProcedure(string StoredProcedure, params SqlParameter[] prs)
        {
            SqlConnection CN = new SqlConnection(ConnectionString);
            try
            {
                CN.Open();
                SqlCommand com = new SqlCommand(StoredProcedure, CN);
                com.CommandType = CommandType.StoredProcedure;
                foreach (var item in prs)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                    com.Parameters.Add(item);
                }
                SqlDataReader rd = com.ExecuteReader();
                DataTable tempTable = new DataTable();
                for (int i = 0; i < rd.FieldCount; i++)
                {
                    tempTable.Columns.Add(rd.GetName(i), rd.GetFieldType(i));
                }
                while (rd.Read())
                {
                    object[] os;
                    os = new object[rd.FieldCount];
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        os[i] = rd[i];
                    }
                    tempTable.Rows.Add(os);
                }
                CN.Close();
                return tempTable;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return new DataTable(); }
            finally { CN.Close(); }
        }

        //============================Return Cell From Select Query=============================================================
        public static string _Scalar(string Query)
        {

            SqlConnection CN = new SqlConnection(ConnectionString);
            try
            {
                CN.Open();
                SqlCommand com = new SqlCommand(Query, CN);
                var temp = com.ExecuteScalar();
                CN.Close();
                if (temp == null)
                    return "";
                else return temp.ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return ""; }
            finally
            {
                CN.Close();
            }
        }
        //============================Return Cell From Select Query=============================================================
        public static string _Scalar_StoredProcedure(string StoredProcedure, params SqlParameter[] prs)
        {
            SqlConnection MyConn = new SqlConnection(ConnectionString);
            try
            {
                MyConn.Open();
                string MyQuery = StoredProcedure;
                SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                MyComm.CommandType = CommandType.StoredProcedure;
                foreach (var item in prs)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                    MyComm.Parameters.Add(item);
                }
                var x = MyComm.ExecuteScalar();
                string temp = x == null ? "" : x.ToString();
                MyConn.Close();
                return temp;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return ""; }
            finally { MyConn.Close(); }
        }
        //============================Return Cell From Select Query=============================================================
        public static string _Scalar(string Query, string ConnectionString)
        {

            SqlConnection CN = new SqlConnection(ConnectionString);
            try
            {
                CN.Open();
                SqlCommand com = new SqlCommand(Query, CN);
                var x = com.ExecuteScalar();
                string temp = x == null ? "" : x.ToString();
                CN.Close();
                return temp;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return ""; }
            finally
            {
                CN.Close();
            }
        }
        //============================Execute Insert,Update,Delete Query And Return The Number Of Rows==========================
        public static int _NonQuery(string Query)
        {
            try
            {
                SqlConnection CN = new SqlConnection(ConnectionString);
                CN.Open();
                SqlCommand com = new SqlCommand(Query, CN);
                int temp = com.ExecuteNonQuery();
                CN.Close();
                return temp;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return 0; }
        }
        //============================Execute Insert,Update,Delete Query And Return The Number Of Rows==========================
        public static int _NonQuery(string Query, string ConnectionString)
        {
            try
            {
                SqlConnection CN = new SqlConnection(ConnectionString);
                CN.Open();
                SqlCommand com = new SqlCommand(Query, CN);
                int temp = com.ExecuteNonQuery();
                CN.Close();
                return temp;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return 0; }
        }
        //============================StoredProcedures + Parameters==========================
        public static bool _StoredProcedure(string StoredProcedure, params SqlParameter[] prs)
        {
            SqlConnection MyConn = new SqlConnection(ConnectionString);
            try
            {
                MyConn.Open();
                string MyQuery = StoredProcedure;
                SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                MyComm.CommandType = CommandType.StoredProcedure;
                foreach (var item in prs)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                    MyComm.Parameters.Add(item);
                }
                MyComm.ExecuteNonQuery();
                MyConn.Close();
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
            finally { MyConn.Close(); }
        }
        //============================StoredProcedures + Parameters + Awaitable==========================
        public static async Task<bool> _StoredProcedureAsync(string StoredProcedure, params SqlParameter[] prs)
        {
            bool b = false;
            await Task.Run(() =>
             {
                 SqlConnection MyConn = new SqlConnection(ConnectionString);
                 try
                 {
                     MyConn.Open();
                     string MyQuery = StoredProcedure;
                     SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                     MyComm.CommandType = CommandType.StoredProcedure;
                     foreach (var item in prs)
                     {
                         if (item.Value == null)
                             item.Value = DBNull.Value;
                         MyComm.Parameters.Add(item);
                     }
                     MyComm.ExecuteNonQuery();
                     MyConn.Close();
                     b = true;
                 }
                 catch (Exception ex) { MessageBox.Show(ex.Message); b = false; }
                 finally { MyConn.Close(); }
             });
            return b;
        }
        //============================StoredProcedures + Parameters==========================
        public static int? _StoredProcedureReturnable(string StoredProcedureName, params SqlParameter[] prs)
        {
            prs[0].Direction = ParameterDirection.Output;
            SqlConnection MyConn = new SqlConnection(ConnectionString);
            try
            {
                MyConn.Open();
                string MyQuery = StoredProcedureName;
                SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                MyComm.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < prs.Length; i++)
                {
                    if (prs[i].Value == null)
                    {
                        prs[i].Value = DBNull.Value;
                    }
                    MyComm.Parameters.Add(prs[i]);
                }
                MyComm.ExecuteNonQuery();
                MyConn.Close();
                return (int?)MyComm.Parameters[0].Value;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
            finally { MyConn.Close(); }
        }
        //============================StoredProcedures + Parameters==========================
        public static bool _StoredProcedureReturnableBool(string StoredProcedureName, params SqlParameter[] prs)
        {
            prs[0].Direction = ParameterDirection.Output;
            SqlConnection MyConn = new SqlConnection(ConnectionString);
            try
            {
                MyConn.Open();
                string MyQuery = StoredProcedureName;
                SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                MyComm.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < prs.Length; i++)
                {
                    if (prs[i].Value == null)
                    {
                        prs[i].Value = DBNull.Value;
                    }
                    MyComm.Parameters.Add(prs[i]);
                }
                MyComm.ExecuteNonQuery();
                MyConn.Close();
                return (bool)MyComm.Parameters[0].Value;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
            finally { MyConn.Close(); }
        }
        //=========================================================================================
        public static void EmptyEverything(params object[] Objects)
        {
            foreach (object item in Objects)
            {
                if (item is ComboBox)
                {
                    ((ComboBox)item).SelectedIndex = -1;
                }
                else if (item is TextBox)
                {
                    ((TextBox)item).Text = "";
                }
                else if (item is CheckBox)
                {
                    ((CheckBox)item).IsChecked = false;
                }
            }
        }
        //=========================================================================================
        public static List<string> GetAllStrings(string Query)
        {
            List<string> Ss = new List<string>();
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand com = new SqlCommand(Query, con);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    if (!(rd[0] is DBNull))
                        Ss.Add(rd.GetString(0));
                }
                rd.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
                return new List<string>();
            }
            finally
            {
                con.Close();
            }
            return Ss;
        }
        //=========================================================================================
        public static List<string> GetAllStrings(string Query, string ConnectionString)
        {
            List<string> Ss = new List<string>();
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand com = new SqlCommand(Query, con);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    if (!(rd[0] is DBNull))
                        Ss.Add(rd.GetString(0));
                }
                rd.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
                return new List<string>();
            }
            finally
            {
                con.Close();
            }
            return Ss;
        }
        //=========================================================================================
        public static List<string> GetAllStringsWithAll(string Query)
        {
            List<string> Ss = new List<string>();
            Ss.Add("الكل");
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand com = new SqlCommand(Query, con);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    if (!(rd[0] is DBNull))
                        Ss.Add(rd.GetString(0));
                }
                rd.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
                return new List<string>();
            }
            finally
            {
                con.Close();
            }
            return Ss;
        }
        //=========================================================================================
        public static string CheckImageFile(string ImagePath, string OldImagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(ImagePath) && ImagePath.Remove(4) != "Data")
                {
                    Random x = new Random();
                    string s = "Data\\" + x.Next(0, 999999999);
                    while (System.IO.File.Exists(s))
                    {
                        s = "Data\\" + x.Next(0, 999999999);
                    }
                    System.IO.File.Copy(ImagePath, s);
                    return s;
                }
                if (!string.IsNullOrEmpty(OldImagePath) && OldImagePath.Remove(4) == "Data")
                {
                    DeleteImageFIle(OldImagePath);
                }
                return ImagePath;
            }
            catch { MessageBox.Show("خطأ في الوصول للملف "); return ImagePath; }
        }
        //=========================================================================================
        public static string CheckImageFile(string ImagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(ImagePath) && ImagePath.Remove(4) != "Data")
                {
                    Random x = new Random();
                    string s = "Data\\" + x.Next(0, 999999999);
                    while (System.IO.File.Exists(s))
                    {
                        s = "Data\\" + x.Next(0, 999999999);
                    }
                    System.IO.File.Copy(ImagePath, s);
                    return s;
                }
                return ImagePath;
            }
            catch { MessageBox.Show("خطأ في الوصول للملف "); return ImagePath; }
        }
        //=========================================================================================
        public static void DeleteImageFIle(string ImagePath)
        {
            try
            {
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
            }
            catch { }
        }
        //=========================================================================================
        public static bool CheckConnection(string ConnectionString, out string Error)
        {
            SqlConnection con = new SqlConnection(ConnectionString + (ConnectionString.Contains("Initial Catalog") ? "" : ";Initial Catalog=master"));
            SqlCommand com = new SqlCommand("Select 1", con);
            try
            {
                com.CommandTimeout = 5000;
                con.Open();
                con.Close();
                Error = "";
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        //=========================================================================================
        public static bool? IsDataBaseExists(string DataBaseName)
        {
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionStringMaster);
            SqlCommand com = new SqlCommand("select case when exists (select Name from sys.databases where Name like '" + DataBaseName + "') then 'E' else 'X' end", con);
            try
            {
                con.Open();
                var s = com.ExecuteScalar();
                con.Close();
                return s.ToString() == "E" ? true : false;
            }
            catch { return null; }
        }
        //=========================================================================================
        public static bool IsStringNumber(string s)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                return false;
            foreach (var item in s)
            {
                if (!char.IsDigit(item))
                    return false;
            }
            return true;
            //=========================================================================================
        }
        //=========================================================================================
        public static void MakeTabItemGreen(TabItem ti)
        {
            ti.Background = App.Current.FindResource("MyTabItemBrush") as LinearGradientBrush;
        }
        public static void MakeTabItemRed(TabItem ti)
        {
            ti.Background = App.Current.FindResource("MyTabItemBrush2") as LinearGradientBrush;
        }
        //=========================================================================================
    }
}
