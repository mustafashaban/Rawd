using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Trained : ModelViewContext
    {
        public enum TrainedEnumType { يتيم = 1, طالب_علم, أم, حاضنة, وصي }
        public static IEnumerable<string> TrainedTypes
        { get { return Enum.GetValues(typeof(TrainedEnumType)).Cast<string>(); } }

        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private Training training;
        public Training Training
        {
            get
            { return training; }
            set
            { training = value; }
        }

        private TrainedEnumType? trainedtype;
        public TrainedEnumType? TrainedType
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
        public string TrainedName { get; set; }

        private bool isattended;
        public bool IsAttended
        {
            get
            { return isattended; }
            set
            { isattended = value; }
        }

        private bool isabiding;
        public bool IsAbiding
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

        private bool attendedexam;
        public bool AttendedExam
        {
            get
            { return attendedexam; }
            set
            { attendedexam = value; }
        }

        private bool takecertificate;
        public bool TakeCertificate
        {
            get
            { return takecertificate; }
            set
            { takecertificate = value; }
        }

        public static bool InsertData(Trained x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_Trained"
            , new SqlParameter("@ID", System.Data.SqlDbType.Int)
            , new SqlParameter("@TrainingID", x.Training.ID)
            , new SqlParameter("@TrainedType", (int)x.TrainedType)
            , new SqlParameter("@TrainedID", x.TrainedID)
            , new SqlParameter("@IsAttended", x.IsAttended)
            , new SqlParameter("@IsAbiding", x.IsAbiding)
            , new SqlParameter("@Evaluation", x.Evaluation)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@AttendedExam", x.AttendedExam)
            , new SqlParameter("@TakeCertificate", x.TakeCertificate));
            return x.ID.HasValue;
        }
        public static bool UpdateData(Trained x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Trained"
            , new SqlParameter("@ID", x.ID)
            , new SqlParameter("@TrainingID", x.Training.ID)
            , new SqlParameter("@TrainedType", (int)x.TrainedType)
            , new SqlParameter("@TrainedID", x.TrainedID)
            , new SqlParameter("@IsAttended", x.IsAttended)
            , new SqlParameter("@IsAbiding", x.IsAbiding)
            , new SqlParameter("@Evaluation", x.Evaluation)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@AttendedExam", x.AttendedExam)
            , new SqlParameter("@TakeCertificate", x.TakeCertificate));
        }
        public static bool DeleteData(Trained x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Trained"
            , new SqlParameter("@ID", x.ID));
        }
        public static List<Trained> GetAllTrainedByTraining(Training t)
        {
            List<Trained> xx = new List<Trained>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Trained_ByTrainingID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@TrainingID", t.ID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Trained x = new Trained();

                    x.ID = int.Parse(rd["ID"].ToString());
                    x.Training = t;
                    x.TrainedType = (TrainedEnumType)rd["TrainedType"];
                    x.TrainedID = int.Parse(rd["TrainedID"].ToString());
                    x.IsAttended = bool.Parse(rd["IsAttended"].ToString());
                    x.IsAbiding = bool.Parse(rd["IsAbiding"].ToString());
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.AttendedExam = bool.Parse(rd["AttendedExam"].ToString());
                    x.TrainedName = rd["TrainedName"].ToString();
                    x.TakeCertificate = bool.Parse(rd["TakeCertificate"].ToString());

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
            if (!TrainedType.HasValue)
            {
                isValid = false;
                this.SetError("TrainedType", "يجب إدخال نوع المتدرب");
            }
            if (!TrainedID.HasValue)
            {
                isValid = false;
                this.SetError("TrainedID", "يجب اختيار المتدرب");
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