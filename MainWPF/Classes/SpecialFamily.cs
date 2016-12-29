using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MainWPF
{
    public class SpecialFamily : ModelViewContext
    {
        private int? id;
        public int? Id
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

        private string pid;
        public string PID
        {
            get
            { return pid; }
            set
            { pid = value; }
        }

        private string fathername;
        public string FatherName
        {
            get
            { return fathername; }
            set
            { fathername = value; }
        }

        private string mothername;
        public string MotherName
        {
            get
            { return mothername; }
            set
            { mothername = value; }
        }

        private string gender;
        public string Gender
        {
            get
            { return gender; }
            set
            { gender = value; }
        }

        private DateTime? dob;
        public DateTime? DOB
        {
            get
            { return dob; }
            set
            { dob = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private int lastuserid;
        public int LastUserID
        {
            get
            { return lastuserid; }
            set
            { lastuserid = value; }
        }
        List<Order> orders;
        public List<Order> Orders
        {
            get
            {
                if (orders == null) orders = new List<Order>();
                return orders;
            }
            set
            {
                orders = value;
                NotifyPropertyChanged("Orders");
            }
        }
        public void GetOrders()
        {
            Orders = Order.GetAllOrderBySpecialFamilyID(Id.Value).OrderByDescending(x => x.Date).ToList();
            NotifyPropertyChanged("Orders");
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
            if (string.IsNullOrEmpty(Gender))
            {
                isValid = false;
                this.SetError("Gender", "يجب إدخال الجنس");
            }
            if (DOB.HasValue)
            {
                var diff = BaseDataBase.DateNow - DOB.Value;
                if (diff.Days / 30 / 12 < 12 || diff.Days / 30 / 12 > 120)
                {
                    isValid = false;
                    this.SetError("DOB", "التاريخ غير صالح يجب ادخال عمر صحيح");
                }
            }
            if (string.IsNullOrEmpty(PID))
            {
                isValid = false;
                this.SetError("PID", "يجب إدخال الرقم الوطني");
            }
            else
            {
                if (!BaseDataBase.IsStringNumber(PID))
                {
                    isValid = false;
                    this.SetError("PID", "الرقم الوطني يجب أن يحوي أرقاماً فقط");
                }
                else if (PID.Length < 10 || PID.Length > 12)
                {
                    isValid = false;
                    this.SetError("PID", "الرقم الوطني يجب أن يحوي 10 أو 11 أو 12 رقم");
                }
                else
                {
                    string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", DBNull.Value), new SqlParameter("@PID", PID));
                    if (!string.IsNullOrEmpty(ExistingFamilyCode))
                    {
                        isValid = false;
                        this.SetError("PID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة " + ExistingFamilyCode);
                    }
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


        public static bool InsertData(SpecialFamily x)
        {
            x.Id = BaseDataBase._StoredProcedureReturnable("sp_Add_SpecialFamily",
                 new SqlParameter("@Id", System.Data.SqlDbType.Int),
                 new SqlParameter("@Name", x.Name),
                 new SqlParameter("@PID", x.PID),
                 new SqlParameter("@FatherName", x.FatherName),
                 new SqlParameter("@MotherName", x.MotherName),
                 new SqlParameter("@Gender", x.Gender),
                 new SqlParameter("@DOB", x.DOB),
                 new SqlParameter("@Notes", x.Notes),
                 new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));

            return x.Id.HasValue;
        }
        public static bool UpdateData(SpecialFamily x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_SpecialFamily",
                new SqlParameter("@Id", x.Id),
                new SqlParameter("@Name", x.Name),
                new SqlParameter("@PID", x.PID),
                new SqlParameter("@FatherName", x.FatherName),
                new SqlParameter("@MotherName", x.MotherName),
                new SqlParameter("@Gender", x.Gender),
                new SqlParameter("@DOB", x.DOB),
                new SqlParameter("@Notes", x.Notes),
                new SqlParameter("@LastUserID", BaseDataBase.CurrentUser.ID));
        }
        public static bool DeleteData(SpecialFamily x)
        {
            x.GetOrders();
            foreach (var item in x.Orders)
                Order.DeleteData(item);
            return BaseDataBase._StoredProcedure("sp_Delete_SpecialFamily",
                new SqlParameter("@Id", x.Id));
        }
        public static SpecialFamily GetSpecialFamilyByID(int id)
        {
            SpecialFamily x = new SpecialFamily();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_SpecialFamily", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.PID = rd["PID"].ToString();
                    x.FatherName = rd["FatherName"].ToString();
                    x.MotherName = rd["MotherName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
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
        public static List<SpecialFamily> GetAllSpecialFamily()
        {
            List<SpecialFamily> xx = new List<SpecialFamily>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_SpecialFamily", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    SpecialFamily x = new SpecialFamily();

                    if (!(rd["Id"] is DBNull))
                        x.Id = int.Parse(rd["Id"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.PID = rd["PID"].ToString();
                    x.FatherName = rd["FatherName"].ToString();
                    x.MotherName = rd["MotherName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["LastUserID"] is DBNull))
                        x.LastUserID = int.Parse(rd["LastUserID"].ToString());
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
    }
}