using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class TempFamily : ModelViewContext
    {
        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private string familyname;
        public string FamilyName
        {
            get
            { return familyname; }
            set
            {
                if (familyname != value)
                {
                    familyname = value;
                    ClearError("FamilyName");
                    if (string.IsNullOrEmpty(FamilyName))
                    {
                        this.SetError("FamilyName", "يجب إدخال اسم العائلة");
                    }
                    NotifyPropertyChanged("FamilyName");
                }
            }
        }

        private string familycode;
        public string FamilyCode
        {
            get
            { return familycode; }
            set
            {
                if (familycode != value)
                {
                    familycode = value;
                    ClearError("FamilyCode");
                    if (string.IsNullOrEmpty(value))
                        this.SetError("FamilyCode", "يجب إدخال رمز للعائلة");
                    //else
                    //{
                    //    if (BaseDataBase._StoredProcedureReturnableBool("sp_IsTempFamilyCodeExists", new SqlParameter("@R", System.Data.SqlDbType.Bit), new SqlParameter("@TempFamilyID", ID), new SqlParameter("@FamilyCode", FamilyCode)))
                    //    {
                    //        this.SetError("FamilyCode", "رمز العائلة موجود في قاعدة البيانات");
                    //    }
                    //}
                    NotifyPropertyChanged("FamilyCode");
                }
            }
        }

        private DateTime? applydate;
        public DateTime? ApplyDate
        {
            get
            { return applydate; }
            set
            {
                if (applydate != value)
                {
                    applydate = value;
                    ClearError("ApplyDate");
                    if (ApplyDate == null)
                    {
                        this.SetError("ApplyDate", "يجب إدخال تاريخ التسجيل");
                    }
                    NotifyPropertyChanged("ApplyDate");
                }
            }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private string familytype;
        public string FamilyType
        {
            get
            { return familytype; }
            set
            {
                if (value != familytype)
                {
                    familytype = value;
                    ClearError("FamilyType");
                    if (string.IsNullOrEmpty(FamilyType))
                    {
                        this.SetError("FamilyType", "يجب إدخال نوع العائلة");
                    }
                    NotifyPropertyChanged("FamilyType");
                }
            }
        }

        private string familyIdentityID;
        public string FamilyIdentityID
        {
            get
            { return familyIdentityID; }
            set
            {
                if (value != familyIdentityID)
                {
                    familyIdentityID = value;
                    ClearError("FamilyIdentityID");
                    bool isValid = true;
                    if (string.IsNullOrEmpty(FamilyIdentityID))
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "يجب إدخال رقم دفتر العائلة");
                    }
                    else if (FamilyIdentityID != "لايوجد دفتر عائلة")
                    {
                        var num = FamilyIdentityID.Split(' ');
                        if (!BaseDataBase.IsStringNumber(num[0]))
                        {
                            isValid = false;
                            this.SetError("FamilyIdentityID", "رقم دفتر العائلة يجب أن يحوي أرقاماً فقط");
                        }
                        if (num.Length > 3)
                        {
                            isValid = false;
                            this.SetError("FamilyIdentityID", "رقم دفتر العائلة يحوي أكثر من فراغين");
                        }
                        else if (num.Length > 1)
                        {
                            if (num[1].Length != 1)
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يكون خانة واحدة");
                            }
                            else if (!char.IsLetter(char.Parse(num[1])))
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يكون حرفاً واحداً فقط(أ، ب/ ت/ ث/ ج)");
                            }
                            if (num.Length == 3 && !BaseDataBase.IsStringNumber(num[2]))
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "الرقم الأسري يجب أن يحوي أرقاماً فقط");
                            }
                        }
                        if (isValid)
                        {
                            string ExistedFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyIDentityID", new SqlParameter("@FamilyID", null), new SqlParameter("@FamilyIdentityID", FamilyIdentityID));
                            if (!string.IsNullOrEmpty(ExistedFamilyCode))
                            {
                                isValid = false;
                                this.SetError("FamilyIdentityID", "رقم دفتر العائلة موجود في قاعدة البيانات برقم عائلة مثبتة " + ExistedFamilyCode);
                            }
                            else
                            {
                                ExistedFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetTempFamilyCodeByFamilyIDentityID", new SqlParameter("@TempFamilyID", ID), new SqlParameter("@FamilyIdentityID", FamilyIdentityID));
                                if (!string.IsNullOrEmpty(ExistedFamilyCode))
                                {
                                    isValid = false;
                                    this.SetError("FamilyIdentityID", "رقم دفتر العائلة موجود في قاعدة البيانات برقم عائلة " + ExistedFamilyCode);
                                }
                            }
                        }
                    }
                    NotifyPropertyChanged("FamilyIdentityID");
                }
            }
        }

        private string fatherfirstname;
        public string FatherFirstName
        {
            get
            { return fatherfirstname; }
            set
            {
                fatherfirstname = value;
                ClearError("FatherFirstName");
            }
        }

        private string fatherfathername;
        public string FatherFatherName
        {
            get
            { return fatherfathername; }
            set
            { fatherfathername = value; }
        }

        private string fatherlastname;
        public string FatherLastName
        {
            get
            { return fatherlastname; }
            set
            {
                fatherlastname = value;
                ClearError("FatherLastName");
            }
        }

        private string fatherbornplace;
        public string FatherBornPlace
        {
            get
            { return fatherbornplace; }
            set
            { fatherbornplace = value; }
        }

        private string fatherpid;
        public string FatherPID
        {
            get
            { return fatherpid; }
            set
            {
                if (value != fatherpid)
                {
                    fatherpid = value;
                    ClearError("FatherPID");
                    if (!string.IsNullOrEmpty(FatherFirstName) || !string.IsNullOrEmpty(FatherLastName))
                    {
                        if (string.IsNullOrEmpty(FatherPID))
                        {
                            this.SetError("FatherPID", "يجب إدخال الرقم الوطني");
                        }
                        else if (FatherPID != "لايوجد رقم وطني")
                        {
                            if (!BaseDataBase.IsStringNumber(FatherPID))
                            {
                                this.SetError("FatherPID", "الرقم الوطني يجب أن يكون يحوي أرقاماً فقط");
                            }
                            else
                            {
                                string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", null), new SqlParameter("@PID", FatherPID));
                                if (!string.IsNullOrEmpty(ExistingFamilyCode))
                                {
                                    this.SetError("FatherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مثبتة " + ExistingFamilyCode);
                                }
                                else
                                {
                                    ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetTempFamilyCodeByParentPID", new SqlParameter("@TempFamilyID", ID), new SqlParameter("@PID", FatherPID));
                                    if (!string.IsNullOrEmpty(ExistingFamilyCode))
                                    {
                                        this.SetError("FatherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مؤقتة " + ExistingFamilyCode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private DateTime? fatherdob;
        public DateTime? FatherDOB
        {
            get
            { return fatherdob; }
            set
            {
                fatherdob = value;
                ClearError("FatherDOB");
            }
        }

        private string motherfirstname;
        public string MotherFirstName
        {
            get
            { return motherfirstname; }
            set
            {
                motherfirstname = value;
                ClearError("MotherFirstName");
            }
        }

        private string motherfathername;
        public string MotherFatherName
        {
            get
            { return motherfathername; }
            set
            { motherfathername = value; }
        }

        private string motherlastname;
        public string MotherLastName
        {
            get
            { return motherlastname; }
            set
            {
                motherlastname = value;
                ClearError("MotherLastName");
            }
        }

        private string motherbornplace;
        public string MotherBornPlace
        {
            get
            { return motherbornplace; }
            set
            { motherbornplace = value; }
        }

        private string motherpid;
        public string MotherPID
        {
            get
            { return motherpid; }
            set
            {
                if (motherpid != value)
                {
                    motherpid = value;
                    ClearError("MotherPID");
                    if (!string.IsNullOrEmpty(MotherFirstName) || !string.IsNullOrEmpty(MotherLastName))
                    {
                        if (string.IsNullOrEmpty(MotherPID))
                        {
                            this.SetError("MotherPID", "يجب إدخال الرقم الوطني");
                        }
                        else if (MotherPID != "لايوجد رقم وطني")
                        {
                            if (!BaseDataBase.IsStringNumber(MotherPID))
                            {
                                this.SetError("MotherPID", "الرقم الوطني يجب أن يحوي أرقاماً فقط");
                            }
                            else
                            {
                                string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", null), new SqlParameter("@PID", MotherPID));
                                if (!string.IsNullOrEmpty(ExistingFamilyCode))
                                {
                                    this.SetError("MotherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مثبتة " + ExistingFamilyCode);
                                }
                                else
                                {
                                    ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetTempFamilyCodeByParentPID", new SqlParameter("@TempFamilyID", ID), new SqlParameter("@PID", MotherPID));
                                    if (!string.IsNullOrEmpty(ExistingFamilyCode))
                                    {
                                        this.SetError("MotherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مؤقتة " + ExistingFamilyCode);
                                    }
                                }
                            }
                        }
                    }
                    NotifyPropertyChanged("MotherPID");
                }
            }
        }

        private DateTime? motherdob;
        public DateTime? MotherDOB
        {
            get
            { return motherdob; }
            set
            {
                motherdob = value;
                ClearError("MotherDOB");
            }
        }

        private string phone;
        public string Phone
        {
            get
            { return phone; }
            set
            { phone = value; }
        }

        private string mobile1;
        public string Mobile1
        {
            get
            { return mobile1; }
            set
            { mobile1 = value; }
        }

        private string mobile2;
        public string Mobile2
        {
            get
            { return mobile2; }
            set
            { mobile2 = value; }
        }

        private bool isprinted;
        public bool IsPrinted
        {
            get
            { return isprinted; }
            set
            { isprinted = value; }
        }

        private string printer;
        public string Printer
        {
            get
            { return printer; }
            set
            { printer = value; }
        }

        private string housesection;
        public string HouseSection
        {
            get
            { return housesection; }
            set
            {
                if (housesection != value)
                {
                    housesection = value;
                    ClearError("HouseSection");
                    if (string.IsNullOrEmpty(HouseSection))
                    {
                        this.SetError("HouseSection", "يجب إدخال منطقة السكن");
                    }
                    NotifyPropertyChanged("HouseSection");
                }
            }
        }

        private string housestreet;
        public string HouseStreet
        {
            get
            { return housestreet; }
            set
            { housestreet = value; }
        }

        private string housebuildingnumber;
        public string HouseBuildingNumber
        {
            get
            { return housebuildingnumber; }
            set
            { housebuildingnumber = value; }
        }

        private string houseFloor;
        public string HouseFloor
        {
            get
            { return houseFloor; }
            set
            { houseFloor = value; }
        }

        private string houseaddress;
        public string HouseAddress
        {
            get
            { return houseaddress; }
            set
            { houseaddress = value; }
        }

        private string houseoldaddress;
        public string HouseOldAddress
        {
            get
            { return houseoldaddress; }
            set
            { houseoldaddress = value; }
        }

        private string creator;
        public string Creator
        {
            get
            { return creator; }
            set
            { creator = value; }
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

        private int? familyPersonCount;
        public int? FamilyPersonCount
        {
            get { return familyPersonCount; }
            set
            {
                if (value != familyPersonCount)
                {
                    familyPersonCount = value;
                    if (!value.HasValue)
                    {
                        this.SetError("FamilyPersonCount", "يجب ادخال عدد الأفراد");
                    }
                    NotifyPropertyChanged("FamilyPersonCount");
                }
            }
        }

        private int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set { familyID = value; }
        }

        private List<TempChild> tempChilds = new List<TempChild>();
        public List<TempChild> TempChilds
        {
            get { return tempChilds; }
            set { tempChilds = value; }
        }

        public static bool InsertData(TempFamily t)
        {
            t.Creator = BaseDataBase.CurrentUser.Name;

            t.ID = BaseDataBase._StoredProcedureReturnable("sp_Add2TempFamily"
                  , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                  , new SqlParameter("@FamilyName", t.FamilyName)
                  , new SqlParameter("@FamilyCode", t.FamilyCode)
                  , new SqlParameter("@ApplyDate", t.ApplyDate)
                  , new SqlParameter("@Notes", t.Notes)
                  , new SqlParameter("@FamilyType", t.FamilyType)
                  , new SqlParameter("@FamilyIdentityID", t.FamilyIdentityID)
                  , new SqlParameter("@FatherFirstName", t.FatherFirstName)
                  , new SqlParameter("@FatherFatherName", t.FatherFatherName)
                  , new SqlParameter("@FatherLastName", t.FatherLastName)
                  , new SqlParameter("@FatherBornPlace", t.FatherBornPlace)
                  , new SqlParameter("@FatherPID", t.FatherPID)
                  , new SqlParameter("@FatherDOB", t.FatherDOB)
                  , new SqlParameter("@MotherFirstName", t.MotherFirstName)
                  , new SqlParameter("@MotherFatherName", t.MotherFatherName)
                  , new SqlParameter("@MotherLastName", t.MotherLastName)
                  , new SqlParameter("@MotherBornPlace", t.MotherBornPlace)
                  , new SqlParameter("@MotherPID", t.MotherPID)
                  , new SqlParameter("@MotherDOB", t.MotherDOB)
                  , new SqlParameter("@Phone", t.Phone)
                  , new SqlParameter("@Mobile1", t.Mobile1)
                  , new SqlParameter("@Mobile2", t.Mobile2)
                  , new SqlParameter("@IsPrinted", t.IsPrinted)
                  , new SqlParameter("@Printer", t.Printer)
                  , new SqlParameter("@HouseSection", t.HouseSection)
                  , new SqlParameter("@HouseStreet", t.HouseStreet)
                  , new SqlParameter("@HouseBuildingNumber", t.HouseBuildingNumber)
                  , new SqlParameter("@HouseFloor", t.HouseFloor)
                  , new SqlParameter("@HouseAddress", t.HouseAddress)
                  , new SqlParameter("@HouseOldAddress", t.HouseOldAddress)
                  , new SqlParameter("@Creator", t.Creator)
                  , new SqlParameter("@IsCancelled", t.IsCancelled)
                  , new SqlParameter("@FamilyPersonCount", t.FamilyPersonCount)
                  , new SqlParameter("@CancelReason", t.CancelReason)
                  , new SqlParameter("@FamilyID", t.FamilyID)
                  );
            t.FamilyCode = BaseDataBase._Scalar("select FamilyCode from TempFamily where Id = " + t.ID);
            foreach (var tc in t.TempChilds)
            {
                tc.TempFamilyID = t.ID;
                TempChild.InsertData(tc);
            }

            return t.ID.HasValue;
        }
        public static bool UpadteData(TempFamily t)
        {
            return BaseDataBase._StoredProcedure("sp_UpdateTempFamily"
                    , new SqlParameter("@ID", t.ID)
                    , new SqlParameter("@FamilyName", t.FamilyName)
                    , new SqlParameter("@FamilyCode", t.FamilyCode)
                    , new SqlParameter("@ApplyDate", t.ApplyDate)
                    , new SqlParameter("@Notes", t.Notes)
                    , new SqlParameter("@FamilyType", t.FamilyType)
                    , new SqlParameter("@FamilyIdentityID", t.FamilyIdentityID)
                    , new SqlParameter("@FatherFirstName", t.FatherFirstName)
                    , new SqlParameter("@FatherFatherName", t.FatherFatherName)
                    , new SqlParameter("@FatherLastName", t.FatherLastName)
                    , new SqlParameter("@FatherBornPlace", t.FatherBornPlace)
                    , new SqlParameter("@FatherPID", t.FatherPID)
                    , new SqlParameter("@FatherDOB", t.FatherDOB)
                    , new SqlParameter("@MotherFirstName", t.MotherFirstName)
                    , new SqlParameter("@MotherFatherName", t.MotherFatherName)
                    , new SqlParameter("@MotherLastName", t.MotherLastName)
                    , new SqlParameter("@MotherBornPlace", t.MotherBornPlace)
                    , new SqlParameter("@MotherPID", t.MotherPID)
                    , new SqlParameter("@MotherDOB", t.MotherDOB)
                    , new SqlParameter("@Phone", t.Phone)
                    , new SqlParameter("@Mobile1", t.Mobile1)
                    , new SqlParameter("@Mobile2", t.Mobile2)
                    , new SqlParameter("@IsPrinted", t.IsPrinted)
                    , new SqlParameter("@Printer", t.Printer)
                    , new SqlParameter("@HouseSection", t.HouseSection)
                    , new SqlParameter("@HouseStreet", t.HouseStreet)
                    , new SqlParameter("@HouseBuildingNumber", t.HouseBuildingNumber)
                    , new SqlParameter("@HouseFloor", t.HouseFloor)
                    , new SqlParameter("@HouseAddress", t.HouseAddress)
                    , new SqlParameter("@HouseOldAddress", t.HouseOldAddress)
                    , new SqlParameter("@IsCancelled", t.IsCancelled)
                    , new SqlParameter("@CancelReason", t.CancelReason)
                    , new SqlParameter("@FamilyPersonCount", t.FamilyPersonCount)
                    , new SqlParameter("@Creator", t.Creator)
                    , new SqlParameter("@FamilyID", t.FamilyID)
                    );
        }
        public static bool DeleteData(TempFamily t)
        {
            foreach (var c in t.TempChilds)
            {
                TempChild.DeleteData(c);
            }
            return BaseDataBase._StoredProcedure("sp_DeleteFromTempFamily"
                    , new SqlParameter("@ID", t.ID));
        }

        public static TempFamily GetTempFamilyByID(int id)
        {
            TempFamily x = new TempFamily();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTempFamilyByID", con);
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
                    x.FamilyName = rd["FamilyName"].ToString();
                    x.FamilyCode = rd["FamilyCode"].ToString();
                    if (!(rd["ApplyDate"] is DBNull))
                        x.ApplyDate = System.DateTime.Parse(rd["ApplyDate"].ToString());
                    x.FamilyIdentityID = rd["FamilyIdentityID"].ToString();
                    if (!(rd["FamilyPersonCount"] is DBNull))
                        x.FamilyPersonCount = System.Int32.Parse(rd["FamilyPersonCount"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    x.FamilyType = rd["FamilyType"].ToString();
                    x.FatherFirstName = rd["FatherFirstName"].ToString();
                    x.FatherFatherName = rd["FatherFatherName"].ToString();
                    x.FatherLastName = rd["FatherLastName"].ToString();
                    x.FatherBornPlace = rd["FatherBornPlace"].ToString();
                    x.FatherPID = rd["FatherPID"].ToString();
                    if (!(rd["FatherDOB"] is DBNull))
                        x.FatherDOB = System.DateTime.Parse(rd["FatherDOB"].ToString());
                    x.MotherFirstName = rd["MotherFirstName"].ToString();
                    x.MotherFatherName = rd["MotherFatherName"].ToString();
                    x.MotherLastName = rd["MotherLastName"].ToString();
                    x.MotherBornPlace = rd["MotherBornPlace"].ToString();
                    x.MotherPID = rd["MotherPID"].ToString();
                    if (!(rd["MotherDOB"] is DBNull))
                        x.MotherDOB = System.DateTime.Parse(rd["MotherDOB"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile1 = rd["Mobile1"].ToString();
                    x.Mobile2 = rd["Mobile2"].ToString();
                    if (!(rd["IsPrinted"] is DBNull))
                        x.IsPrinted = System.Boolean.Parse(rd["IsPrinted"].ToString());
                    x.Printer = rd["Printer"].ToString();
                    x.HouseSection = rd["HouseSection"].ToString();
                    x.HouseStreet = rd["HouseStreet"].ToString();
                    x.HouseBuildingNumber = rd["HouseBuildingNumber"].ToString();
                    x.HouseFloor = rd["HouseFloor"].ToString();
                    x.HouseAddress = rd["HouseAddress"].ToString();
                    x.HouseOldAddress = rd["HouseOldAddress"].ToString();
                    if (!(rd["IsCancelled"] is DBNull))
                        x.IsCancelled = System.Boolean.Parse(rd["IsCancelled"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();
                    x.Creator = rd["Creator"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());

                    x.TempChilds = TempChild.GetTempChildsByTempFamilyID(x.ID.Value);
                }
                rd.Close();
            }
            catch
            {
                x = new TempFamily();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<TempFamily> GetTempFamilyAll()
        {
            List<TempFamily> tfs = new List<TempFamily>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetTempFamilyAll", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    TempFamily x = new TempFamily();

                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    x.FamilyName = rd["FamilyName"].ToString();
                    x.FamilyCode = rd["FamilyCode"].ToString();
                    if (!(rd["ApplyDate"] is DBNull))
                        x.ApplyDate = System.DateTime.Parse(rd["ApplyDate"].ToString());
                    x.FamilyIdentityID = rd["FamilyIdentityID"].ToString();
                    if (!(rd["FamilyPersonCount"] is DBNull))
                        x.FamilyPersonCount = System.Int32.Parse(rd["FamilyPersonCount"].ToString());
                    x.Notes = rd["Notes"].ToString();
                    x.FamilyType = rd["FamilyType"].ToString();
                    x.FatherFirstName = rd["FatherFirstName"].ToString();
                    x.FatherFatherName = rd["FatherFatherName"].ToString();
                    x.FatherLastName = rd["FatherLastName"].ToString();
                    x.FatherBornPlace = rd["FatherBornPlace"].ToString();
                    x.FatherPID = rd["FatherPID"].ToString();
                    if (!(rd["FatherDOB"] is DBNull))
                        x.FatherDOB = System.DateTime.Parse(rd["FatherDOB"].ToString());
                    x.MotherFirstName = rd["MotherFirstName"].ToString();
                    x.MotherFatherName = rd["MotherFatherName"].ToString();
                    x.MotherLastName = rd["MotherLastName"].ToString();
                    x.MotherBornPlace = rd["MotherBornPlace"].ToString();
                    x.MotherPID = rd["MotherPID"].ToString();
                    if (!(rd["MotherDOB"] is DBNull))
                        x.MotherDOB = System.DateTime.Parse(rd["MotherDOB"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile1 = rd["Mobile1"].ToString();
                    x.Mobile2 = rd["Mobile2"].ToString();
                    if (!(rd["IsPrinted"] is DBNull))
                        x.IsPrinted = System.Boolean.Parse(rd["IsPrinted"].ToString());
                    x.Printer = rd["Printer"].ToString();
                    x.HouseSection = rd["HouseSection"].ToString();
                    x.HouseStreet = rd["HouseStreet"].ToString();
                    x.HouseBuildingNumber = rd["HouseBuildingNumber"].ToString();
                    x.HouseFloor = rd["HouseFloor"].ToString();
                    x.HouseAddress = rd["HouseAddress"].ToString();
                    x.HouseOldAddress = rd["HouseOldAddress"].ToString();
                    if (!(rd["IsCancelled"] is DBNull))
                        x.IsCancelled = System.Boolean.Parse(rd["IsCancelled"].ToString());
                    x.CancelReason = rd["CancelReason"].ToString();
                    x.Creator = rd["Creator"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());

                    x.TempChilds = TempChild.GetTempChildsByTempFamilyID(x.ID.Value);

                    tfs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                tfs = new List<TempFamily>();
            }
            finally
            {
                con.Close();
            }
            return tfs;
        }
        public static DataTable GetTempFamilyTable()
        { return BaseDataBase._TablingStoredProcedure("sp_GetTempFamilyTable"); }


        // -------------------
        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            //if (string.IsNullOrEmpty(FamilyCode))
            //{
            //    isValid = false;
            //    this.SetError("FamilyCode", "يجب إدخال رمز للعائلة");
            //}
            //else
            //{
            //    if (BaseDataBase._StoredProcedureReturnableBool("sp_IsTempFamilyCodeExists", new SqlParameter("@R", System.Data.SqlDbType.Bit), new SqlParameter("@TempFamilyID", ID), new SqlParameter("@FamilyCode", FamilyCode)))
            //    {
            //        isValid = false;
            //        this.SetError("FamilyCode", "رمز العائلة موجود في قاعدة البيانات");
            //    }
            //}
            if (string.IsNullOrEmpty(HouseSection))
            {
                isValid = false;
                this.SetError("HouseSection", "يجب إدخال منطقة السكن");
            }
            if (string.IsNullOrEmpty(FamilyIdentityID))
            {
                isValid = false;
                this.SetError("FamilyIdentityID", "يجب إدخال رقم دفتر العائلة");
            }
            else if (FamilyIdentityID != "لايوجد دفتر عائلة")
            {
                var num = FamilyIdentityID.Split(' ');
                if (!BaseDataBase.IsStringNumber(num[0]))
                {
                    isValid = false;
                    this.SetError("FamilyIdentityID", "رقم دفتر العائلة يجب أن يحوي أرقاماً فقط");
                }
                if (num.Length > 3)
                {
                    isValid = false;
                    this.SetError("FamilyIdentityID", "رقم دفتر العائلة يحوي أكثر من فراغين");
                }
                else if (num.Length > 1)
                {
                    if (num[1].Length != 1)
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يكون خانة واحدة");
                    }
                    else if (!char.IsLetter(char.Parse(num[1])))
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "حرف دفتر العائلة يجب أن يكون حرفاً واحداً فقط(أ، ب/ ت/ ث/ ج)");
                    }
                    if (num.Length == 3 && !BaseDataBase.IsStringNumber(num[2]))
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "الرقم الأسري يجب أن يحوي أرقاماً فقط");
                    }
                }
                if (isValid)
                {
                    string ExistedFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyIDentityID", new SqlParameter("@FamilyID", null), new SqlParameter("@FamilyIdentityID", FamilyIdentityID));
                    if (!string.IsNullOrEmpty(ExistedFamilyCode))
                    {
                        isValid = false;
                        this.SetError("FamilyIdentityID", "رقم دفتر العائلة موجود في قاعدة البيانات برقم عائلة مثبتة " + ExistedFamilyCode);
                    }
                    else
                    {
                        ExistedFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetTempFamilyCodeByFamilyIDentityID", new SqlParameter("@TempFamilyID", ID), new SqlParameter("@FamilyIdentityID", FamilyIdentityID));
                        if (!string.IsNullOrEmpty(ExistedFamilyCode))
                        {
                            isValid = false;
                            this.SetError("FamilyIdentityID", "رقم دفتر العائلة موجود في قاعدة البيانات برقم عائلة " + ExistedFamilyCode);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(FamilyName))
            {
                isValid = false;
                this.SetError("FamilyName", "يجب إدخال اسم العائلة");
            }
            if (ApplyDate == null)
            {
                isValid = false;
                this.SetError("ApplyDate", "يجب إدخال تاريخ التسجيل");
            }
            if (!FamilyPersonCount.HasValue)
            {
                isValid = false;
                this.SetError("FamilyPersonCount", "يجب ادخال عدد الأفراد");
            }
            if (string.IsNullOrEmpty(FamilyType))
            {
                isValid = false;
                this.SetError("FamilyType", "يجب إدخال نوع العائلة");
            }
            if (!string.IsNullOrEmpty(FatherFirstName) || !string.IsNullOrEmpty(FatherLastName))
            {
                if (string.IsNullOrEmpty(FatherFirstName))
                {
                    isValid = false;
                    this.SetError("FatherFirstName", "يجب إدخال اسم الاب");
                }
                if (string.IsNullOrEmpty(FatherLastName))
                {
                    isValid = false;
                    this.SetError("FatherLastName", "يجب إدخال كنية الاب");
                }
                if (FatherDOB.HasValue)
                {
                    var diff = BaseDataBase.DateNow - FatherDOB.Value;
                    if (diff.Days / 30 / 12 < 12 || diff.Days / 30 / 12 > 120)
                    {
                        isValid = false;
                        this.SetError("FatherDOB", "التاريخ غير صالح يجب ادخال عمر صحيح");
                    }
                }
                if (string.IsNullOrEmpty(FatherPID))
                {
                    isValid = false;
                    this.SetError("FatherPID", "يجب إدخال الرقم الوطني");
                }
                else if (FatherPID != "لايوجد رقم وطني")
                {
                    if (!BaseDataBase.IsStringNumber(FatherPID))
                    {
                        isValid = false;
                        this.SetError("FatherPID", "الرقم الوطني يجب أن يكون يحوي أرقاماً فقط");
                    }
                    else
                    {
                        string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", null), new SqlParameter("@PID", FatherPID));
                        if (!string.IsNullOrEmpty(ExistingFamilyCode))
                        {
                            isValid = false;
                            this.SetError("FatherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مثبتة " + ExistingFamilyCode);
                        }
                        else
                        {
                            ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetTempFamilyCodeByParentPID", new SqlParameter("@TempFamilyID", ID), new SqlParameter("@PID", FatherPID));
                            if (!string.IsNullOrEmpty(ExistingFamilyCode))
                            {
                                isValid = false;
                                this.SetError("FatherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مؤقتة " + ExistingFamilyCode);
                            }
                            else
                            {
                                ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyPersonPID", new SqlParameter("@PID", FatherPID));
                                if (!string.IsNullOrEmpty(ExistingFamilyCode))
                                {
                                    isValid = false;
                                    this.SetError("FatherPID", "الرقم الوطني موجود في قاعدة البيانات ضمن افراد عائلة رقم " + ExistingFamilyCode);
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(MotherFirstName) || !string.IsNullOrEmpty(MotherLastName))
            {
                if (string.IsNullOrEmpty(MotherFirstName))
                {
                    isValid = false;
                    this.SetError("MotherFirstName", "يجب إدخال اسم الام");
                }
                if (string.IsNullOrEmpty(MotherLastName))
                {
                    isValid = false;
                    this.SetError("MotherLastName", "يجب إدخال كنية الام");
                }
                if (MotherDOB.HasValue)
                {
                    var diff = BaseDataBase.DateNow - MotherDOB.Value;
                    if (diff.Days / 30 / 12 < 12 || diff.Days / 30 / 12 > 120)
                    {
                        isValid = false;
                        this.SetError("MotherDOB", "التاريخ غير صالح يجب ادخال عمر صحيح");
                    }
                }
                if (string.IsNullOrEmpty(MotherPID))
                {
                    isValid = false;
                    this.SetError("MotherPID", "يجب إدخال الرقم الوطني");
                }
                else if (MotherPID != "لايوجد رقم وطني")
                {
                    if (!BaseDataBase.IsStringNumber(MotherPID))
                    {
                        isValid = false;
                        this.SetError("MotherPID", "الرقم الوطني يجب أن يحوي أرقاماً فقط");
                    }
                    else
                    {
                        string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", null), new SqlParameter("@PID", MotherPID));
                        if (!string.IsNullOrEmpty(ExistingFamilyCode))
                        {
                            isValid = false;
                            this.SetError("MotherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مثبتة " + ExistingFamilyCode);
                        }
                        else
                        {
                            ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetTempFamilyCodeByParentPID", new SqlParameter("@TempFamilyID", ID), new SqlParameter("@PID", MotherPID));
                            if (!string.IsNullOrEmpty(ExistingFamilyCode))
                            {
                                isValid = false;
                                this.SetError("MotherPID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة مؤقتة " + ExistingFamilyCode);
                            }
                            else
                            {
                                ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyPersonPID", new SqlParameter("@PID", MotherPID));
                                if (!string.IsNullOrEmpty(ExistingFamilyCode))
                                {
                                    isValid = false;
                                    this.SetError("MotherPID", "الرقم الوطني موجود في قاعدة البيانات ضمن افراد عائلة رقم " + ExistingFamilyCode);
                                }
                            }
                        }
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
    }
}
