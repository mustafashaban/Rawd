using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class AvailableSponsorship : ModelViewContext
    {
        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private Sponsor relatedSponsor;
        public Sponsor RelatedSponsor
        {
            get
            { return relatedSponsor; }
            set
            { relatedSponsor = value; }
        }
        private int? nob;
        public int? NOB
        {
            get
            { return nob; }
            set
            { nob = value; }
        }

        private double? duration;
        public double? Duration
        {
            get
            { return duration; }
            set
            { duration = value; }
        }

        private double? sponsorshipvalue;
        public double? SponsorshipValue
        {
            get
            { return sponsorshipvalue; }
            set
            { sponsorshipvalue = value; }
        }

        private string sponsorshiptype;
        public string SponsorshipType
        {
            get
            { return sponsorshiptype; }
            set
            { sponsorshiptype = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private int? lastuserid;
        public int? LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        private bool iscompensated;
        public bool IsCompensated
        {
            get
            { return iscompensated; }
            set
            { iscompensated = value; }
        }

        private int? overmonths;
        public int? OverMonths
        {
            get
            { return overmonths; }
            set
            { overmonths = value; }
        }

        private string sponsortype;
        public string SponsorType
        {
            get
            { return sponsortype; }
            set
            { sponsortype = value; }
        }

        private DateTime? createdate;
        public DateTime? CreateDate
        {
            get
            { return createdate; }
            set
            { createdate = value; }
        }
        public static bool InsertData(AvailableSponsorship x)
        {
            if (x.SponsorshipType == "")
                x.Duration = null;

            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_AvailableSponsorship"
            , new SqlParameter("@ID",System.Data.SqlDbType.Int)
            , new SqlParameter("@SponsorID", x.RelatedSponsor.SponsorID)
            , new SqlParameter("@NOB", x.NOB)
            , new SqlParameter("@Duration", x.Duration)
            , new SqlParameter("@SponsorshipValue", x.SponsorshipValue)
            , new SqlParameter("@SponsorshipType", x.SponsorshipType)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID)
            , new SqlParameter("@IsCompensated", x.IsCompensated)
            , new SqlParameter("@OverMonths", x.OverMonths)
            , new SqlParameter("@SponsorType", x.SponsorType)
            , new SqlParameter("@CreateDate", x.CreateDate));
            return x.ID.HasValue;
        }
        public static bool UpdateData(AvailableSponsorship x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_AvailableSponsorship"
            , new SqlParameter("@ID", x.ID)
            , new SqlParameter("@SponsorID", x.RelatedSponsor.SponsorID)
            , new SqlParameter("@NOB", x.NOB)
            , new SqlParameter("@Duration", x.Duration)
            , new SqlParameter("@SponsorshipValue", x.SponsorshipValue)
            , new SqlParameter("@SponsorshipType", x.SponsorshipType)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID)
            , new SqlParameter("@IsCompensated", x.IsCompensated)
            , new SqlParameter("@OverMonths", x.OverMonths)
            , new SqlParameter("@SponsorType", x.SponsorType)
            , new SqlParameter("@CreateDate", x.CreateDate));
        }
        public static bool DeleteData(AvailableSponsorship x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_AvailableSponsorship"
            , new SqlParameter("@ID", x.ID));
        }
        public static AvailableSponsorship GetAvailableSponsorshipByID(int id)
        {
            AvailableSponsorship x = new AvailableSponsorship();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_AvailableSponsorship", con);
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
                        x.RelatedSponsor = Sponsor.GetSponsorByID(int.Parse(rd["SponsorID"].ToString()));
                    if (!(rd["NOB"] is DBNull))
                        x.NOB = int.Parse(rd["NOB"].ToString());
                    if (!(rd["Duration"] is DBNull))
                        x.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["SponsorshipValue"] is DBNull))
                        x.SponsorshipValue = double.Parse(rd["SponsorshipValue"].ToString());
                    x.SponsorshipType = rd["SponsorshipType"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["IsCompensated"] is DBNull))
                        x.IsCompensated = bool.Parse(rd["IsCompensated"].ToString());
                    if (!(rd["OverMonths"] is DBNull))
                        x.OverMonths = int.Parse(rd["OverMonths"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
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
        public static List<AvailableSponsorship> GetAllAvailableSponsorship()
        {
            List<AvailableSponsorship> xx = new List<AvailableSponsorship>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_AvailableSponsorship", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    AvailableSponsorship x = new AvailableSponsorship();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["SponsorID"] is DBNull))
                        x.RelatedSponsor = Sponsor.GetSponsorByID(int.Parse(rd["SponsorID"].ToString()));
                    if (!(rd["NOB"] is DBNull))
                        x.NOB = int.Parse(rd["NOB"].ToString());
                    if (!(rd["Duration"] is DBNull))
                        x.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["SponsorshipValue"] is DBNull))
                        x.SponsorshipValue = double.Parse(rd["SponsorshipValue"].ToString());
                    x.SponsorshipType = rd["SponsorshipType"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["IsCompensated"] is DBNull))
                        x.IsCompensated = bool.Parse(rd["IsCompensated"].ToString());
                    if (!(rd["OverMonths"] is DBNull))
                        x.OverMonths = int.Parse(rd["OverMonths"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
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
            if (!NOB.HasValue)
            {
                isValid = false;
                this.SetError("NOB", "يجب إدخال الحد الاعلى للمستفيدين");
            }
            if (string.IsNullOrEmpty(SponsorshipType))
            {
                isValid = false;
                this.SetError("SponsorshipType", "يجب إدخال نوع الكفالة");
            }
            if (SponsorType == "كفالة خاصة" && !Duration.HasValue)
            {
                isValid = false;
                this.SetError("Duration", "يجب إدخال مدة الكفالة");
            }
            if (!OverMonths.HasValue)
            {
                isValid = false;
                this.SetError("OverMonths", "يجب إدخال عدد الاشهر الممكن تجاوزها");
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

        internal static List<AvailableSponsorship> GetAllAvailableSponsorshipBySponsorID(Sponsor s)
        {
            List<AvailableSponsorship> xx = new List<AvailableSponsorship>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_SponsorID_AvailableSponsorship", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@SponsorID", s.SponsorID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    AvailableSponsorship x = new AvailableSponsorship();
                    x.RelatedSponsor = s;
                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["NOB"] is DBNull))
                        x.NOB = int.Parse(rd["NOB"].ToString());
                    if (!(rd["Duration"] is DBNull))
                        x.Duration = double.Parse(rd["Duration"].ToString());
                    if (!(rd["SponsorshipValue"] is DBNull))
                        x.SponsorshipValue = double.Parse(rd["SponsorshipValue"].ToString());
                    x.SponsorshipType = rd["SponsorshipType"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["IsCompensated"] is DBNull))
                        x.IsCompensated = bool.Parse(rd["IsCompensated"].ToString());
                    if (!(rd["OverMonths"] is DBNull))
                        x.OverMonths = int.Parse(rd["OverMonths"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
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
