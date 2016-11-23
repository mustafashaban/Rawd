using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class TeachingStage : ModelViewContext
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


        private string studystage;
        public string StudyStage
        {
            get
            { return studystage; }
            set
            { studystage = value;
            this.ClearError("StudyStage");
            this.NotifyPropertyChanged("StudyStage");
            }
        }


        private string school;
        public string School
        {
            get
            { return school; }
            set
            { school = value; }
        }


        private string _class;
        public string Class
        {
            get
            { return _class; }
            set
            { _class = value;
            this.ClearError("Class");
            this.NotifyPropertyChanged("Class");
            }
        }


        private string evaluation;
        public string Evaluation
        {
            get
            { return evaluation; }
            set
            { evaluation = value; }
        }


        private string evaluationreason;
        public string EvaluationReason
        {
            get
            { return evaluationreason; }
            set
            { evaluationreason = value; }
        }


        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            { date = value; }
        }


        private bool isstopped;
        public bool IsStopped
        {
            get
            { return isstopped; }
            set
            { isstopped = value; }
        }


        private string stopreason;
        public string StopReason
        {
            get
            { return stopreason; }
            set
            { stopreason = value; }
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
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2TeachingStage"
                    , new SqlParameter("@ID",System.Data.SqlDbType.Int)
                    , new SqlParameter("@OrphanID", OrphanID)
                    , new SqlParameter("@StudyStage", StudyStage)
                    , new SqlParameter("@School", School)
                    , new SqlParameter("@Class", Class)
                    , new SqlParameter("@Evaluation", Evaluation)
                    , new SqlParameter("@EvaluationReason", EvaluationReason)
                    , new SqlParameter("@Date", Date)
                    , new SqlParameter("@IsStopped", IsStopped)
                    , new SqlParameter("@StopReason", StopReason)
                    , new SqlParameter("@Notes", Notes));
            return (ID != null);
        }
        public bool UpdateData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateTeachingStage"
                    , new SqlParameter("@ID", ID)
                    , new SqlParameter("@OrphanID", OrphanID)
                    , new SqlParameter("@StudyStage", StudyStage)
                    , new SqlParameter("@School", School)
                    , new SqlParameter("@Class", Class)
                    , new SqlParameter("@Evaluation", Evaluation)
                    , new SqlParameter("@EvaluationReason", EvaluationReason)
                    , new SqlParameter("@Date", Date)
                    , new SqlParameter("@IsStopped", IsStopped)
                    , new SqlParameter("@StopReason", StopReason)
                    , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromTeachingStage"
                , new SqlParameter("@ID", ID));
        }

        public static TeachingStage GetTeachingStageByID(int? id)
        {
            var x = new TeachingStage();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTeachingStageByID", con);
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
                    x.StudyStage = rd["StudyStage"].ToString();
                    x.School = rd["School"].ToString();
                    x.Class = rd["Class"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.EvaluationReason = rd["EvaluationReason"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["IsStopped"] is DBNull))
                        x.IsStopped = System.Boolean.Parse(rd["IsStopped"].ToString());
                    x.StopReason = rd["StopReason"].ToString();
                    x.Notes = rd["Notes"].ToString();
                }
                rd.Close();
            }
            catch
            {
                x = new TeachingStage();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<TeachingStage> GetTeachingStageByOrphanID(int? OrphanID)
        {
            var tss = new List<TeachingStage>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTeachingStageAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    TeachingStage x = new TeachingStage();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    x.StudyStage = rd["StudyStage"].ToString();
                    x.School = rd["School"].ToString();
                    x.Class = rd["Class"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.EvaluationReason = rd["EvaluationReason"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["IsStopped"] is DBNull))
                        x.IsStopped = System.Boolean.Parse(rd["IsStopped"].ToString());
                    x.StopReason = rd["StopReason"].ToString();
                    x.Notes = rd["Notes"].ToString();

                    tss.Add(x);
                }
                rd.Close();
            }
            catch
            {
                tss = new List<TeachingStage>();
            }
            finally
            {
                con.Close();
            }
            return tss;
        }



        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(Class))
            {
                isValid = false;
                this.SetError("Class", "يجب إدخال الصف أو التخصص");
            }
            if (string.IsNullOrEmpty(StudyStage))
            {
                isValid = false;
                this.SetError("StudyStage", "يجب إدخال المرحلة الدراسية");
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