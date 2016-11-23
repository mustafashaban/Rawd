using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class TempChild : ModelViewContext
    {
        private int? childid;
        public int? ChildID
        {
            get
            { return childid; }
            set
            { childid = value; }
        }

        private int? tempFamilyid;
        public int? TempFamilyID
        {
            get
            { return tempFamilyid; }
            set
            { tempFamilyid = value; }
        }

        private string name;
        public string Name
        {
            get
            { return name; }
            set
            {
                name = value;
                ClearError("Name");
            }
        }

        private DateTime? dob;
        public DateTime? DOB
        {
            get
            { return dob; }
            set
            {
                dob = value;
                ClearError("DOB");
            }
        }

        private string gender;
        public string Gender
        {
            get
            { return gender; }
            set
            {
                gender = value;
                ClearError("Gender");
            }
        }


        public static bool InsertData(TempChild tc)
        {
            tc.ChildID = BaseDataBase._StoredProcedureReturnable("sp_Add2TempChild"
                , new SqlParameter("@ChildID", System.Data.SqlDbType.Int)
                , new SqlParameter("@TempFamilyID", tc.TempFamilyID)
                , new SqlParameter("@Name", tc.Name)
                , new SqlParameter("@DOB", tc.DOB)
                , new SqlParameter("@Gender", tc.Gender));

            return tc.ChildID.HasValue;
        }
        public static bool UpdateData(TempChild tc)
        {
            return BaseDataBase._StoredProcedure("sp_UpdateTempChild"
                , new SqlParameter("@ChildID", tc.ChildID)
                , new SqlParameter("@TempFamilyID", tc.TempFamilyID)
                , new SqlParameter("@Name", tc.Name)
                , new SqlParameter("@DOB", tc.DOB)
                , new SqlParameter("@Gender", tc.Gender));
        }
        public static bool DeleteData(TempChild tc)
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromTempChild"
                , new SqlParameter("@ChildID", tc.ChildID));
        }

        public static List<TempChild> GetTempChildsByTempFamilyID(int id)
        {
            List<TempChild> tcs = new List<TempChild>();

            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTempChildsAllByTempFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@TempFamilyID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    TempChild tc = new TempChild();
                    if (!(rd["ChildID"] is DBNull))
                        tc.ChildID = System.Int32.Parse(rd["ChildID"].ToString());
                    tc.Name = rd["Name"].ToString();
                    tc.Gender = rd["Gender"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        tc.DOB = System.DateTime.Parse(rd["DOB"].ToString());

                    tcs.Add(tc);
                }
                rd.Close();
            }
            catch
            {
                tcs = new List<TempChild>();
            }
            finally
            {
                con.Close();
            }
            return tcs;
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
            if (string.IsNullOrEmpty(this.Gender))
            {
                isValid = false;
                this.SetError("Gender", "يجب اختيار الجنس");
            }
            if (!DOB.HasValue)
            {
                isValid = false;
                this.SetError("DOB", "يجب إدخال تاريخ الولادة");
            }
            if (DOB.HasValue)
            {
                var diff = BaseDataBase.DateNow - DOB.Value;
                if (diff.Days < 0 || diff.Days / 30 / 12 > 120)
                {
                    isValid = false;
                    this.SetError("DOB", "التاريخ غير صالح يجب ادخال عمر صحيح");
                }
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
