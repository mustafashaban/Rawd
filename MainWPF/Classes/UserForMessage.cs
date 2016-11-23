using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainWPF
{
    public class UserForMessage : User
    {
        public static new List<UserForMessage> AllUsers { get; set; }
        public UserForMessage()
        {
            Messages = new List<UserMessage>();
        }
        private List<UserMessage> messages;
        public List<UserMessage> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
        public int UnreadedMessages { get; set; }
        public bool ShowNotification { get { return UnreadedMessages > 0 ? true : false; } }
        public UserMessage lastMessage;
        public UserMessage LastMessage { get { return messages == null || messages.Count == 0 ? lastMessage : messages[messages.Count - 1]; } }
        public int MessagesCount { get { return Messages == null ? 0 : Messages.Count; } }

        //static
        public static List<UserForMessage> GetAllUsers()
        {
            List<UserForMessage> users = new List<UserForMessage>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand($"select ID, Name, dbo.fn_LastMessage({BaseDataBase.CurrentUser.ID},ID) LastMessage, dbo.fn_LastMessageDate({BaseDataBase.CurrentUser.ID},ID) LastMessageDate from [Users]", con);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    if ((int)rd["ID"] == BaseDataBase.CurrentUser.ID)
                        continue;
                    UserForMessage x = new UserForMessage();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    x.Name = rd["Name"].ToString();
                    if (rd["LastMessage"].ToString() != "")
                    {
                        x.lastMessage = new UserMessage();
                        x.lastMessage.Message = rd["LastMessage"].ToString();
                        x.lastMessage.Date = (DateTime)rd["LastMessageDate"];
                    }
                    users.Add(x);
                }
                rd.Close();
            }
            catch
            {
                users = null;
            }
            finally
            {
                con.Close();
            }
            return users;
        }
        //static
        public async static Task<int> GetUnReadedMessages()
        {
            int num = 0;
            var dt = await BaseDataBase._TablingStoredProcedureAsync("GetUnReadedMessages",
                new SqlParameter("@RecieverID", BaseDataBase.CurrentUser.ID));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    var u = AllUsers.Where(x => x.ID == (int)item.ItemArray[0]).FirstOrDefault();
                    if (u != null)
                    {
                        u.UnreadedMessages = (int)item.ItemArray[1];
                        u.NotifyPropertyChanged("UnreadedMessages");
                        u.NotifyPropertyChanged("ShowNotification");
                    }
                }
                num = AllUsers.Sum(x => x.UnreadedMessages);
            }
            return num;
        }


        public void MakeMessagesReaded()
        {
            UnreadedMessages = 0;
            NotifyPropertyChanged("UnreadedMessages");
            NotifyPropertyChanged("ShowNotification");

            if (AllUsers.Sum(x => x.UnreadedMessages) == 0)
                (App.Current.MainWindow as MainWindow).brdrMessage.Visibility = Visibility.Collapsed;
        }

        public async void CheckMessages()
        {
            var dt = await BaseDataBase._TablingStoredProcedureAsync("CheckMessages",
                new SqlParameter("@BaseUserID", BaseDataBase.CurrentUser.ID), 
                new SqlParameter("@SubUserID", ID));
            if (dt != null && dt.Rows.Count > 0 && (int)dt.Rows[0].ItemArray[0] != 0)
                foreach (var item in Messages.Where(x => !x.IsReaded && x.MessageID <= (int)dt.Rows[0].ItemArray[0]))
                {
                    item.IsReaded = true;
                    item.NotifyPropertyChanged("IsReaded");
                }
            if (dt != null && dt.Rows.Count > 0 && (int)dt.Rows[0].ItemArray[1] != 0)
                foreach (var item in Messages.Where(x => !x.IsReceived && x.MessageID <= (int)dt.Rows[0].ItemArray[1]))
                {
                    item.IsReceived = true;
                    item.NotifyPropertyChanged("IsReceived");
                }
        }

        public void MakeAllMessagesRecieved()
        {
            foreach (var item in Messages.Where(x => !x.IsReceived))
                item.IsReceived = true;
            UnreadedMessages = 0;
            NotifyPropertyChanged("UnreadedMessages");
            NotifyPropertyChanged("ShowNotification");
        }
        public void GetAllMessages()
        {
            if (Messages == null || Messages.Count == 0)
            {
                GetFirstMessages();
                MakeMessagesReaded();
            }
        }
        public bool GetFirstMessages()
        {
            var msgs = UserMessage.GetUserMessageByUserID(BaseDataBase.CurrentUser.ID.Value, ID.Value, 10, UserMessage.MessagesWay.MessageBefore, Messages == null || Messages.Count == 0 ? null : Messages?[0]?.Date);
            if (msgs != null && msgs.Count > 0)
            {
                messages.InsertRange(0, msgs);
                NotifyPropertyChanged("Messages");
                return true;
            }
            return false;
        }

        public async Task<bool> GetLastMessages()
        {
            DateTime? date = null;
            if (Messages != null && Messages.Count > 0)
                date = Messages.LastOrDefault(x => x.SenderID == ID.Value)?.Date;
            List<UserMessage> msgs = await Task.Run(() => msgs = UserMessage.GetUserMessageByUserID(BaseDataBase.CurrentUser.ID.Value, ID.Value, 10, UserMessage.MessagesWay.MessageAfter, date));
            if (msgs != null && msgs.Count > 0)
            {
                if (Messages.Count > 0)
                    Messages.AddRange(msgs.Where(x => x.SenderID == ID.Value));
                else Messages.AddRange(msgs);
                MakeAllMessagesRecieved();
                NotifyPropertyChanged("LastMessage");
                NotifyPropertyChanged("ShowNotification");
                NotifyPropertyChanged("UnreadedMessages");
                NotifyPropertyChanged("Messages");
                return true;
            }
            return false;
        }
        public void AddMessages(UserMessage msg)
        {
            msg.Date = BaseDataBase.DateNow;
            msg.SenderID = BaseDataBase.CurrentUser.ID.Value;
            msg.RecieverID = ID.Value;
            if (UserMessage.InsertData(msg))
            {
                messages.Add(msg);
                NotifyPropertyChanged("LastMessage");
                NotifyPropertyChanged("Messages");
            }
        }
    }
}
