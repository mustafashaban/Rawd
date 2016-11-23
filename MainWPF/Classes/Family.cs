using MainWPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MainWPF
{
    public class Family : ModelViewContext
    {
        public Family()
        {
            FamilyMother = new Mother();
            FamilyFather = new Father();

            FamilyStatus = "نازح";
        }

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            {
                familyid = value;
            }
        }

        private int? sectorID;
        public int? SectorID
        {
            get
            { return sectorID; }
            set
            {
                sectorID = value;
                this.ClearError("SectorID");
                this.NotifyPropertyChanged("SectorID");
            }
        }

        private string familycode;
        public string FamilyCode
        {
            get
            { return familycode; }
            set
            {
                this.ClearError("FamilyCode");
                familycode = value;
                if (string.IsNullOrEmpty(value))
                    this.SetError("FamilyCode", "يجب إدخال رمز للعائلة");
                else
                {
                    if (BaseDataBase._StoredProcedureReturnableBool("sp_IsFamilyCodeExists", new SqlParameter("@R", System.Data.SqlDbType.Bit), new SqlParameter("@FamilyID", FamilyID), new SqlParameter("@FamilyCode", FamilyCode)))
                    {
                        this.SetError("FamilyCode", "رمز العائلة موجود في قاعدة البيانات");
                    }
                }
                this.NotifyPropertyChanged("FamilyCode");
            }
        }

        private string familyname;
        public string FamilyName
        {
            get
            { return familyname; }
            set
            {
                if (value != familyname)
                {
                    familyname = value;
                    this.ClearError("FamilyName");
                    if (string.IsNullOrEmpty(FamilyName))
                    {
                        this.SetError("FamilyName", "يجب إدخال اسم العائلة");
                    }
                    this.NotifyPropertyChanged("FamilyName");
                }
            }
        }

        private string familytype;
        public string FamilyType
        {
            get
            { return familytype; }
            set
            {
                if (value != familytype)
                {
                    familytype = value;
                    this.ClearError("FamilyType");
                    if (string.IsNullOrEmpty(FamilyType))
                    {
                        this.SetError("FamilyType", "يجب إدخال نوع العائلة");
                    }
                    this.NotifyPropertyChanged("FamilyType");
                }
            }
        }

        private string familyidentityid;
        public string FamilyIdentityID
        {
            get
            { return familyidentityid; }
            set
            {
                if (value != familyidentityid)
                {
                    familyidentityid = value.Trim();
                    this.ClearError("FamilyIdentityID");
                    if (string.IsNullOrEmpty(FamilyIdentityID))
                    {
                        this.SetError("FamilyIdentityID", "يجب إدخال رقم دفتر العائلة");
                    }
                    else if (FamilyIdentityID != "لايوجد دفتر عائلة")
                    {
                        bool isValid = true;
                        var num = FamilyIdentityID.Split(' ');
                        if (!BaseDataBase.IsStringNumber(num[0]))
                        {
                            isValid = false;
                            this.SetError("FamilyIdentityID", "رقم دفتر العائلة يجب أن يحوي أرقاما");
                        }
                        if (num.Length > 3)
                        {
                            isValid = false;
                            this.SetError("FamilyIdentityID", "رقم دفتر العائلة يحوي أكثر من فراغين");
                        }
                        else if (num.Length > 1)
                        {
                            if (num[1].Length != 1)
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يكون خانة واحدة");
                            }
                            else if (!char.IsLetter(char.Parse(num[1])))
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يحوي حرف واحد فقط (أ، ب، ت، ث، ج)");
                            }
                            if (num.Length == 3 && !BaseDataBase.IsStringNumber(num[2]))
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "الرقم الأسري يجب أن يحوي أرقاما");
                            }
                        }
                        if (isValid)
                        {
                            string ExistedFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyIDentityID", new SqlParameter("@FamilyID", FamilyID), new SqlParameter("@FamilyIdentityID", FamilyIdentityID));
                            if (!string.IsNullOrEmpty(ExistedFamilyCode))
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "رقم دفتر العائلة موجود في قاعدة البيانات برقم " + ExistedFamilyCode);
                            }
                        }
                    }
                    this.NotifyPropertyChanged("FamilyIdentityID");
                }
            }
        }
        private DateTime? applyDate;
        public DateTime? ApplyDate
        {
            get
            {
                return applyDate;
            }
            set
            {
                if (applyDate != value)
                {
                    applyDate = value;
                    this.ClearError("ApplyDate");
                    if (ApplyDate == null)
                    {
                        this.SetError("ApplyDate", "يجب إدخال تاريخ التسجيل");
                    }
                    this.NotifyPropertyChanged("ApplyDate");
                }
            }
        }

        private string familyreportimage;
        public string FamilyReportImage
        {
            get
            { return familyreportimage; }
            set
            {
                if (value != familyreportimage)
                {
                    familyreportimage = value;
                    this.NotifyPropertyChanged("FamilyReportImage");
                }
            }
        }

        private double? familysalary;
        public double? FamilySalary
        {
            get
            { return familysalary; }
            set
            { familysalary = value; }
        }

        private string salarysupport;
        public string SalarySupport
        {
            get
            { return salarysupport; }
            set
            { salarysupport = value; }
        }

        private string salarycurrency;
        public string SalaryCurrency
        {
            get
            { return salarycurrency; }
            set
            { salarycurrency = value; }
        }

        private string definedpersonname;
        public string DefinedPersonName
        {
            get
            { return definedpersonname; }
            set
            {
                definedpersonname = value;
            }
        }

        private string definedpersonphone;
        public string DefinedPersonPhone
        {
            get
            { return definedpersonphone; }
            set
            { definedpersonphone = value; }
        }

        private string familyPersonCount;
        public string FamilyPersonCount
        {
            get
            { return familyPersonCount; }
            set
            {
                familyPersonCount = value;
                this.NotifyPropertyChanged("FamilyPersonCount");
            }
        }

        private bool iscanceled;
        public bool IsCanceled
        {
            get
            { return iscanceled; }
            set
            { iscanceled = value; }
        }

        private bool isAcquittance;
        public bool IsAcquittance
        {
            get
            { return isAcquittance; }
            set
            { isAcquittance = value; }
        }

        private string familyStatus;
        public string FamilyStatus
        {
            get
            { return familyStatus; }
            set
            { familyStatus = value; }
        }

        private DateTime? canceldate;
        public DateTime? CancelDate
        {
            get
            { return canceldate; }
            set
            { canceldate = value; }
        }
        private DateTime? acquittanceDate;
        public DateTime? AcquittanceDate
        {
            get
            { return acquittanceDate; }
            set
            { acquittanceDate = value; }
        }

        private string cancelreason;
        public string CancelReason
        {
            get
            { return cancelreason; }
            set
            { cancelreason = value; }
        }

        private string evaluation;
        public string Evaluation
        {
            get
            { return evaluation; }
            set
            { evaluation = value; }
        }

        private bool isRecorded;
        public bool IsRecorded
        {
            get
            { return isRecorded; }
            set
            { isRecorded = value; }
        }

        private bool hasCard;
        public bool HasCard
        {
            get
            { return hasCard; }
            set
            { hasCard = value; }
        }


        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }


        private string createPerson;
        public string CreatePerson
        {
            get
            { return createPerson; }
            set
            { createPerson = value; }
        }
        private DateTime? createDate;
        public DateTime? CreateDate
        {
            get
            { return createDate; }
            set
            { createDate = value; }
        }

        private string modifiedPerson;
        public string ModifiedPerson
        {
            get
            { return modifiedPerson; }
            set
            { modifiedPerson = value; }
        }
        private DateTime? lastModifiedDate;
        public DateTime? LastModifiedDate
        {
            get
            { return lastModifiedDate; }
            set
            { lastModifiedDate = value; }
        }

        private Father familyFather;
        public Father FamilyFather
        {
            get { return familyFather; }
            set { familyFather = value; }
        }

        private Mother familyMother;
        public Mother FamilyMother
        {
            get { return familyMother; }
            set { familyMother = value; }
        }

        private House familyHouse;
        public House FamilyHouse
        {
            get { return familyHouse; }
            set
            {
                familyHouse = value;
                NotifyPropertyChanged("FamilyHouse");
            }
        }

        public static Family GetFamilyCancelDataByID(int? ID)
        {
            Family x = new Family();
            x.FamilyID = ID;
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyCancelDataByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", ID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["IsCanceled"] is DBNull))
                        x.IsCanceled = System.Boolean.Parse(rd["IsCanceled"].ToString());
                    if (!(rd["CancelDate"] is DBNull))
                        x.CancelDate = System.DateTime.Parse(rd["CancelDate"].ToString());
                    if (!(rd["IsAcquittance"] is DBNull))
                        x.IsAcquittance = System.Boolean.Parse(rd["IsAcquittance"].ToString());
                    if (!(rd["AcquittanceDate"] is DBNull))
                        x.AcquittanceDate = System.DateTime.Parse(rd["AcquittanceDate"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();
                }
                rd.Close();
            }
            catch
            {
                x = new Family();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static Family GetFamilyByID(int id)
        {
            Family x = new Family();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FamilyType = rd["FamilyType"].ToString();
                    x.FamilyCode = rd["FamilyCode"].ToString();
                    if (!(rd["SectorID"] is DBNull))
                        x.SectorID = System.Int32.Parse(rd["SectorID"].ToString());
                    x.FamilyName = rd["FamilyName"].ToString();
                    x.FamilyStatus = rd["FamilyStatus"].ToString();
                    x.FamilyIdentityID = rd["FamilyIdentityID"].ToString();
                    if (!(rd["ApplyDate"] is DBNull))
                        x.ApplyDate = System.DateTime.Parse(rd["ApplyDate"].ToString());
                    x.FamilyReportImage = rd["FamilyReportImage"].ToString();
                    if (!(rd["FamilySalary"] is DBNull))
                        x.FamilySalary = System.Single.Parse(rd["FamilySalary"].ToString());
                    x.SalarySupport = rd["SalarySupport"].ToString();
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.DefinedPersonName = rd["DefinedPersonName"].ToString();
                    x.DefinedPersonPhone = rd["DefinedPersonPhone"].ToString();
                    x.FamilyPersonCount = rd["FamilyPersonCount"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.CreatePerson = rd["CreatePerson"].ToString();
                    x.ModifiedPerson = rd["LastModifiedPerson"].ToString();
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = System.DateTime.Parse(rd["CreateDate"].ToString());
                    if (!(rd["LastModifiedDate"] is DBNull))
                        x.LastModifiedDate = System.DateTime.Parse(rd["LastModifiedDate"].ToString());
                    if (!(rd["IsRecorded"] is DBNull))
                        x.IsRecorded = System.Boolean.Parse(rd["IsRecorded"].ToString());
                    if (!(rd["HasCard"] is DBNull))
                        x.HasCard = System.Boolean.Parse(rd["HasCard"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    x.FamilyFather = Father.GetFatherByFamilyID(id);
                    x.FamilyMother = Mother.GetMotherByFamilyID(id);
                }
                rd.Close();
            }
            catch
            {
                x = new Family();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<Family> GetFamilyAll()
        {
            List<Family> fs = new List<Family>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyAll", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Family x = new Family();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FamilyType = rd["FamilyType"].ToString();
                    x.FamilyCode = rd["FamilyCode"].ToString();
                    if (!(rd["SectorID"] is DBNull))
                        x.SectorID = System.Int32.Parse(rd["SectorID"].ToString());
                    x.FamilyName = rd["FamilyName"].ToString();
                    x.FamilyStatus = rd["FamilyStatus"].ToString();
                    x.FamilyIdentityID = rd["FamilyIdentityID"].ToString();
                    if (!(rd["ApplyDate"] is DBNull))
                        x.ApplyDate = System.DateTime.Parse(rd["ApplyDate"].ToString());
                    x.FamilyReportImage = rd["FamilyReportImage"].ToString();
                    if (!(rd["FamilySalary"] is DBNull))
                        x.FamilySalary = System.Single.Parse(rd["FamilySalary"].ToString());
                    x.SalarySupport = rd["SalarySupport"].ToString();
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.DefinedPersonName = rd["DefinedPersonName"].ToString();
                    x.DefinedPersonPhone = rd["DefinedPersonPhone"].ToString();
                    x.FamilyPersonCount = rd["FamilyPersonCount"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    if (!(rd["IsRecorded"] is DBNull))
                        x.IsRecorded = System.Boolean.Parse(rd["IsRecorded"].ToString());
                    if (!(rd["HasCard"] is DBNull))
                        x.HasCard = System.Boolean.Parse(rd["HasCard"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    x.FamilyFather = Father.GetFatherByFamilyID(x.FamilyID.Value);
                    x.FamilyMother = Mother.GetMotherByFamilyID(x.FamilyID.Value);

                    fs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                fs = new List<Family>();
            }
            finally
            {
                con.Close();
            }
            return fs;
        }
        public static List<Family> GetFamilyAllByType(string FamilyType)
        {
            List<Family> fs = new List<Family>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyAllByType", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyType", FamilyType);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Family x = new Family();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FamilyType = rd["FamilyType"].ToString();
                    x.FamilyCode = rd["FamilyCode"].ToString();
                    if (!(rd["SectorID"] is DBNull))
                        x.SectorID = System.Int32.Parse(rd["SectorID"].ToString());
                    x.FamilyName = rd["FamilyName"].ToString();
                    x.FamilyStatus = rd["FamilyStatus"].ToString();
                    x.FamilyIdentityID = rd["FamilyIdentityID"].ToString();
                    if (!(rd["ApplyDate"] is DBNull))
                        x.ApplyDate = System.DateTime.Parse(rd["ApplyDate"].ToString());
                    x.FamilyReportImage = rd["FamilyReportImage"].ToString();
                    if (!(rd["FamilySalary"] is DBNull))
                        x.FamilySalary = System.Single.Parse(rd["FamilySalary"].ToString());
                    x.SalarySupport = rd["SalarySupport"].ToString();
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.DefinedPersonName = rd["DefinedPersonName"].ToString();
                    x.DefinedPersonPhone = rd["DefinedPersonPhone"].ToString();
                    x.FamilyPersonCount = rd["FamilyPersonCount"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    if (!(rd["IsRecorded"] is DBNull))
                        x.IsRecorded = System.Boolean.Parse(rd["IsRecorded"].ToString());
                    if (!(rd["HasCard"] is DBNull))
                        x.HasCard = System.Boolean.Parse(rd["HasCard"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    fs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                fs = new List<Family>();
            }
            finally
            {
                con.Close();
            }
            return fs;
        }
        public void UpdateFamilyPersonCount()
        {
            FamilyPersonCount = BaseDataBase._StoredProcedureReturnable("UpdateFamilyPersonCount",
                new SqlParameter("@FamilyPersonCount", System.Data.SqlDbType.Int),
                new SqlParameter("@FamilyID", FamilyID)).ToString();
        }
        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(FamilyCode))
            {
                isValid = false;
                this.SetError("FamilyCode", "يجب إدخال رمز للعائلة");
            }
            else
            {
                if (BaseDataBase._StoredProcedureReturnableBool("sp_IsFamilyCodeExists", new SqlParameter("@R", System.Data.SqlDbType.Bit), new SqlParameter("@FamilyID", FamilyID), new SqlParameter("@FamilyCode", FamilyCode)))
                {
                    isValid = false;
                    this.SetError("FamilyCode", "رمز العائلة موجود في قاعدة البيانات");
                }
            }
            if (string.IsNullOrEmpty(FamilyIdentityID))
            {
                isValid = false;
                this.SetError("FamilyIdentityID", "يجب إدخال رقم دفتر العائلة");
            }
            else if (FamilyIdentityID != "لايوجد دفتر عائلة")
            {
                var num = FamilyIdentityID.Split(' ');
                if (!BaseDataBase.IsStringNumber(num[0]))
                {
                    isValid = false;
                    this.SetError("FamilyIdentityID", "رقم دفتر العائلة يجب أن يحوي أرقاما");
                }
                if (num.Length > 3)
                {
                    isValid = false;
                    this.SetError("FamilyIdentityID", "رقم دفتر العائلة يحوي أكثر من فراغين");
                }
                else if (num.Length > 1)
                {
                    if (num[1].Length != 1)
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يكون خانة واحدة");
                    }
                    else if (!char.IsLetter(char.Parse(num[1])))
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يحوي حرف واحد فقط (أ، ب، ت، ث، ج)");
                    }
                    if (num.Length == 3 && !BaseDataBase.IsStringNumber(num[2]))
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "الرقم الأسري يجب أن يحوي أرقاما");
                    }
                }
                if (isValid)
                {
                    string ExistedFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyIDentityID", new SqlParameter("@FamilyID", FamilyID), new SqlParameter("@FamilyIdentityID", FamilyIdentityID));
                    if (!string.IsNullOrEmpty(ExistedFamilyCode))
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "رقم دفتر العائلة موجود في قاعدة البيانات برقم " + ExistedFamilyCode);
                    }
                }
            }
            if (string.IsNullOrEmpty(FamilyName))
            {
                isValid = false;
                this.SetError("FamilyName", "يجب إدخال اسم العائلة");
            }
            if (ApplyDate == null)
            {
                isValid = false;
                this.SetError("ApplyDate", "يجب إدخال تاريخ التسجيل");
            }
            if (string.IsNullOrEmpty(FamilyType))
            {
                isValid = false;
                this.SetError("FamilyType", "يجب إدخال نوع العائلة");
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
