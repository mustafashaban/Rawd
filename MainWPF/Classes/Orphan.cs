
using MainWPF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class Orphan : Person
    {

        private Family orphanFamily;
        public Family OrphanFamily
        {
            get
            { return orphanFamily; }
            set
            { orphanFamily = value; }
        }

        public List<ListerGroup> ListerGroups { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public List<FamilyNeed> FamilyNeeds { get; set; }
        public List<SpecialCard> SpecialCards { get; set; }
        public List<ExternalFamilySupport> ExternalFamilySupports { get; set; }
        public List<Order> Orders { get; set; }
        public Account Account { get; set; }


        //public Orphan(FamilyPerson fp)
        //{
        //    try
        //    {
        //        this.BirthPlace = fp.BirthPlace;
        //        this.DeathDate = fp.DeathDate;
        //        this.DeathReason = fp.DeathReason;
        //        this.DOB = fp.DOB;
        //        this.Email = fp.Email;
        //        this.FamilyID = fp.FamilyID;
        //        this.FirstName = fp.FirstName;
        //        this.Gender = fp.Gender;
        //        this.Job = fp.Job;
        //        this.LastName = fp.LastName;
        //        this.MaritalStatus = fp.MaritalStatus;
        //        this.Mobile = fp.Mobile;
        //        this.Nationality = fp.Nationality;
        //        this.Notes = fp.Notes;
        //        this.Phone = fp.Phone;
        //        this.PID = fp.PID;
        //        this.LastName = Family.GetFamilyByID(fp.FamilyID.Value).FamilyFather.LastName;
        //    }
        //    catch { }
        //}

        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private string image;
        public string Image
        {
            get
            { return image; }
            set
            { image = value; }
        }

        private int? tall;
        public int? Tall
        {
            get
            { return tall; }
            set
            { tall = value; }
        }

        private int? weight;
        public int? Weight
        {
            get
            { return weight; }
            set
            { weight = value; }
        }

        private int? feetsize;
        public int? FeetSize
        {
            get
            { return feetsize; }
            set
            { feetsize = value; }
        }

        private int? waistsize;
        public int? WaistSize
        {
            get
            { return waistsize; }
            set
            { waistsize = value; }
        }

        private string status;
        public string Status
        {
            get
            { return status; }
            set
            { status = value; }
        }

        private string type;
        public string Type
        {
            get
            { return type; }
            set
            { type = value; }
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(FirstName))
            {
                isValid = false;
                this.SetError("FirstName", "يجب إدخال الاسم");
            }
            if (string.IsNullOrEmpty(Status))
            {
                isValid = false;
                this.SetError("Status", "يجب إدخال الحالة");
            }
            if (string.IsNullOrEmpty(Type))
            {
                isValid = false;
                this.SetError("Type", "يجب إدخال التصنيف");
            }
            if (string.IsNullOrEmpty(LastName))
            {
                isValid = false;
                this.SetError("LastName", "يجب إدخال الكنية");
            }
            if (string.IsNullOrEmpty(Gender))
            {
                isValid = false;
                this.SetError("Gender", "يجب إدخال الجنس");
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


        public static bool InsertData(Orphan x)
        {
            x.OrphanID = BaseDataBase._StoredProcedureReturnable("sp_Add_Orphan"
            , new SqlParameter("@OrphanID", System.Data.SqlDbType.Int)
            , new SqlParameter("@FamilyID", x.OrphanFamily.FamilyID)
            , new SqlParameter("@FirstName", x.FirstName)
            , new SqlParameter("@LastName", x.LastName)
            , new SqlParameter("@Gender", x.Gender)
            , new SqlParameter("@BirthPlace", x.BirthPlace)
            , new SqlParameter("@DOB", x.DOB)
            , new SqlParameter("@Nationality", x.Nationality)
            , new SqlParameter("@PID", x.PID)
            , new SqlParameter("@Mobile", x.Mobile)
            , new SqlParameter("@Email", x.Email)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@Image", x.Image)
            , new SqlParameter("@Tall", x.Tall)
            , new SqlParameter("@Weight", x.Weight)
            , new SqlParameter("@FeetSize", x.FeetSize)
            , new SqlParameter("@WaistSize", x.WaistSize)
            , new SqlParameter("@Status", x.Status)
            , new SqlParameter("@Type", x.Type));
            return x.OrphanID.HasValue;
        }
        public static bool UpdateData(Orphan x)
        {
            return BaseDataBase._StoredProcedure("sp_Update_Orphan"
            , new SqlParameter("@OrphanID", x.OrphanID)
            , new SqlParameter("@FamilyID", x.OrphanFamily.FamilyID)
            , new SqlParameter("@FirstName", x.FirstName)
            , new SqlParameter("@LastName", x.LastName)
            , new SqlParameter("@Gender", x.Gender)
            , new SqlParameter("@BirthPlace", x.BirthPlace)
            , new SqlParameter("@DOB", x.DOB)
            , new SqlParameter("@Nationality", x.Nationality)
            , new SqlParameter("@PID", x.PID)
            , new SqlParameter("@Mobile", x.Mobile)
            , new SqlParameter("@Email", x.Email)
            , new SqlParameter("@Notes", x.Notes)
            , new SqlParameter("@Image", x.Image)
            , new SqlParameter("@Tall", x.Tall)
            , new SqlParameter("@Weight", x.Weight)
            , new SqlParameter("@FeetSize", x.FeetSize)
            , new SqlParameter("@WaistSize", x.WaistSize)
            , new SqlParameter("@Status", x.Status)
            , new SqlParameter("@Type", x.Type));
        }
        public static bool DeleteData(Orphan x)
        {
            return BaseDataBase._StoredProcedure("sp_Delete_Orphan"
            , new SqlParameter("@OrphanID", x.OrphanID));
        }
        public static Orphan GetOrphanByID(int id, Family f = null)
        {
            Orphan x = new Orphan();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_ID_Orphan", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    x.OrphanFamily = f == null ? Family.GetFamilyByID(int.Parse(rd["FamilyID"].ToString())) : f;
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Nationality = rd["Nationality"].ToString();
                    x.PID = rd["PID"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.Image = rd["Image"].ToString();
                    if (!(rd["Tall"] is DBNull))
                        x.Tall = int.Parse(rd["Tall"].ToString());
                    if (!(rd["Weight"] is DBNull))
                        x.Weight = int.Parse(rd["Weight"].ToString());
                    if (!(rd["FeetSize"] is DBNull))
                        x.FeetSize = int.Parse(rd["FeetSize"].ToString());
                    if (!(rd["WaistSize"] is DBNull))
                        x.WaistSize = int.Parse(rd["WaistSize"].ToString());
                    x.Status = rd["Status"].ToString();
                    x.Type = rd["Type"].ToString();
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
        public static List<Orphan> GetAllOrphan(bool ContainFamilyData = false)
        {
            List<Orphan> xx = new List<Orphan>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_Get_All_Orphan", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Orphan x = new Orphan();

                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                    if (!(rd["FamilyID"] is DBNull) && ContainFamilyData)
                        x.OrphanFamily = Family.GetFamilyByID(int.Parse(rd["FamilyID"].ToString()));
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = DateTime.Parse(rd["DOB"].ToString());
                    x.Nationality = rd["Nationality"].ToString();
                    x.PID = rd["PID"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.Image = rd["Image"].ToString();
                    if (!(rd["Tall"] is DBNull))
                        x.Tall = int.Parse(rd["Tall"].ToString());
                    if (!(rd["Weight"] is DBNull))
                        x.Weight = int.Parse(rd["Weight"].ToString());
                    if (!(rd["FeetSize"] is DBNull))
                        x.FeetSize = int.Parse(rd["FeetSize"].ToString());
                    if (!(rd["WaistSize"] is DBNull))
                        x.WaistSize = int.Parse(rd["WaistSize"].ToString());
                    x.Status = rd["Status"].ToString();
                    x.Type = rd["Type"].ToString();
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


        public static Task<List<Orphan>> GetAllOrphanByFamily(Family f, Orphan o = null)
        {
            List<Orphan> xx = new List<Orphan>();
            Task.Run(() =>
            {
                SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
                SqlCommand com = new SqlCommand("sp_Get_All_Orphan_ByFamilyID", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@FamilyID", f.FamilyID));
                if (o != null && o.OrphanID.HasValue)
                    com.Parameters.Add(new SqlParameter("@OrphanID", o.OrphanID));
                try
                {
                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    while (rd.Read())
                    {
                        Orphan x = new Orphan();

                        if (!(rd["OrphanID"] is DBNull))
                            x.OrphanID = int.Parse(rd["OrphanID"].ToString());
                        x.OrphanFamily = f;
                        x.FirstName = rd["FirstName"].ToString();
                        x.LastName = rd["LastName"].ToString();
                        x.Gender = rd["Gender"].ToString();
                        x.BirthPlace = rd["BirthPlace"].ToString();
                        if (!(rd["DOB"] is DBNull))
                            x.DOB = DateTime.Parse(rd["DOB"].ToString());
                        x.Nationality = rd["Nationality"].ToString();
                        x.PID = rd["PID"].ToString();
                        x.Mobile = rd["Mobile"].ToString();
                        x.Email = rd["Email"].ToString();
                        x.Notes = rd["Notes"].ToString();
                        x.Image = rd["Image"].ToString();
                        if (!(rd["Tall"] is DBNull))
                            x.Tall = int.Parse(rd["Tall"].ToString());
                        if (!(rd["Weight"] is DBNull))
                            x.Weight = int.Parse(rd["Weight"].ToString());
                        if (!(rd["FeetSize"] is DBNull))
                            x.FeetSize = int.Parse(rd["FeetSize"].ToString());
                        if (!(rd["WaistSize"] is DBNull))
                            x.WaistSize = int.Parse(rd["WaistSize"].ToString());
                        x.Status = rd["Status"].ToString();
                        x.Type = rd["Type"].ToString();
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
            });

            return Task.FromResult<List<Orphan>>(xx);
        }
    }
}