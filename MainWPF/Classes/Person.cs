using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public abstract class Person : ModelViewContext
    {
        private string firstname;
        public string FirstName
        {
            get
            { return firstname; }
            set
            {
                this.firstname = value;
                this.ClearError("FirstName");
                this.NotifyPropertyChanged("FirstName");
            }
        }



        private string lastname;
        public string LastName
        {
            get
            { return lastname; }
            set
            {
                lastname = value;
                this.ClearError("LastName");
                this.NotifyPropertyChanged("LastName");
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
                this.ClearError("Gender");
                this.NotifyPropertyChanged("Gender");
            }
        }

        private string birthplace;
        public string BirthPlace
        {
            get
            { return birthplace; }
            set
            { birthplace = value; }
        }



        private DateTime? dob;
        public DateTime? DOB
        {
            get
            { return dob; }
            set
            {
                dob = value;
                this.ClearError("DOB");
                this.NotifyPropertyChanged("DOB");
            }
        }

        private string nationality;
        public string Nationality
        {
            get
            { return nationality; }
            set
            { nationality = value; }
        }


        private string phone;
        public string Phone
        {
            get
            { return phone; }
            set
            {
                ClearError("Phone");
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Length != 7)
                        SetError("Phone", "الهاتف يجب أن يكون 7 أرقام");
                }
                phone = value;
            }
        }


        private string mobile;
        public string Mobile
        {
            get
            { return mobile; }
            set
            {
                ClearError("Mobile");
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Length != 10)
                        SetError("Mobile", "الموبايل يجب أن يكون 10 أرقام");
                }
                mobile = value;
            }
        }



        private string email;
        public string Email
        {
            get
            { return email; }
            set
            { email = value; }
        }



        private string pid;
        public string PID
        {
            get
            { return pid; }
            set
            {
                pid = value;
                this.ClearError("PID");
                this.NotifyPropertyChanged("PID");
            }
        }



        private DateTime? deathdate;
        public DateTime? DeathDate
        {
            get
            { return deathdate; }
            set
            { deathdate = value; }
        }



        private string deathreason;
        public string DeathReason
        {
            get
            { return deathreason; }
            set
            { deathreason = value; }
        }



        private string identityid;
        public string IdentityID
        {
            get
            { return identityid; }
            set
            { identityid = value; }
        }


        private string maritalstatus;
        public string MaritalStatus
        {
            get
            { return maritalstatus; }
            set
            { maritalstatus = value; }
        }
    }
}
