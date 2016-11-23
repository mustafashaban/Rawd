using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Guardian_Orphan : ModelViewContext
    {

        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }

        private int? familyID;
        public int? FamilyID
        {
            get
            { return familyID; }
            set
            { familyID = value; }
        }

        private int? guardianid;
        public int? GuardianID
        {
            get
            { return guardianid; }
            set
            {
                guardianid = value;
                this.ClearError("GuardianID");
                this.NotifyPropertyChanged("GuardianID");
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



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertGuardianOrphanData()
        {
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2Orphan_Guardian"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@FamilyID", FamilyID)
                , new SqlParameter("@GuardianID", GuardianID)
                , new SqlParameter("@Relationship", Relationship)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@EndDate", EndDate)
                , new SqlParameter("@Notes", Notes));
            return (ID != null);
        }
        public bool UpdateOrphan_GuardianData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateOrphan_Guardian"
                , new SqlParameter("@ID", ID)
                , new SqlParameter("@OrphanID", OrphanID)
                , new SqlParameter("@FamilyID", FamilyID)
                , new SqlParameter("@GuardianID", GuardianID)
                , new SqlParameter("@Relationship", Relationship)
                , new SqlParameter("@Date", Date)
                , new SqlParameter("@EndDate", EndDate)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteOrphan_GuardianData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromOrphan_Guardian"
                , new SqlParameter("@ID", ID));
        }


        public static Guardian_Orphan GetGuardianOrphanByID(int id)
        {
            Guardian_Orphan x = new Guardian_Orphan();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetGuardian_OrphanByID", con);
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
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = System.Int32.Parse(rd["GuardianID"].ToString());
                    x.Relationship = rd["Relationship"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["Notes"].ToString();
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
        public static List<Guardian_Orphan> GetGuardianAllByOrphanID(int? OrphanID)
        {
            List<Guardian_Orphan> gos = new List<Guardian_Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetGuardianALLByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Guardian_Orphan x = new Guardian_Orphan();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = System.Int32.Parse(rd["GuardianID"].ToString());
                    x.Relationship = rd["Relationship"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    gos.Add(x);
                }
                rd.Close();
            }
            catch
            {
                gos = new List<Guardian_Orphan>();
            }
            finally
            {
                con.Close();
            }
            return gos;
        }
        public static List<Guardian_Orphan> GetGuardianAllByFamilyID(int? FamilyID)
        {
            List<Guardian_Orphan> gos = new List<Guardian_Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetGuardianALLByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Guardian_Orphan x = new Guardian_Orphan();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = System.Int32.Parse(rd["GuardianID"].ToString());
                    x.Relationship = rd["Relationship"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    gos.Add(x);
                }
                rd.Close();
            }
            catch
            {
                gos = new List<Guardian_Orphan>();
            }
            finally
            {
                con.Close();
            }
            return gos;
        }

        public static List<Guardian_Orphan> GetNursemaidAllAllByOrphanID(int? OrphanID)
        {
            List<Guardian_Orphan> gos = new List<Guardian_Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetNursemaidALLByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Guardian_Orphan x = new Guardian_Orphan();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = System.Int32.Parse(rd["GuardianID"].ToString());
                    x.Relationship = rd["Relationship"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    gos.Add(x);
                }
                rd.Close();
            }
            catch
            {
                gos = new List<Guardian_Orphan>();
            }
            finally
            {
                con.Close();
            }
            return gos;
        }
        public static List<Guardian_Orphan> GetNursemaidAllByFamilyID(int? FamilyID)
        {
            List<Guardian_Orphan> gos = new List<Guardian_Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetNursemaidALLByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Guardian_Orphan x = new Guardian_Orphan();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = System.Int32.Parse(rd["GuardianID"].ToString());
                    x.Relationship = rd["Relationship"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    gos.Add(x);
                }
                rd.Close();
            }
            catch
            {
                gos = new List<Guardian_Orphan>();
            }
            finally
            {
                con.Close();
            }
            return gos;
        }




        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (GuardianID == null)
            {
                isValid = false;
                this.SetError("GuardianID", "يجب إختيار وصي أو حاضنة");
            }
            if (Date == null)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ الحضانة أو الوصاية");
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
