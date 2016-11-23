using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainWPF
{
    class QualificationType
    {

        private int qualificationid;
        public int QualificationID
        {
            get
            { return qualificationid; }
            set
            { qualificationid = value; }
        }



        private string qualificationname;
        public string QualificationName
        {
            get
            { return qualificationname; }
            set
            { qualificationname = value; }
        }


        public bool InsertQualificationData()
        {
            return BaseDataBase._StoredProcedure("sp_Add2Qualification"
                , new SqlParameter("@QualificationName", QualificationName));
        }
        public bool UpdateQualificationData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateQualification"
                , new SqlParameter("@QualificationID", QualificationID)
                , new SqlParameter("@QualificationName", QualificationName));
        }



        public static List<QualificationType> AllQualification
        { get { return QualificationType.GetAllQualifications(); } }
        private static List<QualificationType> GetAllQualifications()
        {
            List<QualificationType> x = new List<QualificationType>();

            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAllQualifications", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    QualificationType qt = new QualificationType();
                    qt.QualificationID = rd.GetInt32(0);
                    qt.QualificationName = rd.GetString(1);
                    x.Add(qt);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message);
                x = null;
            }
            finally
            {
                con.Close();
            }

            return x;
        }

        public static string GetQualificationTypeByID(int id)
        {
            string x = "";
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetQualificationByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    x = rd["QualificationName"].ToString();
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
