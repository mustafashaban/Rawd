using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    class StudySide
    {


        private int studysideid;
        public int StudySideID
        {
            get
            { return studysideid; }
            set
            { studysideid = value; }
        }



        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }



        private string responsibleperson;
        public string ResponsiblePerson
        {
            get
            { return responsibleperson; }
            set
            { responsibleperson = value; }
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


        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }



        

        public bool InsertStudySideData()
        {
            return BaseDataBase._StoredProcedure("sp_Add2StudySide"
                , new SqlParameter("@Name", Name)
                , new SqlParameter("@ResponsiblePerson", ResponsiblePerson)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@Notes", Notes)
                , new SqlParameter("@Address", Address));
        }
        public bool UpdateStudySideData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateStudySide"
                , new SqlParameter("@StudySideID", StudySideID)
                , new SqlParameter("@Name", Name)
                , new SqlParameter("@ResponsiblePerson", ResponsiblePerson)
                , new SqlParameter("@Phone", Phone)
                , new SqlParameter("@Mobile", Mobile)
                , new SqlParameter("@Email", Email)
                , new SqlParameter("@Notes", Notes)
                , new SqlParameter("@Address", Address));
        }
        public bool DeleteStudySideData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromStudySide"
                , new SqlParameter("@StudySideID", StudySideID));
        }

        public static StudySide GetStudySideByID(int id)
        {
            StudySide x = new StudySide();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetStudySideByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@StudySideID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["StudySideID"] is DBNull))
                        x.StudySideID = System.Int32.Parse(rd["StudySideID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.ResponsiblePerson = rd["ResponsiblePerson"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.Address = rd["Address"].ToString();
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
    }
}

