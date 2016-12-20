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

        public AvailableSponsorship AvailableSponsorship { get; set; }

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


        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public string OrphanName { get; set; }

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
            , new SqlParameter("@OrphanID", x.OrphanID)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID)
            , new SqlParameter("@AvailableSponsorshipID", x.AvailableSponsorship.ID));
            return x.ID.HasValue;
        }
        public static bool UpdateData(Sponsorship x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Sponsorship"
            , new SqlParameter("@ID", x.ID)
            , new SqlParameter("@OrphanID", x.OrphanID)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID)
            , new SqlParameter("@AvailableSponsorshipID", x.AvailableSponsorship.ID));
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
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["AvailableSponsorshipID"] is DBNull))
                        x.AvailableSponsorship = AvailableSponsorship.GetAvailableSponsorshipByID(int.Parse(rd["AvailableSponsorshipID"].ToString()));
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
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["AvailableSponsorshipID"] is DBNull))
                        x.AvailableSponsorship = AvailableSponsorship.GetAvailableSponsorshipByID(int.Parse(rd["AvailableSponsorshipID"].ToString()));
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
        public string Status
        {
            get { return !StartDate.HasValue ? "بالانتظار" : EndDate.HasValue ? "منتهية" : "غير منتهية"; }
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
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

        public static List<Sponsorship> GetSponsorshipAllBySponsorID(Sponsor s)
        {
            List<Sponsorship> xx = new List<Sponsorship>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_SponsorID_Sponsorship", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@SponsorID", s.SponsorID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsorship x = new Sponsorship();

                    if (!(rd["SponsorshipID"] is DBNull))
                        x.ID = int.Parse(rd["SponsorshipID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["SponsorshipNotes"].ToString();
                    x.OrphanName = rd["OrphanName"].ToString();
                    if (!(rd["SponsorshipLastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["SponsorshipLastUserID"].ToString());

                    x.AvailableSponsorship = new AvailableSponsorship();
                    x.AvailableSponsorship.RelatedSponsor = s;
                    if (!(rd["AvailableSponsorshipID"] is DBNull))
                        x.AvailableSponsorship.ID = int.Parse(rd["AvailableSponsorshipID"].ToString());
                    if (!(rd["NOB"] is DBNull))
                        x.AvailableSponsorship.NOB = int.Parse(rd["NOB"].ToString());
                    if (!(rd["Duration"] is DBNull))
                        x.AvailableSponsorship.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["SponsorshipValue"] is DBNull))
                        x.AvailableSponsorship.SponsorshipValue = double.Parse(rd["SponsorshipValue"].ToString());
                    x.AvailableSponsorship.SponsorshipType = rd["SponsorshipType"].ToString();
                    x.AvailableSponsorship.SponsorType = rd["SponsorType"].ToString();
                    x.AvailableSponsorship.Notes = rd["AvailableSponsorshipNotes"].ToString();
                    if (!(rd["AvailableSponsorshipLastUserID"] is DBNull))
                        x.AvailableSponsorship.LastUserID = int.Parse(rd["AvailableSponsorshipLastUserID"].ToString());

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
            com.Parameters.Add(new SqlParameter("@OrphanID", OrphanID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsorship x = new Sponsorship();

                    if (!(rd["SponsorshipID"] is DBNull))
                        x.ID = int.Parse(rd["SponsorshipID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["StartDate"] is DBNull))
                        x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["SponsorshipNotes"].ToString();
                    if (!(rd["SponsorshipLastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["SponsorshipLastUserID"].ToString());

                    x.AvailableSponsorship = new AvailableSponsorship();
                    if (!(rd["SponsorID"] is DBNull))
                        x.AvailableSponsorship.RelatedSponsor = new Sponsor() { SponsorID = int.Parse(rd["SponsorID"].ToString()), Name = rd["SponsorName"].ToString() };
                    if (!(rd["AvailableSponsorshipID"] is DBNull))
                        x.AvailableSponsorship.ID = int.Parse(rd["AvailableSponsorshipID"].ToString());
                    if (!(rd["NOB"] is DBNull))
                        x.AvailableSponsorship.NOB = int.Parse(rd["NOB"].ToString());
                    if (!(rd["Duration"] is DBNull))
                        x.AvailableSponsorship.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["SponsorshipValue"] is DBNull))
                        x.AvailableSponsorship.SponsorshipValue = double.Parse(rd["SponsorshipValue"].ToString());
                    x.AvailableSponsorship.SponsorType = rd["SponsorType"].ToString();
                    x.AvailableSponsorship.SponsorshipType = rd["SponsorshipType"].ToString();
                    x.AvailableSponsorship.IsCompensated = (bool)rd["IsCompensated"];
                    x.AvailableSponsorship.Notes = rd["AvailableSponsorshipNotes"].ToString();
                    if (!(rd["AvailableSponsorshipLastUserID"] is DBNull))
                        x.AvailableSponsorship.LastUserID = int.Parse(rd["AvailableSponsorshipLastUserID"].ToString());

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

    }
}
