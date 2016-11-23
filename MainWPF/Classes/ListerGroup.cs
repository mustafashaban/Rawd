using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class ListerGroup : ModelViewContext
    {
        private int? groupid;
        public int? GroupID
        {
            get
            { return groupid; }
            set
            { groupid = value; }
        }

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }

        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
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

        private string evaluation;
        public string Evaluation
        {
            get
            { return evaluation; }
            set
            { evaluation = value; }
        }

        private string createPerson;
        public string CreatePerson
        {
            get
            { return createPerson; }
            set
            { createPerson = value; }
        }

        private DateTime createDate = BaseDataBase.DateNow;
        public DateTime CreateDate
        {
            get
            { return createDate; }
            set
            { createDate = value; }
        }

        private string lastModifiedPerson;
        public string LastModifiedPerson
        {
            get
            { return lastModifiedPerson; }
            set
            { lastModifiedPerson = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private List<Lister> listers;
        public List<Lister> Listers
        {
            set
            {
                listers = value;
                NotifyPropertyChanged("Listers");
            }
            get
            { return listers; }
        }

        private List<FamilyNeed_ListerGroup> familyNeeds = new List<FamilyNeed_ListerGroup>();
        public List<FamilyNeed_ListerGroup> FamilyNeeds
        {
            get { return familyNeeds; }
            set { familyNeeds = value; }
        }

        public static ListerGroup GetListerGroupByID(int? id)
        {
            ListerGroup x = new ListerGroup();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetListerGroupByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@GroupID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["GroupID"] is DBNull))
                        x.GroupID = System.Int32.Parse(rd["GroupID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.CreatePerson = rd["CreatePerson"].ToString();
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = System.DateTime.Parse(rd["CreateDate"].ToString());
                    x.LastModifiedPerson = rd["LastModifiedPerson"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    
                    x.Listers = ListerGroup.GetListersAllByGroupID(x.GroupID);
                    x.FamilyNeeds = FamilyNeed_ListerGroup.GetFamilyNeed_ListerGroupByListerGroupID(x.GroupID.Value);
                }
                rd.Close();
            }
            catch
            {
                x = new ListerGroup();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        private static List<Lister> GetListersAllByGroupID(int? id)
        {
            List<Lister> ls = new List<Lister>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetListersAllByGroupID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@GroupID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    ls.Add(Lister.GetListerByID(rd.GetInt32(0)));
                }
                rd.Close();
            }
            catch
            {
                ls = new List<Lister>();
            }
            finally
            {
                con.Close();
            }
            return ls;
        }

        public static List<ListerGroup> GetListerGroupsByFamilyID(int? FamilyID)
        {
            List<ListerGroup> lgs = new List<ListerGroup>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetListerGroupsByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    ListerGroup x = ListerGroup.GetListerGroupByID(rd.GetInt32(0));
                    lgs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                lgs = new List<ListerGroup>();
            }
            finally
            {
                con.Close();
            }
            return lgs;
        }
        public static List<ListerGroup> GetListerGroupsByOrphanID(int? OrphanID)
        {
            List<ListerGroup> lgs = new List<ListerGroup>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetListerGroupsByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    ListerGroup x = ListerGroup.GetListerGroupByID(rd.GetInt32(0));
                    lgs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                lgs = new List<ListerGroup>();
            }
            finally
            {
                con.Close();
            }
            return lgs;
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (Date == null)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ التقييم");
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
