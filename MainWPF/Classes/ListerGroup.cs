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
            {
                if (listers == null)
                    listers = new List<Lister>();
                return listers;
            }
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

        //Done :)
        public static List<ListerGroup> GetListerGroupsByFamilyID(int? FamilyID)
        {
            List<ListerGroup> lgs = new List<ListerGroup>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_ListerGroups_ByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                ListerGroup lg = null;
                while (rd.Read())
                {
                    if (lg == null || (int)rd["ListerGroupID"] != lg.GroupID.Value)
                    {
                        lg = new ListerGroup();
                        lg.GroupID = (int)rd["ListerGroupID"];
                        if (!(rd["OrphanID"] is DBNull))
                            lg.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                        if (!(rd["Date"] is DBNull))
                            lg.Date = System.DateTime.Parse(rd["Date"].ToString());
                        lg.Evaluation = rd["Evaluation"].ToString();
                        lg.Notes = rd["ListerGroupNotes"].ToString();
                        lg.CreatePerson = rd["CreatePerson"].ToString();
                        if (!(rd["CreateDate"] is DBNull))
                            lg.CreateDate = System.DateTime.Parse(rd["CreateDate"].ToString());
                        lg.LastModifiedPerson = rd["LastModifiedPerson"].ToString();
                        lg.Notes = rd["ListerGroupNotes"].ToString();
                        if (!(rd["FamilyID"] is DBNull))
                            lg.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());

                        lgs.Add(lg);
                    }
                    Lister l = new Lister();
                    l.ListerID = System.Int32.Parse(rd["ListerListerID"].ToString());
                    l.FirstName = rd["FirstName"].ToString();
                    l.LastName = rd["LastName"].ToString();
                    l.Gender = rd["Gender"].ToString();
                    l.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        l.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    l.Job = rd["Job"].ToString();
                    l.Phone = rd["Phone"].ToString();
                    l.Mobile = rd["Mobile"].ToString();
                    l.Email = rd["Email"].ToString();
                    l.MaritalStatus = rd["MaritalStatus"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        l.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    l.PlaceAddress = rd["PlaceAddress"].ToString();
                    l.ScientificQualifier = rd["ScientificQualifier"].ToString();
                    l.IdentityImage = rd["IdentityImage"].ToString();
                    l.Notes = rd["ListerNotes"].ToString();

                    lg.Listers.Add(l);
                    // lg.FamilyNeeds = FamilyNeed_ListerGroup.GetFamilyNeed_ListerGroupByListerGroupID(lg.GroupID.Value);
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
