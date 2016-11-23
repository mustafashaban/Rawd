using System; using System.Collections.Generic; using System.Data.SqlClient; using System.Linq; using System.Web;  namespace MainWPF {     public class FormedBasket_Sector : ModelViewContext     {         public static bool SaveFormedBasketSectors(FormedBasket fb)
        {
            try
            {
                BaseDataBase._NonQuery("delete from FormedBasket_Sector where FormedBasketID = " + fb.Id.Value);
                foreach (var sector in fb.RelatedSectors)
                {
                    BaseDataBase._NonQuery($"INSERT INTO [dbo].[FormedBasket_Sector]([FormedBasketID],[SectorID],[LastUserID]) VALUES({fb.Id}, {sector.ID}, {BaseDataBase.CurrentUser.ID})");
                }
                return true;
            }
            catch (Exception ex) { MyMessageBox.Show(ex.Message); return false; }
        }

        internal static bool DeleteFormedBasketSectors(FormedBasket fb)
        {
            try
            {
                BaseDataBase._NonQuery("delete from FormedBasket_Sector where FormedBasketID = " + fb.Id.Value);
                return true;
            }
            catch (Exception ex) { MyMessageBox.Show(ex.Message); return false; }
        }

        //private int? formedbasketid;
        //public int? FormedBasketID
        //{
        //    get
        //    { return formedbasketid; }
        //    set
        //    { formedbasketid = value; }
        //}

        //private int? sectorid;
        //public int? SectorID
        //{
        //    get
        //    { return sectorid; }
        //    set
        //    { sectorid = value; }
        //}

        //private int lastuserid;
        //public int LastUserID
        //{
        //    get
        //    { return lastuserid; }
        //    set
        //    { lastuserid = value; }
        //}
        //public static bool InsertData(FormedBasket_Sector x)
        //{
        //    x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_FormedBasket_Sector");
        //    return x.ID.HasValue;
        //}
        //public static bool UpdateData(FormedBasket_Sector x)
        //{
        //    return BaseDataBase._StoredProcedure("sp_Update_FormedBasket_Sector");
        //}
        //public static bool DeleteData(FormedBasket_Sector x)
        //{
        //    return BaseDataBase._StoredProcedure("sp_Delete_FormedBasket_Sector");
        //}
        //public static FormedBasket_Sector GetFormedBasket_SectorByID(int id)
        //{
        //    FormedBasket_Sector x = new FormedBasket_Sector();
        //    SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
        //    SqlCommand com = new SqlCommand("sp_Get_ID_FormedBasket_Sector", con);
        //    com.CommandType = System.Data.CommandType.StoredProcedure;
        //    SqlParameter pr = new SqlParameter("@ID", id);
        //    com.Parameters.Add(pr);
        //    try
        //    {
        //        con.Open();
        //        SqlDataReader rd = com.ExecuteReader();
        //        if (rd.Read())
        //        {
        //            if (!(rd["FormedBasketID"] is DBNull))
        //                x.FormedBasketID = int.Parse(rd["FormedBasketID"].ToString());
        //            if (!(rd["SectorID"] is DBNull))
        //                x.SectorID = int.Parse(rd["SectorID"].ToString());
        //            if (!(rd["LastUserID"] is DBNull))
        //                x.LastUserID = int.Parse(rd["LastUserID"].ToString());
        //        }
        //        rd.Close();
        //    }
        //    catch
        //    {
        //        x = null;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return x;
        //}
        //public static List<FormedBasket_Sector> GetAllFormedBasket_Sector()
        //{
        //    List<FormedBasket_Sector> xx = new List<FormedBasket_Sector>();
        //    SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
        //    SqlCommand com = new SqlCommand("sp_Get_All_FormedBasket_Sector", con);
        //    com.CommandType = System.Data.CommandType.StoredProcedure;
        //    try
        //    {
        //        con.Open();
        //        SqlDataReader rd = com.ExecuteReader();
        //        while (rd.Read())
        //        {
        //            FormedBasket_Sector x = new FormedBasket_Sector();

        //            if (!(rd["FormedBasketID"] is DBNull))
        //                x.FormedBasketID = int.Parse(rd["FormedBasketID"].ToString());
        //            if (!(rd["SectorID"] is DBNull))
        //                x.SectorID = int.Parse(rd["SectorID"].ToString());
        //            if (!(rd["LastUserID"] is DBNull))
        //                x.LastUserID = int.Parse(rd["LastUserID"].ToString());
        //            xx.Add(x);
        //        }
        //        rd.Close();
        //    }
        //    catch
        //    {
        //        xx = null;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return xx;
        //}
    } }