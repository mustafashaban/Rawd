using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class FamilyPerson : Person
    {
        public FamilyPerson()
        {
            gender = "ذكر";
            RelationShip = "ابن";
        }

        private int? familypersonid;
        public int? FamilyPersonID
        {
            get
            { return familypersonid; }
            set
            { familypersonid = value; }
        }

        private string gender;
        public new string Gender
        {
            get { return gender; }
            set
            {
                if (value != gender)
                {
                    gender = value;
                    ClearError("Gender");
                    if (string.IsNullOrEmpty(gender))
                    {
                        SetError("Gender", "يجب اختيار الجنس");
                    }
                    else if (gender == "أنثى")
                    {
                        switch (relationship)
                        {
                            case "ابن":
                                RelationShip = "ابنة";
                                break;
                            case "أخ":
                                RelationShip = "أخت";
                                break;
                            case "جد":
                                RelationShip = "جدة";
                                break;
                            case "عم":
                                RelationShip = "عمة";
                                break;
                            case "خال":
                                RelationShip = "خالة";
                                break;
                            case "حفيد":
                                RelationShip = "حفيدة";
                                break;
                            case "زوج":
                                RelationShip = "زوجة";
                                break;
                            case "والد الزوج":
                                RelationShip = "والدة الزوج";
                                break;
                            case "والد الزوجة":
                                RelationShip = "والدة الزوجة";
                                break;
                            default:
                                RelationShip = "غير ذلك";
                                break;
                        }
                    }
                    else
                    {
                        switch (relationship)
                        {
                            case "ابنة":
                                RelationShip = "ابن";
                                break;
                            case "أخت":
                                RelationShip = "أخ";
                                break;
                            case "جدة":
                                RelationShip = "جد";
                                break;
                            case "عمة":
                                RelationShip = "عم";
                                break;
                            case "خالة":
                                RelationShip = "خال";
                                break;
                            case "حفيدة":
                                RelationShip = "حفيد";
                                break;
                            case "زوجة":
                                RelationShip = "زوج";
                                break;
                            case "والدة الزوج":
                                RelationShip = "والد الزوج";
                                break;
                            case "والدة الزوجة":
                                RelationShip = "والد الزوجة";
                                break;
                            default:
                                RelationShip = "غير ذلك";
                                break;
                        }
                    }
                    NotifyPropertyChanged("Gender");
                    NotifyPropertyChanged("RelationShip");
                }
            }
        }

        private string relationship;
        public string RelationShip
        {
            get
            { return relationship; }
            set
            {
                if (relationship != value)
                {
                    relationship = value;
                    ClearError("RelationShip");
                    if (string.IsNullOrEmpty(value))
                    {
                        SetError("RelationShip", "يجب اختيار صفة الشخص");
                    }
                    NotifyPropertyChanged("RelationShip");
                }
            }
        }

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }

        private string job;
        public string Job
        {
            get
            { return job; }
            set
            { job = value; }
        }



        private bool? isStudyCutOff;
        public bool? IsStudyCutOff
        {
            get
            { return isStudyCutOff; }
            set
            { isStudyCutOff = value; }
        }



        private string salary;
        public string Salary
        {
            get
            { return salary; }
            set
            { salary = value; }
        }



        private string salarycurrency;
        public string SalaryCurrency
        {
            get
            { return salarycurrency; }
            set
            { salarycurrency = value; }
        }

        private bool? isimpeded;
        public bool? IsImpeded
        {
            get
            { return isimpeded; }
            set
            {
                isimpeded = value;
                NotifyPropertyChanged("IsImpeded");
            }
        }

        private string impededtype;
        public string impededType
        {
            get
            { return impededtype; }
            set
            { impededtype = value; }
        }

        private string impededkind;
        public string ImpededKind
        {
            get
            { return impededkind; }
            set
            { impededkind = value; }
        }

        private DateTime? impededdate;
        public DateTime? impededDate
        {
            get
            { return impededdate; }
            set
            { impededdate = value; }
        }



        private bool? isWantToCompleteStudy;
        public bool? IsWantToCompleteStudy
        {
            get
            { return isWantToCompleteStudy; }
            set
            { isWantToCompleteStudy = value; }
        }



        private string studyCuttOffReason;
        public string StudyCuttOffReason
        {
            get
            { return studyCuttOffReason; }
            set
            { studyCuttOffReason = value; }
        }



        private DateTime? lostdate;
        public DateTime? LostDate
        {
            get
            { return lostdate; }
            set
            { lostdate = value; }
        }



        private string backdetailes;
        public string BackDetailes
        {
            get
            { return backdetailes; }
            set
            { backdetailes = value; }
        }



        private DateTime? backdate;
        public DateTime? BackDate
        {
            get
            { return backdate; }
            set
            { backdate = value; }
        }

        private string deathreportid;
        public string DeathReportID
        {
            get
            { return deathreportid; }
            set
            { deathreportid = value; }
        }



        private DateTime? deathreportdate;
        public DateTime? DeathReportDate
        {
            get
            { return deathreportdate; }
            set
            { deathreportdate = value; }
        }



        private string bondplace;
        public string BondPlace
        {
            get
            { return bondplace; }
            set
            { bondplace = value; }
        }



        private string bondnumber;
        public string BondNumber
        {
            get
            { return bondnumber; }
            set
            { bondnumber = value; }
        }

        private DateTime? identitygivindate;
        public DateTime? IdentityGivinDate
        {
            get
            { return identitygivindate; }
            set
            { identitygivindate = value; }
        }

        private int? tall;
        public int? Tall
        {
            get
            { return tall; }
            set
            { tall = value; }
        }

        private int? wieght;
        public int? Wieght
        {
            get
            { return wieght; }
            set
            { wieght = value; }
        }

        private string psychicalsituation;
        public string PsychicalSituation
        {
            get
            { return psychicalsituation; }
            set
            { psychicalsituation = value; }
        }

        private string ethicssituation;
        public string EthicsSituation
        {
            get
            { return ethicssituation; }
            set
            { ethicssituation = value; }
        }

        private string homeplace;
        public string HomePlace
        {
            get
            { return homeplace; }
            set
            { homeplace = value; }
        }

        private string homebaseplace;
        public string HomeBasePlace
        {
            get
            { return homebaseplace; }
            set
            { homebaseplace = value; }
        }

        private string childrencount;
        public string ChildrenCount
        {
            get
            { return childrencount; }
            set
            { childrencount = value; }
        }

        private string property;
        public string Property
        {
            get
            { return property; }
            set
            { property = value; }
        }

        private string deathreportimage;
        public string DeathReportImage
        {
            get
            { return deathreportimage; }
            set
            {
                if (value != deathreportimage)
                {
                    deathreportimage = value;
                    this.NotifyPropertyChanged("DeathReportImage");
                }
            }
        }

        private string identityimage;
        public string IdentityImage
        {
            get
            { return identityimage; }
            set
            {
                if (value != identityimage)
                {
                    identityimage = value;
                    this.NotifyPropertyChanged("IdentityImage");
                }
            }
        }

        private string personalimage;
        public string PersonalImage
        {
            get
            { return personalimage; }
            set
            {
                if (value != personalimage)
                {
                    personalimage = value;
                    this.NotifyPropertyChanged("PersonalImage");
                }
            }
        }

        private string studystatus;
        public string StudyStatus
        {
            get
            { return studystatus; }
            set
            { studystatus = value; }
        }

        private string healthstatus;
        public string HealthStatus
        {
            get
            { return healthstatus; }
            set
            { healthstatus = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private bool isCancel;
        public bool IsCancel
        {
            get
            { return isCancel; }
            set
            {
                isCancel = value;
                NotifyPropertyChanged("IsCancel");
            }
        }

        private string cancelReason;
        public string CancelReason
        {
            get
            { return cancelReason; }
            set
            { cancelReason = value; }
        }

        public static FamilyPerson GetFamilyPersonByID(int id)
        {
            FamilyPerson x = new FamilyPerson();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyPersonByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyPersonID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["FamilyPersonID"] is DBNull))
                        x.FamilyPersonID = System.Int32.Parse(rd["FamilyPersonID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Job = rd["Job"].ToString();
                    if (!(rd["IsStudyCutOff"] is DBNull))
                        x.IsStudyCutOff = System.Boolean.Parse(rd["IsStudyCutOff"].ToString());
                    x.Salary = rd["Salary"].ToString();
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    if (!(rd["IsImpeded"] is DBNull))
                        x.IsImpeded = System.Boolean.Parse(rd["IsImpeded"].ToString());
                    x.impededType = rd["impededType"].ToString();
                    x.ImpededKind = rd["ImpededKind"].ToString();
                    if (!(rd["impededDate"] is DBNull))
                        x.impededDate = System.DateTime.Parse(rd["impededDate"].ToString());
                    if (!(rd["IsWantToCompleteStudy"] is DBNull))
                        x.IsWantToCompleteStudy = System.Boolean.Parse(rd["IsWantToCompleteStudy"].ToString());
                    x.StudyCuttOffReason = rd["StudyCuttOffReason"].ToString();
                    if (!(rd["LostDate"] is DBNull))
                        x.LostDate = System.DateTime.Parse(rd["LostDate"].ToString());
                    x.BackDetailes = rd["BackDetailes"].ToString();
                    if (!(rd["BackDate"] is DBNull))
                        x.BackDate = System.DateTime.Parse(rd["BackDate"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.PID = rd["PID"].ToString();
                    if (!(rd["DeathDate"] is DBNull))
                        x.DeathDate = System.DateTime.Parse(rd["DeathDate"].ToString());
                    x.DeathReason = rd["DeathReason"].ToString();
                    x.DeathReportID = rd["DeathReportID"].ToString();
                    if (!(rd["DeathReportDate"] is DBNull))
                        x.DeathReportDate = System.DateTime.Parse(rd["DeathReportDate"].ToString());
                    x.BondPlace = rd["BondPlace"].ToString();
                    x.BondNumber = rd["BondNumber"].ToString();
                    x.IdentityID = rd["IdentityID"].ToString();
                    if (!(rd["IdentityGivinDate"] is DBNull))
                        x.IdentityGivinDate = System.DateTime.Parse(rd["IdentityGivinDate"].ToString());
                    if (!(rd["Tall"] is DBNull))
                        x.Tall = System.Int32.Parse(rd["Tall"].ToString());
                    if (!(rd["Wieght"] is DBNull))
                        x.Wieght = System.Int32.Parse(rd["Wieght"].ToString());
                    x.PsychicalSituation = rd["PsychicalSituation"].ToString();
                    x.EthicsSituation = rd["EthicsSituation"].ToString();
                    x.HomePlace = rd["HomePlace"].ToString();
                    x.HomeBasePlace = rd["HomeBasePlace"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.ChildrenCount = rd["ChildrenCount"].ToString();
                    x.Property = rd["Property"].ToString();
                    x.PersonalImage = rd["PersonalImage"].ToString();
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.DeathReportImage = rd["DeathReportImage"].ToString();
                    x.RelationShip = rd["RelationShip"].ToString();
                    x.StudyStatus = rd["StudyStatus"].ToString();
                    x.HealthStatus = rd["HealthStatus"].ToString();
                    if (!(rd["IsCancel"] is DBNull))
                        x.IsCancel = System.Boolean.Parse(rd["IsCancel"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();
                    x.Notes = rd["Notes"].ToString();
                }
                rd.Close();
            }
            catch
            {
                x = null;
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<FamilyPerson> GetFamilyPersonByFamilyID(int FamilyID)
        {
            List<FamilyPerson> fps = new List<FamilyPerson>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyPersonAllByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    FamilyPerson x = new FamilyPerson();

                    if (!(rd["FamilyPersonID"] is DBNull))
                        x.FamilyPersonID = System.Int32.Parse(rd["FamilyPersonID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Job = rd["Job"].ToString();
                    if (!(rd["IsStudyCutOff"] is DBNull))
                        x.IsStudyCutOff = System.Boolean.Parse(rd["IsStudyCutOff"].ToString());
                    x.Salary = rd["Salary"].ToString();
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    if (!(rd["IsImpeded"] is DBNull))
                        x.IsImpeded = System.Boolean.Parse(rd["IsImpeded"].ToString());
                    x.impededType = rd["impededType"].ToString();
                    x.ImpededKind = rd["ImpededKind"].ToString();
                    if (!(rd["impededDate"] is DBNull))
                        x.impededDate = System.DateTime.Parse(rd["impededDate"].ToString());
                    if (!(rd["IsWantToCompleteStudy"] is DBNull))
                        x.IsWantToCompleteStudy = System.Boolean.Parse(rd["IsWantToCompleteStudy"].ToString());
                    x.StudyCuttOffReason = rd["StudyCuttOffReason"].ToString();
                    if (!(rd["LostDate"] is DBNull))
                        x.LostDate = System.DateTime.Parse(rd["LostDate"].ToString());
                    x.BackDetailes = rd["BackDetailes"].ToString();
                    if (!(rd["BackDate"] is DBNull))
                        x.BackDate = System.DateTime.Parse(rd["BackDate"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.PID = rd["PID"].ToString();
                    if (!(rd["DeathDate"] is DBNull))
                        x.DeathDate = System.DateTime.Parse(rd["DeathDate"].ToString());
                    x.DeathReason = rd["DeathReason"].ToString();
                    x.DeathReportID = rd["DeathReportID"].ToString();
                    if (!(rd["DeathReportDate"] is DBNull))
                        x.DeathReportDate = System.DateTime.Parse(rd["DeathReportDate"].ToString());
                    x.BondPlace = rd["BondPlace"].ToString();
                    x.BondNumber = rd["BondNumber"].ToString();
                    x.IdentityID = rd["IdentityID"].ToString();
                    if (!(rd["IdentityGivinDate"] is DBNull))
                        x.IdentityGivinDate = System.DateTime.Parse(rd["IdentityGivinDate"].ToString());
                    if (!(rd["Tall"] is DBNull))
                        x.Tall = System.Int32.Parse(rd["Tall"].ToString());
                    if (!(rd["Wieght"] is DBNull))
                        x.Wieght = System.Int32.Parse(rd["Wieght"].ToString());
                    x.PsychicalSituation = rd["PsychicalSituation"].ToString();
                    x.EthicsSituation = rd["EthicsSituation"].ToString();
                    x.HomePlace = rd["HomePlace"].ToString();
                    x.HomeBasePlace = rd["HomeBasePlace"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.ChildrenCount = rd["ChildrenCount"].ToString();
                    x.Property = rd["Property"].ToString();
                    x.PersonalImage = rd["PersonalImage"].ToString();
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.DeathReportImage = rd["DeathReportImage"].ToString();
                    x.RelationShip = rd["RelationShip"].ToString();
                    x.StudyStatus = rd["StudyStatus"].ToString();
                    x.HealthStatus = rd["HealthStatus"].ToString();
                    if (!(rd["IsCancel"] is DBNull))
                        x.IsCancel = System.Boolean.Parse(rd["IsCancel"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();
                    x.Notes = rd["Notes"].ToString();

                    fps.Add(x);
                }
                rd.Close();
            }
            catch
            {
                fps = new List<FamilyPerson>();
            }
            finally
            {
                con.Close();
            }
            return fps;
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(FirstName))
            {
                isValid = false;
                this.SetError("FirstName", "يجب إدخال الاسم");
            }
            if (string.IsNullOrEmpty(RelationShip))
            {
                isValid = false;
                this.SetError("RelationShip", "يجب اختيار صفة الشخص");
            }
            if (string.IsNullOrEmpty(this.Gender))
            {
                isValid = false;
                this.SetError("Gender", "يجب اختيار الجنس");
            }
            if (!DOB.HasValue)
            {
                isValid = false;
                this.SetError("DOB", "يجب إدخال تاريخ الولادة");
            }
            /*
            اضافة كود بتاريخ 16-5-2016 بفؤرض تقييد رقم الهاتف
            */
            /* if (Mobile.Length!=10)
             {
                 isValid = false;
               //  this.SetError("Mobile", "يجب التحقق من رقم الموبايل");
             }
             if (Phone.Length != 7)
             {
                 isValid = false;
                 //this.SetError("Phone", "يجب التحقق من رقم الهاتف");
             }*/
            /////////
            if (DOB.HasValue)
            {
                var diff = BaseDataBase.DateNow - DOB.Value;
                if (diff.Days < 0 || diff.Days / 30 / 12 > 120)
                {
                    isValid = false;
                    this.SetError("DOB", "التاريخ غير صالح يجب ادخال عمر صحيح");
                }
            }
            if (!string.IsNullOrEmpty(PID))
            {
                if (!BaseDataBase.IsStringNumber(PID))
                {
                    isValid = false;
                    this.SetError("PID", "الرقم الوطني يجب أن يحوي أرقاماً فقط");
                }
                else
                {
                    string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", null), new SqlParameter("@PID", PID));
                    if (!string.IsNullOrEmpty(ExistingFamilyCode))
                    {
                        isValid = false;
                        this.SetError("PID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مثبتة " + ExistingFamilyCode);
                    }
                    else
                    {
                        ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetTempFamilyCodeByParentPID", new SqlParameter("@TempFamilyID", null), new SqlParameter("@PID", PID));
                        if (!string.IsNullOrEmpty(ExistingFamilyCode))
                        {
                            isValid = false;
                            this.SetError("PID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مؤقتة " + ExistingFamilyCode);
                        }
                        else
                        {
                            ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyPersonPID", new SqlParameter("@FamilyPersonID", FamilyPersonID), new SqlParameter("@PID", PID));
                            if (!string.IsNullOrEmpty(ExistingFamilyCode))
                            {
                                isValid = false;
                                this.SetError("PID", "الرقم الوطني موجود في قاعدة البيانات ضمن افراد عائلة رقم " + ExistingFamilyCode);
                            }
                        }
                    }
                }
            }
            if (!isValid)
            {
                string s = "";
                foreach (var item in errors)
                {
                    s += item.Value + "\n";
                }
                MyMessageBox.Show(s);
            }
            return isValid;
        }
    }
}
