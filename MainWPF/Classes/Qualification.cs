using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class Qualification : ModelViewContext
    {

        public enum QualificationPersonType { Mother, Father, Child, Orphan, Lister, Supervisor };


        public Qualification()
        {
        }
        public Qualification(QualificationPersonType PersonType, int PersonID, string QualificationName, DateTime Date, string Image, string Notes)
        {
            this.PersonType = PersonType;
            this.PersonID = PersonID;
            this.QualificationName = QualificationName;
            this.DateOfQualification = Date;
            this.QualificationImage = Image;
            this.Notes = Notes;
        }


        private int? personQualificationID;
        public int? PersonQualificationID
        {
            get
            {
                return personQualificationID;
            }
            set
            {
                personQualificationID = value;
            }
        }

        private int? personID;
        public int? PersonID
        {
            get
            {
                return personID;
            }
            set
            {
                personID = value;
            }
        }

        private QualificationPersonType personType;
        public QualificationPersonType PersonType
        {
            get
            {
                return personType;
            }
            set
            {
                personType = value;
            }
        }

        private string qualificationImage;
        public string QualificationImage
        {
            get
            {
                return qualificationImage;
            }
            set
            {
                if (qualificationImage != value)
                {
                    qualificationImage = value;
                    this.NotifyPropertyChanged("QualificationImage");                
                }
            }
        }

        private DateTime? dateOfQualification;
        public DateTime? DateOfQualification
        {
            get
            {
                return dateOfQualification;
            }
            set
            {
                dateOfQualification = value;
            }
        }

        private string notes;
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }
        }

        private string qualificationName;
        public string QualificationName
        {
            get
            {
                return qualificationName;
            }
            set
            {
                qualificationName = value;
                this.ClearError("QualificationName");
                this.NotifyPropertyChanged("QualificationName");
            }
        }

        public bool InsertPersonQualificationData()
        {
            QualificationImage = BaseDataBase.CheckImageFile(QualificationImage);
            PersonQualificationID = BaseDataBase._StoredProcedureReturnable("sp_Add2PersonQualification"
                , new SqlParameter("@PersonQualificationID", System.Data.SqlDbType.Int)
                , new SqlParameter("@PersonType", PersonType)
                , new SqlParameter("@PersonID", PersonID)
                , new SqlParameter("@QualificationName", QualificationName)
                , new SqlParameter("@DateOfQualification", DateOfQualification)
                , new SqlParameter("@QualificationImage", QualificationImage)
                , new SqlParameter("@Notes", Notes));

            if (PersonQualificationID == null) return false;
            return true;
        }
        public bool UpdatePersonQualificationData()
        {
            QualificationImage = BaseDataBase.CheckImageFile(QualificationImage, Qualification.GetQualificationByID(PersonQualificationID).QualificationImage);
            return BaseDataBase._StoredProcedure("sp_UpdatePersonQualification"
                , new SqlParameter("@PersonQualificationID", PersonQualificationID)
                , new SqlParameter("@PersonID", PersonID)
                , new SqlParameter("@QualificationName", QualificationName)
                , new SqlParameter("@DateOfQualification", DateOfQualification)
                , new SqlParameter("@QualificationImage", QualificationImage)
                , new SqlParameter("@Notes", Notes));
        }
        public bool DeletePersonQualificationData()
        {
            if (BaseDataBase._StoredProcedure("sp_DeleteFromPersonQualification"
                , new SqlParameter("@PersonQualificationID", PersonQualificationID)))
            {
                BaseDataBase.DeleteImageFIle(QualificationImage);
                return true;
            }
            return false;
        }

        public static Qualification GetQualificationByID(int? id)
        {
            Qualification x = new Qualification();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetPersonQualificationByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@PersonQualificationID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["PersonQualificationID"] is DBNull))
                        x.PersonQualificationID = System.Int32.Parse(rd["PersonQualificationID"].ToString());
                    if (!(rd["PersonID"] is DBNull))
                        x.PersonID = System.Int32.Parse(rd["PersonID"].ToString());
                    x.QualificationName = rd["QualificationName"].ToString();
                    if (!(rd["DateOfQualification"] is DBNull))
                        x.DateOfQualification = System.DateTime.Parse(rd["DateOfQualification"].ToString());
                    x.QualificationImage = rd["QualificationImage"].ToString();
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
        public static List<Qualification> GetQualificationAllByPersonID(QualificationPersonType Type, int? ID)
        {
            List<Qualification> b = new List<Qualification>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetPersonQualificationAllByPersonID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr1 = new SqlParameter("@PersonID", ID);
            SqlParameter pr2 = new SqlParameter("@PersonType", Type);
            com.Parameters.Add(pr1);
            com.Parameters.Add(pr2);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetQualificationByID(rd.GetInt32(0)));
                }
            }
            finally
            {
                con.Close();
            }
            return b;
        }


        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(QualificationName))
            {
                isValid = false;
                this.SetError("QualificationName", "يجب إدخال اسم المؤهل أو الكفاءة");
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
