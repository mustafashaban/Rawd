using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class FormedBasket_Item : ModelViewContext
    {
        private FormedBasket formedBasket;
        public FormedBasket FormedBasket
        {
            get
            { return formedBasket; }
            set
            { formedBasket = value; }
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
        public static bool InsertData(FormedBasket_Item x)
        {
            return BaseDataBase._StoredProcedure("sp_Add_FormedBasket_Item"
                , new SqlParameter("@FormedBasketID", x.FormedBasket.Id)
                , new SqlParameter("@ItemID", x.RelatedItem.Id)
                , new SqlParameter("@Quantity", x.Quantity)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool UpdateData(FormedBasket_Item x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_FormedBasket_Item"
                , new SqlParameter("@FormedBasketID", x.FormedBasket.Id)
                , new SqlParameter("@ItemID", x.RelatedItem.Id)
                , new SqlParameter("@Quantity", x.Quantity)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(FormedBasket_Item x)
        {
            FormedBasket_Item.UpdateData(x);
            return BaseDataBase._StoredProcedure("sp_Delete_FormedBasket_Item"
                , new SqlParameter("@FormedBasketID", x.FormedBasket.Id)
                , new SqlParameter("@ItemID", x.RelatedItem.Id));
        }
        public static FormedBasket_Item GetBasket_ItemByID(int FormedBasketID, int ItemID)
        {
            FormedBasket_Item x = new FormedBasket_Item();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_FormedBasket_Item", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@FormedBasketID", FormedBasketID);
            com.Parameters.Add(pr1);
            SqlParameter pr2 = new SqlParameter("@ItemID", ItemID);
            com.Parameters.Add(pr2);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["FormedBasketID"] is DBNull))
                        x.FormedBasket = FormedBasket.GetItemByID(int.Parse(rd["FormedBasketID"].ToString()));
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
        public static List<FormedBasket_Item> GetAllBasket_ItemByBasket(FormedBasket FormedBasket)
        {
            List<FormedBasket_Item> xx = new List<FormedBasket_Item>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_FormedBasket_Item_ByFormedBasketID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@FormedBasketID", FormedBasket.Id);
            com.Parameters.Add(pr1);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    FormedBasket_Item x = new FormedBasket_Item();
                    x.FormedBasket = FormedBasket;
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
    }
}