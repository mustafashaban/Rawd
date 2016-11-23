using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Sponsor_Orphan : ModelViewContext
    {


        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }



        private int? sponsorid;
        public int? SponsorID
        {
            get
            { return sponsorid; }
            set
            {
                sponsorid = value;
                this.ClearError("SponsorID");
                this.NotifyPropertyChanged("SponsorID");
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


        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            {
                date = value;
                this.ClearError("Date");
                this.NotifyPropertyChanged("Date");
            }
        }



        private DateTime? enddate;
        public DateTime? EndDate
        {
            get
            { return enddate; }
            set
            { enddate = value; }
        }



        private string sponsortype;
        public string SponsorType
        {
            get
            { return sponsortype; }
            set
            {
                sponsortype = value;
                this.ClearError("SponsorType");
                this.NotifyPropertyChanged("SponsorType");
            }
        }



        private string relationship;
        public string Relationship
        {
            get
            { return relationship; }
            set
            { relationship = value; }
        }



        private double? sponsorvalue;
        public double? SponsorValue
        {
            get
            { return sponsorvalue; }
            set
            { sponsorvalue = value; }
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
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2Orphan_Sponsor"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@SponsorID", SponsorID)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@EndDate", EndDate)
                , new SqlParameter("@SponsorType", SponsorType)
                , new SqlParameter("@Relationship", Relationship)
                , new SqlParameter("@SponsorValue", SponsorValue)
                , new SqlParameter("@Notes", Notes));
            return ID != null;
        }
        public bool UpdateData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateOrphan_Sponsor"
                , new SqlParameter("@ID", ID)
                , new SqlParameter("@SponsorID", SponsorID)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@EndDate", EndDate)
                , new SqlParameter("@SponsorType", SponsorType)
                , new SqlParameter("@Relationship", Relationship)
                , new SqlParameter("@SponsorValue", SponsorValue)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromOrphan_Sponsor"
                , new SqlParameter("@ID", ID));
        }

        public static List<string> GetAllSponsorTypes
        {
            get
            {
                List<string> SponsorType = new List<string>();
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_GetAllSponsorTypes", con);
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

        public static List<Sponsor_Orphan> GetSponsorAllByOrphanID(int? OrphanID)
        {
            var sos = new List<Sponsor_Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetOrphanSponsorAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Sponsor_Orphan x = new Sponsor_Orphan();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["SponsorID"] is DBNull))
                        x.SponsorID = System.Int32.Parse(rd["SponsorID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.SponsorType = rd["SponsorType"].ToString();
                    x.Relationship = rd["Relationship"].ToString();
                    if (!(rd["SponsorValue"] is DBNull))
                        x.SponsorValue = System.Single.Parse(rd["SponsorValue"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    sos.Add(x);
                }
                rd.Close();
            }
            catch
            {
                sos = new List<Sponsor_Orphan>();
            }
            finally
            {
                con.Close();
            }
            return sos;
        }




        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(SponsorType))
            {
                isValid = false;
                this.SetError("SponsorType", "يجب إدخال نوع الكفالة");
            }
            if (SponsorID == null)
            {
                isValid = false;
                this.SetError("SponsorID", "يجب إختيار كفيل");
            }
            if (Date == null)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ بداية الكفالة");
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
