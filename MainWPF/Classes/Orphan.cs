
using MainWPF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Orphan : Person
    {
        public Orphan()
        { }
        public Orphan(FamilyPerson fp)
        {
            try
            {
                this.BirthPlace = fp.BirthPlace;
                this.DeathDate = fp.DeathDate;
                this.DeathReason = fp.DeathReason;
                this.DOB = fp.DOB;
                this.Email = fp.Email;
                this.FamilyID = fp.FamilyID;
                this.FirstName = fp.FirstName;
                this.Gender = fp.Gender;
                this.Job = fp.Job;
                this.LastName = fp.LastName;
                this.MaritalStatus = fp.MaritalStatus;
                this.Mobile = fp.Mobile;
                this.Nationality = fp.Nationality;
                this.Notes = fp.Notes;
                this.Phone = fp.Phone;
                this.PID = fp.PID;
                this.LastName = Family.GetFamilyByID(fp.FamilyID.Value).FamilyFather.LastName;
            }
            catch { }
        }

        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }



        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }

        private string middlename;
        public string MiddleName
        {
            get
            { return middlename; }
            set
            { middlename = value; }
        }


        private string job;
        public string Job
        {
            get
            { return job; }
            set
            { job = value; }
        }



        private string salary;
        public string Salary
        {
            get
            { return salary; }
            set
            { salary = value; }
        }



        private string salarycurrency;
        public string SalaryCurrency
        {
            get
            { return salarycurrency; }
            set
            { salarycurrency = value; }
        }


        private string personalimage;
        public string PersonalImage
        {
            get
            { return personalimage; }
            set
            {
                if (personalimage != value)
                {
                    personalimage = value;
                    this.NotifyPropertyChanged("PersonalImage");
                }
            }
        }



        private string bondplace;
        public string BondPlace
        {
            get
            { return bondplace; }
            set
            { bondplace = value; }
        }



        private string bondnumber;
        public string BondNumber
        {
            get
            { return bondnumber; }
            set
            { bondnumber = value; }
        }



        private DateTime? givingdate;
        public DateTime? GivingDate
        {
            get
            { return givingdate; }
            set
            { givingdate = value; }
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


        private int? tall;
        public int? Tall
        {
            get
            { return tall; }
            set
            { tall = value; }
        }

        private int? weight;
        public int? Weight
        {
            get
            { return weight; }
            set
            { weight = value; }
        }

        private int? feetSize;
        public int? FeetSize
        {
            get { return feetSize; }
            set { feetSize = value; }
        }

        private int? waistSize;
        public int? WaistSize
        {
            get { return waistSize; }
            set { waistSize = value; }
        }


        private bool isDetached;
        public bool IsDetached
        {
            get { return isDetached; }
            set { isDetached = value; }
        }


        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertOrphanData()
        {
            Orphan o = Orphan.GetOrphanByID(OrphanID);
            PersonalImage = BaseDataBase.CheckImageFile(PersonalImage);
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage);

            OrphanID = BaseDataBase._StoredProcedureReturnable("sp_Add2Orphan"
               , new SqlParameter("@OrphanID", System.Data.SqlDbType.Int)
               , new SqlParameter("@FamilyID", FamilyID)
               , new SqlParameter("@FirstName", FirstName)
               , new SqlParameter("@MiddleName", MiddleName)
               , new SqlParameter("@LastName", LastName)
               , new SqlParameter("@Gender", Gender)
               , new SqlParameter("@BirthPlace", BirthPlace)
               , new SqlParameter("@DOB", DOB)
               , new SqlParameter("@Nationality", Nationality)
               , new SqlParameter("@Job", Job)
               , new SqlParameter("@Salary", Salary)
               , new SqlParameter("@SalaryCurrency", SalaryCurrency)
               , new SqlParameter("@MaritalStatus", MaritalStatus)
               , new SqlParameter("@PID", PID)
               , new SqlParameter("@Phone", Phone)
               , new SqlParameter("@Mobile", Mobile)
               , new SqlParameter("@Email", Email)
               , new SqlParameter("@PersonalImage", PersonalImage)
               , new SqlParameter("@BondPlace", BondPlace)
               , new SqlParameter("@BondNumber", BondNumber)
               , new SqlParameter("@IdentityID", IdentityID)
               , new SqlParameter("@GivingDate", GivingDate)
               , new SqlParameter("@IdentityImage", IdentityImage)
               , new SqlParameter("@DateOfDeath", DeathDate)
               , new SqlParameter("@ReasonOfDeath", DeathReason)
               , new SqlParameter("@Tall", Tall)
               , new SqlParameter("@Weight", Weight)
               , new SqlParameter("@FeetSize", FeetSize)
               , new SqlParameter("@WaistSize", WaistSize)
               , new SqlParameter("@IsDetached", IsDetached)
               , new SqlParameter("@Notes", Notes));

            return OrphanID != null;
        }
        public bool UpdateOrphanData()
        {
            Orphan o = Orphan.GetOrphanByID(OrphanID);
            PersonalImage = BaseDataBase.CheckImageFile(PersonalImage, o.PersonalImage);
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage, o.IdentityImage);

            return BaseDataBase._StoredProcedure("sp_UpdateOrphan"
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@FamilyID", FamilyID)
                , new SqlParameter("@FirstName", FirstName)
                , new SqlParameter("@MiddleName", MiddleName)
                , new SqlParameter("@LastName", LastName)
                , new SqlParameter("@Gender", Gender)
                , new SqlParameter("@BirthPlace", BirthPlace)
                , new SqlParameter("@DOB", DOB)
                , new SqlParameter("@Nationality", Nationality)
                , new SqlParameter("@Job", Job)
                , new SqlParameter("@Salary", Salary)
                , new SqlParameter("@SalaryCurrency", SalaryCurrency)
                , new SqlParameter("@MaritalStatus", MaritalStatus)
                , new SqlParameter("@PID", PID)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@PersonalImage", PersonalImage)
                , new SqlParameter("@BondPlace", BondPlace)
                , new SqlParameter("@BondNumber", BondNumber)
                , new SqlParameter("@IdentityID", IdentityID)
                , new SqlParameter("@GivingDate", GivingDate)
                , new SqlParameter("@IdentityImage", IdentityImage)
                , new SqlParameter("@DateOfDeath", DeathDate)
                , new SqlParameter("@ReasonOfDeath", DeathReason)
                , new SqlParameter("@Tall", Tall)
                , new SqlParameter("@Weight", Weight)
                , new SqlParameter("@FeetSize", FeetSize)
                , new SqlParameter("@WaistSize", WaistSize)
                , new SqlParameter("@IsDetached", IsDetached)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteOrphanData()
        {
            if (BaseDataBase._StoredProcedure("sp_DeleteFromOrphan"
                , new SqlParameter("@OrphanID", OrphanID)))
            {
                BaseDataBase.DeleteImageFIle(PersonalImage);
                BaseDataBase.DeleteImageFIle(IdentityImage);
                return true;
            }
            return false;
        }

        public static Orphan GetOrphanByID(int? id)
        {
            Orphan x = new Orphan();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.MiddleName = rd["MiddleName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Nationality = rd["Nationality"].ToString();
                    x.Job = rd["Job"].ToString();
                    x.Salary = rd["Salary"].ToString();
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.PID = rd["PID"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.PersonalImage = rd["PersonalImage"].ToString();
                    x.BondPlace = rd["BondPlace"].ToString();
                    x.BondNumber = rd["BondNumber"].ToString();
                    x.IdentityID = rd["IdentityID"].ToString();
                    if (!(rd["GivingDate"] is DBNull))
                        x.GivingDate = System.DateTime.Parse(rd["GivingDate"].ToString());
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    if (!(rd["DateOfDeath"] is DBNull))
                        x.DeathDate = System.DateTime.Parse(rd["DateOfDeath"].ToString());
                    x.DeathReason = rd["ReasonOfDeath"].ToString();
                    if (!(rd["Tall"] is DBNull))
                        x.Tall = System.Int32.Parse(rd["Tall"].ToString());
                    if (!(rd["Weight"] is DBNull))
                        x.Weight = System.Int32.Parse(rd["Weight"].ToString());
                    if (!(rd["FeetSize"] is DBNull))
                        x.FeetSize = System.Int32.Parse(rd["FeetSize"].ToString());
                    if (!(rd["WaistSize"] is DBNull))
                        x.WaistSize = System.Int32.Parse(rd["WaistSize"].ToString());
                    if (!(rd["IsDetached"] is DBNull))
                        x.IsDetached = System.Boolean.Parse(rd["IsDetached"].ToString());
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
        public static List<Orphan> GetOrphanAll()
        {
            List<Orphan> os = new List<Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanAll", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Orphan x = new Orphan();
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.MiddleName = rd["MiddleName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    if (!(rd["IsDetached"] is DBNull))
                        x.IsDetached = System.Boolean.Parse(rd["IsDetached"].ToString());

                    os.Add(x);
                }
                rd.Close();
            }
            catch
            {
                os = new List<Orphan>();
            }
            finally
            {
                con.Close();
            }
            return os;
        }

        public static List<Orphan> GetOrphanAllBySponsorID(int SponsorID)
        {
            List<Orphan> b = new List<Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanAllBySponsorID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@SponsorID", SponsorID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetOrphanByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }
        public static List<Orphan> GetOrphanAllBySupervisorID(int SupervisorID)
        {
            List<Orphan> b = new List<Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanAllBySupervisorID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@SupervisorID", SupervisorID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetOrphanByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }
        public static List<Orphan> GetOrphanAllByGuardianID(int GuardianID)
        {
            List<Orphan> b = new List<Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanAllByGuardianID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@GuardianID", GuardianID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetOrphanByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }
        public static List<Orphan> GetOrphanAllByListerID(int ListerID)
        {
            List<Orphan> b = new List<Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanAllByListerID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ListerID", ListerID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetOrphanByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }
        public static List<Orphan> GetOrphanAllByFamilyID(int? FamilyID)
        {
            List<Orphan> b = new List<Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanAllByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetOrphanByID(rd.GetInt32(0)));
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
            if (string.IsNullOrEmpty(LastName))
            {
                isValid = false;
                this.SetError("LastName", "يجب إدخال الكنية");
            }
            if (string.IsNullOrEmpty(Gender))
            {
                isValid = false;
                this.SetError("Gender", "يجب إدخال الجنس");
            }
            if (!DOB.HasValue)
            {
                isValid = false;
                this.SetError("DOB", "يجب إدخال تاريخ الولادة");
            }
            if (DOB.HasValue)
            {
                var diff = BaseDataBase.DateNow - DOB.Value;
                if (diff.Days < 0 || diff.Days / 30 / 12 > 120)
                {
                    isValid = false;
                    this.SetError("DOB", "التاريخ غير صالح يجب ادخال عمر صحيح");
                }
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
