using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Guardian : Person
    {


        private int? guardianid;
        public int? GuardianID
        {
            get
            { return guardianid; }
            set
            { guardianid = value; }
        }



        private string job;
        public string Job
        {
            get
            { return job; }
            set
            { job = value; }
        }



        private double? slary;
        public double? Slary
        {
            get
            { return slary; }
            set
            { slary = value; }
        }



        private string salarycurrency;
        public string SalaryCurrency
        {
            get
            { return salarycurrency; }
            set
            { salarycurrency = value; }
        }





        private string healthsituation;
        public string HealthSituation
        {
            get
            { return healthsituation; }
            set
            { healthsituation = value; }
        }



        private string ethicalsituation;
        public string EthicalSituation
        {
            get
            { return ethicalsituation; }
            set
            { ethicalsituation = value; }
        }





        private int? childcount;
        public int? ChildCount
        {
            get
            { return childcount; }
            set
            { childcount = value; }
        }



        private string placeaddress;
        public string PlaceAddress
        {
            get
            { return placeaddress; }
            set
            { placeaddress = value; }
        }


        private string identityimage;
        public string IdentityImage
        {
            get
            { return identityimage; }
            set
            {
                if (identityimage != value)
                {
                    identityimage = value;
                    this.NotifyPropertyChanged("IdentityImage");
                }
            }
        }


        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public static List<Guardian> GetAllGuardianMale
        { get { return GetGuardianAll("ذكر"); } }
        public static List<Guardian> GetAllGuardianFeMale
        { get { return GetGuardianAll("أنثى"); } }


        public bool InsertGuardianData()
        {
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage);

            GuardianID = BaseDataBase._StoredProcedureReturnable("sp_Add2Guardian"
                , new SqlParameter("@GuardianID", SqlDbType.Int)
                , new SqlParameter("@FirstName", FirstName)
                , new SqlParameter("@LastName", LastName)
                , new SqlParameter("@Gender", Gender)
                , new SqlParameter("@BirthPlace", BirthPlace)
                , new SqlParameter("@DOB", DOB)
                , new SqlParameter("@Job", Job)
                , new SqlParameter("@Slary", Slary)
                , new SqlParameter("@SalaryCurrency", SalaryCurrency)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@HealthSituation", HealthSituation)
                , new SqlParameter("@EthicalSituation", EthicalSituation)
                , new SqlParameter("@MaritalStatus", MaritalStatus)
                , new SqlParameter("@ChildCount", ChildCount)
                , new SqlParameter("@PlaceAddress", PlaceAddress)
                , new SqlParameter("@IdentityImage", IdentityImage)
                , new SqlParameter("@Notes", Notes));
            return GuardianID != null;
        }
        public bool UpdateGuardianData()
        {
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage, Guardian.GetGuardianByID(GuardianID).IdentityImage);

            return BaseDataBase._StoredProcedure("sp_UpdateGuardian"
                , new SqlParameter("@GuardianID", GuardianID)
                , new SqlParameter("@FirstName", FirstName)
                , new SqlParameter("@LastName", LastName)
                , new SqlParameter("@Gender", Gender)
                , new SqlParameter("@BirthPlace", BirthPlace)
                , new SqlParameter("@DOB", DOB)
                , new SqlParameter("@Job", Job)
                , new SqlParameter("@Slary", Slary)
                , new SqlParameter("@SalaryCurrency", SalaryCurrency)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@HealthSituation", HealthSituation)
                , new SqlParameter("@EthicalSituation", EthicalSituation)
                , new SqlParameter("@MaritalStatus", MaritalStatus)
                , new SqlParameter("@ChildCount", ChildCount)
                , new SqlParameter("@PlaceAddress", PlaceAddress)
                , new SqlParameter("@IdentityImage", IdentityImage)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteGuardianData()
        {
            if (BaseDataBase._StoredProcedure("sp_DeleteFromGuardian"
                , new SqlParameter("@GuardianID", GuardianID)))
            {
                BaseDataBase.DeleteImageFIle(IdentityImage);
                return true;
            }
            return false;
        }


        public static List<Guardian> GetGuardianAll(string Gender)
        {
            List<Guardian> lg = new List<Guardian>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetGuardianAll", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@Gender", Gender);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Guardian x = new Guardian();
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = System.Int32.Parse(rd["GuardianID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    if (!(rd["Slary"] is DBNull))
                        x.Slary = System.Single.Parse(rd["Slary"].ToString());
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.HealthSituation = rd["HealthSituation"].ToString();
                    x.EthicalSituation = rd["EthicalSituation"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        x.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    lg.Add(x);
                }
                rd.Close();
            }
            catch
            {
                lg = null;
            }
            finally
            {
                con.Close();
            }
            return lg;
        }
        public static Guardian GetGuardianByID(int? id)
        {
            Guardian x = new Guardian();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetGuardianByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@GuardianID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = System.Int32.Parse(rd["GuardianID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    if (!(rd["Slary"] is DBNull))
                        x.Slary = System.Single.Parse(rd["Slary"].ToString());
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.HealthSituation = rd["HealthSituation"].ToString();
                    x.EthicalSituation = rd["EthicalSituation"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        x.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.IdentityImage = rd["IdentityImage"].ToString();
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

        public static DataView GetAllNursemaidTableMethod()
        {
            return BaseDataBase._TablingStoredProcedure("sp_GetAllNursemaidTable").DefaultView;
        }


        public static DataView GetAllGuardianTable
        {
            get { return BaseDataBase._TablingStoredProcedure("sp_GetAllGuardiansTable").DefaultView; }
        }
        public static DataView GetAllNursemaidTable
        {
            get { return GetAllNursemaidTableMethod(); }
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
            if (string.IsNullOrEmpty(LastName))
            {
                isValid = false;
                this.SetError("LastName", "يجب إدخال الكنية");
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
