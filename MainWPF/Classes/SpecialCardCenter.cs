using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class SpecialCardCenter : ModelViewContext
    {
        private int? id;
        public int? Id
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        private bool isactive;
        public bool IsActive
        {
            get
            { return isactive; }
            set
            { isactive = value; }
        }

        private int? lastuserid;
        public int? LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
            {
                isValid = false;
                this.SetError("Name", "يجب إدخال اسم المركز");
            }
            if (!isValid)
            {
                string s = "";
                foreach (var item in errors)
                    s += item.Value + "\n";
                MyMessageBox.Show(s);
            }
            return isValid;
        }
        internal bool CanRemove()
        {
            var r = BaseDataBase._Scalar("select count(*) from SpecialCardSource where CenterID = " + id);
            if (!string.IsNullOrEmpty(r) && int.Parse(r) > 0)
                return false;
            return true;
        }
        public static bool InsertData(SpecialCardCenter x)
        {
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_SpecialCardCenter"
            , new SqlParameter("@Id", System.Data.SqlDbType.Int)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@IsActive", x.IsActive)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            return x.Id.HasValue;
        }
        public static bool UpdateData(SpecialCardCenter x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_SpecialCardCenter"
            , new SqlParameter("@Id", x.Id)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@IsActive", x.IsActive)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(SpecialCardCenter x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_SpecialCardCenter"
            , new SqlParameter("@Id", x.Id));
        }
        public static SpecialCardCenter GetSpecialCardCenterByID(int id)
        {
            SpecialCardCenter x = new SpecialCardCenter();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_SpecialCardCenter", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (!(rd["IsActive"] is DBNull))
                        x.IsActive = bool.Parse(rd["IsActive"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
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
        public static List<SpecialCardCenter> GetAllSpecialCardCenter()
        {
            List<SpecialCardCenter> xx = new List<SpecialCardCenter>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_SpecialCardCenter", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    SpecialCardCenter x = new SpecialCardCenter();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (!(rd["IsActive"] is DBNull))
                        x.IsActive = bool.Parse(rd["IsActive"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    xx.Add(x);
                }
                rd.Close();
            }
            catch
            {
                xx = null;
            }
            finally
            {
                con.Close();
            }
            return xx;
        }
    }
}