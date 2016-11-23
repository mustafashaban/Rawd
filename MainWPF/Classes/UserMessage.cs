using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class UserMessage : ModelViewContext
    {
        private int? messageid;
        public int? MessageID
        {
            get
            { return messageid; }
            set
            { messageid = value; }
        }

        private int senderid;
        public int SenderID
        {
            get
            { return senderid; }
            set
            { senderid = value; }
        }

        private int recieverid;
        public int RecieverID
        {
            get
            { return recieverid; }
            set
            { recieverid = value; }
        }

        private string message;
        public string Message
        {
            get
            { return message; }
            set
            { message = value; }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            { return date; }
            set
            { date = value; }
        }

        private bool isreaded;
        public bool IsReaded
        {
            get
            { return isreaded; }
            set
            { isreaded = value; }
        }

        private DateTime? readdate;
        public DateTime? ReadDate
        {
            get
            { return readdate; }
            set
            { readdate = value; }
        }

        private bool isreceived;
        public bool IsReceived
        {
            get
            { return isreceived; }
            set
            { isreceived = value; }
        }

        private DateTime? receivedate;
        public DateTime? ReceiveDate
        {
            get
            { return receivedate; }
            set
            { receivedate = value; }
        }

        public bool IsRight
        { get { return SenderID == BaseDataBase.CurrentUser.ID.Value; } }

        public static bool InsertData(UserMessage x)
        {
            x.MessageID = BaseDataBase._StoredProcedureReturnable("sp_Add_UserMessage"
            , new SqlParameter("@MessageID", System.Data.SqlDbType.Int)
            , new SqlParameter("@SenderID", x.SenderID)
            , new SqlParameter("@RecieverID", x.RecieverID)
            , new SqlParameter("@Message", x.Message)
            , new SqlParameter("@Date", x.Date)
            , new SqlParameter("@IsReaded", x.IsReaded)
            , new SqlParameter("@ReadDate", x.ReadDate)
            , new SqlParameter("@IsReceived", x.IsReceived)
            , new SqlParameter("@ReceiveDate", x.ReceiveDate));
            return x.MessageID.HasValue;
        }
        public static bool DeleteData(UserMessage x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_UserMessage"
            , new SqlParameter("@MessageID", x.MessageID));
        }
        public enum MessagesWay { MessageBefore, MessageAfter }
        public static List<UserMessage> GetUserMessageByUserID(int MainUserID, int SubUserID, int MessageCount, MessagesWay Way, DateTime? MessageDate = null)
        {
            List<UserMessage> xx = new List<MainWPF.UserMessage>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_UserMessageByUserID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@MainUserID", MainUserID);
            SqlParameter pr2 = new SqlParameter("@SubUserID", SubUserID);
            SqlParameter pr3 = new SqlParameter("@MessageCount", MessageCount);
            SqlParameter pr4 = new SqlParameter("@Way", Way.ToString());
            com.Parameters.Add(pr1);
            com.Parameters.Add(pr2);
            com.Parameters.Add(pr3);
            com.Parameters.Add(pr4);

            if (MessageDate.HasValue)
                com.Parameters.Add(new SqlParameter("@MessageDate", MessageDate));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    UserMessage x = new UserMessage();
                    if (!(rd["MessageID"] is DBNull))
                        x.MessageID = int.Parse(rd["MessageID"].ToString());
                    if (!(rd["SenderID"] is DBNull))
                        x.SenderID = int.Parse(rd["SenderID"].ToString());
                    if (!(rd["RecieverID"] is DBNull))
                        x.RecieverID = int.Parse(rd["RecieverID"].ToString());
                    x.Message = rd["Message"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["IsReaded"] is DBNull))
                        x.IsReaded = bool.Parse(rd["IsReaded"].ToString());
                    if (!(rd["ReadDate"] is DBNull))
                        x.ReadDate = DateTime.Parse(rd["ReadDate"].ToString());
                    if (!(rd["IsReceived"] is DBNull))
                        x.IsReceived = bool.Parse(rd["IsReceived"].ToString());
                    if (!(rd["ReceiveDate"] is DBNull))
                        x.ReceiveDate = DateTime.Parse(rd["ReceiveDate"].ToString());
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