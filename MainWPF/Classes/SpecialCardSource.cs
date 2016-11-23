using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class SpecialCardSource : ModelViewContext
    {
        private int? id;
        public int? Id
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private int? specialcardid;
        public int? SpecialCardID
        {
            get
            { return specialcardid; }
            set
            {
                specialcardid = value;
                NotifyPropertyChanged("SpecialCardID");
            }
        }

        private int? centerid;
        public int? CenterID
        {
            get
            { return centerid; }
            set
            {
                centerid = value;
                NotifyPropertyChanged("CenterID");
            }
        }

        private int? beneficiarytype;
        public int? BeneficiaryType
        {
            get
            { return beneficiarytype; }
            set
            {
                beneficiarytype = value;
                NotifyPropertyChanged("BeneficiaryType");
            }
        }

        private int? beneficiaryid;
        public int? BeneficiaryID
        {
            get
            { return beneficiaryid; }
            set
            {
                beneficiaryid = value;
                NotifyPropertyChanged("BeneficiaryID");
            }
        }

        private DateTime? startdate;
        public DateTime? StartDate
        {
            get
            { return startdate; }
            set
            {
                startdate = value;
                NotifyPropertyChanged("StartDate");
            }
        }

        private DateTime? enddate;
        public DateTime? EndDate
        {
            get
            { return enddate; }
            set
            {
                enddate = value;
                NotifyPropertyChanged("EndDate");
            }
        }

        private int? lastuserid;
        public int? LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private string code;
        public string Code
        {
            get
            { return code; }
            set
            { code = value; }
        }

        public static Dictionary<int, string> Types
        {
            get
            {
                return new Dictionary<int, string>() {
                    { 1, "العائلة" },
                    { 2, "الاباء" },
                    { 3, "افراد الاسرة" } };
            }
        }


        public static bool InsertData(SpecialCardSource x)
        {
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_SpecialCardSource"
            , new SqlParameter("@Id", System.Data.SqlDbType.Int)
            , new SqlParameter("@SpecialCardID", x.SpecialCardID)
            , new SqlParameter("@CenterID", x.CenterID)
            , new SqlParameter("@BeneficiaryType", x.BeneficiaryType)
            , new SqlParameter("@BeneficiaryID", x.BeneficiaryID)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@Code", x.Code)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            return x.Id.HasValue;
        }
        public static bool UpdateData(SpecialCardSource x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_SpecialCardSource"
            , new SqlParameter("@Id", x.Id)
            , new SqlParameter("@SpecialCardID", x.SpecialCardID)
            , new SqlParameter("@CenterID", x.CenterID)
            , new SqlParameter("@BeneficiaryType", x.BeneficiaryType)
            , new SqlParameter("@BeneficiaryID", x.BeneficiaryID)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@Code", x.Code)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(SpecialCardSource x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_SpecialCardSource"
            , new SqlParameter("@Id", x.Id));
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(Code))
            {
                isValid = false;
                this.SetError("Code", "يجب ادخال رمز البطاقة");
            }
            else
            {
                string s = BaseDataBase._Scalar_StoredProcedure("sp_IsSpecialCardCodeExist", new SqlParameter("@ID", Id), new SqlParameter("@Code", Code));
                if (!string.IsNullOrEmpty(s))
                { isValid = false; this.SetError("Code", "الرمز موجود قي قاعدة البيانات"); }
            }
            if (!SpecialCardID.HasValue)
            {
                isValid = false;
                this.SetError("SpecialCardID", "يجب اختيار البطاقة الخاصة");
            }
            if (!CenterID.HasValue)
            {
                isValid = false;
                this.SetError("CenterID", "يجب اختيار المركز");
            }
            if (!BeneficiaryType.HasValue)
            {
                isValid = false;
                this.SetError("BeneficiaryType", "يجب اختيار نوع المستلم");
            }
            if (!BeneficiaryID.HasValue)
            {
                isValid = false;
                this.SetError("BeneficiaryID", "يجب اختيار اسم المستلم");
            }
            if (!StartDate.HasValue)
            {
                isValid = false;
                this.SetError("StartDate", "يجب ادخال تاريخ بداية البطاقة");
            }
            if (!EndDate.HasValue)
            {
                isValid = false;
                this.SetError("EndDate", "يجب ادخال تاريخ نهاية البطاقة");
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

        public static SpecialCardSource GetSpecialCardSourceByID(int id)
        {
            SpecialCardSource x = new SpecialCardSource();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_SpecialCardSource", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    if (!(rd["SpecialCardID"] is DBNull))
                        x.SpecialCardID = int.Parse(rd["SpecialCardID"].ToString());
                    if (!(rd["CenterID"] is DBNull))
                        x.CenterID = int.Parse(rd["CenterID"].ToString());
                    if (!(rd["BeneficiaryType"] is DBNull))
                        x.BeneficiaryType = int.Parse(rd["BeneficiaryType"].ToString());
                    if (!(rd["BeneficiaryID"] is DBNull))
                        x.BeneficiaryID = int.Parse(rd["BeneficiaryID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    x.Code = rd["Code"].ToString();
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
        public static List<SpecialCardSource> GetAllSpecialCardSource()
        {
            List<SpecialCardSource> xx = new List<SpecialCardSource>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_SpecialCardSource", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    SpecialCardSource x = new SpecialCardSource();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    if (!(rd["SpecialCardID"] is DBNull))
                        x.SpecialCardID = int.Parse(rd["SpecialCardID"].ToString());
                    if (!(rd["CenterID"] is DBNull))
                        x.CenterID = int.Parse(rd["CenterID"].ToString());
                    if (!(rd["BeneficiaryType"] is DBNull))
                        x.BeneficiaryType = int.Parse(rd["BeneficiaryType"].ToString());
                    if (!(rd["BeneficiaryID"] is DBNull))
                        x.BeneficiaryID = int.Parse(rd["BeneficiaryID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    x.Code = rd["Code"].ToString();
                    xx.Add(x);
                }
                rd.Close();
            }
            catch
            {
                xx = null;
            }
            finally
            {
                con.Close();
            }
            return xx;
        }

        public static List<SpecialCardSource> GetAllSpecialCardSourcByFamilyID(int FamilyID)
        {
            List<SpecialCardSource> xx = new List<SpecialCardSource>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_FamilyID_SpecialCardSource", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    SpecialCardSource x = new SpecialCardSource();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    if (!(rd["SpecialCardID"] is DBNull))
                        x.SpecialCardID = int.Parse(rd["SpecialCardID"].ToString());
                    if (!(rd["CenterID"] is DBNull))
                        x.CenterID = int.Parse(rd["CenterID"].ToString());
                    if (!(rd["BeneficiaryType"] is DBNull))
                        x.BeneficiaryType = int.Parse(rd["BeneficiaryType"].ToString());
                    if (!(rd["BeneficiaryID"] is DBNull))
                        x.BeneficiaryID = int.Parse(rd["BeneficiaryID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    x.Code = rd["Code"].ToString();
                    xx.Add(x);
                }
                rd.Close();
            }
            catch
            {
                xx = new List<MainWPF.SpecialCardSource>();
            }
            finally
            {
                con.Close();
            }
            return xx;
        }
    }
}