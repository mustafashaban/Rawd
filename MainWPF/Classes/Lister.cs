using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Lister : Person
    {
        private int? listerid;
        public int? ListerID
        {
            get
            { return listerid; }
            set
            { listerid = value; }
        }




        private string job;
        public string Job
        {
            get
            { return job; }
            set
            { job = value; }
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



        private string scientificqualifier;
        public string ScientificQualifier
        {
            get
            { return scientificqualifier; }
            set
            { scientificqualifier = value; }
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

        public static List<Lister> GetAllListers
        {
            get { return GetAllListersMethod(); }
        }
        public static DataView GetAllListersTable
        {
            get { return GetAllListersTableMethod(); }
        }

        public static Lister GetListerByID(int? id)
        {
            Lister x = new Lister();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetListerByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ListerID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ListerID"] is DBNull))
                        x.ListerID = System.Int32.Parse(rd["ListerID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        x.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.ScientificQualifier = rd["ScientificQualifier"].ToString();
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
        public static List<Lister> GetListerAllByOrphanID(int OrphanID)
        {
            List<Lister> b = new List<Lister>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetListerAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetListerByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }
        public static List<Lister> GetAllListersMethod()
        {
            List<Lister> AllListers = new List<Lister>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAllListers", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while(rd.Read())
                {
                    Lister x = new Lister();
                    if (!(rd["ListerID"] is DBNull))
                        x.ListerID = System.Int32.Parse(rd["ListerID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        x.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    x.PlaceAddress = rd["PlaceAddress"].ToString();
                    x.ScientificQualifier = rd["ScientificQualifier"].ToString();
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    AllListers.Add(x);
                }
                rd.Close();
            }
            catch
            {
                AllListers = null;
            }
            finally
            {
                con.Close();
            }
            return AllListers;
        }
        public static DataView GetAllListersTableMethod()
        {
            return BaseDataBase._TablingStoredProcedure("sp_GetAllListersTable").DefaultView;
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
