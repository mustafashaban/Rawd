using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class FamilyProperty : ModelViewContext
    {
        private int? familypropertyid;
        public int? FamilyPropertyID
        {
            get
            { return familypropertyid; }
            set
            { familypropertyid = value; }
        }



        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }



        private string propertytype;
        public string PropertyType
        {
            get
            { return propertytype; }
            set
            {
                propertytype = value;
                this.ClearError("PropertyType");
                this.NotifyPropertyChanged("PropertyType");
            }
        }



        private double? investmentvalue;
        public double? InvestmentValue
        {
            get
            { return investmentvalue; }
            set
            { investmentvalue = value; }
        }



        private string investmentcurrency;
        public string InvestmentCurrency
        {
            get
            { return investmentcurrency; }
            set
            { investmentcurrency = value; }
        }



        private double? realvalue;
        public double? RealValue
        {
            get
            { return realvalue; }
            set
            { realvalue = value; }
        }



        private string valuecurrency;
        public string ValueCurrency
        {
            get
            { return valuecurrency; }
            set
            { valuecurrency = value; }
        }



        private string evaluation;
        public string Evaluation
        {
            get
            { return evaluation; }
            set
            { evaluation = value; }
        }



        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            { date = value; }
        }



        private bool isexist;
        public bool IsExist
        {
            get
            { return isexist; }
            set
            { isexist = value; }
        }



        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public static FamilyProperty GetFamilyPropertyByID(int id)
        {
            FamilyProperty x = new FamilyProperty();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyPropertyByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyPropertyID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["FamilyPropertyID"] is DBNull))
                        x.FamilyPropertyID = System.Int32.Parse(rd["FamilyPropertyID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.PropertyType = rd["PropertyType"].ToString();
                    if (!(rd["InvestmentValue"] is DBNull))
                        x.InvestmentValue = System.Single.Parse(rd["InvestmentValue"].ToString());
                    x.InvestmentCurrency = rd["InvestmentCurrency"].ToString();
                    if (!(rd["RealValue"] is DBNull))
                        x.RealValue = System.Single.Parse(rd["RealValue"].ToString());
                    x.ValueCurrency = rd["ValueCurrency"].ToString();
                    x.Evaluation = rd["Evaluation"].ToString();
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["IsExist"] is DBNull))
                        x.IsExist = System.Boolean.Parse(rd["IsExist"].ToString());
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
        public static List<FamilyProperty> GetFamilyPropertyDataAllByFamilyID(int? FamilyID)
        {
            List<FamilyProperty> b = new List<FamilyProperty>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFamilyPropertyAllByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    b.Add(GetFamilyPropertyByID(rd.GetInt32(0)));
                }
            }
            catch { return b = new List<FamilyProperty>(); }
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
            if (string.IsNullOrEmpty(PropertyType))
            {
                isValid = false;
                this.SetError("PropertyType", "يجب إدخال نوع الملكية");
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
