using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for ExcelImport4Aleppo.xaml
    /// </summary>
    public partial class ExcelImport4Aleppo : UserControl
    {
        DataTable dt;
        public ExcelImport4Aleppo()
        {
            InitializeComponent();
        }

        private DataTable MakeDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns["ID"].Caption = "التسلسل";
            dt.Columns["ID"].Unique = true;
            dt.Columns.Add("Name", typeof(string));
            dt.Columns["Name"].Caption = "الاسم الثلاثي";
            dt.Columns["Name"].MaxLength = 100;
            dt.Columns.Add("PID", typeof(string));
            dt.Columns["PID"].Caption = "الرقم الوطني";
            dt.Columns["PID"].MaxLength = 50;
            dt.Columns.Add("WifeName", typeof(string));
            dt.Columns["WifeName"].Caption = "اسم الزوجة";
            dt.Columns["WifeName"].MaxLength = 100;
            dt.Columns.Add("WifePID", typeof(string));
            dt.Columns["WifePID"].Caption = "الرقم الوطني للزوجة";
            dt.Columns["WifePID"].MaxLength = 50;
            dt.Columns.Add("FamilyBookID", typeof(string));
            dt.Columns["FamilyBookID"].Caption = "رقم دفتر العائلة";
            dt.Columns["FamilyBookID"].MaxLength = 20;
            dt.Columns.Add("FamilyBookChar", typeof(string));
            dt.Columns["FamilyBookChar"].Caption = "حرف دفتر العائلة";
            dt.Columns["FamilyBookChar"].MaxLength = 5;
            dt.Columns.Add("FamilyNumber", typeof(string));
            dt.Columns["FamilyNumber"].Caption = "الرقم الأسري";
            dt.Columns["FamilyNumber"].MaxLength = 10;
            dt.Columns.Add("FamilyMemberCount", typeof(int));
            dt.Columns["FamilyMemberCount"].Caption = "عدد الأفراد";
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns["Phone"].Caption = "الهاتف";
            dt.Columns["Phone"].MaxLength = 30;
            dt.Columns.Add("Mobile", typeof(string));
            dt.Columns["Mobile"].Caption = "الموبايل";
            dt.Columns["Mobile"].MaxLength = 30;
            dt.Columns.Add("FamilyStatus", typeof(string));
            dt.Columns["FamilyStatus"].Caption = "حالة العائلة";
            dt.Columns.Add("BasePlace", typeof(string));
            dt.Columns["BasePlace"].Caption = "المكان الأصلي";
            dt.Columns["BasePlace"].MaxLength = 200;
            dt.Columns.Add("CurrentPlace", typeof(string));
            dt.Columns["CurrentPlace"].Caption = "السكن الحالي";
            dt.Columns.Add("Notes", typeof(string));
            dt.Columns["Notes"].Caption = "ملاحظات";
            dt.Columns.Add("Diseases", typeof(string));
            dt.Columns["Diseases"].Caption = "الأمراض المزمنة";
            dt.Columns.Add("Baby1", typeof(int));
            dt.Columns["Baby1"].Caption = "الأطفال 1";
            dt.Columns.Add("Baby2", typeof(int));
            dt.Columns["Baby2"].Caption = "الأطفال 2";
            dt.Columns.Add("Baby3", typeof(int));
            dt.Columns["Baby3"].Caption = "الأطفال 3";
            dt.Columns.Add("Male1", typeof(int));
            dt.Columns["Male1"].Caption = "الذكور 1";
            dt.Columns.Add("Male2", typeof(int));
            dt.Columns["Male2"].Caption = "الذكور 2";
            dt.Columns.Add("Female1", typeof(int));
            dt.Columns["Female1"].Caption = "الإناث 1";
            dt.Columns.Add("Female2", typeof(int));
            dt.Columns["Female2"].Caption = "الإناث 2";
            dt.Columns.Add("Female3", typeof(int));
            dt.Columns["Female3"].Caption = "الإناث 3";
            dt.Columns.Add("Evaluation", typeof(string));
            dt.Columns["Evaluation"].Caption = "تقييم العائلة";
            dt.Columns["Evaluation"].MaxLength = 10;
            dt.Columns.Add("FamilySituation", typeof(string));
            dt.Columns["FamilySituation"].Caption = "الحالة العائلية";
            dt.Columns["FamilySituation"].MaxLength = 20;
            dt.Columns.Add("MedicalSituation", typeof(string));
            dt.Columns["MedicalSituation"].Caption = "الحالة الصحية";
            dt.Columns["MedicalSituation"].MaxLength = 50;
            dt.Columns.Add("IsProblem", typeof(bool));
            dt.Columns.Add("Problem", typeof(string));
            dt.Columns["Problem"].Caption = "المشكلة";
            return dt;
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (cLoadingShape.Visibility != System.Windows.Visibility.Visible)
            {
                btnSlideLeft.IsEnabled = false;
                txtExcelDataDetail2.Text = "";
                dt = null;
                dgExcelFile.ItemsSource = null;

                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Title = "إختر ملف الإكسل";
                myDialog.Filter = "Excel WorkSheets(*.xlsx)(*.xls)|*.xls;*.xlsx";
                myDialog.CheckFileExists = true;
                if (myDialog.ShowDialog() == true && myDialog.CheckFileExists)
                {
                    txtFileName.Text = myDialog.FileName;
                    cLoadingShape.Visibility = System.Windows.Visibility.Visible;

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.WorkerSupportsCancellation = true;
                    bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                    bw.DoWork += bw_DoWork;
                    bw.RunWorkerAsync(txtFileName.Text);
                }
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dtx = new DataTable();

            if (File.Exists(e.Argument.ToString()))
            {
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + e.Argument.ToString() + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(strConn);
                try
                {
                    conn.Open();
                    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    DataRow schemaRow = schemaTable.Rows[0];
                    string sheet = schemaRow["TABLE_NAME"].ToString();
                    if (!sheet.EndsWith("_"))
                    {
                        string query = "SELECT  * FROM [" + sheet + "]";
                        OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                        dtx.Locale = CultureInfo.CurrentCulture;
                        daexcel.Fill(dtx);
                        conn.Close();
                        dt = dtx.Copy();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                    dtx.Dispose();
                }
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                cLoadingShape.Visibility = System.Windows.Visibility.Hidden;
                btnSlideLeft.IsEnabled = true;
                while (dt.Rows[0].ItemArray[1].ToString().Trim() == "")
                    dt.Rows.RemoveAt(0);

                while (dt.Rows[dt.Rows.Count - 1].ItemArray[1].ToString() == "")
                    dt.Rows.RemoveAt(dt.Rows.Count - 1);
                txtExcelDataDetail2.Text = "عدد السجلات في الملف : " + dt.Rows.Count;
                dgExcelFile.ItemsSource = dt.DefaultView;
            }
            else
            {
                btnSlideLeft.IsEnabled = false;
                txtExcelDataDetail2.Text = "حدث خطأ أثناء تحميل الملف";
                cLoadingShape.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        object GetNumber(object o)
        {
            if (o is DBNull)
                return o;
            var x = o.ToString().Trim();
            if (string.IsNullOrEmpty(x))
                return DBNull.Value;
            foreach (var c in x)
            {
                if (!char.IsDigit(c))
                {
                    return DBNull.Value;
                }
            }
            return int.Parse(x);
        }
        string GetPIDString(object o)
        {
            if (o is DBNull)
                return o.ToString();
            var x = o.ToString().Trim();
            if (string.IsNullOrEmpty(x))
                return "";
            string s = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (char.IsDigit(x[i]))
                {
                    s += x[i];
                }
            }
            return s;
        }
        string GetString(object o)
        {
            if (o is DBNull)
                return o.ToString();
            var x = o.ToString().Trim();
            if (string.IsNullOrEmpty(x))
                return "";
            string s = "";
            bool IsLastCharSpace = false; ;
            for (int i = 0; i < x.Length; i++)
            {
                if (char.IsWhiteSpace(x[i]))
                {
                    if (IsLastCharSpace)
                        continue;
                    s += " ";
                    IsLastCharSpace = true;
                    continue;
                }
                else IsLastCharSpace = false;
                if (char.IsSymbol(x[i]))
                {
                    if (x[i] == '+')
                        s += x[i];
                    continue;
                }
                if (char.IsLetter(x[i]))
                    s += x[i];
            }
            if (s == "متوفي" || s == "متوفيه" || s == "متوفى" || s == "أرمله" || s == "مطلقه") return "";
            return s;
        }

        private void btnSaveTable_Click(object sender, RoutedEventArgs e)
        {
            if (dgExcelFile.ItemsSource != null && dgExcelFile.Items.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Family f = new Family();
                    House h = new House();
                    List<FamilyPerson> fps = new List<FamilyPerson>();
                    try
                    {
                        if (dt.DefaultView[i][1].ToString() == "")
                            continue;
                        f.FamilyCode = dt.DefaultView[i][1].ToString();
                        f.FamilyType = dt.DefaultView[i][2].ToString();
                        f.FamilyIdentityID = dt.DefaultView[i][3].ToString();
                        if (dt.DefaultView[i][4].ToString() != "")
                        {
                            f.FamilyIdentityID += " " + dt.DefaultView[i][4].ToString();
                            if (dt.DefaultView[i][5].ToString() != "")
                                f.FamilyIdentityID += " " + dt.DefaultView[i][5].ToString();
                        }
                        f.ApplyDate = GetDate(dt.DefaultView[i][6].ToString());
                        f.FamilyPersonCount = dt.DefaultView[i][7].ToString();
                        f.FamilyName = dt.DefaultView[i][8].ToString();
                        f.Notes = dt.DefaultView[i][9].ToString();

                        f.FamilyFather.FirstName = dt.DefaultView[i][10].ToString();
                        f.FamilyFather.FatherName = dt.DefaultView[i][11].ToString();
                        f.FamilyFather.LastName = dt.DefaultView[i][12].ToString();
                        f.FamilyFather.BirthPlace = dt.DefaultView[i][13].ToString();
                        f.FamilyFather.DOB = GetDate(dt.DefaultView[i][14].ToString());
                        f.FamilyFather.PID = dt.DefaultView[i][15].ToString();

                        f.FamilyMother.FirstName = dt.DefaultView[i][16].ToString();
                        f.FamilyMother.FatherName = dt.DefaultView[i][17].ToString();
                        f.FamilyMother.LastName = dt.DefaultView[i][18].ToString();
                        f.FamilyMother.BirthPlace = dt.DefaultView[i][19].ToString();
                        f.FamilyMother.DOB = GetDate(dt.DefaultView[i][20].ToString());
                        f.FamilyMother.PID = dt.DefaultView[i][21].ToString();

                        h.Address = dt.DefaultView[i][22].ToString() + ((dt.DefaultView[i][23].ToString().Trim() == "") ? "" : (" بناء " + dt.DefaultView[i][23].ToString())) + ((dt.DefaultView[i][24].ToString().Trim() == "") ? "" : " طابق  " + dt.DefaultView[i][24].ToString());
                        h.HouseType = dt.DefaultView[i][25].ToString();

                        f.FamilyFather.Phone = dt.DefaultView[i][26].ToString();
                        f.FamilyFather.Mobile = dt.DefaultView[i][27].ToString();

                        h.OldAddress = dt.DefaultView[i][28].ToString();

                        int j = 29;
                        while (dt.DefaultView[i][j].ToString().Trim() != "")
                        {
                            FamilyPerson c = new FamilyPerson();
                            c.FirstName = dt.DefaultView[i][j].ToString();
                            c.DOB = GetDate(dt.DefaultView[i][j + 1].ToString());
                            c.Gender = (dt.DefaultView[i][j + 2].ToString() == "ذ") ? "ذكر" : "أنثى";
                            j += 3;

                            fps.Add(c);
                        }


                        string temp = BaseDataBase._Scalar("select FamilyID from Family where FamilyCode = '" + f.FamilyCode + "'");
                        if (string.IsNullOrEmpty(temp))
                        {
                            f.FamilyID = null;
                            f.FamilyID = BaseDataBase._StoredProcedureReturnable("sp_Add2Family4Aleppo"
                                 , new SqlParameter("@FamilyID", SqlDbType.Int)
                                 , new SqlParameter("@FamilyCode", f.FamilyCode)
                                 , new SqlParameter("@FamilyName", f.FamilyName)
                                 , new SqlParameter("@FamilyType", f.FamilyType)
                                 , new SqlParameter("@FamilyIdentityID", f.FamilyIdentityID)
                                 , new SqlParameter("@ApplyDate", f.ApplyDate)
                                 , new SqlParameter("@FamilyPersonCount", f.FamilyPersonCount)
                                 , new SqlParameter("@FamilyStatus", f.FamilyStatus)
                                 , new SqlParameter("@Evaluation", f.Evaluation)
                                 , new SqlParameter("@Notes", f.Notes)
                                 , new SqlParameter("@Phone", f.FamilyFather.Phone)
                                 , new SqlParameter("@Mobile", f.FamilyFather.Mobile)
                                 , new SqlParameter("@FirstNameFF", f.FamilyFather.FirstName)
                                 , new SqlParameter("@FirstNameFFa", f.FamilyFather.FatherName)
                                 , new SqlParameter("@FirstNameFL", f.FamilyFather.LastName)
                                 , new SqlParameter("@BirthPlaceF", f.FamilyFather.BirthPlace)
                                 , new SqlParameter("@DOBF", f.FamilyFather.DOB)
                                 , new SqlParameter("@PIDF", f.FamilyFather.PID)
                                 , new SqlParameter("@FirstNameMF", f.FamilyMother.FirstName)
                                 , new SqlParameter("@FirstNameMFa", f.FamilyMother.FatherName)
                                 , new SqlParameter("@FirstNameML", f.FamilyMother.LastName)
                                 , new SqlParameter("@BirthPlaceM", f.FamilyMother.BirthPlace)
                                 , new SqlParameter("@DOBM", f.FamilyMother.DOB)
                                 , new SqlParameter("@PIDM", f.FamilyMother.PID)
                                 , new SqlParameter("@OldAddress", h.OldAddress)
                                 , new SqlParameter("@Address", h.Address));
                            if (!f.FamilyID.HasValue)
                            {
                                MessageBox.Show("خطأ في بيانات العائلة " + f.FamilyName + "\nذات الرقم " + f.FamilyCode);
                                continue;
                            }
                        }
                        else
                        {
                            f.FamilyID = int.Parse(temp);
                            if (!BaseDataBase._StoredProcedure("sp_Add2Family4Aleppo1"
                                , new SqlParameter("@FamilyID", f.FamilyID)
                                , new SqlParameter("@FamilyCode", f.FamilyCode)
                                , new SqlParameter("@FamilyName", f.FamilyName)
                                , new SqlParameter("@FamilyType", f.FamilyType)
                                , new SqlParameter("@FamilyIdentityID", f.FamilyIdentityID)
                                , new SqlParameter("@ApplyDate", f.ApplyDate)
                                , new SqlParameter("@FamilyPersonCount", f.FamilyPersonCount)
                                 , new SqlParameter("@FamilyStatus", f.FamilyStatus)
                                , new SqlParameter("@Evaluation", f.Evaluation)
                                , new SqlParameter("@Notes", f.Notes)
                                , new SqlParameter("@Phone", f.FamilyFather.Phone)
                                , new SqlParameter("@Mobile", f.FamilyFather.Mobile)
                                , new SqlParameter("@FirstNameFF", f.FamilyFather.FirstName)
                                , new SqlParameter("@FirstNameFFa", f.FamilyFather.FatherName)
                                , new SqlParameter("@FirstNameFL", f.FamilyFather.LastName)
                                , new SqlParameter("@BirthPlaceF", f.FamilyFather.BirthPlace)
                                , new SqlParameter("@DOBF", f.FamilyFather.DOB)
                                , new SqlParameter("@PIDF", f.FamilyFather.PID)
                                , new SqlParameter("@FirstNameMF", f.FamilyMother.FirstName)
                                , new SqlParameter("@FirstNameMFa", f.FamilyMother.FatherName)
                                , new SqlParameter("@FirstNameML", f.FamilyMother.LastName)
                                , new SqlParameter("@BirthPlaceM", f.FamilyMother.BirthPlace)
                                , new SqlParameter("@DOBM", f.FamilyMother.DOB)
                                , new SqlParameter("@PIDM", f.FamilyMother.PID)
                                , new SqlParameter("@OldAddress", h.OldAddress)
                                , new SqlParameter("@Address", h.Address)))
                            {
                                MessageBox.Show("خطأ في بيانات العائلة " + f.FamilyName + "\nذات الرقم " + f.FamilyCode);
                                continue;
                            }
                        }

                        BaseDataBase._NonQuery("delete from child where FamilyID = " + f.FamilyID);
                        foreach (var item in fps)
                        {
                            item.FamilyID = f.FamilyID;
                            DBMain.InsertData(item);
                        }
                    }
                    catch { }
                }
            }
        }

        DateTime? GetDate(string s)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                s = s.Trim();
                s = s.Replace("\\", "-").Replace("\\\\", "-");
                s = s.Replace("/", "-").Replace("//", "-");
                if (s.Length == 0)
                    return null;
                if (s.Length == 4)
                {
                    return DateTime.Parse("01-" + "01-" + s);
                }
                else
                {
                    var xs = s.Split('-');
                    return DateTime.ParseExact(((xs[0].Length == 1) ? "0" + xs[0] : xs[0]) + "-" + ((xs[1].Length == 1) ? "0" + xs[1] : xs[1]) + "-" + xs[2], "dd-MM-yyyy", provider);
                }
            }
            catch { return null; }
        }

        private void btnI7san_Click(object sender, RoutedEventArgs e)
        {
            if (cLoadingShape.Visibility != System.Windows.Visibility.Visible)
            {
                btnSlideLeft.IsEnabled = false;
                txtExcelDataDetail2.Text = "";
                dt = null;
                dgExcelFile.ItemsSource = null;

                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Title = "إختر ملف الإكسل";
                myDialog.Filter = "Excel WorkSheets(*.xlsx)(*.xls)|*.xls;*.xlsx";
                myDialog.CheckFileExists = true;
                if (myDialog.ShowDialog() == true && myDialog.CheckFileExists)
                {
                    txtFileName.Text = myDialog.FileName;
                    cLoadingShape.Visibility = System.Windows.Visibility.Visible;

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.WorkerSupportsCancellation = true;
                    bw.RunWorkerCompleted += bw_RunWorkerCompletedI7san;
                    bw.DoWork += bw_DoWork;
                    bw.RunWorkerAsync(txtFileName.Text);
                }
            }
        }

        private void btnSaveTableI7san_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("GetI7sanData", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@tb", (dgExcelFile.ItemsSource as DataView).Table));
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
        private void bw_RunWorkerCompletedI7san(object sender, RunWorkerCompletedEventArgs e)
        {
            cLoadingShape.Visibility = System.Windows.Visibility.Hidden;
            if (!e.Cancelled)
            {
                dgExcelFile.ItemsSource = dt.DefaultView;
            }
            else
            {
                txtExcelDataDetail2.Text = "حدث خطأ أثناء تحميل الملف";
            }
        }
    }
}