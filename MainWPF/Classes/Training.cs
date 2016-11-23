using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Training : ModelViewContext
    {
        private int? trainingid;
        public int? TrainingID
        {
            get
            { return trainingid; }
            set
            { trainingid = value; }
        }



        private string trainingname;
        public string TrainingName
        {
            get
            { return trainingname; }
            set
            {
                trainingname = value;
                this.ClearError("TrainingName");
                this.NotifyPropertyChanged("TrainingName");
            }
        }



        private DateTime? trainingdate;
        public DateTime? TrainingDate
        {
            get
            { return trainingdate; }
            set
            {
                trainingdate = value;
                this.ClearError("TrainingDate");
                this.NotifyPropertyChanged("TrainingDate");
            }
        }



        private DateTime? trainingenddate;
        public DateTime? TrainingEndDate
        {
            get
            { return trainingenddate; }
            set
            { trainingenddate = value; }
        }



        private string traininggoal;
        public string TrainingGoal
        {
            get
            { return traininggoal; }
            set
            { traininggoal = value; }
        }



        private bool isprivatetraining = false;
        public bool IsPrivateTraining
        {
            get
            { return isprivatetraining; }
            set
            {
                isprivatetraining = value;
            }
        }

        private string privateSide;
        public string PrivateSide
        {
            get
            { return privateSide; }
            set
            { privateSide = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertTrainingData()
        {
            TrainingID = BaseDataBase._StoredProcedureReturnable("sp_Add2Training"
                , new SqlParameter("@TrainingID", System.Data.SqlDbType.Int)
                 , new SqlParameter("@TrainingName", TrainingName)
                 , new SqlParameter("@TrainingDate", TrainingDate)
                 , new SqlParameter("@TrainingEndDate", TrainingEndDate)
                 , new SqlParameter("@TrainingGoal", TrainingGoal)
                 , new SqlParameter("@IsPrivateTraining", IsPrivateTraining)
                 , new SqlParameter("@Notes", Notes)
                 , new SqlParameter("@PrivateSide", PrivateSide));
            return TrainingID != null;
        }
        public bool UpdateTrainingData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateTraining"
                , new SqlParameter("@TrainingID", TrainingID)
                , new SqlParameter("@TrainingName", TrainingName)
                , new SqlParameter("@TrainingDate", TrainingDate)
                , new SqlParameter("@TrainingEndDate", TrainingEndDate)
                , new SqlParameter("@TrainingGoal", TrainingGoal)
                , new SqlParameter("@IsPrivateTraining", IsPrivateTraining)
                , new SqlParameter("@Notes", Notes)
                , new SqlParameter("@PrivateSide", PrivateSide));
        }
        public bool DeleteTrainingData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromTraining"
                , new SqlParameter("@TrainingID", TrainingID));
        }

        public static DataView GetAllTrainingsTable
        {
            get
            {
                return BaseDataBase._TablingStoredProcedure("sp_GetAllTrainingTable").DefaultView;
            }
        }


        public static Training GetTrainingByID(int? id)
        {
            Training x = new Training();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTrainingByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@TrainingID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["TrainingID"] is DBNull))
                        x.TrainingID = System.Int32.Parse(rd["TrainingID"].ToString());
                    x.TrainingName = rd["TrainingName"].ToString();
                    if (!(rd["TrainingDate"] is DBNull))
                        x.TrainingDate = System.DateTime.Parse(rd["TrainingDate"].ToString());
                    if (!(rd["TrainingEndDate"] is DBNull))
                        x.TrainingEndDate = System.DateTime.Parse(rd["TrainingEndDate"].ToString());
                    x.TrainingGoal = rd["TrainingGoal"].ToString();
                    if (!(rd["IsPrivateTraining"] is DBNull))
                        x.IsPrivateTraining = System.Boolean.Parse(rd["IsPrivateTraining"].ToString());
                    x.PrivateSide = rd["PrivateSide"].ToString();
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
        public static List<Training> GetTrainingAllByTrainedID(int? TrainedID)
        {
            List<Training> b = new List<Training>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTrainingAllByTrainedID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@TrainedID", TrainedID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetTrainingByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(TrainingName))
            {
                isValid = false;
                this.SetError("TrainingName", "يجب إدخال اسم التدريب");
            }
            if (TrainingDate == null)
            {
                isValid = false;
                this.SetError("TrainingDate", "يجب إدخال تاريخ بداية التدريب");
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
