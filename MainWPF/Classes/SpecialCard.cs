using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class SpecialCard : ModelViewContext
    {
        private int? id;
        public int? ID
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
        public static bool InsertData(SpecialCard x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_SpecialCard"
            , new SqlParameter("@ID", System.Data.SqlDbType.Int)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@IsActive", x.IsActive)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            return x.ID.HasValue;
        }
        public static bool UpdateData(SpecialCard x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_SpecialCard"
            , new SqlParameter("@ID", x.ID)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@IsActive", x.IsActive)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name))
            {
                isValid = false;
                this.SetError("Name", "يجب إدخال اسم البطاقة");
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

        public static bool DeleteData(SpecialCard x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_SpecialCard"
            , new SqlParameter("@ID", x.ID));
        }
        public static SpecialCard GetSpecialCardByID(int id)
        {
            SpecialCard x = new SpecialCard();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_SpecialCard", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
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

        internal bool CanRemove()
        {
           var r= BaseDataBase._Scalar("select count(*) from SpecialCardSource where SpecialCardID = " + ID);
            if (!string.IsNullOrEmpty(r) && int.Parse(r) > 0)
                return false;
            return true;
        }

        public static List<SpecialCard> GetAllSpecialCard()
        {
            List<SpecialCard> xx = new List<SpecialCard>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_SpecialCard", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    SpecialCard x = new SpecialCard();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
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