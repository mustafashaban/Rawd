using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Orphan_Supervisor : ModelViewContext
    {

        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }



        private int? supervisorid;
        public int? SupervisorID
        {
            get
            { return supervisorid; }
            set
            {
                supervisorid = value;
                this.ClearError("SupervisorID");
                this.NotifyPropertyChanged("SupervisorID");
            }
        }



        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }



        private DateTime? endDate;
        public DateTime? EndDate
        {
            get
            { return endDate; }
            set
            { endDate = value; }
        }



        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            { date = value;
            this.ClearError("Date");
            this.NotifyPropertyChanged("Date");
            }
        }



        private string supervisortype;
        public string SupervisorType
        {
            get
            { return supervisortype; }
            set
            { supervisortype = value;
            this.ClearError("SupervisorType");
            this.NotifyPropertyChanged("SupervisorType");
            }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertData()
        {
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2Orphan_Supervisor"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@SupervisorID", SupervisorID)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@EndDate", EndDate)
                , new SqlParameter("@SupervisorType", SupervisorType)
                , new SqlParameter("@Notes", Notes));
            return ID != null;
        }
        public bool UpdateData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateOrphan_Supervisor"
                , new SqlParameter("@ID", ID)
                , new SqlParameter("@SupervisorID", SupervisorID)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@EndDate", EndDate)
                , new SqlParameter("@SupervisorType", SupervisorType)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromOrphan_Supervisor"
                , new SqlParameter("@ID", ID));
        }

        public static List<string> GetAllSupervisorTypes
        {
            get
            {
                List<string> SponsorType = new List<string>();
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_GetAllSupervisorTypes", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        SponsorType.Add(rd.GetString(0));
                    }
                }
                catch { SponsorType = new List<string>(); }
                finally { con.Close(); }
                return SponsorType;
            }
        }

        public static List<Orphan_Supervisor> GetOrphanSupervisorAllByOrphanID(int? id)
        {
            List<Orphan_Supervisor> oss = new List<Orphan_Supervisor>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanSupervisorAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Orphan_Supervisor x = new Orphan_Supervisor();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["SupervisorID"] is DBNull))
                        x.SupervisorID = System.Int32.Parse(rd["SupervisorID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    x.SupervisorType = rd["SupervisorType"].ToString();
                    x.Notes = rd["Notes"].ToString();

                    oss.Add(x);
                }
                rd.Close();
            }
            catch
            {
                oss = new List<Orphan_Supervisor>();
            }
            finally
            {
                con.Close();
            }
            return oss;
        }





        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(SupervisorType))
            {
                isValid = false;
                this.SetError("SupervisorType", "يجب إدخال نوع الإشراف");
            }
            if (SupervisorID == null)
            {
                isValid = false;
                this.SetError("SupervisorID", "يجب إختيار مشرف");
            }
            if (Date == null)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ بداية الإشراف");
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
