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
        private int? sponsorid;
        public int? SponsorID
        {
            get
            { return sponsorid; }
            set
            { sponsorid = value; }
        }



        private bool? isgovernment;
        public bool? IsGovernment
        {
            get
            { return isgovernment; }
            set
            { isgovernment = value; }
        }


        private double? slary;
        public double? Slary
        {
            get
            { return slary; }
            set
            { slary = value; }
        }



        private string identityimage;
        public string IdentityImage
        {
            get
            { return identityimage; }
            set
            {
                if (value != identityimage)
                {
                    identityimage = value;
                    this.NotifyPropertyChanged("IdentityImage");
                }
            }
        }



        private string placeaddress;
        public string PlaceAddress
        {
            get
            { return placeaddress; }
            set
            { placeaddress = value; }
        }



        private string scientificqualifier;
        public string ScientificQualifier
        {
            get
            { return scientificqualifier; }
            set
            { scientificqualifier = value; }
        }



        private string mainbail;
        public string MainBail
        {
            get
            { return mainbail; }
            set
            { mainbail = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertSponsorData()
        {
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage);
            
            SponsorID = BaseDataBase._StoredProcedureReturnable("sp_Add2Sponsor"
                , new SqlParameter("@SponsorID", System.Data.SqlDbType.Int)
                , new SqlParameter("@FirstName", FirstName)
                , new SqlParameter("@LastName", LastName)
                , new SqlParameter("@Nationality", Nationality)
                , new SqlParameter("@IsGovernment", IsGovernment)
                , new SqlParameter("@BirthPlace", BirthPlace)
                , new SqlParameter("@DOB", DOB)
                , new SqlParameter("@Gender", Gender)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@IdentityImage", IdentityImage)
                , new SqlParameter("@PlaceAddress", PlaceAddress)
                , new SqlParameter("@ScientificQualifier", ScientificQualifier)
                , new SqlParameter("@MainBail", MainBail)
                , new SqlParameter("@Notes", Notes));
            return (SponsorID != null);
        }
        public bool UpdateSponsorData()
        {
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage, Sponsor.GetSponsorByID(SponsorID).IdentityImage);
           
            return BaseDataBase._StoredProcedure("sp_UpdateSponsor"
                , new SqlParameter("@SponsorID", SponsorID)
                , new SqlParameter("@FirstName", FirstName)
                , new SqlParameter("@LastName", LastName)
                , new SqlParameter("@Nationality", Nationality)
                , new SqlParameter("@IsGovernment", IsGovernment)
                , new SqlParameter("@BirthPlace", BirthPlace)
                , new SqlParameter("@DOB", DOB)
                , new SqlParameter("@Gender", Gender)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@IdentityImage", IdentityImage)
                , new SqlParameter("@PlaceAddress", PlaceAddress)
                , new SqlParameter("@ScientificQualifier", ScientificQualifier)
                , new SqlParameter("@MainBail", MainBail)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteSponsorData()
        {
            if (BaseDataBase._StoredProcedure("sp_DeleteFromSponsor"
                , new SqlParameter("@SponsorID", SponsorID)))
            {
                BaseDataBase.DeleteImageFIle(IdentityImage);
                return true;
            }
            return false;
        }


        public static List<Sponsor> GetAllSponsors
        {
            get { return GetAllSponsorsMethod(); }
        }
        public static List<Sponsor> GetAllSponsorsMethod()
        {
            List<Sponsor> ss = new List<Sponsor>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAllSponsors", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsor x = new Sponsor();
                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = System.Int32.Parse(rd["SponsorID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    if (!(rd["IsGovernment"] is DBNull))
                        x.IsGovernment = System.Boolean.Parse(rd["IsGovernment"].ToString());
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.ScientificQualifier = rd["ScientificQualifier"].ToString();
                    x.MainBail = rd["MainBail"].ToString();
                    x.Notes = rd["Notes"].ToString();

                    ss.Add(x);
                }
                rd.Close();
            }
            catch
            {
                ss = new List<Sponsor>();
            }
            finally
            {
                con.Close();
            }
            return ss;
        }
        public static DataView GetAllSponsorTable
        { get { return GetAllSponsorTableMethod(); } }
        public static DataView GetAllSponsorTableMethod()
        {
            return BaseDataBase._TablingStoredProcedure("sp_GetAllSponsorTable").DefaultView;
        }


        public static Sponsor GetSponsorByID(int? id)
        {
            Sponsor x = new Sponsor();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetSponsorByID", con);
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
                        x.SponsorID = System.Int32.Parse(rd["SponsorID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    if (!(rd["IsGovernment"] is DBNull))
                        x.IsGovernment = System.Boolean.Parse(rd["IsGovernment"].ToString());
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.ScientificQualifier = rd["ScientificQualifier"].ToString();
                    x.MainBail = rd["MainBail"].ToString();
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
        public static List<Sponsor> GetSponsorAllByOrphanID(int OrphanID)
        {
            List<Sponsor> b = new List<Sponsor>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetSponsorAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetSponsorByID(rd.GetInt32(0)));
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
            if (string.IsNullOrEmpty(FirstName))
            {
                isValid = false;
                this.SetError("FirstName", "يجب إدخال الاسم");
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