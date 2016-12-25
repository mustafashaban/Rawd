using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class Account : ModelViewContext
    {
        public enum AccountType { Expenses = -1, Fund = 0, Sponsor, Orphan, OrphanStudent, Student };
        public static Dictionary<int, string> accountTypes = new Dictionary<int, string>()
        { { -1, "نفقات" },{ 0, "صندوق" },{ 1, "كفيل" },{ 2, "يتيم غير طالب" },{ 3, "يتيم طالب" },{ 4, "طالب علم" } };

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
            { name = value; }
        }

        private AccountType type;
        public AccountType Type
        {
            get
            { return type; }
            set
            { type = value; }
        }
        public string TypeText
        {
            get { return accountTypes[(int)type]; }
        }
        private double currentbalance;
        public double CurrentBalance
        {
            get
            { return currentbalance; }
            set
            { currentbalance = value; }
        }

        private DateTime createdate;
        public DateTime CreateDate
        {
            get
            { return createdate; }
            set
            { createdate = value; }
        }

        private int? ownerid;
        public int? OwnerID
        {
            get
            { return ownerid; }
            set
            { ownerid = value; }
        }

        private string status;
        public string Status
        {
            get
            { return status; }
            set
            { status = value; }
        }

        private int lastuserid;
        public int LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private int code;
        public int Code
        {
            get
            { return code; }
            set
            { code = value; }
        }
        private bool isDebit;
        public bool IsDebit
        {
            get
            { return isDebit; }
            set
            { isDebit = value; }
        }

        public List<Transition> Transitions { get; set; }

        public static bool InsertData(Account x)
        {
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_Account"
            , new SqlParameter("@Id", System.Data.SqlDbType.Int)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@Type", (int)x.Type)
            , new SqlParameter("@CurrentBalance", x.CurrentBalance)
            , new SqlParameter("@CreateDate", x.CreateDate)
            , new SqlParameter("@OwnerID", x.OwnerID)
            , new SqlParameter("@Status", x.Status)
            , new SqlParameter("@LastUserID", x.LastUserID)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@IsDebit", x.IsDebit)
            , new SqlParameter("@Code", x.Code));
            return x.Id.HasValue;
        }
        public static bool UpdateData(Account x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Account"
            , new SqlParameter("@Id", x.Id)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@Type", (int)x.Type)
            , new SqlParameter("@CurrentBalance", x.CurrentBalance)
            , new SqlParameter("@CreateDate", x.CreateDate)
            , new SqlParameter("@OwnerID", x.OwnerID)
            , new SqlParameter("@Status", x.Status)
            , new SqlParameter("@LastUserID", x.LastUserID)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@IsDebit", x.IsDebit)
            , new SqlParameter("@Code", x.Code));
        }
        public static bool DeleteData(Account x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Account"
            , new SqlParameter("@Id", x.Id));
        }
        public static Account GetAccountByID(int id)
        {
            Account x = new Account();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Account", con);
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
                    if (!(rd["Type"] is DBNull))
                        x.Type = (AccountType)rd["Type"];
                    if (!(rd["CurrentBalance"] is DBNull))
                        x.CurrentBalance = double.Parse(rd["CurrentBalance"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    if (!(rd["OwnerID"] is DBNull))
                        x.OwnerID = int.Parse(rd["OwnerID"].ToString());
                    x.Status = rd["Status"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["Code"] is DBNull))
                        x.Code = int.Parse(rd["Code"].ToString());
                    x.IsDebit = (bool)rd["IsDebit"];

                    //x.Transitions = Transition.GetAllTransitionByAccount(x);
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
        public static Account GetAccountByOwnerID(Account.AccountType Type, int OwnerID, bool bringDetails = true)
        {
            Account x = new Account();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_OwnerID_Account", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@Type", (int)Type));
            com.Parameters.Add(new SqlParameter("@OwnerID", OwnerID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (!(rd["Type"] is DBNull))
                        x.Type = (AccountType)rd["Type"];
                    if (!(rd["CurrentBalance"] is DBNull))
                        x.CurrentBalance = double.Parse(rd["CurrentBalance"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    if (!(rd["OwnerID"] is DBNull))
                        x.OwnerID = int.Parse(rd["OwnerID"].ToString());
                    x.Status = rd["Status"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["Code"] is DBNull))
                        x.Code = int.Parse(rd["Code"].ToString());
                    x.IsDebit = (bool)rd["IsDebit"];
                    if (bringDetails)
                        x.Transitions = Transition.GetAllTransitionByAccount(x);
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

        public static List<Account> GetAllAccount()
        {
            List<Account> xx = new List<Account>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Account", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Account x = new Account();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (!(rd["Type"] is DBNull))
                        x.Type = (AccountType)rd["Type"];
                    if (!(rd["CurrentBalance"] is DBNull))
                        x.CurrentBalance = double.Parse(rd["CurrentBalance"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    if (!(rd["OwnerID"] is DBNull))
                        x.OwnerID = int.Parse(rd["OwnerID"].ToString());
                    x.Status = rd["Status"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["Code"] is DBNull))
                        x.Code = int.Parse(rd["Code"].ToString());
                    x.IsDebit = (bool)rd["IsDebit"];
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

        public static DataTable GetAllAccountTable
        {
            get { return BaseDataBase._Tabling("sp_GetAllAccountTable"); }
        }

    }
}