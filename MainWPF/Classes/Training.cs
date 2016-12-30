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
        public enum TrainingType { نشاط = 0, حفلة, دورة, تدريب };
        public static List<string> TrainingTypes
        { get { return Enum.GetNames(typeof(TrainingType)).ToList(); } }


        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        private DateTime? startdate;
        public DateTime? StartDate
        {
            get
            { return startdate; }
            set
            { startdate = value; }
        }

        private DateTime? enddate;
        public DateTime? EndDate
        {
            get
            { return enddate; }
            set
            { enddate = value; }
        }

        private string traininggoal;
        public string TrainingGoal
        {
            get
            { return traininggoal; }
            set
            { traininggoal = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private bool iscancelled;
        public bool IsCancelled
        {
            get
            { return iscancelled; }
            set
            { iscancelled = value; }
        }

        private string cancelreason;
        public string CancelReason
        {
            get
            { return cancelreason; }
            set
            { cancelreason = value; }
        }

        private TrainingType? type;
        public TrainingType? Type
        {
            get
            { return type; }
            set
            {
                type = value;
                selectedType = (int)value;
            }
        }
        private int selectedType = -1;
        public int SelectedType
        {
            get
            { return selectedType; }
            set
            {
                selectedType = value;
                type = (TrainingType)value;
            }
        }
        public string TypeText
        {
            get { return selectedType == -1 ? "" : TrainingTypes[selectedType]; }
        }

        private string trainer;
        public string Trainer
        {
            get
            { return trainer; }
            set
            { trainer = value; }
        }

        private List<Trained> trainees;
        public List<Trained> Trainees
        {
            get
            {
                if (trainees == null)
                    trainees = new List<Trained>();
                return trainees;
            }
            set
            { trainees = value; }
        }

        public static bool InsertData(Training x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add_Training"
            , new SqlParameter("@ID", SqlDbType.Int)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@TrainingGoal", x.TrainingGoal)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@IsCancelled", x.IsCancelled)
            , new SqlParameter("@CancelReason", x.CancelReason)
            , new SqlParameter("@Trainer", x.Trainer)
            , new SqlParameter("@Type", (int)x.Type));
            return x.ID.HasValue;
        }
        public static bool UpdateData(Training x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Training"
            , new SqlParameter("@ID", x.ID)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@StartDate", x.StartDate)
            , new SqlParameter("@EndDate", x.EndDate)
            , new SqlParameter("@TrainingGoal", x.TrainingGoal)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@IsCancelled", x.IsCancelled)
            , new SqlParameter("@CancelReason", x.CancelReason)
            , new SqlParameter("@Trainer", x.Trainer)
            , new SqlParameter("@Type", (int)x.Type));
        }
        public static bool DeleteData(Training x)
        {
            foreach (var t in x.Trainees)
                Trained.DeleteData(t);

            return BaseDataBase._StoredProcedure("sp_Delete_Training"
            , new SqlParameter("@ID", x.ID));
        }
        public static Training GetTrainingByID(int id)
        {
            Training x = new Training();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Training", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    x.ID = int.Parse(rd["ID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.TrainingGoal = rd["TrainingGoal"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.IsCancelled = bool.Parse(rd["IsCancelled"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();
                    x.Trainer = rd["Trainer"].ToString();
                    x.Type = (TrainingType)rd["Type"];
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
        public static List<Training> GetAllTraining()
        {
            List<Training> xx = new List<Training>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Training", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Training x = new Training();

                    x.ID = int.Parse(rd["ID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.StartDate = DateTime.Parse(rd["StartDate"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = DateTime.Parse(rd["EndDate"].ToString());
                    x.TrainingGoal = rd["TrainingGoal"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.IsCancelled = bool.Parse(rd["IsCancelled"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();
                    x.Trainer = rd["Trainer"].ToString();
                    x.Type = (TrainingType)rd["Type"];

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
        public void GetTrainees()
        {
            if (ID.HasValue)
            {
                Trainees = Trained.GetAllTrainedByTraining(this);
            }
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(Name))
            {
                isValid = false;
                this.SetError("Name", "يجب إدخال اسم التدريب");
            }
            if (string.IsNullOrEmpty(Trainer))
            {
                isValid = false;
                this.SetError("Trainer", "يجب إدخال اسم المدرب");
            }
            if (StartDate == null)
            {
                isValid = false;
                this.SetError("StartDate", "يجب إدخال تاريخ بداية التدريب");
            }
            else if (EndDate.HasValue && (EndDate.Value - StartDate.Value).Days < 0)
            {
                isValid = false;
                this.SetError("EndDate", "تاريخ نهاية التدريب يجب أن يكون اكبر من تاريخ البداية");
            }
            if (Type == null)
            {
                isValid = false;
                this.SetError("Type", "يجب إدخال نوع التدريب");
            }
            if (Trainees.Count == 0)
            {
                isValid = false;
                this.SetError("Trainees", "لم يتم اضافة اي متدرب بعد");
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
