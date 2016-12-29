using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MainWPF
{
    public class Order : ModelViewContext
    {
        private int? id;
        public int? Id
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private int orderCode;
        public int OrderCode
        {
            get
            { return orderCode; }
            set
            { orderCode = value; }
        }

        private int inventoryid;
        public int InventoryID
        {
            get
            { return inventoryid; }
            set
            { inventoryid = value; }
        }

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }

        private int? specialFamilyID;
        public int? SpecialFamilyID
        {
            get
            { return specialFamilyID; }
            set
            { specialFamilyID = value; }
        }
        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }

        private int? type;
        public int? Type
        {
            get
            { return type; }
            set
            { type = value; }
        }
        public string TypeText
        {
            get
            {
                switch (type)
                {
                    case 1:
                        return "امر ادخال";
                    case 2:
                        return "امر اخراج";
                    case 3:
                        return "حصة عائلة";
                    case 4:
                        return "حصة عائلة خاصة";
                    case 5:
                        return "نقل الى مستودع";
                    default:
                        return "";
                }
            }
        }
        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            {
                date = value;
                this.ClearError("Date");
                this.NotifyPropertyChanged("Date");
            }
        }

        private DateTime? nextorderdate;
        public DateTime? NextOrderDate
        {
            get
            { return nextorderdate; }
            set
            {
                nextorderdate = value;
                this.ClearError("NextOrderDate");
                this.NotifyPropertyChanged("NextOrderDate");
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

        private string source;
        public string Source
        {
            get
            { return source; }
            set
            {
                source = value;
                this.ClearError("Source");
                this.NotifyPropertyChanged("Source");
            }
        }

        private string invoiceserial;
        public string InvoiceSerial
        {
            get
            { return invoiceserial; }
            set
            { invoiceserial = value; }
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
            {
                lastuserid = value;
                //Task.Run(() => GetUserName());
            }
        }

        public bool isUrgentOrder;
        public bool IsUrgentOrder
        {
            get
            {
                if (id.HasValue && FamilyID.HasValue && !NextOrderDate.HasValue)
                    return true;
                return isUrgentOrder;
            }
            set { isUrgentOrder = value; }
        }
        private string barCode;
        public string BarCode
        {
            get
            { return barCode; }
            set
            {
                barCode = value;
                NotifyPropertyChanged("BarCode");
            }
        }
        public string InventoryName
        {
            get { return BaseDataBase._Scalar("select Name from Inventory where Id = " + InventoryID); }
        }
        public string LastUserName
        {
            get
            { return User.GetUserAll().Where(x => x.ID == LastUserID).First().Name; }
        }

        List<Order_Item> ois;
        public List<Order_Item> OIs
        {
            get
            {
                if (ois == null) ois = new List<Order_Item>();
                return ois;
            }
            set { ois = value; }
        }
        private List<FamilyNeed_ListerGroup> familyNeeds;
        public List<FamilyNeed_ListerGroup> FamilyNeeds
        {
            get { return familyNeeds; }
            set { familyNeeds = value; }
        }

        internal bool IsValidateMainOrderData()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (!Date.HasValue)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ التسليم");
            }
            if (InventoryID == 0)
            {
                isValid = false;
                this.SetError("InventoryID", "يجب اختيار المستودع");
            }
            if (type == 3 && !IsUrgentOrder && !NextOrderDate.HasValue)
            {
                isValid = false;
                this.SetError("NextOrderDate", "يجب إدخال تاريخ التسليم القادم");
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
        internal bool IsValidateOrderItemsData()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (OIs.Count == 0)
            {
                isValid = false;
                this.SetError("OIs", "يجب ادخال مواد ضمن الامر");
            }
            else if (OIs.Where(x => x.Quantity == 0).Count() > 0)
            {
                isValid = false;
                this.SetError("OIs", "المواد المدخلة تحوي كمية معدومة");
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

        public static bool InsertData(Order x, bool InsertOrderItems = false)
        {
            x.LastUserID = BaseDataBase.CurrentUser.ID.Value;
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_Order"
                , new SqlParameter("@Id", System.Data.SqlDbType.Int)
                , new SqlParameter("@InventoryID", x.InventoryID)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@OrphanID", x.OrphanID)
                , new SqlParameter("@SpecialFamilyID", x.SpecialFamilyID)
                , new SqlParameter("@Type", x.Type)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@NextOrderDate", x.NextOrderDate)
                , new SqlParameter("@Description", x.Description)
                , new SqlParameter("@Source", x.Source)
                , new SqlParameter("@InvoiceSerial", x.InvoiceSerial)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@LastUserID", x.LastUserID));

            if (InsertOrderItems)
            {
                foreach (var item in x.OIs)
                {
                    item.Order = x;
                    Order_Item.InsertData(item);
                }
            }
            x.OrderCode = int.Parse(BaseDataBase._Scalar($"select OrderCode from [Order] where Id = {x.Id}"));
            x.BarCode = BaseDataBase._Scalar($"select BarCode from [Order] where Id = {x.Id}");
            //if (x.Id.HasValue)
            //{
            //    foreach (var item in x.FamilyNeeds)
            //    {
            //        if (item.IsEnsured)
            //            item.OrderID = x.Id;
            //        else item.OrderID = null;
            //        FamilyNeed_ListerGroup.UpdateData(item);
            //    }
            //}
            return x.Id.HasValue;
        }
        public static bool UpdateData(Order x)
        {
            //foreach (var item in x.FamilyNeeds)
            //{
            //    if (item.IsEnsured)
            //        item.OrderID = x.Id;
            //    else item.OrderID = null;
            //    FamilyNeed_ListerGroup.UpdateData(item);
            //}

            x.LastUserID = BaseDataBase.CurrentUser.ID.Value;
            return BaseDataBase._StoredProcedure("sp_Update_Order"
                , new SqlParameter("@Id", x.Id)
                , new SqlParameter("@InventoryID", x.InventoryID)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@SpecialFamilyID", x.SpecialFamilyID)
                , new SqlParameter("@OrphanID", x.OrphanID)
                , new SqlParameter("@Type", x.Type)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@NextOrderDate", x.NextOrderDate)
                , new SqlParameter("@Description", x.Description)
                , new SqlParameter("@Source", x.Source)
                , new SqlParameter("@InvoiceSerial", x.InvoiceSerial)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@LastUserID", x.LastUserID));
        }
        public static bool DeleteData(Order x)
        {
            //foreach (var item in x.FamilyNeeds)
            //{
            //    item.OrderID = null;
            //    FamilyNeed_ListerGroup.UpdateData(item);
            //}
            foreach (var item in x.OIs)
                Order_Item.DeleteData(item);
            return BaseDataBase._StoredProcedure("sp_Delete_Order"
                , new SqlParameter("@Id", x.Id));
        }
        public static Order GetOrderByID(int id)
        {
            Order x = new Order();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Order", con);
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
                    if (!(rd["OrderCode"] is DBNull))
                        x.OrderCode = int.Parse(rd["OrderCode"].ToString());
                    if (!(rd["InventoryID"] is DBNull))
                        x.InventoryID = int.Parse(rd["InventoryID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = int.Parse(rd["FamilyID"].ToString());
                    if (!(rd["SpecialFamilyID"] is DBNull))
                        x.SpecialFamilyID = int.Parse(rd["SpecialFamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["Type"] is DBNull))
                        x.Type = int.Parse(rd["Type"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["NextOrderDate"] is DBNull))
                        x.NextOrderDate = DateTime.Parse(rd["NextOrderDate"].ToString());
                    x.Description = rd["Description"].ToString();
                    x.Source = rd["Source"].ToString();
                    x.InvoiceSerial = rd["InvoiceSerial"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.BarCode = rd["BarCode"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());

                    x.OIs = Order_Item.GetAllOrder_ItemByOrder(x);
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
        public static async Task<DataTable> GetAllOrderTable()
        {
            return await BaseDataBase._TablingStoredProcedureAsync("sp_Get_All_Order_Table");
        }

        //Done :)
        public static List<Order> GetAllOrderByFamilyID(int FamilyID)
        {
            List<Order> xx = new List<Order>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Order_ByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@FamilyID", FamilyID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                Order o = null;
                while (rd.Read())
                {
                    if (o == null || (int)rd["OrderID"] != o.Id.Value)
                    {
                        o = new Order();

                        o.Id = (int)rd["OrderID"];
                        o.InventoryID = (int)rd["InventoryID"];
                        if (!(rd["FamilyID"] is DBNull))
                            o.FamilyID = int.Parse(rd["FamilyID"].ToString());
                        if (!(rd["SpecialFamilyID"] is DBNull))
                            o.SpecialFamilyID = int.Parse(rd["SpecialFamilyID"].ToString());
                        if (!(rd["OrphanID"] is DBNull))
                            o.OrphanID = int.Parse(rd["OrphanID"].ToString());
                        o.Type = (int)rd["Type"];
                        o.Date = (DateTime)rd["Date"];
                        if (!(rd["NextOrderDate"] is DBNull))
                            o.NextOrderDate = (DateTime)rd["NextOrderDate"];
                        o.Description = rd["OrderDescription"].ToString();
                        o.Source = rd["OrderSource"].ToString();
                        o.InvoiceSerial = rd["InvoiceSerial"].ToString();
                        o.Notes = rd["OrderNotes"].ToString();
                        o.LastUserID = int.Parse(rd["OrderLastUserID"].ToString());
                        o.BarCode = rd["OrderBarcode"].ToString();
                        o.OrderCode = (int)rd["OrderCode"];

                        xx.Add(o);
                    }

                    Item i = new Item();
                    i.Id = (int)rd["ItemID"];
                    i.Name = rd["Name"].ToString();
                    i.IsActive = (bool)rd["IsActive"];
                    i.Source = rd["ItemSource"].ToString();
                    i.Barcode = rd["ItemBarcode"].ToString();
                    i.ItemType = rd["ItemType"].ToString();
                    i.Description = rd["ItemDescription"].ToString();
                    i.StandardUnit = rd["StandardUnit"].ToString();
                    i.Unit2 = rd["Unit2"].ToString();
                    if (!(rd["Unit2Convert"] is DBNull))
                        i.Unit2Convert = (float)rd["Unit2Convert"];
                    i.Unit3 = rd["Unit3"].ToString();
                    if (!(rd["Unit3Convert"] is DBNull))
                        i.Unit3Convert = (float)rd["Unit3Convert"];
                    if (!(rd["MinimumQuantity"] is DBNull))
                        i.MinimumQuantity = (float)rd["MinimumQuantity"];
                    if (!(rd["MaximumQuantity"] is DBNull))
                        i.MaximumQuantity = (float)rd["MaximumQuantity"];
                    if (!(rd["MaxQuantityPerOrder"] is DBNull))
                        i.MaxQuantityPerOrder = (float)rd["MaxQuantityPerOrder"];
                    if (!(rd["MaxQuantityPerFamily"] is DBNull))
                        i.MaxQuantityPerFamily = (float)rd["MaxQuantityPerFamily"];
                    if (!(rd["MaxQuantityPerDay"] is DBNull))
                        i.MaxQuantityPerDay = (float)rd["MaxQuantityPerDay"];
                    if (!(rd["WarningQuantity"] is DBNull))
                        i.WarningQuantity = (float)rd["WarningQuantity"];
                    if (!(rd["Weight"] is DBNull))
                        i.Weight = (float)rd["Weight"];
                    i.DefaultLocation = rd["DefaultLocation"].ToString();
                    i.Notes = rd["ItemNotes"].ToString();
                    i.LastUserID = (int)rd["ItemLastUserID"];


                    Order_Item oi = new Order_Item();
                    oi.Order = o;
                    oi.Item = i;
                    oi.Quantity = (float)rd["Quantity"];
                    oi.LastUserID = (int)rd["Order_ItemLastUserID"];
                    o.OIs.Add(oi);

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

        //Done
        public static List<Order> GetAllOrderBySpecialFamilyID(int SpecialFamilyID)
        {
            List<Order> xx = new List<Order>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Order_BySpecialFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@SpecialFamilyID", SpecialFamilyID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                Order o = null;
                while (rd.Read())
                {
                    if (o == null || (int)rd["OrderID"] != o.Id.Value)
                    {
                        o = new Order();

                        o.Id = (int)rd["OrderID"];
                        o.InventoryID = (int)rd["InventoryID"];
                        if (!(rd["FamilyID"] is DBNull))
                            o.FamilyID = int.Parse(rd["FamilyID"].ToString());
                        if (!(rd["SpecialFamilyID"] is DBNull))
                            o.SpecialFamilyID = int.Parse(rd["SpecialFamilyID"].ToString());
                        if (!(rd["OrphanID"] is DBNull))
                            o.OrphanID = int.Parse(rd["OrphanID"].ToString());
                        o.Type = (int)rd["Type"];
                        o.Date = (DateTime)rd["Date"];
                        if (!(rd["NextOrderDate"] is DBNull))
                            o.NextOrderDate = (DateTime)rd["NextOrderDate"];
                        o.Description = rd["OrderDescription"].ToString();
                        o.Source = rd["OrderSource"].ToString();
                        o.InvoiceSerial = rd["InvoiceSerial"].ToString();
                        o.Notes = rd["OrderNotes"].ToString();
                        o.LastUserID = int.Parse(rd["OrderLastUserID"].ToString());
                        o.BarCode = rd["OrderBarcode"].ToString();
                        o.OrderCode = (int)rd["OrderCode"];

                        xx.Add(o);
                    }

                    Item i = new Item();
                    i.Id = (int)rd["ItemID"];
                    i.Name = rd["Name"].ToString();
                    i.IsActive = (bool)rd["IsActive"];
                    i.Source = rd["ItemSource"].ToString();
                    i.Barcode = rd["ItemBarcode"].ToString();
                    i.ItemType = rd["ItemType"].ToString();
                    i.Description = rd["ItemDescription"].ToString();
                    i.StandardUnit = rd["StandardUnit"].ToString();
                    i.Unit2 = rd["Unit2"].ToString();
                    if (!(rd["Unit2Convert"] is DBNull))
                        i.Unit2Convert = (float)rd["Unit2Convert"];
                    i.Unit3 = rd["Unit3"].ToString();
                    if (!(rd["Unit3Convert"] is DBNull))
                        i.Unit3Convert = (float)rd["Unit3Convert"];
                    if (!(rd["MinimumQuantity"] is DBNull))
                        i.MinimumQuantity = (float)rd["MinimumQuantity"];
                    if (!(rd["MaximumQuantity"] is DBNull))
                        i.MaximumQuantity = (float)rd["MaximumQuantity"];
                    if (!(rd["MaxQuantityPerOrder"] is DBNull))
                        i.MaxQuantityPerOrder = (float)rd["MaxQuantityPerOrder"];
                    if (!(rd["MaxQuantityPerFamily"] is DBNull))
                        i.MaxQuantityPerFamily = (float)rd["MaxQuantityPerFamily"];
                    if (!(rd["MaxQuantityPerDay"] is DBNull))
                        i.MaxQuantityPerDay = (float)rd["MaxQuantityPerDay"];
                    if (!(rd["WarningQuantity"] is DBNull))
                        i.WarningQuantity = (float)rd["WarningQuantity"];
                    if (!(rd["Weight"] is DBNull))
                        i.Weight = (float)rd["Weight"];
                    i.DefaultLocation = rd["DefaultLocation"].ToString();
                    i.Notes = rd["ItemNotes"].ToString();
                    i.LastUserID = (int)rd["ItemLastUserID"];


                    Order_Item oi = new Order_Item();
                    oi.Order = o;
                    oi.Item = i;
                    oi.Quantity = (float)rd["Quantity"];
                    oi.LastUserID = (int)rd["Order_ItemLastUserID"];
                    o.OIs.Add(oi);
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

        //To Re Create
        public static List<Order> GetAllOrderByOrphanID(int OrphanID)
        {
            List<Order> xx = new List<Order>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Order_ByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@OrphanID", OrphanID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Order x = new Order();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    if (!(rd["InventoryID"] is DBNull))
                        x.InventoryID = int.Parse(rd["InventoryID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = int.Parse(rd["FamilyID"].ToString());
                    if (!(rd["SpecialFamilyID"] is DBNull))
                        x.SpecialFamilyID = int.Parse(rd["SpecialFamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["Type"] is DBNull))
                        x.Type = int.Parse(rd["Type"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["NextOrderDate"] is DBNull))
                        x.NextOrderDate = DateTime.Parse(rd["NextOrderDate"].ToString());
                    x.Description = rd["Description"].ToString();
                    x.Source = rd["Source"].ToString();
                    x.InvoiceSerial = rd["InvoiceSerial"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());

                    x.OIs = Order_Item.GetAllOrder_ItemByOrder(x);
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

        public static List<string> GetDistinctSource
        {
            get { return BaseDataBase.GetAllStrings("select distinct Source from [Order] where Type in (1,2) and Source is not null"); }
        }
    }
}