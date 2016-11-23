using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Trained
    {
        public enum TrainedEnumType { يتيم, والدة, مقيّم, مشرف, حاضنة }



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



        private TrainedEnumType trainedtype;
        public TrainedEnumType TrainedType
        {
            get
            { return trainedtype; }
            set
            { trainedtype = value; }
        }



        private int? trainedid;
        public int? TrainedID
        {
            get
            { return trainedid; }
            set
            { trainedid = value; }
        }



        private bool? isattended;
        public bool? IsAttended
        {
            get
            { return isattended; }
            set
            { isattended = value; }
        }



        private bool? isabiding;
        public bool? IsAbiding
        {
            get
            { return isabiding; }
            set
            { isabiding = value; }
        }



        private string evaluation;
        public string Evaluation
        {
            get
            { return evaluation; }
            set
            { evaluation = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }



        public bool InsertTrainedData()
        {
            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2Trained"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@TrainingID", TrainingID)
                , new SqlParameter("@TrainedType", TrainedType)
                , new SqlParameter("@TrainedID", TrainedID)
                , new SqlParameter("@IsAttended", IsAttended)
                , new SqlParameter("@IsAbiding", IsAbiding)
                , new SqlParameter("@Evaluation", Evaluation)
                , new SqlParameter("@Notes", Notes));
            return ID != null;
        }
        public bool UpdateTrainedData()
        {
            return BaseDataBase._StoredProcedure("sp_UpdateTrained"
              , new SqlParameter("@ID", ID)
              , new SqlParameter("@TrainingID", TrainingID)
              , new SqlParameter("@TrainedType", TrainedType)
              , new SqlParameter("@TrainedID", TrainedID)
              , new SqlParameter("@IsAttended", IsAttended)
              , new SqlParameter("@IsAbiding", IsAbiding)
              , new SqlParameter("@Evaluation", Evaluation)
              , new SqlParameter("@Notes", Notes));
        }
        public bool DeleteTrainedData()
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromTrained",
                new SqlParameter("@ID", ID));
        }

        public static Trained GetTrainedByID(int? id)
        {
            Trained x = new Trained();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTrainedByID", con);
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
                    if (!(rd["TrainingID"] is DBNull))
                        x.TrainingID = System.Int32.Parse(rd["TrainingID"].ToString());
                    x.TrainedType = (Trained.TrainedEnumType)int.Parse(rd["TrainedType"].ToString());
                    if (!(rd["TrainedID"] is DBNull))
                        x.TrainedID = System.Int32.Parse(rd["TrainedID"].ToString());
                    if (!(rd["IsAttended"] is DBNull))
                        x.IsAttended = System.Boolean.Parse(rd["IsAttended"].ToString());
                    if (!(rd["IsAbiding"] is DBNull))
                        x.IsAbiding = System.Boolean.Parse(rd["IsAbiding"].ToString());
                    x.Evaluation = rd["Evaluation"].ToString();
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
        public static List<Trained> GetTrainedAllByTrainingID(int? TrainingID)
        {
            List<Trained> b = new List<Trained>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTrainedAllByTrainingID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@TrainingID", TrainingID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetTrainedByID(rd.GetInt32(0)));
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
