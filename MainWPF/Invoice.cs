using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainWPF
{
    public class Invoice : ModelViewContext
    {
        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private DateTime? createdate;
        public DateTime? CreateDate
        {
            get
            { return createdate; }
            set
            { createdate = value; }
        }

        private string receiver;
        public string Receiver
        {
            get
            { return receiver; }
            set
            { receiver = value; }
        }

        public Transition AddTransition()
        {
            if (Transitions == null)
                Transitions = new List<Transition>();
            var t = new Transition() { TransitionInvoice = this };
            Transitions.Add(t);
            return t;
        }

        private double? totalvalue;
        public double? TotalValue
        {
            set { totalvalue = value; }
            get
            {
                if (totalvalue.HasValue) return totalvalue;
                return Transitions == null || Transitions.Count == 0 ? 0 : Transitions.Sum(x => x.Value);
            }
        }

        private string barcode;
        public string Barcode
        {
            get
            { return barcode; }
            set
            { barcode = value; }
        }

        private int? lastuserid;
        public int? LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }

        private string description;
        public string Description
        {
            get
            { return description; }
            set
            { description = value; }
        }

        List<Transition> transitions;
        public List<Transition> Transitions
        {
            get { return transitions; }
            set { transitions = value; }
        }

        public static bool InsertData(Invoice x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_Invoice"
            , new SqlParameter("@ID", System.Data.SqlDbType.Int)
            , new SqlParameter("@CreateDate", x.CreateDate)
            , new SqlParameter("@Receiver", x.Receiver)
            , new SqlParameter("@TotalValue", x.TotalValue)
            , new SqlParameter("@Barcode", x.Barcode)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID)
            , new SqlParameter("@Description", x.Description));
            return x.ID.HasValue;
        }
        public static bool UpdateData(Invoice x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Invoice"
            , new SqlParameter("@ID", x.ID)
            , new SqlParameter("@CreateDate", x.CreateDate)
            , new SqlParameter("@Receiver", x.Receiver)
            , new SqlParameter("@TotalValue", x.TotalValue)
            , new SqlParameter("@Barcode", x.Barcode)
            , new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID)
            , new SqlParameter("@Description", x.Description));
        }



        public static bool DeleteData(Invoice x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Invoice"
            , new SqlParameter("@ID", x.ID));
        }
        public static Invoice GetInvoiceByID(int id)
        {
            Invoice x = new Invoice();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Invoice", con);
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
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    x.Receiver = rd["Receiver"].ToString();
                    if (!(rd["TotalValue"] is DBNull))
                        x.TotalValue = double.Parse(rd["TotalValue"].ToString());
                    x.Barcode = rd["Barcode"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Description = rd["Description"].ToString();
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
        public static List<Invoice> GetAllInvoice()
        {
            List<Invoice> xx = new List<Invoice>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Invoice", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Invoice x = new Invoice();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    x.Receiver = rd["Receiver"].ToString();
                    if (!(rd["TotalValue"] is DBNull))
                        x.TotalValue = double.Parse(rd["TotalValue"].ToString());
                    x.Barcode = rd["Barcode"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Description = rd["Description"].ToString();
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

        internal static List<Invoice> GetAllInvoiceByFamilyID(int FamilyID)
        {
            List<Invoice> xx = new List<Invoice>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_FamilyID_Invoice", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@FamilyID", FamilyID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Invoice x = new Invoice();

                    if (!(rd["ID"] is DBNull))
                        x.ID = int.Parse(rd["ID"].ToString());
                    if (!(rd["CreateDate"] is DBNull))
                        x.CreateDate = DateTime.Parse(rd["CreateDate"].ToString());
                    x.Receiver = rd["Receiver"].ToString();
                    if (!(rd["TotalValue"] is DBNull))
                        x.TotalValue = double.Parse(rd["TotalValue"].ToString());
                    x.Barcode = rd["Barcode"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
                    x.Description = rd["Description"].ToString();
                    x.Transitions = Transition.GetAllTransitionByInvoice(x);

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
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(Receiver))
            {
                isValid = false;
                this.SetError("Receiver", "يجب إدخال المستلم");
            }
            if (string.IsNullOrEmpty(Description))
            {
                isValid = false;
                this.SetError("Description", "يجب إدخال البيان");
            }
            foreach (var item in Transitions)
            {
                if (!item.IsValidate())
                {
                    isValid = false;
                    break;
                }
            }
            if (!isValid)
            {
                string s = "";
                foreach (var item in errors)
                {
                    s += item.Value + "\n";
                }
                MyMessageBox.Show(s);
            }
            return isValid;
        }
    }
}
