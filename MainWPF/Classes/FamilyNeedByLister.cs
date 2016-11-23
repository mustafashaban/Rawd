using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class FamilyNeedByLister : ModelViewContext
    {
        int? id;
        public int? ID
        {
            get { return id; }
            set { id = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        int count = 1;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public static FamilyNeedByLister GetFamilyNeedByListerByID(int id)
        {
            FamilyNeedByLister x = new FamilyNeedByLister();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyNeedByListerByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("ID", id));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    x.Name = rd["Name"].ToString();
                }
                rd.Close();
            }
            catch
            {
                x = new FamilyNeedByLister();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<FamilyNeedByLister> GetFamilyNeedByListerAll
        {
            get
            {
                List<FamilyNeedByLister> fnl = new List<FamilyNeedByLister>();
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_GetFamilyNeedByListerAll", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        FamilyNeedByLister x = new FamilyNeedByLister();
                        if (!(rd["ID"] is DBNull))
                            x.ID = System.Int32.Parse(rd["ID"].ToString());
                        x.Name = rd["Name"].ToString();

                        fnl.Add(x);
                    }
                    rd.Close();
                }
                catch
                {
                    fnl = new List<FamilyNeedByLister>();
                }
                finally
                {
                    con.Close();
                }
                return fnl;
            }
        }
    }
}