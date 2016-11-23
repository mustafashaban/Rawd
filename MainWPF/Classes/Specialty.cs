using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Specialty
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



        private int? specialtyid;
        public int? SpecialtyID
        {
            get
            { return specialtyid; }
            set
            { specialtyid = value; }
        }



        private string specialtyvalue;
        public string SpecialtyValue
        {
            get
            { return specialtyvalue; }
            set
            { specialtyvalue = value; }
        }



        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            { date = value; }
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
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2Orphan_Specialty"
               , new SqlParameter("@ID", System.Data.SqlDbType.Int)
               , new SqlParameter("@OrphanID", OrphanID)
               , new SqlParameter("@SpecialtyID", SpecialtyID)
               , new SqlParameter("@SpecialtyValue", SpecialtyValue)
               , new SqlParameter("@Date", Date)
               , new SqlParameter("@Notes", Notes));
            return ID != null;
        }
        public bool UpdateData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateOrphan_Specialty"
               , new SqlParameter("@ID", ID)
               , new SqlParameter("@OrphanID", OrphanID)
               , new SqlParameter("@SpecialtyID", SpecialtyID)
               , new SqlParameter("@SpecialtyValue", SpecialtyValue)
               , new SqlParameter("@Date", Date)
               , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromOrphan_Specialty"
                , new SqlParameter("@ID", ID));
        }


        public static List<Specialty> GetAllSpecialtyByOrphanID(int? OrphanID, int? SpecialtyID)
        {
            List<Specialty> ss = new List<Specialty>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetSpecialtyAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr1);
            SqlParameter pr2 = new SqlParameter("@SpecialtyID", SpecialtyID);
            com.Parameters.Add(pr2);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while(rd.Read())
                {
                    Specialty x = new Specialty();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    x.OrphanID = OrphanID;
                    x.SpecialtyID = SpecialtyID;
                    x.SpecialtyValue = rd["SpecialtyValue"].ToString();
                    x.Notes = rd["Notes"].ToString();

                    ss.Add(x);
                }
                rd.Close();
            }
            catch
            {
                ss = new List<Specialty>();
            }
            finally
            {
                con.Close();
            }
            return ss;
        }
    }
}
