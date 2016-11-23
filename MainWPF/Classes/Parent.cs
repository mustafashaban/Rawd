using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public abstract class Parent : Person
    {
        public Parent()
        { }
        public Parent(int? FamilyID)
        {
            this.FamilyID = FamilyID;
        }


        private int? parentrid;
        public int? ParentrID
        {
            get
            { return parentrid; }
            set
            {
                parentrid = value;
            }
        }

        private string status;
        public string Status
        {
            get
            { return status; }
            set
            {
                if (value != Status)
                {
                    status = value;
                    ClearError("Status");
                    IsDead = value == "متوفى" || value == "شهيد";
                    if (string.IsNullOrEmpty(value))
                        this.SetError("Status", "يجب اختيار الحالة");
                    NotifyPropertyChanged("Status");
                    NotifyPropertyChanged("IsDead");
                }
            }
        }

        private string pid;
        public new string PID
        {
            get
            { return pid; }
            set
            {
                pid = value;
                this.ClearError("PID");
                if (!string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName))
                {
                    if (string.IsNullOrEmpty(PID))
                    {
                        this.SetError("PID", "يجب إدخال الرقم الوطني");
                    }
                    else if (PID != "لايوجد رقم وطني")
                    {
                        if (!BaseDataBase.IsStringNumber(PID))
                        {
                            this.SetError("PID", "الرقم الوطني يجب أن يحوي أرقاماً فقط");
                        }
                        else if (PID.Length < 10 || PID.Length > 12)
                        {
                            this.SetError("PID", "الرقم الوطني يجب أن يحوي 10 أو 11 أو 12 رقم");
                        }
                        else
                        {
                            string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", ParentrID), new SqlParameter("@PID", PID));
                            if (!string.IsNullOrEmpty(ExistingFamilyCode))
                            {
                                this.SetError("PID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة " + ExistingFamilyCode);
                            }
                        }
                    }
                }
                this.NotifyPropertyChanged("PID");
            }
        }

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            {
                familyid = value;

            }
        }


        private string nickname;
        public string Nickname
        {
            get
            { return nickname; }
            set
            { nickname = value; }
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



        private string job;
        public string Job
        {
            get
            { return job; }
            set
            { job = value; }
        }



        private bool isWorking;
        public bool IsWorking
        {
            get
            { return isWorking; }
            set
            {
                isWorking = value;
                NotifyPropertyChanged("IsWorking");
            }
        }



        private double? salary;
        public double? Salary
        {
            get
            { return salary; }
            set
            { salary = value; }
        }



        private string salarycurrency;
        public string SalaryCurrency
        {
            get
            { return salarycurrency; }
            set
            { salarycurrency = value; }
        }



        private string jobappointment;
        public string JobAppointment
        {
            get
            { return jobappointment; }
            set
            { jobappointment = value; }
        }

        private bool isimpeded;
        public bool IsImpeded
        {
            get
            { return isimpeded; }
            set
            {
                isimpeded = value;
                NotifyPropertyChanged("IsImpeded");
            }
        }

        private string impededtype;
        public string impededType
        {
            get
            { return impededtype; }
            set
            { impededtype = value; }
        }

        private string impededkind;
        public string ImpededKind
        {
            get
            { return impededkind; }
            set
            { impededkind = value; }
        }

        private DateTime? impededdate;
        public DateTime? impededDate
        {
            get
            { return impededdate; }
            set
            { impededdate = value; }
        }



        private bool islost;
        public bool IsLost
        {
            get
            { return islost; }
            set
            { islost = value; }
        }



        private string lostplace;
        public string LostPlace
        {
            get
            { return lostplace; }
            set
            { lostplace = value; }
        }



        private DateTime? lostdate;
        public DateTime? LostDate
        {
            get
            { return lostdate; }
            set
            { lostdate = value; }
        }



        private string backdetailes;
        public string BackDetailes
        {
            get
            { return backdetailes; }
            set
            { backdetailes = value; }
        }



        private DateTime? backdate;
        public DateTime? BackDate
        {
            get
            { return backdate; }
            set
            { backdate = value; }
        }

        private bool isdead;
        public bool IsDead
        {
            get
            { return isdead; }
            set
            { isdead = value; }
        }

        private string deathreportid;
        public string DeathReportID
        {
            get
            { return deathreportid; }
            set
            { deathreportid = value; }
        }



        private DateTime? deathreportdate;
        public DateTime? DeathReportDate
        {
            get
            { return deathreportdate; }
            set
            { deathreportdate = value; }
        }


        private string bondplace;
        public string BondPlace
        {
            get
            { return bondplace; }
            set
            { bondplace = value; }
        }



        private string bondnumber;
        public string BondNumber
        {
            get
            { return bondnumber; }
            set
            { bondnumber = value; }
        }




        private DateTime? identitygivindate;
        public DateTime? IdentityGivinDate
        {
            get
            { return identitygivindate; }
            set
            { identitygivindate = value; }
        }



        private string identityimage;
        public string IdentityImage
        {
            get
            { return identityimage; }
            set
            {
                if (value != identityimage)
                {
                    identityimage = value;
                    this.NotifyPropertyChanged("IdentityImage");
                }
            }
        }



        private string personalimage;
        public string PersonalImage
        {
            get
            { return personalimage; }
            set
            {
                if (value != personalimage)
                {
                    personalimage = value;
                    this.NotifyPropertyChanged("PersonalImage");
                }
                personalimage = value;
            }
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

        private int? feetSize;
        public int? FeetSize
        {
            get { return feetSize; }
            set { feetSize = value; }
        }

        private int? waistSize;
        public int? WaistSize
        {
            get { return waistSize; }
            set { waistSize = value; }
        }


        private string generalwealthysituation;
        public string GeneralWealthySituation
        {
            get
            { return generalwealthysituation; }
            set
            { generalwealthysituation = value; }
        }



        private string generalethicalsituation;
        public string GeneralEthicalSituation
        {
            get
            { return generalethicalsituation; }
            set
            { generalethicalsituation = value; }
        }



        private string baseJob;
        public string BaseJob
        {
            get
            { return baseJob; }
            set
            { baseJob = value; }
        }



        private string fathersalary;
        public string FatherSalary
        {
            get
            { return fathersalary; }
            set
            { fathersalary = value; }
        }



        private bool isfatheralive;
        public bool IsFatherAlive
        {
            get
            { return isfatheralive; }
            set
            { isfatheralive = value; }
        }



        private bool ismotheralive;
        public bool IsMotherAlive
        {
            get
            { return ismotheralive; }
            set
            { ismotheralive = value; }
        }



        private string psychicalsituation;
        public string PsychicalSituation
        {
            get
            { return psychicalsituation; }
            set
            { psychicalsituation = value; }
        }



        private string ethics;
        public string Ethics
        {
            get
            { return ethics; }
            set
            { ethics = value; }
        }



        private string homeplace;
        public string HomePlace
        {
            get
            { return homeplace; }
            set
            { homeplace = value; }
        }



        private bool isnursemaid;
        public bool IsNursemaid
        {
            get
            { return isnursemaid; }
            set
            { isnursemaid = value; }
        }


        private string healthstatus;
        public string HealthStatus
        {
            get
            { return healthstatus; }
            set
            { healthstatus = value; }
        }



        private string studystatus;
        public string StudyStatus
        {
            get
            { return studystatus; }
            set
            { studystatus = value; }
        }


        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private bool ispregnant;
        public bool IsPregnant
        {
            get
            { return ispregnant; }
            set
            {
                ispregnant = value;
                NotifyPropertyChanged("IsPregnant");
            }
        }
        private DateTime? pregnantdate;
        public DateTime? PregnantDate
        {
            get
            { return pregnantdate; }
            set
            { pregnantdate = value; }
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
            if (DOB.HasValue)
            {
                var diff = BaseDataBase.DateNow - DOB.Value;
                if (diff.Days / 30 / 12 < 12 || diff.Days / 30 / 12 > 120)
                {
                    isValid = false;
                    this.SetError("DOB", "التاريخ غير صالح يجب ادخال عمر صحيح");
                }
            }
            if (PregnantDate.HasValue)
            {
                var diff = BaseDataBase.DateNow - PregnantDate.Value;
                if (diff.Days < 25 || diff.Days > 299)
                {
                    isValid = false;
                    this.SetError("PregnantDate", "التاريخ غير صالح يجب ادخال تاريخ بداية حمل صحيح");
                }
            }
            if (string.IsNullOrEmpty(Status))
            {
                isValid = false;
                this.SetError("Status", "يجب اختيار الحالة");
            }
            if (string.IsNullOrEmpty(PID))
            {
                isValid = false;
                this.SetError("PID", "يجب إدخال الرقم الوطني");
            }
            else if (PID != "لايوجد رقم وطني")
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
                    string ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByParentPID", new SqlParameter("@ParenttID", ParentrID), new SqlParameter("@PID", PID));
                    if (!string.IsNullOrEmpty(ExistingFamilyCode))
                    {
                        isValid = false;
                        this.SetError("PID", "الرقم الوطني موجود في قاعدة البيانات برقم عائلة " + ExistingFamilyCode);
                    }
                    else
                    {
                        ExistingFamilyCode = BaseDataBase._Scalar_StoredProcedure("sp_GetFamilyCodeByFamilyPersonPID", new SqlParameter("@PID", PID));
                        if (!string.IsNullOrEmpty(ExistingFamilyCode))
                        {
                            isValid = false;
                            this.SetError("PID", "الرقم الوطني موجود في قاعدة البيانات ضمن افراد عائلة رقم " + ExistingFamilyCode);
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
