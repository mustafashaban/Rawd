using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MainWPF
{
    public class Inventory : ModelViewContext
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
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private bool isactive = false;
        public bool IsActive
        {
            get
            { return isactive; }
            set
            {
                isactive = value;
                NotifyPropertyChanged("IsActive");
            }
        }

        private int lastuserid;
        public int LastUserID
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
            if (string.IsNullOrEmpty(Name))
            {
                SetError("Name", "قيمة اجبارية");
                isValid = false;
            }
            //if (IsActive)
            //{
            //    SetError("Name", "قيمة اجبارية");
            //    isValid = false;
            //}
            if (!isValid)
            {
                string s = "";
                foreach (var item in errors)
                    s += item.Value + "\n";
                MyMessageBox.Show(s);
            }
            return isValid;
        }



        public static bool InsertData(Inventory x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_Inventory"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@Name", x.Name)
                , new SqlParameter("@IsActive", x.IsActive)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            return x.ID.HasValue;
        }
        public static async Task<bool> UpdateData(Inventory x)
        {
            return await BaseDataBase._StoredProcedureAsync("sp_Update_Inventory"
                , new SqlParameter("@ID", x.ID)
                , new SqlParameter("@Name", x.Name)
                , new SqlParameter("@IsActive", x.IsActive)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static async Task<bool> DeleteData(Inventory x)
        {
            await Inventory.UpdateData(x);
            return await BaseDataBase._StoredProcedureAsync("sp_Delete_Inventory"
                         , new SqlParameter("@ID", x.ID));
        }
        internal bool CanRemove()
        {
            //var r = BaseDataBase._Scalar("select count(*) from SpecialCardSource where SpecialCardID = " + ID);
            //if (!string.IsNullOrEmpty(r) && int.Parse(r) > 0)
            //    return false;
            return true;
        }

        public static Inventory GetInventoryByID(int id)
        {
            Inventory x = new Inventory();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Inventory", con);
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
        public static List<Inventory> GetAllInventory()
        {
            List<Inventory> xx = new List<Inventory>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Inventory", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Inventory x = new Inventory();

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
        public static List<Inventory> AllInventories
        {
           get { return GetAllInventory(); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}