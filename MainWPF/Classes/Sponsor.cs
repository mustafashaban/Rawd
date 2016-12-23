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

        private string status;
        public string Status
        {
            get
            { return status; }
            set
            { status = value; }
        }
        private string mediatorName;
        public string MediatorName
        {
            get
            { return mediatorName; }
            set
            { mediatorName = value; }
        }
        private string mediatorMobile;
        public string MediatorMobile
        {
            get
            { return mediatorMobile; }
            set
            { mediatorMobile = value; }
        }


        List<Sponsorship> sponsorships;
        public List<Sponsorship> Sponsorships
        {
            get { return sponsorships; }
            set { sponsorships = value; }
        }
        List<AvailableSponsorship> availableSponsorships;
        public List<AvailableSponsorship> AvailableSponsorships
        {
            get { return availableSponsorships; }
            set { availableSponsorships = value; }
        }
        public int AllSponsorships
        {
            get { return Sponsorships == null || Sponsorships.Count == 0 ? 0 : Sponsorships.Count; }
        }
        public int EndedSponsorships
        {
            get { return Sponsorships == null || Sponsorships.Count == 0 ? 0 : Sponsorships.Where(x => x.EndDate.HasValue).Count(); }
        }
        public int CurrentSponsorships
        {
            get { return Sponsorships == null || Sponsorships.Count == 0 ? 0 : Sponsorships.Where(x => x.StartDate.HasValue && !x.EndDate.HasValue).Count(); }
        }
        public Account Account { get; set; }


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
            , new SqlParameter("@MediatorName", x.MediatorName)
            , new SqlParameter("@MediatorMobile", x.MediatorMobile)
            , new SqlParameter("@Status", x.Status));

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
            , new SqlParameter("@MediatorName", x.MediatorName)
            , new SqlParameter("@MediatorMobile", x.MediatorMobile)
            , new SqlParameter("@Status", x.Status));
        }
        public static bool DeleteData(Sponsor x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Sponsor"
            , new SqlParameter("@SponsorID", x.SponsorID));
        }
        public static Sponsor GetSponsorByID(int id, bool bringDetails = false)
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
                    x.MediatorName = rd["MediatorName"].ToString();
                    x.MediatorMobile = rd["MediatorMobile"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.Status = rd["Status"].ToString();

                    if (bringDetails)
                    {
                        x.Account = Account.GetAccountByOwnerID(Account.AccountType.Sponsor, x.SponsorID.Value);
                        x.Sponsorships = Sponsorship.GetSponsorshipAllBySponsorID(x);
                        x.AvailableSponsorships = AvailableSponsorship.GetAllAvailableSponsorshipBySponsorID(x);
                    }
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
                    x.MediatorName = rd["MediatorName"].ToString();
                    x.MediatorMobile = rd["MediatorMobile"].ToString();
                    x.Notes = rd["Notes"].ToString();
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