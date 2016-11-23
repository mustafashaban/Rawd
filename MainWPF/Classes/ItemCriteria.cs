using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class ItemCriteria : ModelViewContext
    {
        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private Item criteriaItem;
        public Item CriteriaItem
        {
            get
            { return criteriaItem; }
            set
            { criteriaItem = value; }
        }

        private int? startcriteria;
        public int? StartCriteria
        {
            get
            { return startcriteria; }
            set
            { startcriteria = value; }
        }

        private int? endcriteria;
        public int? EndCriteria
        {
            get
            { return endcriteria; }
            set
            { endcriteria = value; }
        }

        private string criteriatype;
        public string CriteriaType
        {
            get
            { return criteriatype; }
            set
            { criteriatype = value; }
        }

        private int? count;
        public int? Count
        {
            get
            { return count; }
            set
            { count = value; }
        }

        private string evaluation;
        public string Evaluation
        {
            get
            { return evaluation; }
            set
            { evaluation = value; }
        }

        private int lastuserid;
        public int LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }
        public static bool InsertData(ItemCriteria x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_ItemCriteria"
                    , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                    , new SqlParameter("@ItemID", x.CriteriaItem.Id.Value)
                    , new SqlParameter("@StartCriteria", x.StartCriteria)
                    , new SqlParameter("@EndCriteria", x.EndCriteria)
                    , new SqlParameter("@CriteriaType", x.CriteriaType)
                    , new SqlParameter("@Count", x.Count)
                    , new SqlParameter("@Evaluation", x.Evaluation)
                    , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
            return x.ID.HasValue;
        }
        public static bool UpdateData(ItemCriteria x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_ItemCriteria"
                , new SqlParameter("@ID", x.ID)
                , new SqlParameter("@ItemID", x.CriteriaItem.Id.Value)
                , new SqlParameter("@StartCriteria", x.StartCriteria)
                , new SqlParameter("@EndCriteria", x.EndCriteria)
                , new SqlParameter("@CriteriaType", x.CriteriaType)
                , new SqlParameter("@Count", x.Count)
                , new SqlParameter("@Evaluation", x.Evaluation)
                , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(ItemCriteria x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_ItemCriteria"
                        , new SqlParameter("@ID", x.ID));
        }
        public static ItemCriteria GetItemCriteriaByID(int id)
        {
            ItemCriteria x = new ItemCriteria();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_ItemCriteria", con);
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

                    if (!(rd["ItemID"] is DBNull))
                        x.CriteriaItem = Item.GetItemByID(int.Parse(rd["ItemID"].ToString()));

                    if (!(rd["StartCriteria"] is DBNull))
                        x.StartCriteria = int.Parse(rd["StartCriteria"].ToString());
                    if (!(rd["EndCriteria"] is DBNull))
                        x.EndCriteria = int.Parse(rd["EndCriteria"].ToString());
                    x.CriteriaType = rd["CriteriaType"].ToString();
                    if (!(rd["Count"] is DBNull))
                        x.Count = int.Parse(rd["Count"].ToString());
                    x.Evaluation = rd["Evaluation"].ToString();
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
        public static List<ItemCriteria> GetAllItemCriteria()
        {
            List<ItemCriteria> xx = new List<ItemCriteria>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_ItemCriteria", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    ItemCriteria x = new ItemCriteria();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());

                    if (!(rd["ItemID"] is DBNull))
                        x.CriteriaItem = Item.GetItemByID(int.Parse(rd["ItemID"].ToString()));

                    if (!(rd["StartCriteria"] is DBNull))
                        x.StartCriteria = int.Parse(rd["StartCriteria"].ToString());
                    if (!(rd["EndCriteria"] is DBNull))
                        x.EndCriteria = int.Parse(rd["EndCriteria"].ToString());
                    x.CriteriaType = rd["CriteriaType"].ToString();
                    if (!(rd["Count"] is DBNull))
                        x.Count = int.Parse(rd["Count"].ToString());
                    x.Evaluation = rd["Evaluation"].ToString();
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

        internal bool IsValidate()
        {
            if (!Count.HasValue)
            {
                MyMessageBox.Show("يجب ادخال العدد");
                return false;
            }
            if (CriteriaType == "عمر الشخص")
            {
                if (StartCriteria > EndCriteria)
                {
                    MyMessageBox.Show("بداية المجال يجب أن تكون أصغر من نهايته");
                    return false; ;
                }
            }
            else if (CriteriaType == "عدد الافراد")
            {
                if (StartCriteria > EndCriteria)
                {
                    MyMessageBox.Show("بداية المجال يجب أن تكون أصغر من نهايته");
                    return false;
                }
            }
            return true;
        }
    }
}