using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    class NominationSide
    {
        private int nominationsideid;
        public int NominationSideID
        {
            get
            { return nominationsideid; }
            set
            { nominationsideid = value; }
        }


        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }


        private DateTime? adddate;
        public DateTime? AddDate
        {
            get
            { return adddate; }
            set
            { adddate = value; }
        }



        private string phone;
        public string Phone
        {
            get
            { return phone; }
            set
            { phone = value; }
        }



        private string mobile;
        public string Mobile
        {
            get
            { return mobile; }
            set
            { mobile = value; }
        }



        private string email;
        public string Email
        {
            get
            { return email; }
            set
            { email = value; }
        }



        private string address;
        public string Address
        {
            get
            { return address; }
            set
            { address = value; }
        }



        private string responsibleperson;
        public string ResponsiblePerson
        {
            get
            { return responsibleperson; }
            set
            { responsibleperson = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }


        public bool InsertNominationData()
        {
            return BaseDataBase._StoredProcedure("sp_Add2NominationSide"
                , new SqlParameter("@Name", Name)
                , new SqlParameter("@AddDate", AddDate)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@Address", Address)
                , new SqlParameter("@ResponsiblePerson", ResponsiblePerson)
                , new SqlParameter("@Notes", Notes));
        }
        public bool UpdateNominationData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateNominationSide"
                , new SqlParameter("@NominationSideID", NominationSideID)
                , new SqlParameter("@Name", Name)
                , new SqlParameter("@AddDate", AddDate)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@Address", Address)
                , new SqlParameter("@ResponsiblePerson", ResponsiblePerson)
                , new SqlParameter("@Notes", Notes));
        }

        public static NominationSide GetNominationSideByID(int id)
        {
            NominationSide x = new NominationSide();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetNominationSideByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@NominationSideID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["NominationSideID"] is DBNull))
                        x.NominationSideID = System.Int32.Parse(rd["NominationSideID"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (!(rd["AddDate"] is DBNull))
                        x.AddDate = System.DateTime.Parse(rd["AddDate"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.Address = rd["Address"].ToString();
                    x.ResponsiblePerson = rd["ResponsiblePerson"].ToString();
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

        public static List<NominationSide> GetAllNominationSides()
        {
            List<NominationSide> nss = new List<NominationSide>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAllNominationSides", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    NominationSide x = new NominationSide();
                    if (!(rd["NominationSideID"] is DBNull))
                        x.NominationSideID = System.Int32.Parse(rd["NominationSideID"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (!(rd["AddDate"] is DBNull))
                        x.AddDate = System.DateTime.Parse(rd["AddDate"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.Address = rd["Address"].ToString();
                    x.ResponsiblePerson = rd["ResponsiblePerson"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    nss.Add(x);
                }
                rd.Close();
            }
            catch
            {
                nss = null;
            }
            finally
            {
                con.Close();
            }
            return nss;
        }


        internal bool IsValidate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            return true;
        }
    }
}
