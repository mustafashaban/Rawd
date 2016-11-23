using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class MergedFamily
    {

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }



        private int? studysideid;
        public int? StudySideID
        {
            get
            { return studysideid; }
            set
            { studysideid = value; }
        }




        private string familyname;
        public string FamilyName
        {
            get
            { return familyname; }
            set
            { familyname = value; }
        }

        private string familytype;
        public string FamilyType
        {
            get
            { return familytype; }
            set
            { familytype = value; }
        }

        private string familyidentityid;
        public string FamilyIdentityID
        {
            get
            { return familyidentityid; }
            set
            { familyidentityid = value; }
        }

        private string fatherfirstname;
        public string FatherFirstName
        {
            get
            { return fatherfirstname; }
            set
            { fatherfirstname = value; }
        }

        private string fatherlastname;
        public string FatherLastName
        {
            get
            { return fatherlastname; }
            set
            { fatherlastname = value; }
        }

        private string motherfirstname;
        public string MotherFirstName
        {
            get
            { return motherfirstname; }
            set
            { motherfirstname = value; }
        }

        private string motherlastname;
        public string MotherLastName
        {
            get
            { return motherlastname; }
            set
            { motherlastname = value; }
        }

        private int? childcount;
        public int? ChildCount
        {
            get
            { return childcount; }
            set
            { childcount = value; }
        }

        private string fatherpid;
        public string FatherPID
        {
            get
            { return fatherpid; }
            set
            { fatherpid = value; }
        }

        private string motherpid;
        public string MotherPID
        {
            get
            { return motherpid; }
            set
            { motherpid = value; }
        }

        private DateTime? applydate;
        public DateTime? ApplyDate
        {
            get
            { return applydate; }
            set
            { applydate = value; }
        }

        private DateTime? listdate;
        public DateTime? ListDate
        {
            get
            { return listdate; }
            set
            { listdate = value; }
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

        private DateTime? dateofevaluation;
        public DateTime? DateOfEvaluation
        {
            get
            { return dateofevaluation; }
            set
            { dateofevaluation = value; }
        }

        private bool iscanceled;
        public bool IsCanceled
        {
            get
            { return iscanceled; }
            set
            { iscanceled = value; }
        }

        private DateTime? canceldate;
        public DateTime? CancelDate
        {
            get
            { return canceldate; }
            set
            { canceldate = value; }
        }

        private string cancelreason;
        public string CancelReason
        {
            get
            { return cancelreason; }
            set
            { cancelreason = value; }
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
            return BaseDataBase._StoredProcedure("sp_Add2MergedFamilyTable"
                  , new SqlParameter("@FamilyID", FamilyID)
                  , new SqlParameter("@StudySideID", StudySideID)
                  , new SqlParameter("@FamilyName", FamilyName)
                  , new SqlParameter("@FamilyType", FamilyType)
                  , new SqlParameter("@FamilyIdentityID", FamilyIdentityID)
                  , new SqlParameter("@FatherFirstName", FatherFirstName)
                  , new SqlParameter("@FatherLastName", FatherLastName)
                  , new SqlParameter("@MotherFirstName", MotherFirstName)
                  , new SqlParameter("@MotherLastName", MotherLastName)
                  , new SqlParameter("@ChildCount", ChildCount)
                  , new SqlParameter("@FatherPID", FatherPID)
                  , new SqlParameter("@MotherPID", MotherPID)
                  , new SqlParameter("@ApplyDate", ApplyDate)
                  , new SqlParameter("@ListDate", ListDate)
                  , new SqlParameter("@Evaluation", Evaluation)
                  , new SqlParameter("@EvaluationReason", EvaluationReason)
                  , new SqlParameter("@DateOfEvaluation", DateOfEvaluation)
                  , new SqlParameter("@IsCanceled", IsCanceled)
                  , new SqlParameter("@CancelDate", CancelDate)
                  , new SqlParameter("@CancelReason", CancelReason)
                  , new SqlParameter("@Notes", Notes));
        }

        public static bool UpdateCurrentData(int StudySideID)
        {
            return BaseDataBase._StoredProcedure("sp_Merge_UpdateData"
                , new SqlParameter("@StudySideID", StudySideID));
        }

        public static List<MergedFamily> GetAllMergedFamily()
        {
            List<MergedFamily> mfs = new List<MergedFamily>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("GetMergedData1", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    MergedFamily x = new MergedFamily();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());

                    //x.StudySideID = BaseDataBase.StudySideID;

                    x.FamilyName = rd["FamilyName"].ToString();
                    x.FamilyType = rd["FamilyType"].ToString();
                    x.FamilyIdentityID = rd["FamilyIdentityID"].ToString();
                    x.FatherFirstName = rd["FatherFirstName"].ToString();
                    x.FatherLastName = rd["FatherLastName"].ToString();
                    x.MotherFirstName = rd["MotherFirstName"].ToString();
                    x.MotherLastName = rd["MotherLastName"].ToString();
                    if (!(rd["ChildCount"] is DBNull))
                        x.ChildCount = System.Int32.Parse(rd["ChildCount"].ToString());
                    x.FatherPID = rd["FatherPID"].ToString();
                    x.MotherPID = rd["MotherPID"].ToString();
                    if (!(rd["ApplyDate"] is DBNull))
                        x.ApplyDate = System.DateTime.Parse(rd["ApplyDate"].ToString());
                    x.Evaluation = rd["Evaluation"].ToString();
                    x.EvaluationReason = rd["EvaluationReason"].ToString();
                    if (!(rd["DateOfEvaluation"] is DBNull))
                        x.DateOfEvaluation = System.DateTime.Parse(rd["DateOfEvaluation"].ToString());
                    if (!(rd["ListDate"] is DBNull))
                        x.ListDate = System.DateTime.Parse(rd["ListDate"].ToString());
                    if (!(rd["IsCanceled"] is DBNull))
                        x.IsCanceled = System.Boolean.Parse(rd["IsCanceled"].ToString());
                    if (!(rd["CancelDate"] is DBNull))
                        x.CancelDate = System.DateTime.Parse(rd["CancelDate"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();

                    mfs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                mfs = new List<MergedFamily>();
            }
            finally
            {
                con.Close();
            }
            return mfs;
        }
    }
}
