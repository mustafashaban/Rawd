using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Sponsor : Person
    {
        public Sponsor()
        {
            IsOrganazation = false;
            IsGeneral = false;
            IsCompensated = false;
            AllowValue = 0;
        }
        private int? sponsorid;
        public int? SponsorID
        {
            get
            { return sponsorid; }
            set
            { sponsorid = value; }
        }

        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        private bool isorganazation;
        public bool IsOrganazation
        {
            get
            { return isorganazation; }
            set
            { isorganazation = value; }
        }

        private string address;
        public string Address
        {
            get
            { return address; }
            set
            { address = value; }
        }

        private string mainsponsorship;
        public string MainSponsorship
        {
            get
            { return mainsponsorship; }
            set
            { mainsponsorship = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private bool isgeneral;
        public bool IsGeneral
        {
            get
            { return isgeneral; }
            set
            { isgeneral = value; }
        }

        private bool iscompensated;
        public bool IsCompensated
        {
            get
            { return iscompensated; }
            set
            { iscompensated = value; }
        }

        private double? allowvalue;
        public double? AllowValue
        {
            get
            { return allowvalue; }
            set
            { allowvalue = value; }
        }

        private string status;
        public string Status
        {
            get
            { return status; }
            set
            { status = value; }
        }


        List<Sponsorship> sponsorships;
        public List<Sponsorship> Sponsorships
        {
            get { return sponsorships; }
            set { sponsorships = value; }
        }
        public int AllSponsorships
        {
            get { return Sponsorships == null ? 0 : Sponsorships.Count; }
        }
        public int EndedSponsorships
        {
            get { return Sponsorships == null ? 0 : Sponsorships.Where(x => x.EndDate.HasValue).Count(); }
        }
        public int CurrentSponsorships
        {
            get { return Sponsorships == null ? 0 : Sponsorships.Where(x => x.StartDate.HasValue && !x.EndDate.HasValue).Count(); }
        }
        public int WaitSponsorships
        {
            get { return Sponsorships == null ? 0 : Sponsorships.Where(x => !x.StartDate.HasValue).Count(); }
        }


        public static bool InsertData(Sponsor x)
        {
            x.SponsorID = BaseDataBase._StoredProcedureReturnable("sp_Add_Sponsor"
            , new SqlParameter("@SponsorID", SqlDbType.Int)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@Nationality", x.Nationality)
            , new SqlParameter("@IsOrganazation", x.IsOrganazation)
            , new SqlParameter("@DOB", x.DOB)
            , new SqlParameter("@Gender", x.Gender)
            , new SqlParameter("@Phone", x.Phone)
            , new SqlParameter("@Mobile", x.Mobile)
            , new SqlParameter("@Email", x.Email)
            , new SqlParameter("@Address", x.Address)
            , new SqlParameter("@MainSponsorship", x.MainSponsorship)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@IsGeneral", x.IsGeneral)
            , new SqlParameter("@IsCompensated", x.IsCompensated)
            , new SqlParameter("@AllowValue", x.AllowValue)
            , new SqlParameter("@Status", x.Status));

            if (x.SponsorID.HasValue)
            {
                Account a = new MainWPF.Account();
                a.Type = 1;
                a.Name = x.Name;
            }

            return x.SponsorID.HasValue;
        }
        public static bool UpdateData(Sponsor x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Sponsor"
            , new SqlParameter("@SponsorID", x.SponsorID)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@Nationality", x.Nationality)
            , new SqlParameter("@IsOrganazation", x.IsOrganazation)
            , new SqlParameter("@DOB", x.DOB)
            , new SqlParameter("@Gender", x.Gender)
            , new SqlParameter("@Phone", x.Phone)
            , new SqlParameter("@Mobile", x.Mobile)
            , new SqlParameter("@Email", x.Email)
            , new SqlParameter("@Address", x.Address)
            , new SqlParameter("@MainSponsorship", x.MainSponsorship)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@IsGeneral", x.IsGeneral)
            , new SqlParameter("@IsCompensated", x.IsCompensated)
            , new SqlParameter("@AllowValue", x.AllowValue)
            , new SqlParameter("@Status", x.Status));
        }
        public static bool DeleteData(Sponsor x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Sponsor"
            , new SqlParameter("@SponsorID", x.SponsorID));
        }
        public static Sponsor GetSponsorByID(int id)
        {
            Sponsor x = new Sponsor();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Sponsor", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@SponsorID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = int.Parse(rd["SponsorID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    if (!(rd["IsOrganazation"] is DBNull))
                        x.IsOrganazation = bool.Parse(rd["IsOrganazation"].ToString());
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.Address = rd["Address"].ToString();
                    x.MainSponsorship = rd["MainSponsorship"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["IsGeneral"] is DBNull))
                        x.IsGeneral = bool.Parse(rd["IsGeneral"].ToString());
                    if (!(rd["IsCompensated"] is DBNull))
                        x.IsCompensated = bool.Parse(rd["IsCompensated"].ToString());
                    if (!(rd["AllowValue"] is DBNull))
                        x.AllowValue = double.Parse(rd["AllowValue"].ToString());
                    x.Status = rd["Status"].ToString();

                    x.Sponsorships = Sponsorship.GetSponsorshipAllBySponsorID(x.SponsorID.Value);
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
        public static List<Sponsor> GetAllSponsor()
        {
            List<Sponsor> xx = new List<Sponsor>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Sponsor", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsor x = new Sponsor();

                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = int.Parse(rd["SponsorID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    if (!(rd["IsOrganazation"] is DBNull))
                        x.IsOrganazation = bool.Parse(rd["IsOrganazation"].ToString());
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.Address = rd["Address"].ToString();
                    x.MainSponsorship = rd["MainSponsorship"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["IsGeneral"] is DBNull))
                        x.IsGeneral = bool.Parse(rd["IsGeneral"].ToString());
                    if (!(rd["IsCompensated"] is DBNull))
                        x.IsCompensated = bool.Parse(rd["IsCompensated"].ToString());
                    if (!(rd["AllowValue"] is DBNull))
                        x.AllowValue = double.Parse(rd["AllowValue"].ToString());
                    x.Status = rd["Status"].ToString();
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

        public static List<Sponsor> GetAllSponsors
        {
            get { return GetAllSponsor(); }
        }
        public static DataView GetAllSponsorTable
        { get { return GetAllSponsorTableMethod(); } }
        public static DataView GetAllSponsorTableMethod()
        {
            return BaseDataBase._TablingStoredProcedure("sp_GetAllSponsorTable").DefaultView;
        }


        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(Name))
            {
                isValid = false;
                this.SetError("Name", "يجب إدخال الاسم");
            }
            if (string.IsNullOrEmpty(Status))
            {
                isValid = false;
                this.SetError("Status", "يجب إدخال الحالة");
            }
            if (!AllowValue.HasValue)
            {
                isValid = false;
                this.SetError("AllowValue", "يجب إدخال حد التجاوز المسموح به");
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