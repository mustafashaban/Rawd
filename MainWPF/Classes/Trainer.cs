using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Trainer
    {


        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }



        private int? trainingid;
        public int? TrainingID
        {
            get
            { return trainingid; }
            set
            { trainingid = value; }
        }



        private string trainername;
        public string TrainerName
        {
            get
            { return trainername; }
            set
            { trainername = value; }
        }



        private string trainerqualification;
        public string TrainerQualification
        {
            get
            { return trainerqualification; }
            set
            { trainerqualification = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public bool InsertTrainerData()
        {
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2Trainer"
                , new SqlParameter("@ID",System.Data.SqlDbType.Int)
                , new SqlParameter("@TrainingID", TrainingID)
                , new SqlParameter("@TrainerName", TrainerName)
                , new SqlParameter("@TrainerQualification", TrainerQualification)
                , new SqlParameter("@Notes", Notes));
            return ID != null;
        }
        public bool UpdateTrainerData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateTrainer"
                , new SqlParameter("@ID", ID)
                , new SqlParameter("@TrainingID", TrainingID)
                , new SqlParameter("@TrainerName", TrainerName)
                , new SqlParameter("@TrainerQualification", TrainerQualification)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteTrainerData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromTrainer"
                , new SqlParameter("@ID", ID));
        }

        public static Trainer GetTrainerByID(int? id)
        {
            Trainer x = new Trainer();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTrainerByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@TrainerID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    if (!(rd["TrainingID"] is DBNull))
                        x.TrainingID = System.Int32.Parse(rd["TrainingID"].ToString());
                    x.TrainerName = rd["TrainerName"].ToString();
                    x.TrainerQualification = rd["TrainerQualification"].ToString();
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
        public static List<Trainer> GetTrainerAllByTraining(int? id)
        {
            List<Trainer> b = new List<Trainer>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTrainerAllByTrainingID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@TrainingID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetTrainerByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }
    }
}
