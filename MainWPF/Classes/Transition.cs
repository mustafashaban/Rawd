using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class Transition : ModelViewContext
    {
        private int? id;
        public int? Id
        {
            get
            { return id; }
            set
            { id = value; }
        }
        public Account MainAccount { get; set; }

        private Account rightaccount;
        public Account RightAccount
        {
            get
            { return rightaccount; }
            set
            { rightaccount = value; }
        }

        private Account leftaccount;
        public Account LeftAccount
        {
            get
            { return leftaccount; }
            set
            { leftaccount = value; }
        }

        private double? value;
        public double? Value
        {
            get
            { return value; }
            set
            { this.value = value; }
        }
        public double? LeftValue
        {
            get { return MainAccount.Id.Value == LeftValue.Value && MainAccount.Type == 0 ? value : null;  }
        }
        public double? Rightvalue
        {
            get { return MainAccount.Id.Value == LeftValue.Value && MainAccount.Type > 0 ? value : null; }
        }

        private DateTime? createdate;
        public DateTime? CreateDate
        {
            get
            { return createdate; }
            set
            { createdate = value; }
        }

        private string details;
        public string Details
        {
            get
            { return details; }
            set
            { details = value; }
        }

        private int? lastuserid;
        public int? LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        private int? sponsorshipid;
        public int? SponsorshipID
        {
            get
            { return sponsorshipid; }
            set
            { sponsorshipid = value; }
        }
        public static bool InsertData(Transition x)
        {
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_Transition"
            , new SqlParameter("@Id", System.Data.SqlDbType.Int)
            , new SqlParameter("@RightAccount", x.RightAccount.Id)
            , new SqlParameter("@LeftAccount", x.LeftAccount.Id)
            , new SqlParameter("@Value", x.Value)
            , new SqlParameter("@CreateDate", x.CreateDate)
            , new SqlParameter("@Details", x.Details)
            , new SqlParameter("@LastUserID", x.LastUserID)
            , new SqlParameter("@SponsorshipID", x.SponsorshipID));
            return x.Id.HasValue;
        }
        public static bool UpdateData(Transition x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Transition"
            , new SqlParameter("@Id", x.Id)
            , new SqlParameter("@RightAccount", x.RightAccount.Id)
            , new SqlParameter("@LeftAccount", x.LeftAccount.Id)
            , new SqlParameter("@Value", x.Value)
            , new SqlParameter("@CreateDate", x.CreateDate)
            , new SqlParameter("@Details", x.Details)
            , new SqlParameter("@LastUserID", x.LastUserID)
            , new SqlParameter("@SponsorshipID", x.SponsorshipID));
        }
        public static bool DeleteData(Transition x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Transition"
            , new SqlParameter("@Id", x.Id));
        }
        public static Transition GetTransitionByID(int id)
        {
            Transition x = new Transition();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Transition", con);
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
                    if (!(rd["RightAccount"] is DBNull))
                        x.RightAccount = Account.GetAccountByID(int.Parse(rd["RightAccount"].ToString()));
                    if (!(rd["LeftAccount"] is DBNull))
                        x.LeftAccount = Account.GetAccountByID(int.Parse(rd["LeftAccount"].ToString()));
                    if (!(rd["Value"] is DBNull))
                        x.Value = double.Parse(rd["Value"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    x.Details = rd["Details"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["SponsorshipID"] is DBNull))
                        x.SponsorshipID = int.Parse(rd["SponsorshipID"].ToString());
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
        public static List<Transition> GetAllTransition()
        {
            List<Transition> xx = new List<Transition>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Transition", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Transition x = new Transition();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    if (!(rd["RightAccount"] is DBNull))
                        x.RightAccount = Account.GetAccountByID(int.Parse(rd["RightAccount"].ToString()));
                    if (!(rd["LeftAccount"] is DBNull))
                        x.LeftAccount = Account.GetAccountByID(int.Parse(rd["LeftAccount"].ToString()));
                    if (!(rd["Value"] is DBNull))
                        x.Value = double.Parse(rd["Value"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    x.Details = rd["Details"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["SponsorshipID"] is DBNull))
                        x.SponsorshipID = int.Parse(rd["SponsorshipID"].ToString());
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
        public static List<Transition> GetAllTransitionByAccount(Account a)
        {
            List<Transition> xx = new List<Transition>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_AccountID_Transition", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@ID", a.Id));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Transition x = new Transition();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.MainAccount = a;
                    if (!(rd["RightAccount"] is DBNull))
                        x.RightAccount = rd["RightAccount"].ToString() == a.Id.ToString() ? a : Account.GetAccountByID(int.Parse(rd["RightAccount"].ToString()));
                    if (!(rd["LeftAccount"] is DBNull))
                        x.LeftAccount = rd["LeftAccount"].ToString() == a.Id.ToString() ? a : Account.GetAccountByID(int.Parse(rd["LeftAccount"].ToString()));
                    if (!(rd["Value"] is DBNull))
                        x.Value = double.Parse(rd["Value"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    x.Details = rd["Details"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    if (!(rd["SponsorshipID"] is DBNull))
                        x.SponsorshipID = int.Parse(rd["SponsorshipID"].ToString());

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