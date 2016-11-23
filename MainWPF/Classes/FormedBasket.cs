using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MainWPF
{
    public class FormedBasket : ModelViewContext
    {
        public FormedBasket()
        {
        }

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
            {
                name = value;
                ClearError("Name");
            }
        }

        private bool isactive;
        public bool IsActive
        {
            get
            { return isactive; }
            set
            { isactive = value; }
        }

        private bool isUrgent;
        public bool IsUrgent
        {
            get
            { return isUrgent; }
            set
            { isUrgent = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private int lastuserid;
        public int LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        List<FormedBasket_Item> foresbasketItems;
        public List<FormedBasket_Item> FormedBasketItems
        {
            get
            {
                if (foresbasketItems == null) foresbasketItems = new List<FormedBasket_Item>();
                return foresbasketItems;
            }
            set { foresbasketItems = value; }
        }

        List<Sector> relatedSectors;
        public List<Sector> RelatedSectors
        {
            get
            {
                if (relatedSectors == null) relatedSectors = new List<Sector>();
                return relatedSectors;
            }
            set { relatedSectors = value; }
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(Name))
            {
                isValid = false;
                this.SetError("Name", "يجب إدخال الاسم");
            }
            if (FormedBasketItems.Count == 0)
            {
                isValid = false;
                this.SetError("FormedBasketItems", "يجب اختيار المواد المتضمنة بهذه المادة");
            }
            if (RelatedSectors.Count == 0)
            {
                isValid = false;
                this.SetError("RelatedSectors", "يجب اختيار القطاعات المرتبطة بالسلة المشكلة");
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

        public static bool InsertData(FormedBasket x)
        {
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_FormedBasket"
                , new SqlParameter("@Id", System.Data.SqlDbType.Int)
                , new SqlParameter("@Name", x.Name)
                , new SqlParameter("@IsActive", x.IsActive)
                , new SqlParameter("@IsUrgent", x.IsUrgent)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));

            foreach (var item in x.FormedBasketItems)
            {
                if (item.FormedBasket == null)
                {
                    item.FormedBasket = x;
                    FormedBasket_Item.InsertData(item);
                }
            }
            if (x.id.HasValue)
                FormedBasket_Sector.SaveFormedBasketSectors(x);
            return x.Id.HasValue;
        }
        public static bool UpdateData(FormedBasket x)
        {
            BaseDataBase._NonQuery("delete from FormedBasket_Item where FormedBasket_Item.FormedBasketID = " + x.Id);
            bool b = BaseDataBase._StoredProcedure("sp_Update_FormedBasket"
                    , new SqlParameter("@Id", x.Id)
                    , new SqlParameter("@Name", x.Name)
                    , new SqlParameter("@IsActive", x.IsActive)
                    , new SqlParameter("@IsUrgent", x.IsUrgent)
                    , new SqlParameter("@Notes", x.Notes)
                    , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            foreach (var item in x.FormedBasketItems)
            {
                if (item.FormedBasket == null)
                    item.FormedBasket = x;
                FormedBasket_Item.InsertData(item);
            }
            if (x.id.HasValue)
                FormedBasket_Sector.SaveFormedBasketSectors(x);
            return b;
        }
        public static bool DeleteData(FormedBasket x)
        {
            if (x.id.HasValue)
                FormedBasket_Sector.DeleteFormedBasketSectors(x);
            return BaseDataBase._StoredProcedure("sp_Delete_FormedBasket"
                    , new SqlParameter("@Id", x.Id));
        }
        public static FormedBasket GetItemByID(int id)
        {
            FormedBasket x = new FormedBasket();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_FormedBasket", con);
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
                    if (!(rd["IsUrgent"] is DBNull))
                        x.IsUrgent = bool.Parse(rd["IsUrgent"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());

                    x.FormedBasketItems = FormedBasket_Item.GetAllBasket_ItemByBasket(x);

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

        internal static List<FormedBasket> GetFormedBasketByFamilyID(int familyID)
        {
            List<FormedBasket> xx = new List<FormedBasket>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_FormedBasketByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@FamilyID", familyID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    FormedBasket x = new FormedBasket();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (!(rd["IsActive"] is DBNull))
                        x.IsActive = bool.Parse(rd["IsActive"].ToString());
                    if (!(rd["IsUrgent"] is DBNull))
                        x.IsUrgent = bool.Parse(rd["IsUrgent"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.FormedBasketItems = FormedBasket_Item.GetAllBasket_ItemByBasket(x);
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

        public static List<FormedBasket> AllItems
        {
            get
            {
                List<FormedBasket> xx = new List<FormedBasket>();
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_Get_All_FormedBasket", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        FormedBasket x = new FormedBasket();

                        if (!(rd["Id"] is DBNull))
                            x.Id = int.Parse(rd["Id"].ToString());
                        x.Name = rd["Name"].ToString();
                        if (!(rd["IsActive"] is DBNull))
                            x.IsActive = bool.Parse(rd["IsActive"].ToString());
                        if (!(rd["IsUrgent"] is DBNull))
                            x.IsUrgent = bool.Parse(rd["IsUrgent"].ToString());
                        x.Notes = rd["Notes"].ToString();
                        if (!(rd["LastUserID"] is DBNull))
                            x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                        x.FormedBasketItems = FormedBasket_Item.GetAllBasket_ItemByBasket(x);
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
        public static List<FormedBasket> ActiveItems
        {
            get
            {
                List<FormedBasket> xx = new List<FormedBasket>();
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_Get_All_FormedBasket_Active", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        FormedBasket x = new FormedBasket();

                        if (!(rd["Id"] is DBNull))
                            x.Id = int.Parse(rd["Id"].ToString());
                        x.Name = rd["Name"].ToString();
                        if (!(rd["IsActive"] is DBNull))
                            x.IsActive = bool.Parse(rd["IsActive"].ToString());
                        if (!(rd["IsUrgent"] is DBNull))
                            x.IsUrgent = bool.Parse(rd["IsUrgent"].ToString());
                        x.Notes = rd["Notes"].ToString();
                        if (!(rd["LastUserID"] is DBNull))
                            x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                        x.FormedBasketItems = FormedBasket_Item.GetAllBasket_ItemByBasket(x);
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
}