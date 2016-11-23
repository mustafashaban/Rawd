using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class SystemProperties
    {
        public enum Property { Owner, NextOrderGap, Slogan, NextOrderSpecialFamily }

        public Property PropertyName { get; set; }
        public string PropertyValue { get; set; }


        public static bool UpdateData(Property p, string value)
        {
            return BaseDataBase._StoredProcedure("sp_Update_SystemProperties",
                 new SqlParameter("@PropertyName", p.ToString()),
                 new SqlParameter("@PropertyValue", value),
                 new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }


        public static string GetSystemPropertyValue(Property p)
        {
            string value;
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_SystemPropertyValue", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@PropertyName", p.ToString());
            com.Parameters.Add(pr1);
            try
            {
                con.Open();
                value = com.ExecuteScalar().ToString();
            }
            catch
            {
                value = null;
            }
            finally
            {
                con.Close();
            }
            return value;
        }


    }

}
