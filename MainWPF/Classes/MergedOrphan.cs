using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    class MergedOrphan
    {

        private int? studysideid;
        public int? StudySideID
        {
            get
            { return studysideid; }
            set
            { studysideid = value; }
        }



        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }



        private int? Familyid;
        public int? FamilyID
        {
            get
            { return Familyid; }
            set
            { Familyid = value; }
        }

        private string firstname;
        public string FirstName
        {
            get
            { return firstname; }
            set
            { firstname = value; }
        }

        private string middlename;
        public string MiddleName
        {
            get
            { return middlename; }
            set
            { middlename = value; }
        }

        private string lastname;
        public string LastName
        {
            get
            { return lastname; }
            set
            { lastname = value; }
        }

        private DateTime? dob;
        public DateTime? DOB
        {
            get
            { return dob; }
            set
            { dob = value; }
        }

        private string gender;
        public string Gender
        {
            get
            { return gender; }
            set
            { gender = value; }
        }

        private string sponsortype;
        public string SponsorType
        {
            get
            { return sponsortype; }
            set
            { sponsortype = value; }
        }
    }
}
