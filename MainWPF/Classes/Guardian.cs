using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Guardian : Person
    {
        private int? guardianid;
        public int? GuardianID
        {
            get
            { return guardianid; }
            set
            { guardianid = value; }
        }

        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        private string job;
        public string Job
        {
            get
            { return job; }
            set
            { job = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }
        public static bool InsertData(Guardian x)
        {
            x.GuardianID = BaseDataBase._StoredProcedureReturnable("sp_Add_Guardian"
            , new SqlParameter("@GuardianID", SqlDbType.Int)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@Gender", x.Gender)
            , new SqlParameter("@DOB", x.DOB)
            , new SqlParameter("@Job", x.Job)
            , new SqlParameter("@Phone", x.Phone)
            , new SqlParameter("@Mobile", x.Mobile)
            , new SqlParameter("@Email", x.Email)
            , new SqlParameter("@MaritalStatus", x.MaritalStatus)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@PID", x.PID)
            , new SqlParameter("@FamilyID", x.FamilyID));
            return x.GuardianID.HasValue;
        }
        public static bool UpdateData(Guardian x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Guardian"
            , new SqlParameter("@GuardianID", x.GuardianID)
            , new SqlParameter("@Name", x.Name)
            , new SqlParameter("@Gender", x.Gender)
            , new SqlParameter("@DOB", x.DOB)
            , new SqlParameter("@Job", x.Job)
            , new SqlParameter("@Phone", x.Phone)
            , new SqlParameter("@Mobile", x.Mobile)
            , new SqlParameter("@Email", x.Email)
            , new SqlParameter("@MaritalStatus", x.MaritalStatus)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@PID", x.PID)
            , new SqlParameter("@FamilyID", x.FamilyID));
        }
        public static bool DeleteData(Guardian x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Guardian"
            , new SqlParameter("@GuardianID", x.GuardianID));
        }
        public static Guardian GetGuardianByID(int id)
        {
            Guardian x = new Guardian();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Guardian", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = int.Parse(rd["GuardianID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.PID = rd["PID"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = int.Parse(rd["FamilyID"].ToString());
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
        public static List<Guardian> GetAllGuardian()
        {
            List<Guardian> xx = new List<Guardian>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Guardian", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Guardian x = new Guardian();

                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = int.Parse(rd["GuardianID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.PID = rd["PID"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = int.Parse(rd["FamilyID"].ToString());
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
        public static List<Guardian> GetAllGuardianByFamily(Family f)
        {
            List<Guardian> xx = new List<Guardian>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_FamilyID_Guardian", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@FamilyID", f.FamilyID));
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Guardian x = new Guardian();

                    if (!(rd["GuardianID"] is DBNull))
                        x.GuardianID = int.Parse(rd["GuardianID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.PID = rd["PID"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = int.Parse(rd["FamilyID"].ToString());

                    if (x.Gender == "ذكر")
                        f.OrphanGuardian = x;
                    else
                        f.OrphanNursemaid = x;

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
            if (string.IsNullOrEmpty(Name))
            {
                isValid = false;
                this.SetError("Name", "يجب إدخال الاسم");
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
