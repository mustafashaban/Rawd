using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class FamilyNeed_ListerGroup : ModelViewContext
    {
        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private FamilyNeedByLister familyNeedByLister;
        public FamilyNeedByLister FamilyNeedByLister
        {
            get
            { return familyNeedByLister; }
            set
            { familyNeedByLister = value; }
        }

        private int? listergroupid;
        public int? ListerGroupID
        {
            get
            { return listergroupid; }
            set
            { listergroupid = value; }
        }

        private int? orderID;
        public int? OrderID
        {
            get
            { return orderID; }
            set
            { orderID = value; }
        }

        private int count = 1;
        public int Count
        {
            get
            { return count; }
            set
            { count = value; }
        }

        public bool isEnsured = false;
        public bool IsEnsured
        {
            get { return isEnsured; }
            set
            {
                isEnsured = value;
                NotifyPropertyChanged("IsEnsured");
            }
        }


        public static bool InsertData(FamilyNeed_ListerGroup x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add2FamilyNeed_ListerGroup"
           , new SqlParameter("@ID", System.Data.SqlDbType.Int)
           , new SqlParameter("@FamilyNeedListerID", x.FamilyNeedByLister.ID)
           , new SqlParameter("@ListerGroupID", x.ListerGroupID)
           , new SqlParameter("@OrderID", x.OrderID)
           , new SqlParameter("@Count", x.Count));

            return x.ID.HasValue;
        }
        public static bool UpdateData(FamilyNeed_ListerGroup x)
        {
            return BaseDataBase._StoredProcedure("sp_UpdateFamilyNeed_ListerGroup"
           , new SqlParameter("@ID", x.ID)
           , new SqlParameter("@FamilyNeedListerID", x.FamilyNeedByLister.ID)
           , new SqlParameter("@ListerGroupID", x.ListerGroupID)
           , new SqlParameter("@OrderID", x.OrderID)
           , new SqlParameter("@Count", x.Count));
        }
        public static bool DeleteData(FamilyNeed_ListerGroup x)
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromFamilyNeed_ListerGroup"
                    , new SqlParameter("@ID", x.ID));
        }
        public static bool DeleteAllByListerGroupID(int listerGroupID)
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromFamilyNeed_ListerGroupByListerGroupID"
                , new SqlParameter("LiserID", listerGroupID));
        }

        public static FamilyNeed_ListerGroup GetFamilyNeed_ListerGroupByID(int id)
        {
            FamilyNeed_ListerGroup x = new FamilyNeed_ListerGroup();

            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyNeed_ListerGroupByID", con);
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
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["FamilyNeedListerID"] is DBNull))
                        x.FamilyNeedByLister = FamilyNeedByLister.GetFamilyNeedByListerByID(System.Int32.Parse(rd["FamilyNeedListerID"].ToString()));
                    if (!(rd["ListerGroupID"] is DBNull))
                        x.ListerGroupID = System.Int32.Parse(rd["ListerGroupID"].ToString());
                    if (!(rd["OrderID"] is DBNull))
                        x.OrderID = System.Int32.Parse(rd["OrderID"].ToString());
                    if (!(rd["Count"] is DBNull))
                        x.Count = System.Int32.Parse(rd["Count"].ToString());
                    x.IsEnsured = x.OrderID.HasValue;
                }
                rd.Close();
            }
            catch
            {
                x = new FamilyNeed_ListerGroup();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<FamilyNeed_ListerGroup> GetFamilyNeed_ListerGroupAll()
        {
            List<FamilyNeed_ListerGroup> fns = new List<FamilyNeed_ListerGroup>();

            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyNeed_ListerGroupAll", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    FamilyNeed_ListerGroup x = new FamilyNeed_ListerGroup();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["FamilyNeedListerID"] is DBNull))
                        x.FamilyNeedByLister = FamilyNeedByLister.GetFamilyNeedByListerByID(System.Int32.Parse(rd["FamilyNeedListerID"].ToString()));
                    if (!(rd["ListerGroupID"] is DBNull))
                        x.ListerGroupID = System.Int32.Parse(rd["ListerGroupID"].ToString());
                    if (!(rd["OrderID"] is DBNull))
                        x.OrderID = System.Int32.Parse(rd["OrderID"].ToString());
                    if (!(rd["Count"] is DBNull))
                        x.Count = System.Int32.Parse(rd["Count"].ToString());

                    x.IsEnsured = x.OrderID.HasValue;
                    fns.Add(x);
                }
                rd.Close();
            }
            catch
            {
                fns = new List<FamilyNeed_ListerGroup>();
            }
            finally
            {
                con.Close();
            }
            return fns;
        }
        public static List<FamilyNeed_ListerGroup> GetFamilyNeed_ListerGroupByListerGroupID(int ListerGroupID)
        {
            List<FamilyNeed_ListerGroup> lgfns = new List<FamilyNeed_ListerGroup>();

            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyNeed_ListerGroupByListerGroupID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ListerGroupID", ListerGroupID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    FamilyNeed_ListerGroup x = new FamilyNeed_ListerGroup();

                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["FamilyNeedListerID"] is DBNull))
                        x.FamilyNeedByLister = FamilyNeedByLister.GetFamilyNeedByListerByID(System.Int32.Parse(rd["FamilyNeedListerID"].ToString()));
                    if (!(rd["ListerGroupID"] is DBNull))
                        x.ListerGroupID = System.Int32.Parse(rd["ListerGroupID"].ToString());
                    if (!(rd["OrderID"] is DBNull))
                        x.OrderID = System.Int32.Parse(rd["OrderID"].ToString());
                    if (!(rd["Count"] is DBNull))
                        x.Count = System.Int32.Parse(rd["Count"].ToString());
                    x.IsEnsured = x.OrderID.HasValue;

                    lgfns.Add(x);
                }
                rd.Close();
            }
            catch
            {
                lgfns = new List<FamilyNeed_ListerGroup>();
            }
            finally
            {
                con.Close();
            }
            return lgfns;
        }
        public static List<FamilyNeed_ListerGroup> GetFamilyNeed_ListerGroupBySupportGroupID(int? OrderID, int FamilyID)
        {
            List<FamilyNeed_ListerGroup> lgfns = new List<FamilyNeed_ListerGroup>();

            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyNeed_ListerGroupBySupportGroupID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@OrderID", OrderID);
            com.Parameters.Add(pr1);
            SqlParameter pr2 = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr2);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    FamilyNeed_ListerGroup x = new FamilyNeed_ListerGroup();

                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["FamilyNeedListerID"] is DBNull))
                        x.FamilyNeedByLister = FamilyNeedByLister.GetFamilyNeedByListerByID(System.Int32.Parse(rd["FamilyNeedListerID"].ToString()));
                    if (!(rd["ListerGroupID"] is DBNull))
                        x.ListerGroupID = System.Int32.Parse(rd["ListerGroupID"].ToString());
                    if (!(rd["OrderID"] is DBNull))
                        x.OrderID = System.Int32.Parse(rd["OrderID"].ToString());
                    if (!(rd["Count"] is DBNull))
                        x.Count = System.Int32.Parse(rd["Count"].ToString());
                    x.IsEnsured = x.OrderID.HasValue;

                    lgfns.Add(x);
                }
                rd.Close();
            }
            catch
            {
                lgfns = new List<FamilyNeed_ListerGroup>();
            }
            finally
            {
                con.Close();
            }
            return lgfns;
        }
    }
}
