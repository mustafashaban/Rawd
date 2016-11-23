using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class Basket_Item : ModelViewContext
    {
        private Item basket;
        public Item Basket
        {
            get
            { return basket; }
            set
            { basket = value; }
        }

        private Item relatedItem;
        public Item RelatedItem
        {
            get
            { return relatedItem; }
            set
            { relatedItem = value; }
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
        public static bool InsertData(Basket_Item x)
        {
            return BaseDataBase._StoredProcedure("sp_Add_Basket_Item"
                , new SqlParameter("@BasketID", x.Basket.Id)
                , new SqlParameter("@ItemID", x.RelatedItem.Id)
                , new SqlParameter("@Quantity", x.Quantity)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool UpdateData(Basket_Item x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Basket_Item"
                , new SqlParameter("@BasketID", x.Basket.Id)
                , new SqlParameter("@ItemID", x.RelatedItem.Id)
                , new SqlParameter("@Quantity", x.Quantity)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(Basket_Item x)
        {
            Basket_Item.UpdateData(x);
            return BaseDataBase._StoredProcedure("sp_Delete_Basket_Item"
                , new SqlParameter("@BasketID", x.Basket.Id)
                , new SqlParameter("@ItemID", x.RelatedItem.Id));
        }
        public static Basket_Item GetBasket_ItemByID(int BasketID, int ItemID)
        {
            Basket_Item x = new Basket_Item();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Basket_Item", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@BasketID", BasketID);
            com.Parameters.Add(pr1);
            SqlParameter pr2 = new SqlParameter("@ItemID", ItemID);
            com.Parameters.Add(pr2);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["BasketID"] is DBNull))
                        x.Basket = Item.GetItemByID(int.Parse(rd["BasketID"].ToString()));
                    if (!(rd["ItemID"] is DBNull))
                        x.RelatedItem = Item.GetItemByID(int.Parse(rd["ItemID"].ToString()));
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
        public static List<Basket_Item> GetAllBasket_Item()
        {
            List<Basket_Item> xx = new List<Basket_Item>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Basket_Item", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Basket_Item x = new Basket_Item();

                    if (!(rd["BasketID"] is DBNull))
                        x.Basket = Item.GetItemByID(int.Parse(rd["BasketID"].ToString()));
                    if (!(rd["ItemID"] is DBNull))
                        x.RelatedItem = Item.GetItemByID(int.Parse(rd["ItemID"].ToString()));
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

        public static List<Basket_Item> GetAllBasket_ItemByBasket(Item Basket)
        {
            List<Basket_Item> xx = new List<Basket_Item>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Basket_Item_ByBasketID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@BasketID", Basket.Id);
            com.Parameters.Add(pr1);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Basket_Item x = new Basket_Item();
                    x.Basket = Basket;
                    if (!(rd["ItemID"] is DBNull))
                        x.RelatedItem = Item.GetItemByID(int.Parse(rd["ItemID"].ToString()));
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

        //no need to it
        private static List<Basket_Item> GetAllBasket_ItemByItem(Item Item)
        {
            List<Basket_Item> xx = new List<Basket_Item>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Basket_Item_ByItem", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@ItemID", Item.Id);
            com.Parameters.Add(pr1);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Basket_Item x = new Basket_Item();

                    if (!(rd["BasketID"] is DBNull))
                        x.Basket = Item.GetItemByID(int.Parse(rd["BasketID"].ToString()));
                    x.RelatedItem = Item;
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
    }
}