using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class ExternalCourse
    {
        public enum ExternalCoursePersonType { Parent, Orphan, Guardian }


        private int? externalcourseid;
        public int? ExternalCourseID
        {
            get
            { return externalcourseid; }
            set
            { externalcourseid = value; }
        }



        private string externalcoursename;
        public string ExternalCourseName
        {
            get
            { return externalcoursename; }
            set
            { externalcoursename = value; }
        }



        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            { date = value; }
        }



        private int? personid;
        public int? PersonID
        {
            get
            { return personid; }
            set
            { personid = value; }
        }



        private ExternalCoursePersonType type;
        public ExternalCoursePersonType Type
        {
            get
            { return type; }
            set
            { type = value; }
        }



        private string supportcourse;
        public string SupportCourse
        {
            get
            { return supportcourse; }
            set
            { supportcourse = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }


        public static ExternalCourse GetExternalCourseByID(int id)
        {
            ExternalCourse x = new ExternalCourse();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetExternalCourserByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ExternalCourseID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ExternalCourseID"] is DBNull))
                        x.ExternalCourseID = System.Int32.Parse(rd["ExternalCourseID"].ToString());
                    x.ExternalCourseName = rd["ExternalCourseName"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["PersonID"] is DBNull))
                        x.PersonID = System.Int32.Parse(rd["PersonID"].ToString());
                    x.Type = (ExternalCoursePersonType)rd["Type"];
                    x.SupportCourse = rd["SupportCourse"].ToString();
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
        public static List<ExternalCourse> GetExternalCourseAllByPersonID(int? PersonID, ExternalCoursePersonType PersonType)
        {
            List<ExternalCourse> b = new List<ExternalCourse>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetExternalCourseAllByPersonID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@PersonID", PersonID);
            SqlParameter pr2 = new SqlParameter("@PersonType", PersonType);
            com.Parameters.Add(pr1);
            com.Parameters.Add(pr2);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetExternalCourseByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }
    }
}
