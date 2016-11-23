using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MainWPF
{
    public class Item : ModelViewContext
    {
        public Item(bool IsBasket = false)
        {
            this.IsBasket = IsBasket;
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

        private bool isbasket;
        public bool IsBasket
        {
            get
            { return isbasket; }
            private set
            { isbasket = value; }
        }

        private string source;
        public string Source
        {
            get
            { return source; }
            set
            {
                source = value;
                ClearError("Source");
            }
        }

        private string barcode;
        public string Barcode
        {
            get
            { return barcode; }
            set
            { barcode = value; }
        }

        private string itemtype;
        public string ItemType
        {
            get
            { return itemtype; }
            set
            {
                itemtype = value;
                ClearError("ItemType");
            }
        }

        private string description;
        public string Description
        {
            get
            { return description; }
            set
            { description = value; }
        }

        private string standardunit;
        public string StandardUnit
        {
            get
            { return standardunit; }
            set
            {
                standardunit = value;
                ClearError("StandardUnit");
            }
        }

        private string unit2;
        public string Unit2
        {
            get
            { return unit2; }
            set
            { unit2 = value; }
        }

        private double? unit2convert;
        public double? Unit2Convert
        {
            get
            { return unit2convert; }
            set
            {
                unit2convert = value;
                ClearError("Unit2Convert");
            }
        }

        private string unit3;
        public string Unit3
        {
            get
            { return unit3; }
            set
            { unit3 = value; }
        }

        private double? unit3convert;
        public double? Unit3Convert
        {
            get
            { return unit3convert; }
            set
            {
                unit3convert = value;
                ClearError("Unit3Convert");
            }
        }

        private double? minimumquantity;
        public double? MinimumQuantity
        {
            get
            { return minimumquantity; }
            set
            { minimumquantity = value; }
        }

        private double? maximumquantity;
        public double? MaximumQuantity
        {
            get
            { return maximumquantity; }
            set
            { maximumquantity = value; }
        }

        private double? maxquantityperorder;
        public double? MaxQuantityPerOrder
        {
            get
            { return maxquantityperorder; }
            set
            { maxquantityperorder = value; }
        }

        private double? maxquantityperday;
        public double? MaxQuantityPerDay
        {
            get
            { return maxquantityperday; }
            set
            { maxquantityperday = value; }
        }
        private double? maxQuantityPerFamily;
        public double? MaxQuantityPerFamily
        {
            get
            { return maxQuantityPerFamily; }
            set
            { maxQuantityPerFamily = value; }
        }

        private double? warningquantity;
        public double? WarningQuantity
        {
            get
            { return warningquantity; }
            set
            { warningquantity = value; }
        }

        private double? weight;
        public double? Weight
        {
            get
            { return weight; }
            set
            { weight = value; }
        }

        private string defaultlocation;
        public string DefaultLocation
        {
            get
            { return defaultlocation; }
            set
            { defaultlocation = value; }
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

        public List<string> AvailableUnits
        {
            get
            {
                List<string> units = new List<string>();
                units.Add(StandardUnit);
                if (!string.IsNullOrEmpty(Unit2))
                    units.Add(Unit2);
                if (!string.IsNullOrEmpty(Unit3))
                    units.Add(Unit3);
                return units;
            }
        }

        List<Basket_Item> basketItems;
        public List<Basket_Item> BasketItems
        {
            get
            {
                if (basketItems == null) basketItems = new List<Basket_Item>();
                return basketItems;
            }
            set { basketItems = value; }
        }

        double currentQuantity;
        public double CurrentQuantity
        {
            get
            { return currentQuantity == 0 ? 1000000 : currentQuantity; }
            set
            {
                currentQuantity = value;
                NotifyPropertyChanged("CurrentQuantity");
            }
        }


        //IsSelected property for check if the current item is selected for VoucherCritera Issues
        bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }


        public void GetBasketItems()
        {
            this.BasketItems = Basket_Item.GetAllBasket_ItemByBasket(this);
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
            if (string.IsNullOrEmpty(ItemType))
            {
                isValid = false;
                this.SetError("ItemType", "يجب ادخال نوع المادة");
            }
            if (string.IsNullOrEmpty(Source))
            {
                isValid = false;
                this.SetError("Source", "يجب ادخال جهة المادة");
            }
            if (string.IsNullOrEmpty(StandardUnit))
            {
                isValid = false;
                this.SetError("StandardUnit", "يجب ادخال الوحدة القياسية");
            }
            if (!string.IsNullOrEmpty(unit2) && !Unit2Convert.HasValue)
            {
                isValid = false;
                this.SetError("Unit2Convert", "يجب ادخال قيمة التحويل");
            }
            if (!string.IsNullOrEmpty(unit3) && !Unit3Convert.HasValue)
            {
                isValid = false;
                this.SetError("Unit3Convert", "يجب ادخال قيمة التحويل");
            }
            if (IsBasket && BasketItems.Count == 0)
            {
                isValid = false;
                this.SetError("BasketItems", "يجب اختيار المواد المتضمنة بهذه المادة");
            }
            if (MinimumQuantity.HasValue)
            {
                if (MaximumQuantity.HasValue)
                {
                    if (MaximumQuantity.Value < MinimumQuantity.Value)
                    {
                        isValid = false;
                        this.SetError("MaximumQuantity", "الحد الاعلى يجب ان يكون اصغر او يساوي الحد الادنى");
                    }
                    if (WarningQuantity.HasValue && (WarningQuantity.Value < MinimumQuantity.Value || WarningQuantity.Value > MaximumQuantity.Value))
                    {
                        isValid = false;
                        this.SetError("WarningQuantity", "حد التنبيه يجب ان يكون اصغر من الحد الاعلى واكبر من الحد الادنى");
                    }
                }
            }
            else
            {
                if (MaximumQuantity.HasValue)
                {
                    if (WarningQuantity.HasValue && (WarningQuantity.Value > MaximumQuantity.Value))
                    {
                        isValid = false;
                        this.SetError("WarningQuantity", "حد التنبيه يجب ان يكون اصغر من الحد الاعلى");
                    }
                }
            }
            if (MaxQuantityPerOrder.HasValue)
            {
                if (MaxQuantityPerFamily.HasValue && MaxQuantityPerFamily.Value < MaxQuantityPerOrder.Value)
                {
                    isValid = false;
                    this.SetError("MaxQuantityPerFamily", "الحد الاعلى للعائلة يجب ان يكون اكبر او يساوي الحد الاعلى في الامر");
                }
                if (MaxQuantityPerDay.HasValue && MaxQuantityPerDay.Value < MaxQuantityPerOrder.Value)
                {
                    isValid = false;
                    this.SetError("MaxQuantityPerDay", "الحد الاعلى في اليوم يجب ان اكبر اصغر او يساوي الحد الاعلى في الامر");
                }
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


        public static bool InsertData(Item x)
        {
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_Item"
                , new SqlParameter("@Id", System.Data.SqlDbType.Int)
                , new SqlParameter("@Name", x.Name)
                , new SqlParameter("@IsActive", x.IsActive)
                , new SqlParameter("@IsBasket", x.IsBasket)
                , new SqlParameter("@Source", x.Source)
                , new SqlParameter("@Barcode", x.Barcode)
                , new SqlParameter("@ItemType", x.ItemType)
                , new SqlParameter("@Description", x.Description)
                , new SqlParameter("@StandardUnit", x.StandardUnit)
                , new SqlParameter("@Unit2", x.Unit2)
                , new SqlParameter("@Unit2Convert", x.Unit2Convert)
                , new SqlParameter("@Unit3", x.Unit3)
                , new SqlParameter("@Unit3Convert", x.Unit3Convert)
                , new SqlParameter("@MinimumQuantity", x.MinimumQuantity)
                , new SqlParameter("@MaximumQuantity", x.MaximumQuantity)
                , new SqlParameter("@MaxQuantityPerOrder", x.MaxQuantityPerOrder)
                , new SqlParameter("@MaxQuantityPerFamily", x.MaxQuantityPerFamily)
                , new SqlParameter("@MaxQuantityPerDay", x.MaxQuantityPerDay)
                , new SqlParameter("@WarningQuantity", x.WarningQuantity)
                , new SqlParameter("@Weight", x.Weight)
                , new SqlParameter("@DefaultLocation", x.DefaultLocation)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            return x.Id.HasValue;
        }
        public static bool UpdateData(Item x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Item"
                    , new SqlParameter("@Id", x.Id)
                    , new SqlParameter("@Name", x.Name)
                    , new SqlParameter("@IsActive", x.IsActive)
                    , new SqlParameter("@IsBasket", x.IsBasket)
                    , new SqlParameter("@Source", x.Source)
                    , new SqlParameter("@Barcode", x.Barcode)
                    , new SqlParameter("@ItemType", x.ItemType)
                    , new SqlParameter("@Description", x.Description)
                    , new SqlParameter("@StandardUnit", x.StandardUnit)
                    , new SqlParameter("@Unit2", x.Unit2)
                    , new SqlParameter("@Unit2Convert", x.Unit2Convert)
                    , new SqlParameter("@Unit3", x.Unit3)
                    , new SqlParameter("@Unit3Convert", x.Unit3Convert)
                    , new SqlParameter("@MinimumQuantity", x.MinimumQuantity)
                    , new SqlParameter("@MaximumQuantity", x.MaximumQuantity)
                    , new SqlParameter("@MaxQuantityPerOrder", x.MaxQuantityPerOrder)
                    , new SqlParameter("@MaxQuantityPerFamily", x.MaxQuantityPerFamily)
                    , new SqlParameter("@MaxQuantityPerDay", x.MaxQuantityPerDay)
                    , new SqlParameter("@WarningQuantity", x.WarningQuantity)
                    , new SqlParameter("@Weight", x.Weight)
                    , new SqlParameter("@DefaultLocation", x.DefaultLocation)
                    , new SqlParameter("@Notes", x.Notes)
                    , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(Item x)
        {
            Item.UpdateData(x);
            return BaseDataBase._StoredProcedure("sp_Delete_Item"
                    , new SqlParameter("@Id", x.Id));
        }
        public static Item GetItemByID(int id)
        {
            Item x = new Item();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Item", con);
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
                    if (!(rd["IsBasket"] is DBNull))
                        x.IsBasket = bool.Parse(rd["IsBasket"].ToString());
                    x.Source = rd["Source"].ToString();
                    x.Barcode = rd["Barcode"].ToString();
                    x.ItemType = rd["ItemType"].ToString();
                    x.Description = rd["Description"].ToString();
                    x.StandardUnit = rd["StandardUnit"].ToString();
                    x.Unit2 = rd["Unit2"].ToString();
                    if (!(rd["Unit2Convert"] is DBNull))
                        x.Unit2Convert = double.Parse(rd["Unit2Convert"].ToString());
                    x.Unit3 = rd["Unit3"].ToString();
                    if (!(rd["Unit3Convert"] is DBNull))
                        x.Unit3Convert = double.Parse(rd["Unit3Convert"].ToString());
                    if (!(rd["MinimumQuantity"] is DBNull))
                        x.MinimumQuantity = double.Parse(rd["MinimumQuantity"].ToString());
                    if (!(rd["MaximumQuantity"] is DBNull))
                        x.MaximumQuantity = double.Parse(rd["MaximumQuantity"].ToString());
                    if (!(rd["MaxQuantityPerOrder"] is DBNull))
                        x.MaxQuantityPerOrder = double.Parse(rd["MaxQuantityPerOrder"].ToString());
                    if (!(rd["MaxQuantityPerFamily"] is DBNull))
                        x.MaxQuantityPerFamily = double.Parse(rd["MaxQuantityPerFamily"].ToString());
                    if (!(rd["MaxQuantityPerDay"] is DBNull))
                        x.MaxQuantityPerDay = double.Parse(rd["MaxQuantityPerDay"].ToString());
                    if (!(rd["WarningQuantity"] is DBNull))
                        x.WarningQuantity = double.Parse(rd["WarningQuantity"].ToString());
                    if (!(rd["Weight"] is DBNull))
                        x.Weight = double.Parse(rd["Weight"].ToString());
                    x.DefaultLocation = rd["DefaultLocation"].ToString();
                    x.Notes = rd["Notes"].ToString();
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
        public static Item GetItemByID(int ItemID, int InventoryId)
        {
            Item x = new Item();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Item_InventoryID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@ItemID", ItemID));
            com.Parameters.Add(new SqlParameter("@InventoryID", InventoryId));
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
                    if (!(rd["IsBasket"] is DBNull))
                        x.IsBasket = bool.Parse(rd["IsBasket"].ToString());
                    x.Source = rd["Source"].ToString();
                    x.Barcode = rd["Barcode"].ToString();
                    x.ItemType = rd["ItemType"].ToString();
                    x.Description = rd["Description"].ToString();
                    x.StandardUnit = rd["StandardUnit"].ToString();
                    x.Unit2 = rd["Unit2"].ToString();
                    if (!(rd["Unit2Convert"] is DBNull))
                        x.Unit2Convert = double.Parse(rd["Unit2Convert"].ToString());
                    x.Unit3 = rd["Unit3"].ToString();
                    if (!(rd["Unit3Convert"] is DBNull))
                        x.Unit3Convert = double.Parse(rd["Unit3Convert"].ToString());
                    if (!(rd["MinimumQuantity"] is DBNull))
                        x.MinimumQuantity = double.Parse(rd["MinimumQuantity"].ToString());
                    if (!(rd["MaximumQuantity"] is DBNull))
                        x.MaximumQuantity = double.Parse(rd["MaximumQuantity"].ToString());
                    if (!(rd["MaxQuantityPerOrder"] is DBNull))
                        x.MaxQuantityPerOrder = double.Parse(rd["MaxQuantityPerOrder"].ToString());
                    if (!(rd["MaxQuantityPerFamily"] is DBNull))
                        x.MaxQuantityPerFamily = double.Parse(rd["MaxQuantityPerFamily"].ToString());
                    if (!(rd["MaxQuantityPerDay"] is DBNull))
                        x.MaxQuantityPerDay = double.Parse(rd["MaxQuantityPerDay"].ToString());
                    if (!(rd["WarningQuantity"] is DBNull))
                        x.WarningQuantity = double.Parse(rd["WarningQuantity"].ToString());
                    if (!(rd["Weight"] is DBNull))
                        x.Weight = double.Parse(rd["Weight"].ToString());
                    if (!(rd["CurrentQuantity"] is DBNull))
                        x.CurrentQuantity = double.Parse(rd["CurrentQuantity"].ToString());
                    x.DefaultLocation = rd["DefaultLocation"].ToString();
                    x.Notes = rd["Notes"].ToString();
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



        public static List<Item> AllItems
        {
            get
            {
                List<Item> xx = new List<Item>();
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_Get_All_Item", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        Item x = new Item();

                        if (!(rd["Id"] is DBNull))
                            x.Id = int.Parse(rd["Id"].ToString());
                        x.Name = rd["Name"].ToString();
                        if (!(rd["IsActive"] is DBNull))
                            x.IsActive = bool.Parse(rd["IsActive"].ToString());
                        if (!(rd["IsBasket"] is DBNull))
                            x.IsBasket = bool.Parse(rd["IsBasket"].ToString());
                        x.Source = rd["Source"].ToString();
                        x.Barcode = rd["Barcode"].ToString();
                        x.ItemType = rd["ItemType"].ToString();
                        x.Description = rd["Description"].ToString();
                        x.StandardUnit = rd["StandardUnit"].ToString();
                        x.Unit2 = rd["Unit2"].ToString();
                        if (!(rd["Unit2Convert"] is DBNull))
                            x.Unit2Convert = double.Parse(rd["Unit2Convert"].ToString());
                        x.Unit3 = rd["Unit3"].ToString();
                        if (!(rd["Unit3Convert"] is DBNull))
                            x.Unit3Convert = double.Parse(rd["Unit3Convert"].ToString());
                        if (!(rd["MinimumQuantity"] is DBNull))
                            x.MinimumQuantity = double.Parse(rd["MinimumQuantity"].ToString());
                        if (!(rd["MaximumQuantity"] is DBNull))
                            x.MaximumQuantity = double.Parse(rd["MaximumQuantity"].ToString());
                        if (!(rd["MaxQuantityPerOrder"] is DBNull))
                            x.MaxQuantityPerOrder = double.Parse(rd["MaxQuantityPerOrder"].ToString());
                        if (!(rd["MaxQuantityPerFamily"] is DBNull))
                            x.MaxQuantityPerFamily = double.Parse(rd["MaxQuantityPerFamily"].ToString());
                        if (!(rd["MaxQuantityPerDay"] is DBNull))
                            x.MaxQuantityPerDay = double.Parse(rd["MaxQuantityPerDay"].ToString());
                        if (!(rd["WarningQuantity"] is DBNull))
                            x.WarningQuantity = double.Parse(rd["WarningQuantity"].ToString());
                        if (!(rd["Weight"] is DBNull))
                            x.Weight = double.Parse(rd["Weight"].ToString());
                        x.DefaultLocation = rd["DefaultLocation"].ToString();
                        x.Notes = rd["Notes"].ToString();
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
        public static List<Item> ActiveItems
        {
            get
            {
                return (from x in Item.AllItems where x.IsActive select x).ToList();
            }
        }

        public async static Task<List<Item>> GetAllItemsByInventory(int InventoryID)
        {
            List<Item> xx = new List<Item>();
            await Task.Run(() =>
            {
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_Get_All_ItemByInventory", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@InventoryID", InventoryID));
                try
                {
                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        Item x = new Item();

                        if (!(rd["Id"] is DBNull))
                            x.Id = int.Parse(rd["Id"].ToString());
                        x.Name = rd["Name"].ToString();
                        if (!(rd["IsActive"] is DBNull))
                            x.IsActive = bool.Parse(rd["IsActive"].ToString());
                        if (!(rd["IsBasket"] is DBNull))
                            x.IsBasket = bool.Parse(rd["IsBasket"].ToString());
                        x.Source = rd["Source"].ToString();
                        x.Barcode = rd["Barcode"].ToString();
                        x.ItemType = rd["ItemType"].ToString();
                        x.Description = rd["Description"].ToString();
                        x.StandardUnit = rd["StandardUnit"].ToString();
                        x.Unit2 = rd["Unit2"].ToString();
                        if (!(rd["Unit2Convert"] is DBNull))
                            x.Unit2Convert = double.Parse(rd["Unit2Convert"].ToString());
                        x.Unit3 = rd["Unit3"].ToString();
                        if (!(rd["Unit3Convert"] is DBNull))
                            x.Unit3Convert = double.Parse(rd["Unit3Convert"].ToString());
                        if (!(rd["MinimumQuantity"] is DBNull))
                            x.MinimumQuantity = double.Parse(rd["MinimumQuantity"].ToString());
                        if (!(rd["MaximumQuantity"] is DBNull))
                            x.MaximumQuantity = double.Parse(rd["MaximumQuantity"].ToString());
                        if (!(rd["MaxQuantityPerOrder"] is DBNull))
                            x.MaxQuantityPerOrder = double.Parse(rd["MaxQuantityPerOrder"].ToString());
                        if (!(rd["MaxQuantityPerFamily"] is DBNull))
                            x.MaxQuantityPerFamily = double.Parse(rd["MaxQuantityPerFamily"].ToString());
                        if (!(rd["MaxQuantityPerDay"] is DBNull))
                            x.MaxQuantityPerDay = double.Parse(rd["MaxQuantityPerDay"].ToString());
                        if (!(rd["WarningQuantity"] is DBNull))
                            x.WarningQuantity = double.Parse(rd["WarningQuantity"].ToString());
                        if (!(rd["Weight"] is DBNull))
                            x.Weight = double.Parse(rd["Weight"].ToString());
                        if (!(rd["CurrentQuantity"] is DBNull))
                            x.CurrentQuantity = double.Parse(rd["CurrentQuantity"].ToString());
                        x.DefaultLocation = rd["DefaultLocation"].ToString();
                        x.Notes = rd["Notes"].ToString();
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
            });
            return xx;
        }
        //public async static Task<List<Item>> GetAllFormedBasket(int InventoryID)
        //{
        //    List<Item> xx = new List<Item>();
        //    await Task.Run(() =>
        //    {
        //        SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
        //        SqlCommand com = new SqlCommand("sp_Get_All_FormedBasket", con);
        //        com.CommandType = System.Data.CommandType.StoredProcedure;
        //        com.Parameters.Add(new SqlParameter("@InventoryID", InventoryID));
        //        try
        //        {
        //            con.Open();
        //            SqlDataReader rd = com.ExecuteReader();
        //            while (rd.Read())
        //            {
        //                Item x = new Item();

        //                if (!(rd["Id"] is DBNull))
        //                    x.Id = int.Parse(rd["Id"].ToString());
        //                x.Name = rd["Name"].ToString();
        //                if (!(rd["IsActive"] is DBNull))
        //                    x.IsActive = bool.Parse(rd["IsActive"].ToString());
        //                if (!(rd["IsBasket"] is DBNull))
        //                    x.IsBasket = bool.Parse(rd["IsBasket"].ToString());
        //                x.Source = rd["Source"].ToString();
        //                x.Barcode = rd["Barcode"].ToString();
        //                x.ItemType = rd["ItemType"].ToString();
        //                x.Description = rd["Description"].ToString();
        //                x.StandardUnit = rd["StandardUnit"].ToString();
        //                x.Unit2 = rd["Unit2"].ToString();
        //                if (!(rd["Unit2Convert"] is DBNull))
        //                    x.Unit2Convert = double.Parse(rd["Unit2Convert"].ToString());
        //                x.Unit3 = rd["Unit3"].ToString();
        //                if (!(rd["Unit3Convert"] is DBNull))
        //                    x.Unit3Convert = double.Parse(rd["Unit3Convert"].ToString());
        //                if (!(rd["MinimumQuantity"] is DBNull))
        //                    x.MinimumQuantity = double.Parse(rd["MinimumQuantity"].ToString());
        //                if (!(rd["MaximumQuantity"] is DBNull))
        //                    x.MaximumQuantity = double.Parse(rd["MaximumQuantity"].ToString());
        //                if (!(rd["MaxQuantityPerOrder"] is DBNull))
        //                    x.MaxQuantityPerOrder = double.Parse(rd["MaxQuantityPerOrder"].ToString());
        //                if (!(rd["MaxQuantityPerFamily"] is DBNull))
        //                    x.MaxQuantityPerFamily = double.Parse(rd["MaxQuantityPerFamily"].ToString());
        //                if (!(rd["MaxQuantityPerDay"] is DBNull))
        //                    x.MaxQuantityPerDay = double.Parse(rd["MaxQuantityPerDay"].ToString());
        //                if (!(rd["WarningQuantity"] is DBNull))
        //                    x.WarningQuantity = double.Parse(rd["WarningQuantity"].ToString());
        //                if (!(rd["Weight"] is DBNull))
        //                    x.Weight = double.Parse(rd["Weight"].ToString());
        //                if (!(rd["CurrentQuantity"] is DBNull))
        //                    x.CurrentQuantity = double.Parse(rd["CurrentQuantity"].ToString());
        //                x.DefaultLocation = rd["DefaultLocation"].ToString();
        //                x.Notes = rd["Notes"].ToString();
        //                if (!(rd["LastUserID"] is DBNull))
        //                    x.LastUserID = int.Parse(rd["LastUserID"].ToString());
        //                xx.Add(x);
        //            }
        //            rd.Close();
        //        }
        //        catch
        //        {
        //            xx = null;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    });
        //    return xx;
        //}


        public static async Task<DataTable> GetAllItemsTalbe()
        {
            return await BaseDataBase._TablingStoredProcedureAsync("sp_Get_All_Item_Table");
        }

        public static List<string> GetDistinctItemType
        {
            get { return BaseDataBase.GetAllStrings("select distinct ItemType from Item where ItemType is not null"); }
        }
        public static List<string> GetDistinctSource
        {
            get { return BaseDataBase.GetAllStrings("select distinct Source from Item where Source is not null"); }
        }
    }
}