using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Sponsorship : ModelViewContext
    {
        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private int? sponsorid;
        public int? SponsorID
        {
            get
            { return sponsorid; }
            set
            { sponsorid = value; }
        }

        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }

        private DateTime? startdate;
        public DateTime? StartDate
        {
            get
            { return startdate; }
            set
            { startdate = value; }
        }

        private DateTime? enddate;
        public DateTime? EndDate
        {
            get
            { return enddate; }
            set
            { enddate = value; }
        }

        private string sponsortype;
        public string SponsorType
        {
            get
            { return sponsortype; }
            set
            { sponsortype = value; }
        }

        private double? sponsorvalue;
        public double? SponsorValue
        {
            get
            { return sponsorvalue; }
            set
            { sponsorvalue = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private double? duration;
        public double? Duration
        {
            get
            { return duration; }
            set
            { duration = value; }
        }

        public string Status
        {
            get{ return !StartDate.HasValue ? "بالانتظار" : EndDate.HasValue ? "منتهية" : "غير منتهية"; }
        }

        private int lastuserid;
        public int LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }
        public static bool InsertData(Sponsorship x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_Sponsorship"
            , new SqlParameter("@ID", System.Data.SqlDbType.Int)
            , new SqlParameter("@SponsorID", x.SponsorID)
            , new SqlParameter("@OrphanID", x.OrphanID)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@SponsorType", x.SponsorType)
            , new SqlParameter("@SponsorValue", x.SponsorValue)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@Duration", x.Duration)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            return x.ID.HasValue;
        }
        public static bool UpdateData(Sponsorship x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Sponsorship"
            , new SqlParameter("@ID", x.ID)
            , new SqlParameter("@SponsorID", x.SponsorID)
            , new SqlParameter("@OrphanID", x.OrphanID)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@SponsorType", x.SponsorType)
            , new SqlParameter("@SponsorValue", x.SponsorValue)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@Duration", x.Duration)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(Sponsorship x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Sponsorship"
            , new SqlParameter("@ID", x.ID));
        }
        public static Sponsorship GetSponsorshipByID(int id)
        {
            Sponsorship x = new Sponsorship();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Sponsorship", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = int.Parse(rd["SponsorID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    if (!(rd["SponsorValue"] is DBNull))
                        x.SponsorValue = double.Parse(rd["SponsorValue"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["Duration"] is DBNull))
                        x.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
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
        public static List<Sponsorship> GetAllSponsorship()
        {
            List<Sponsorship> xx = new List<Sponsorship>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Sponsorship", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsorship x = new Sponsorship();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = int.Parse(rd["SponsorID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    if (!(rd["SponsorValue"] is DBNull))
                        x.SponsorValue = double.Parse(rd["SponsorValue"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["Duration"] is DBNull))
                        x.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
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




        public static List<Sponsorship> GetSponsorshipAllByOrphanID(int OrphanID)
        {
            List<Sponsorship> xx = new List<Sponsorship>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_OrphanID_Sponsorship", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsorship x = new Sponsorship();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = int.Parse(rd["SponsorID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    if (!(rd["SponsorValue"] is DBNull))
                        x.SponsorValue = double.Parse(rd["SponsorValue"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["Duration"] is DBNull))
                        x.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
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

        public static List<Sponsorship> GetSponsorshipAllBySponsorID(int SponsorID)
        {
            List<Sponsorship> xx = new List<Sponsorship>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_SponsorID_Sponsorship", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@SponsorID", SponsorID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsorship x = new Sponsorship();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = int.Parse(rd["SponsorID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    if (!(rd["SponsorValue"] is DBNull))
                        x.SponsorValue = double.Parse(rd["SponsorValue"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["Duration"] is DBNull))
                        x.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
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

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (SponsorID == null)
            {
                isValid = false;
                this.SetError("SponsorID", "يجب إختيار كفيل");
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
