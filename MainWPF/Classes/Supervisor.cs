using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Supervisor : Person
    {
        private int? supervisorid;
        public int? SupervisorID
        {
            get
            { return supervisorid; }
            set
            { supervisorid = value; }
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


        private int? childcount;
        public int? ChildCount
        {
            get
            { return childcount; }
            set
            { childcount = value; }
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

        private string major;
        public string Major
        {
            get
            { return major; }
            set
            { major = value; }
        }

        private bool? isactivated;
        public bool? IsActivated
        {
            get
            { return isactivated; }
            set
            { isactivated = value; }
        }



        private string notactivatedreason;
        public string NotActivatedReason
        {
            get
            { return notactivatedreason; }
            set
            { notactivatedreason = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertSupervisorData()
        {
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage);

            SupervisorID = BaseDataBase._StoredProcedureReturnable("sp_Add2Supervisor"
                , new SqlParameter("@SupervisorID", System.Data.SqlDbType.Int)
                , new SqlParameter("@FirstName", FirstName)
                , new SqlParameter("@LastName", LastName)
                , new SqlParameter("@BirthPlace", BirthPlace)
                , new SqlParameter("@DOB", DOB)
                , new SqlParameter("@Gender", Gender)
                , new SqlParameter("@Job", Job)
                , new SqlParameter("@Slary", Slary)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@MaritalStatus", MaritalStatus)
                , new SqlParameter("@ChildCount", ChildCount)
                , new SqlParameter("@IdentityImage", IdentityImage)
                , new SqlParameter("@PlaceAddress", PlaceAddress)
                , new SqlParameter("@ScientificQualifier", ScientificQualifier)
                , new SqlParameter("@Major", Major)
                , new SqlParameter("@IsActivated", IsActivated)
                , new SqlParameter("@NotActivatedReason", NotActivatedReason)
                , new SqlParameter("@Notes", Notes));
            return SupervisorID != null;
        }
        public bool UpdateSupervisorData()
        {
            IdentityImage = BaseDataBase.CheckImageFile(IdentityImage, Supervisor.GetSupervisorByID(SupervisorID).IdentityImage);

            return BaseDataBase._StoredProcedure("sp_UpdateSupervisor"
                , new SqlParameter("@SupervisorID", SupervisorID)
                , new SqlParameter("@FirstName", FirstName)
                , new SqlParameter("@LastName", LastName)
                , new SqlParameter("@BirthPlace", BirthPlace)
                , new SqlParameter("@DOB", DOB)
                , new SqlParameter("@Gender", Gender)
                , new SqlParameter("@Job", Job)
                , new SqlParameter("@Slary", Slary)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@MaritalStatus", MaritalStatus)
                , new SqlParameter("@ChildCount", ChildCount)
                , new SqlParameter("@IdentityImage", IdentityImage)
                , new SqlParameter("@PlaceAddress", PlaceAddress)
                , new SqlParameter("@ScientificQualifier", ScientificQualifier)
                , new SqlParameter("@Major", Major)
                , new SqlParameter("@IsActivated", IsActivated)
                , new SqlParameter("@NotActivatedReason", NotActivatedReason)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteSupervisorData()
        {
            if (BaseDataBase._StoredProcedure("sp_DeleteFromSupervisor"
                , new SqlParameter("@SupervisorID", SupervisorID)))
            {
                BaseDataBase.DeleteImageFIle(IdentityImage);
                return true;
            }
            return false;
        }

        public static List<Supervisor> GetAllSupervisors
        { get { return GetAllSupervisorsMethod(); } }
        public static List<Supervisor> GetAllSupervisorsMethod()
        {
            var ss = new List<Supervisor>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAllSupervisors", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Supervisor x = new Supervisor();
                    if (!(rd["SupervisorID"] is DBNull))
                        x.SupervisorID = System.Int32.Parse(rd["SupervisorID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Job = rd["Job"].ToString();
                    if (!(rd["Slary"] is DBNull))
                        x.Slary = System.Single.Parse(rd["Slary"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        x.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.ScientificQualifier = rd["ScientificQualifier"].ToString();
                    x.Major = rd["Major"].ToString();
                    if (!(rd["IsActivated"] is DBNull))
                        x.IsActivated = System.Boolean.Parse(rd["IsActivated"].ToString());
                    x.NotActivatedReason = rd["NotActivatedReason"].ToString();
                    x.Notes = rd["Notes"].ToString();

                    ss.Add(x);
                }
                rd.Close();
            }
            catch
            {
                ss = new List<Supervisor>();
            }
            finally
            {
                con.Close();
            }
            return ss;
        }

        public static DataView GetAllSupervisorTable
        { get { return GetAllSupervisorTableMethod(); } }
        public static DataView GetAllSupervisorTableMethod()
        {
            return BaseDataBase._TablingStoredProcedure("sp_GetAllSupervisorsTable").DefaultView;
        }

        public static Supervisor GetSupervisorByID(int? id)
        {
            Supervisor x = new Supervisor();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetSupervisorByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@SupervisorID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["SupervisorID"] is DBNull))
                        x.SupervisorID = System.Int32.Parse(rd["SupervisorID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Gender = rd["Gender"].ToString();
                    x.Job = rd["Job"].ToString();
                    if (!(rd["Slary"] is DBNull))
                        x.Slary = System.Single.Parse(rd["Slary"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        x.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.ScientificQualifier = rd["ScientificQualifier"].ToString();
                    x.Major = rd["Major"].ToString();
                    if (!(rd["IsActivated"] is DBNull))
                        x.IsActivated = System.Boolean.Parse(rd["IsActivated"].ToString());
                    x.NotActivatedReason = rd["NotActivatedReason"].ToString();
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
        public static List<Supervisor> GetSupervisorAllByOrphanID(int OrphanID)
        {
            List<Supervisor> b = new List<Supervisor>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetSupervisorAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetSupervisorByID(rd.GetInt32(0)));
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
