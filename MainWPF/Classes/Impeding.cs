using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class Impeding : ModelViewContext
    {


        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }



        private string impedingtype;
        public string impedingType
        {
            get
            { return impedingtype; }
            set
            {
                impedingtype = value;
                this.ClearError("impedingType");
                this.NotifyPropertyChanged("impedingType");
            }
        }



        private string impedingdegree;
        public string impedingDegree
        {
            get
            { return impedingdegree; }
            set
            { impedingdegree = value; }
        }



        private string impedingidentityid;
        public string impedingIdentityID
        {
            get
            { return impedingidentityid; }
            set
            { impedingidentityid = value; }
        }



        private string impedingidentityimage;
        public string impedingIdentityImage
        {
            get
            { return impedingidentityimage; }
            set
            {
                if (impedingidentityimage != value)
                {
                    impedingidentityimage = value;
                    this.NotifyPropertyChanged("impedingIdentityImage");
                }
            }
        }



        private string relatedassociation;
        public string RelatedAssociation
        {
            get
            { return relatedassociation; }
            set
            { relatedassociation = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        private DateTime? impedingDate;
        public DateTime? ImpedingDate
        {
            get
            { return impedingDate; }
            set
            { impedingDate = value; }
        }


        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }



        public bool InsertData()
        {
            impedingIdentityImage = BaseDataBase.CheckImageFile(impedingidentityimage);

            ID = BaseDataBase._StoredProcedureReturnable("sp_Add2impeding"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@impedingType", impedingType)
                , new SqlParameter("@impedingDegree", impedingDegree)
                , new SqlParameter("@impedingIdentityID", impedingIdentityID)
                , new SqlParameter("@impedingIdentityImage", impedingIdentityImage)
                , new SqlParameter("@RelatedAssociation", RelatedAssociation)
                , new SqlParameter("@ImpedingDate", ImpedingDate)
                , new SqlParameter("@Notes", Notes)
                , new SqlParameter("@OrphanID", OrphanID));
            return (ID != null);
        }
        public bool UpdateData()
        {
            impedingIdentityImage = BaseDataBase.CheckImageFile(impedingidentityimage, Impeding.GetImpedingByID(ID).impedingIdentityImage);

            return BaseDataBase._StoredProcedure("sp_Updateimpeding"
                , new SqlParameter("@ID", ID)
                , new SqlParameter("@impedingType", impedingType)
                , new SqlParameter("@impedingDegree", impedingDegree)
                , new SqlParameter("@impedingIdentityID", impedingIdentityID)
                , new SqlParameter("@impedingIdentityImage", impedingIdentityImage)
                , new SqlParameter("@RelatedAssociation", RelatedAssociation)
                , new SqlParameter("@ImpedingDate", ImpedingDate)
                , new SqlParameter("@Notes", Notes)
                , new SqlParameter("@OrphanID", OrphanID));
        }
        public bool DeleteData()
        {
            if (BaseDataBase._StoredProcedure("sp_DeleteFromimpeding"
                , new SqlParameter("@ID", ID)))
            {
                BaseDataBase.DeleteImageFIle(impedingIdentityImage);
                return true;
            }
            return false;
        }


        public static Impeding GetImpedingByID(int? id)
        {
            Impeding x = new Impeding();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetimpedingByID", con);
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
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    x.impedingType = rd["impedingType"].ToString();
                    x.impedingDegree = rd["impedingDegree"].ToString();
                    x.impedingIdentityID = rd["impedingIdentityID"].ToString();
                    x.impedingIdentityImage = rd["impedingIdentityImage"].ToString();
                    x.RelatedAssociation = rd["RelatedAssociation"].ToString();
                    if (!(rd["ImpedingDate"] is DBNull))
                        x.ImpedingDate = DateTime.Parse(rd["ImpedingDate"].ToString());
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
        public static List<Impeding> GetImpedingAllByOrphanID(int? id)
        {
            var ois = new List<Impeding>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetImpedingAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    Impeding x = new Impeding();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    x.impedingType = rd["impedingType"].ToString();
                    x.impedingDegree = rd["impedingDegree"].ToString();
                    x.impedingIdentityID = rd["impedingIdentityID"].ToString();
                    x.impedingIdentityImage = rd["impedingIdentityImage"].ToString();
                    x.RelatedAssociation = rd["RelatedAssociation"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    if (!(rd["ImpedingDate"] is DBNull))
                        x.ImpedingDate = DateTime.Parse(rd["ImpedingDate"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());

                    ois.Add(x);
                }
                rd.Close();
            }
            catch
            {
                ois = new List<Impeding>();
            }
            finally
            {
                con.Close();
            }
            return ois;
        }


        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(impedingType))
            {
                isValid = false;
                this.SetError("impedingType", "يجب إدخال نوع الإعاقة");
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
