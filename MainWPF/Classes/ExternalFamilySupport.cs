using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class ExternalFamilySupport : ModelViewContext
    {


        private int? externalfamilysupportid;
        public int? ExternalFamilySupportID
        {
            get
            { return externalfamilysupportid; }
            set
            { externalfamilysupportid = value; }
        }



        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }



        private string supporttype;
        public string SupportType
        {
            get
            { return supporttype; }
            set
            {
                supporttype = value;
                this.ClearError("SupportType");
                this.NotifyPropertyChanged("SupportType");
            }
        }



        private string supportsourse;
        public string SupportSourse
        {
            get
            { return supportsourse; }
            set
            { supportsourse = value; }
        }



        private string _value;
        public string Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }



        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            {
                date = value;
                this.ClearError("Date");
                this.NotifyPropertyChanged("Date");
            }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertExternalFamilyData()
        {
            ExternalFamilySupportID = BaseDataBase._StoredProcedureReturnable("sp_Add2ExternalFamilySupport"
            , new SqlParameter("@ExternalFamilySupportID", System.Data.SqlDbType.Int)
            , new SqlParameter("@FamilyID", FamilyID)
            , new SqlParameter("@SupportType", SupportType)
            , new SqlParameter("@SupportSourse", SupportSourse)
            , new SqlParameter("@Value", Value)
            , new SqlParameter("@Date", Date)
            , new SqlParameter("@Notes", Notes));
            if (ExternalFamilySupportID == null) return false;
            return true;
        }
        public bool UpdateExternalFamilyFata()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateExternalFamilySupport"
            , new SqlParameter("@ExternalFamilySupportID", ExternalFamilySupportID)
            , new SqlParameter("@FamilyID", FamilyID)
            , new SqlParameter("@SupportType", SupportType)
            , new SqlParameter("@SupportSourse", SupportSourse)
            , new SqlParameter("@Value", Value)
            , new SqlParameter("@Date", Date)
            , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteExternalFamilyData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromExternalFamilySupport"
                , new SqlParameter("@ExternalFamilySupportID", ExternalFamilySupportID));
        }

        public static ExternalFamilySupport GetExternalFamilySupportByID(int id)
        {
            ExternalFamilySupport x = new ExternalFamilySupport();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetExternalFamilySupportByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ExternalFamilySupportID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ExternalFamilySupportID"] is DBNull))
                        x.ExternalFamilySupportID = System.Int32.Parse(rd["ExternalFamilySupportID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.SupportType = rd["SupportType"].ToString();
                    x.SupportSourse = rd["SupportSourse"].ToString();
                    x.Value = rd["Value"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
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
        public static List<ExternalFamilySupport> GetExternalFamilySupportAllByFamilyID(int? FamilyID)
        {
            List<ExternalFamilySupport> b = new List<ExternalFamilySupport>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetExternalFamilySupportAllByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetExternalFamilySupportByID(rd.GetInt32(0)));
                }
            }
            catch { return new List<ExternalFamilySupport>(); }
            finally
            {
                con.Close();
            }
            return b;
        }


        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(SupportType))
            {
                isValid = false;
                this.SetError("Evaluation", "يجب إدخال نوع الاستفادة");
            }
            if (Date == null)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ الاستفادة");
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
