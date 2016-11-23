using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Orphan_Health : ModelViewContext
    {


        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }



        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }



        private string healthsituation;
        public string HealthSituation
        {
            get
            { return healthsituation; }
            set
            {
                healthsituation = value;
                this.ClearError("HealthSituation");
                this.NotifyPropertyChanged("HealthSituation");
            }
        }



        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            { date = value; }
        }



        private string dayneeded;
        public string DayNeeded
        {
            get
            { return dayneeded; }
            set
            { dayneeded = value; }
        }



        private string specialneeded;
        public string SpecialNeeded
        {
            get
            { return specialneeded; }
            set
            { specialneeded = value; }
        }



        private bool? isexist;
        public bool? IsExist
        {
            get
            { return isexist; }
            set
            { isexist = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }



        public bool InsertOrphanHealthData()
        {
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2OrphanHealth"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@HealthSituation", HealthSituation)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@DayNeeded", DayNeeded)
                , new SqlParameter("@SpecialNeeded", SpecialNeeded)
                , new SqlParameter("@IsExist", IsExist)
                , new SqlParameter("@Notes", Notes));
            return (ID != null);
        }
        public bool UpdateOrphanHealthData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateOrphanHealth"
                , new SqlParameter("@ID", ID)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@HealthSituation", HealthSituation)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@DayNeeded", DayNeeded)
                , new SqlParameter("@SpecialNeeded", SpecialNeeded)
                , new SqlParameter("@IsExist", IsExist)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteOrphanHealthData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromOrphanHealth"
                , new SqlParameter("@ID", ID));
        }

        public static Orphan_Health GetOrphanHealthByID(int id)
        {
            Orphan_Health x = new Orphan_Health();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanHealthByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanHealthID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    x.HealthSituation = rd["HealthSituation"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    x.DayNeeded = rd["DayNeeded"].ToString();
                    x.SpecialNeeded = rd["SpecialNeeded"].ToString();
                    if (!(rd["IsExist"] is DBNull))
                        x.IsExist = System.Boolean.Parse(rd["IsExist"].ToString());
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
        public static List<Orphan_Health> GetOrphanHealthAllByOrphanID(int? OrphanID)
        {
            List<Orphan_Health> b = new List<Orphan_Health>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanHelathAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetOrphanHealthByID(rd.GetInt32(0)));
                }
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
            if (string.IsNullOrEmpty(HealthSituation))
            {
                isValid = false;
                this.SetError("HealthSituation", "يجب إدخال نوع الحالة الصحية");
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
