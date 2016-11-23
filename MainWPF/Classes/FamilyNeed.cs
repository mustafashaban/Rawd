using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class FamilyNeed : ModelViewContext
    {
        private int? familyneedid;
        public int? FamilyNeedID
        {
            get
            { return familyneedid; }
            set
            { familyneedid = value; }
        }



        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }

        private string needtype;
        public string NeedType
        {
            get
            { return needtype; }
            set
            {
                needtype = value;
                this.ClearError("NeedType");
                this.NotifyPropertyChanged("NeedType");
            }
        }

        private string needname;
        public string NeedName
        {
            get
            { return needname; }
            set
            { needname = value; }
        }



        private string _value;
        public string Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }



        private string askedby;
        public string AskedBy
        {
            get
            { return askedby; }
            set
            { askedby = value; }
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



        private bool isensured;
        public bool IsEnsured
        {
            get
            { return isensured; }
            set
            { isensured = value; }
        }



        private string ensuredsupport;
        public string EnsuredSupport
        {
            get
            { return ensuredsupport; }
            set
            { ensuredsupport = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public static FamilyNeed GetFamilyNeedByID(int id)
        {
            FamilyNeed x = new FamilyNeed();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyNeedByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyNeedID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["FamilyNeedID"] is DBNull))
                        x.FamilyNeedID = System.Int32.Parse(rd["FamilyNeedID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.NeedType = rd["NeedType"].ToString();
                    x.NeedName = rd["NeedName"].ToString();
                    x.Value = rd["Value"].ToString();
                    x.AskedBy = rd["AskedBy"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["IsEnsured"] is DBNull))
                        x.IsEnsured = System.Boolean.Parse(rd["IsEnsured"].ToString());
                    x.EnsuredSupport = rd["EnsuredSupport"].ToString();
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
        public static List<FamilyNeed> GetFamilyNeedAllByFamilyID(int? FamilyID)
        {
            List<FamilyNeed> b = new List<FamilyNeed>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyNeedAllByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetFamilyNeedByID(rd.GetInt32(0)));
                }
            }
            catch
            {
                return new List<FamilyNeed>();
            }
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
            if (string.IsNullOrEmpty(NeedType))
            {
                isValid = false;
                this.SetError("NeedType", "يجب إدخال نوع الاحتياج");
            }
            if (Date == null)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ الاحتياج");
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
