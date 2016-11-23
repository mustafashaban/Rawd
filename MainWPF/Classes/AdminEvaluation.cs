using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class AdminEvaluation : ModelViewContext
    {


        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }



        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }



        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }



        private string evaluator;
        public string Evaluator
        {
            get
            { return evaluator; }
            set
            { evaluator = value; }
        }



        private string evaluation;
        public string Evaluation
        {
            get
            { return evaluation; }
            set
            {
                evaluation = value;
                this.ClearError("Evaluation");
                this.NotifyPropertyChanged("Evaluation");
            }
        }

        private string reason;
        public string Reason
        {
            get
            { return reason; }
            set
            { reason = value; }
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



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public static AdminEvaluation GetAdminEvaluationByID(int? id)
        {
            AdminEvaluation x = new AdminEvaluation();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAdminEvaluationByID", con);
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
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    x.Evaluator = rd["Evaluator"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.Reason = rd["Reason"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    x.Notes = rd["Notes"].ToString();
                }
                rd.Close();
            }
            catch
            {
                x = new AdminEvaluation();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<AdminEvaluation> GetAdminEvaluationsByFamilyID(int? FamilyID)
        {
            List<AdminEvaluation> aes = new List<AdminEvaluation>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAdminEvaluationsByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    AdminEvaluation x = new AdminEvaluation();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    x.Evaluator = rd["Evaluator"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.Reason = rd["Reason"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    aes.Add(x);
                }
                rd.Close();
            }
            catch
            {
                aes = new List<AdminEvaluation>();
            }
            finally
            {
                con.Close();
            }
            return aes;
        }
        public static List<AdminEvaluation> GetAdminEvaluationByOrphanID(int? OrphanID)
        {
            List<AdminEvaluation> aes = new List<AdminEvaluation>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetAdminEvaluationsByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    AdminEvaluation x = new AdminEvaluation();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    x.Evaluator = rd["Evaluator"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.Reason = rd["Reason"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    x.Notes = rd["Notes"].ToString();

                    aes.Add(x);
                }
                rd.Close();
            }
            catch
            {
                aes = new List<AdminEvaluation>();
            }
            finally
            {
                con.Close();
            }
            return aes;
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(Evaluation))
            {
                isValid = false;
                this.SetError("Evaluation", "يجب إدخال التقييم");
            }
            if (Date == null)
            {
                isValid = false;
                this.SetError("Date", "يجب إدخال تاريخ التقييم");
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
