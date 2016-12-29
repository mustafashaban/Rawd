using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class Order_Item : ModelViewContext
    {
        private Order order;
        public Order Order
        {
            get
            { return order; }
            set
            { order = value; }
        }

        private Item item;
        public Item Item
        {
            get
            { return item; }
            set
            { item = value; }
        }

        private double quantity;
        public double Quantity
        {
            get
            { return quantity; }
            set
            { quantity = value; }
        }

        private int lastuserid;
        public int LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        public static bool InsertData(Order_Item x)
        {
            return BaseDataBase._StoredProcedure("sp_Add_Order_Item"
                    , new SqlParameter("@OrderID", x.Order.Id)
                    , new SqlParameter("@ItemID", x.Item.Id)
                    , new SqlParameter("@Quantity", x.Quantity)
                    , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool UpdateData(Order_Item x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Order_Item"
                    , new SqlParameter("@OrderID", x.Order.Id)
                    , new SqlParameter("@ItemID", x.Item.Id)
                    , new SqlParameter("@Quantity", x.Quantity)
                    , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(Order_Item x)
        {
            //Order_Item.UpdateData(x);
            return BaseDataBase._StoredProcedure("sp_Delete_Order_Item"
                     , new SqlParameter("@OrderID", x.Order.Id)
                    , new SqlParameter("@ItemID", x.Item.Id));
        }
        public static Order_Item GetOrder_ItemByID(Order Order, Item Item)
        {
            Order_Item x = new Order_Item();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Order_Item", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@OrderID", Order.Id);
            com.Parameters.Add(pr1);
            SqlParameter pr2 = new SqlParameter("@Item", Item.Id);
            com.Parameters.Add(pr2);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    x.Order = Order;
                    x.Item = Item;
                    if (!(rd["Quantity"] is DBNull))
                        x.Quantity = double.Parse(rd["Quantity"].ToString());
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
        public static List<Order_Item> GetAllOrder_Item()
        {
            List<Order_Item> xx = new List<Order_Item>();
            //SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            //SqlCommand com = new SqlCommand("sp_Get_All_Order_Item", con);
            //com.CommandType = System.Data.CommandType.StoredProcedure;
            //try
            //{
            //    con.Open();
            //    SqlDataReader rd = com.ExecuteReader();
            //    while (rd.Read())
            //    {
            //        Order_Item x = new Order_Item();

            //        if (!(rd["OrderID"] is DBNull))
            //            x.OrderID = int.Parse(rd["OrderID"].ToString());
            //        if (!(rd["ItemID"] is DBNull))
            //            x.ItemID = int.Parse(rd["ItemID"].ToString());
            //        if (!(rd["Quantity"] is DBNull))
            //            x.Quantity = double.Parse(rd["Quantity"].ToString());
            //        if (!(rd["LastUserID"] is DBNull))
            //            x.LastUserID = int.Parse(rd["LastUserID"].ToString());
            //        xx.Add(x);
            //    }
            //    rd.Close();
            //}
            //catch
            //{
            //    xx = null;
            //}
            //finally
            //{
            //    con.Close();
            //}
            return xx;
        }

        public static List<Order_Item> GetAllOrder_ItemByOrder(Order Order)
        {
            List<Order_Item> xx = new List<Order_Item>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Order_Item_ByOrderID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@OrderID", Order.Id);
            com.Parameters.Add(pr1);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Order_Item x = new Order_Item();
                    x.Order = Order;
                    x.Item = Item.GetItemByID(int.Parse(rd["ItemID"].ToString()));
                    x.Quantity = double.Parse(rd["Quantity"].ToString());
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
        public static List<Order_Item> GetAllOrder_ItemByItem(Item Item)
        {
            List<Order_Item> xx = new List<Order_Item>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Order_Item_ByItemID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@ItemID", Item.Id);
            com.Parameters.Add(pr1);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Order_Item x = new Order_Item();

                    if (!(rd["OrderID"] is DBNull))
                        x.Order = Order.GetOrderByID(int.Parse(rd["OrderID"].ToString()));
                    x.Item = Item;
                    if (!(rd["Quantity"] is DBNull))
                        x.Quantity = double.Parse(rd["Quantity"].ToString());
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


        public static List<Order_Item> GetFamilyCriteria(int FamilyID)
        {
            List<Order_Item> OIs = new List<Order_Item>();

            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("GetFamilyCriteria", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    var x = new Order_Item();

                    if (!(rd["ID"] is DBNull))
                        x.Item = Item.GetItemByID(System.Int32.Parse(rd["ID"].ToString()));
                    if (!(rd["Count"] is DBNull))
                        x.Quantity = System.Int32.Parse(rd["Count"].ToString());

                    OIs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                OIs = null;
            }
            finally
            {
                con.Close();
            }
            return OIs;
        }
    }
}