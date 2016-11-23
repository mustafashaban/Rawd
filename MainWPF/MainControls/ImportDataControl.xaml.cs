using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for ImportDataControl.xaml
    /// </summary>
    public partial class ImportDataControl : UserControl
    {
        DataTable dt;
        public ImportDataControl()
        {
            InitializeComponent();
        }

        private DataTable MakeDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
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
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + e.Argument.ToString() + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(strConn);
                int i = -1;
                int j = -1;
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
                    }

                    if (dtx.Columns.Count < 24)
                    {
                        MessageBox.Show("الملف الذي تم اختياره يحوي أقل من عدد الأعمدة المطلوبة ");
                        e.Cancel = true;
                        return;
                    }
                    if (dtx.Rows.Count == 0)
                    {
                        MessageBox.Show("الملف الذي تم اختياره فارغ!!");
                        e.Cancel = true;
                        return;
                    }

                    for (int l = 0; l < dtx.Rows.Count; l++)
                    {
                        try
                        {
                            if (dtx.Rows[0].ItemArray[0].ToString().Trim() != null)
                            {
                                break;
                            }
                            else
                            {
                                dtx.Rows.RemoveAt(0);
                            }
                        }
                        catch
                        {
                            dtx.Rows.RemoveAt(0);
                        }
                    }

                    dt = MakeDataTable();
                    for (i = 0; i < dtx.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dtx.Rows[i].ItemArray[0].ToString().Trim()) || string.IsNullOrEmpty(dtx.Rows[i].ItemArray[1].ToString().Trim()) && string.IsNullOrEmpty(dtx.Rows[i].ItemArray[3].ToString().Trim()))
                        {
                            continue;
                        }

                        j = 0;
                        try
                        {
                            DataRow dr = dt.NewRow();
                            dr["ID"] = dtx.Rows[i].ItemArray[j++];
                            dr["Name"] = dtx.Rows[i].ItemArray[j++].ToString().Trim().Length > 100 ? "" : GetString(dtx.Rows[i].ItemArray[j - 1]);
                            dr["PID"] = GetPIDString(dtx.Rows[i].ItemArray[j++]);
                            dr["WifeName"] = GetString(dtx.Rows[i].ItemArray[j++]);
                            dr["WifePID"] = GetPIDString(dtx.Rows[i].ItemArray[j++]);
                            dr["FamilyBookID"] = GetPIDString(dtx.Rows[i].ItemArray[j++]);
                            if (dtx.Rows[i].ItemArray[j++].ToString().Length <= 5)
                                dr["FamilyBookChar"] = dtx.Rows[i].ItemArray[j - 1];
                            else dr["Notes"] = dtx.Rows[i].ItemArray[j - 1] + " - ";
                            dr["FamilyNumber"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["FamilyMemberCount"] = dtx.Rows[i].ItemArray[j++];
                            dr["Phone"] = dtx.Rows[i].ItemArray[j++];
                            dr["Mobile"] = dtx.Rows[i].ItemArray[j++];
                            dr["FamilyStatus"] = dtx.Rows[i].ItemArray[j++];
                            dr["BasePlace"] = dtx.Rows[i].ItemArray[j++];
                            dr["CurrentPlace"] = dtx.Rows[i].ItemArray[j++];
                            dr["Notes"] = dtx.Rows[i].ItemArray[j++] + ((dtx.Rows[i].ItemArray[6].ToString().Length > 5) ? " - " + dtx.Rows[i].ItemArray[6].ToString() : "");
                            dr["Diseases"] = dtx.Rows[i].ItemArray[j++];
                            dr["Baby1"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["Baby2"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["Baby3"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["Male1"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["Male2"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["Female1"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["Female2"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["Female3"] = GetNumber(dtx.Rows[i].ItemArray[j++]);
                            dr["IsProblem"] = false;
                            dr["Problem"] = "";

                            if (dtx.Columns.Count >= 27)
                            {
                                dr["Evaluation"] = dtx.Rows[i].ItemArray[j++];
                                dr["FamilySituation"] = dtx.Rows[i].ItemArray[j++];
                                dr["MedicalSituation"] = dtx.Rows[i].ItemArray[j++];
                            }
                            dt.Rows.Add(dr);
                        }
                        catch (ArgumentException ae)
                        {
                            if (MessageBox.Show("النص في السطر " + (i + 1) + " والعمود '" + dt.Columns[j - 1].Caption + "' كبير\n\n" + ae.Message + "\nهل تريد تجاوز الخطأ\n\nملاحظة: في حال الضغط على \"نعم\" سيتم تجاوز الخطأ وإكمال تعبئة البيانات\nفي حال الضغط على \"لا\" سيتم التوقف عند السطر " + (i + 1), "", MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.Yes, MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
                                continue;
                            else break;
                        }
                        catch (ConstraintException)
                        {
                            MessageBox.Show("خطأ في السطر " + (i + 1) + " \nيوجد قيمة مكررة في عمود التسلسل");
                            conn.Close();
                            e.Cancel = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 0)
                            {
                                MessageBox.Show(ex.Message);
                                break;
                            }
                            if (MessageBox.Show("خطأ في السطر  " + (i + 1) + " والعمود '" + dt.Columns[j - 1].Caption + "'\nتسلسل رقم  " + dtx.Rows[i].ItemArray[0].ToString().Trim() + "\n" + ex.Message + "\nهل تريد تجاوز الخطأ\n\nملاحظة: في حال الضغط على \"نعم\" سيتم تجاوز الخطأ وإكمال تعبئة البيانات\nفي حال الضغط على \"لا\" سيتم التوقف عند السطر " + (i + 1), "", MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.Yes, MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
                                continue;
                            else break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show((i == -1) ? "" : ("خطأ في السطر :" + (i + 1) + "\n\n") + ex.Message);
                    e.Cancel = true;
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
            if (cmboFamilyType.SelectedIndex == -1)
            {
                MyMessageBox.Show("يجب اختيار نوع العوائل");
                return;
            }
            if (dgExcelFile.ItemsSource != null && dgExcelFile.Items.Count > 0 && MyMessageBox.Show("هل تريد تأكيد إضافة البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BackgroundWorker bw2 = new BackgroundWorker();
                bw2.WorkerSupportsCancellation = true;
                bw2.WorkerReportsProgress = true;
                bw2.RunWorkerCompleted += bw2_RunWorkerCompleted;
                bw2.DoWork += bw2_DoWork;
                bw2.ProgressChanged += new ProgressChangedEventHandler(bw2_ProgressChanged);
                dt.TableName = cmboFamilyType.Text;
                bw2.RunWorkerAsync(dt);
            }
        }
        private void bw2_DoWork(object sender, DoWorkEventArgs e)
        {
            var bw = sender as BackgroundWorker;
            DataTable dt = e.Argument as DataTable;
            Family f = new Family();
            f.FamilyType = dt.TableName;
            House h = new House();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                f.FamilyCode = dt.DefaultView[i]["ID"].ToString();
                f.FamilyName = dt.DefaultView[i]["Name"].ToString() != "" ? dt.DefaultView[i]["Name"].ToString() : dt.DefaultView[i]["WifeName"].ToString();
                f.FamilyFather.FirstName = dt.DefaultView[i]["Name"].ToString();
                f.FamilyFather.PID = dt.DefaultView[i]["PID"].ToString();
                f.FamilyMother.FirstName = dt.DefaultView[i]["WifeName"].ToString();
                f.FamilyMother.PID = dt.DefaultView[i]["WifePID"].ToString();
                f.FamilyIdentityID = dt.DefaultView[i]["FamilyBookID"].ToString() + ((dt.DefaultView[i]["FamilyBookChar"].ToString() == "") ? "" : ((" " + dt.DefaultView[i]["FamilyBookChar"].ToString()) + ((dt.DefaultView[i]["FamilyNumber"].ToString() == "") ? "" : " " + dt.DefaultView[i]["FamilyNumber"].ToString())));
                if (f.FamilyIdentityID == "") f.FamilyIdentityID = "لايوجد دفتر عائلة";
                f.FamilyPersonCount = dt.DefaultView[i]["FamilyMemberCount"].ToString();
                f.FamilyFather.Phone = dt.DefaultView[i]["Phone"].ToString();
                f.FamilyFather.Mobile = dt.DefaultView[i]["Mobile"].ToString();
                f.FamilyStatus = dt.DefaultView[i]["FamilyStatus"].ToString();
                h.OldAddress = dt.DefaultView[i]["BasePlace"].ToString();
                h.Address = dt.DefaultView[i]["CurrentPlace"].ToString();
                f.Notes = dt.DefaultView[i]["Notes"].ToString();
                f.Evaluation = dt.DefaultView[i]["Evaluation"].ToString();
                f.ApplyDate = BaseDataBase.DateNow;

                List<FamilyPerson> fps = new List<FamilyPerson>();

                int b1 = dt.DefaultView[i]["Baby1"] is DBNull ? 0 : (int)dt.DefaultView[i]["Baby1"];
                int b2 = dt.DefaultView[i]["Baby2"] is DBNull ? 0 : (int)dt.DefaultView[i]["Baby2"];
                int b3 = dt.DefaultView[i]["Baby3"] is DBNull ? 0 : (int)dt.DefaultView[i]["Baby3"];
                int m1 = dt.DefaultView[i]["Male1"] is DBNull ? 0 : (int)dt.DefaultView[i]["Male1"];
                int m2 = dt.DefaultView[i]["Male2"] is DBNull ? 0 : (int)dt.DefaultView[i]["Male2"] - 1;
                int f1 = dt.DefaultView[i]["Female1"] is DBNull ? 0 : (int)dt.DefaultView[i]["Female1"];
                int f2 = dt.DefaultView[i]["Female2"] is DBNull ? 0 : (int)dt.DefaultView[i]["Female2"];
                int f3 = dt.DefaultView[i]["Female3"] is DBNull ? 0 : (int)dt.DefaultView[i]["Female3"];
                if (f3 > 0)
                {
                    f3--;
                    f.FamilyMother.DOB = BaseDataBase.DateNow.AddYears(-50);
                    f.FamilyFather.DOB = BaseDataBase.DateNow.AddYears(-55);
                }
                else
                {
                    f2--;
                    f.FamilyMother.DOB = BaseDataBase.DateNow.AddYears(-30);
                    f.FamilyFather.DOB = BaseDataBase.DateNow.AddYears(-40);
                }


                int index = 0;
                while (b1 > 0)
                {
                    b1--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddMonths(-2);
                    if (m1 > 0) { m1--; fp.Gender = "ذكر"; }
                    else { f1--; fp.Gender = "أنثى"; }

                    fps.Add(fp);
                }
                while (b2 > 0)
                {
                    b2--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddMonths(-10);
                    if (m1 > 0) { m1--; fp.Gender = "ذكر"; }
                    else { f1--; fp.Gender = "أنثى"; }

                    fps.Add(fp);
                }
                while (b3 > 0)
                {
                    b3--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddMonths(-18);
                    if (m1 > 0) { m1--; fp.Gender = "ذكر"; }
                    else { f1--; fp.Gender = "أنثى"; }

                    fps.Add(fp);
                }
                while (m1 > 0)
                {
                    m1--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddYears(-8);
                    fp.Gender = "ذكر";

                    fps.Add(fp);
                }
                while (m2 > 0)
                {
                    m2--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddYears(-15);
                    fp.Gender = "ذكر";

                    fps.Add(fp);
                }
                while (f1 > 0)
                {
                    f1--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddYears(-8);
                    fp.Gender = "أنثى";

                    fps.Add(fp);
                }
                while (f2 > 0)
                {
                    f2--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddYears(-15);
                    fp.Gender = "أنثى";

                    fps.Add(fp);
                }
                while (f3 > 0)
                {
                    f3--;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FirstName = "الابن " + ++index;
                    fp.DOB = BaseDataBase.DateNow.AddYears(-50);
                    fp.Gender = "أنثى";

                    fps.Add(fp);
                }


                f.FamilyID = BaseDataBase._StoredProcedureReturnable("sp_Add2FamilyHilal"
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
                        , new SqlParameter("@FirstNameF", f.FamilyFather.FirstName)
                        , new SqlParameter("@DOBF", f.FamilyFather.DOB)
                        , new SqlParameter("@PIDF", f.FamilyFather.PID)
                        , new SqlParameter("@FirstNameM", f.FamilyMother.FirstName)
                        , new SqlParameter("@DOBM", f.FamilyMother.DOB)
                        , new SqlParameter("@PIDM", f.FamilyMother.PID)
                        , new SqlParameter("@OldAddress", h.OldAddress)
                        , new SqlParameter("@Address", h.Address));

                int ChildID = int.Parse(BaseDataBase._Scalar("select IsNull(Max(FamilyPersonID)+1,1) from FamilyPerson"));
                for (int j = 0; j < fps.Count; j++)
                {
                    string s = string.Format("insert into FamilyPerson(FamilyPersonID,FamilyID,FirstName,Gender,DOB) values ({0},{1},'{2}','{3}',",
                        ChildID + j, f.FamilyID, fps[j].FirstName, fps[j].Gender);
                    if (fps[j].DOB.HasValue) s += "'" + fps[j].DOB.Value.ToString("MM/dd/yyyy") + "')";
                    else s += "null)";
                    BaseDataBase._NonQuery(s);
                }

                bw.ReportProgress(i);
            }
        }
        private void bw2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            btnSlideLeft.IsEnabled = false;
            if (!e.Cancelled)
            {
                MyMessageBox.Show("تمت الاضافة بنجاح");
            }
        }
        private void bw2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage * 100 / dt.Rows.Count;
            this.txtProgress.Text = e.ProgressPercentage + " من " + dt.Rows.Count;
        }
    }
}
