using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class Father : Parent
    {
        public Father()
        {
            Gender = "ذكر";
        }   

        public static Father GetFatherByID(int id)
        {
            Father x = new Father();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetParentByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@ParentID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ParentrID"] is DBNull))
                        x.ParentrID = System.Int32.Parse(rd["ParentrID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Nickname = rd["Nickname"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.FatherName = rd["FatherName"].ToString();
                    x.MotherName = rd["MotherName"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    if (!(rd["IsWorking"] is DBNull))
                        x.IsWorking = System.Boolean.Parse(rd["IsWorking"].ToString());
                    if (!(rd["Salary"] is DBNull))
                        x.Salary = System.Single.Parse(rd["Salary"].ToString());
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.JobAppointment = rd["JobAppointment"].ToString();
                    if (!(rd["IsImpeded"] is DBNull))
                        x.IsImpeded = System.Boolean.Parse(rd["IsImpeded"].ToString());
                    x.impededType = rd["impededType"].ToString();
                    x.ImpededKind = rd["ImpededKind"].ToString();

                    if (!(rd["impededDate"] is DBNull))
                        x.impededDate = System.DateTime.Parse(rd["impededDate"].ToString());
                    if (!(rd["IsLost"] is DBNull))
                        x.IsLost = System.Boolean.Parse(rd["IsLost"].ToString());
                    x.LostPlace = rd["LostPlace"].ToString();
                    if (!(rd["LostDate"] is DBNull))
                        x.LostDate = System.DateTime.Parse(rd["LostDate"].ToString());
                    x.BackDetailes = rd["BackDetailes"].ToString();
                    if (!(rd["BackDate"] is DBNull))
                        x.BackDate = System.DateTime.Parse(rd["BackDate"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.PID = rd["PID"].ToString();
                    if (!(rd["IsDead"] is DBNull))
                        x.IsDead = System.Boolean.Parse(rd["IsDead"].ToString());
                    if (!(rd["DeathDate"] is DBNull))
                        x.DeathDate = System.DateTime.Parse(rd["DeathDate"].ToString());
                    x.DeathReason = rd["DeathReason"].ToString();
                    x.DeathReportID = rd["DeathReportID"].ToString();
                    if (!(rd["DeathReportDate"] is DBNull))
                        x.DeathReportDate = System.DateTime.Parse(rd["DeathReportDate"].ToString());
                    x.Status = rd["Status"].ToString();
                    x.BondPlace = rd["BondPlace"].ToString();
                    x.BondNumber = rd["BondNumber"].ToString();
                    x.IdentityID = rd["IdentityID"].ToString();
                    if (!(rd["IdentityGivinDate"] is DBNull))
                        x.IdentityGivinDate = System.DateTime.Parse(rd["IdentityGivinDate"].ToString());
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.PersonalImage = rd["PersonalImage"].ToString();
                    if (!(rd["Tall"] is DBNull))
                        x.Tall = System.Int32.Parse(rd["Tall"].ToString());
                    if (!(rd["Weight"] is DBNull))
                        x.Weight = System.Int32.Parse(rd["Weight"].ToString());
                    if (!(rd["FeetSize"] is DBNull))
                        x.FeetSize = System.Int32.Parse(rd["FeetSize"].ToString());
                    if (!(rd["WaistSize"] is DBNull))
                        x.WaistSize = System.Int32.Parse(rd["WaistSize"].ToString());
                    x.GeneralWealthySituation = rd["GeneralWealthySituation"].ToString();
                    x.GeneralEthicalSituation = rd["GeneralEthicalSituation"].ToString();
                    x.BaseJob = rd["BaseJob"].ToString();
                    x.FatherSalary = rd["FatherSalary"].ToString();
                    if (!(rd["IsFatherAlive"] is DBNull))
                        x.IsFatherAlive = System.Boolean.Parse(rd["IsFatherAlive"].ToString());
                    if (!(rd["IsMotherAlive"] is DBNull))
                        x.IsMotherAlive = System.Boolean.Parse(rd["IsMotherAlive"].ToString());
                    x.PsychicalSituation = rd["PsychicalSituation"].ToString();
                    x.Ethics = rd["Ethics"].ToString();
                    x.HomePlace = rd["HomePlace"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.HealthStatus = rd["HealthStatus"].ToString();
                    x.StudyStatus = rd["StudyStatus"].ToString();
                    if (!(rd["IsNursemaid"] is DBNull))
                        x.IsNursemaid = System.Boolean.Parse(rd["IsNursemaid"].ToString());
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
        public static Father GetFatherByFamilyID(int id)
        {
            Father x = new Father();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetFatherByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ParentrID"] is DBNull))
                        x.ParentrID = System.Int32.Parse(rd["ParentrID"].ToString());
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    x.FirstName = rd["FirstName"].ToString();
                    x.LastName = rd["LastName"].ToString();
                    x.Nickname = rd["Nickname"].ToString();
                    x.Gender = rd["Gender"].ToString();
                    x.FatherName = rd["FatherName"].ToString();
                    x.MotherName = rd["MotherName"].ToString();
                    x.Nationality = rd["Nationality"].ToString();
                    x.BirthPlace = rd["BirthPlace"].ToString();
                    if (!(rd["DOB"] is DBNull))
                        x.DOB = System.DateTime.Parse(rd["DOB"].ToString());
                    x.Job = rd["Job"].ToString();
                    if (!(rd["IsWorking"] is DBNull))
                        x.IsWorking = System.Boolean.Parse(rd["IsWorking"].ToString());
                    if (!(rd["Salary"] is DBNull))
                        x.Salary = System.Single.Parse(rd["Salary"].ToString());
                    x.SalaryCurrency = rd["SalaryCurrency"].ToString();
                    x.JobAppointment = rd["JobAppointment"].ToString();
                    if (!(rd["IsImpeded"] is DBNull))
                        x.IsImpeded = System.Boolean.Parse(rd["IsImpeded"].ToString());
                    x.impededType = rd["impededType"].ToString();
                    x.ImpededKind = rd["ImpededKind"].ToString();
                    if (!(rd["impededDate"] is DBNull))
                        x.impededDate = System.DateTime.Parse(rd["impededDate"].ToString());
                    if (!(rd["IsLost"] is DBNull))
                        x.IsLost = System.Boolean.Parse(rd["IsLost"].ToString());
                    x.LostPlace = rd["LostPlace"].ToString();
                    if (!(rd["LostDate"] is DBNull))
                        x.LostDate = System.DateTime.Parse(rd["LostDate"].ToString());
                    x.BackDetailes = rd["BackDetailes"].ToString();
                    if (!(rd["BackDate"] is DBNull))
                        x.BackDate = System.DateTime.Parse(rd["BackDate"].ToString());
                    x.Phone = rd["Phone"].ToString();
                    x.Mobile = rd["Mobile"].ToString();
                    x.Email = rd["Email"].ToString();
                    x.PID = rd["PID"].ToString();
                    if (!(rd["IsDead"] is DBNull))
                        x.IsDead = System.Boolean.Parse(rd["IsDead"].ToString());
                    if (!(rd["DeathDate"] is DBNull))
                        x.DeathDate = System.DateTime.Parse(rd["DeathDate"].ToString());
                    x.DeathReason = rd["DeathReason"].ToString();
                    x.DeathReportID = rd["DeathReportID"].ToString();
                    if (!(rd["DeathReportDate"] is DBNull))
                        x.DeathReportDate = System.DateTime.Parse(rd["DeathReportDate"].ToString());
                    x.Status = rd["Status"].ToString();
                    x.BondPlace = rd["BondPlace"].ToString();
                    x.BondNumber = rd["BondNumber"].ToString();
                    x.IdentityID = rd["IdentityID"].ToString();
                    if (!(rd["IdentityGivinDate"] is DBNull))
                        x.IdentityGivinDate = System.DateTime.Parse(rd["IdentityGivinDate"].ToString());
                    x.IdentityImage = rd["IdentityImage"].ToString();
                    x.PersonalImage = rd["PersonalImage"].ToString();
                    if (!(rd["Tall"] is DBNull))
                        x.Tall = System.Int32.Parse(rd["Tall"].ToString());
                    if (!(rd["Weight"] is DBNull))
                        x.Weight = System.Int32.Parse(rd["Weight"].ToString());
                    if (!(rd["FeetSize"] is DBNull))
                        x.FeetSize = System.Int32.Parse(rd["FeetSize"].ToString());
                    if (!(rd["WaistSize"] is DBNull))
                        x.WaistSize = System.Int32.Parse(rd["WaistSize"].ToString());
                    x.GeneralWealthySituation = rd["GeneralWealthySituation"].ToString();
                    x.GeneralEthicalSituation = rd["GeneralEthicalSituation"].ToString();
                    x.BaseJob = rd["BaseJob"].ToString();
                    x.FatherSalary = rd["FatherSalary"].ToString();
                    if (!(rd["IsFatherAlive"] is DBNull))
                        x.IsFatherAlive = System.Boolean.Parse(rd["IsFatherAlive"].ToString());
                    if (!(rd["IsMotherAlive"] is DBNull))
                        x.IsMotherAlive = System.Boolean.Parse(rd["IsMotherAlive"].ToString());
                    x.PsychicalSituation = rd["PsychicalSituation"].ToString();
                    x.Ethics = rd["Ethics"].ToString();
                    x.HomePlace = rd["HomePlace"].ToString();
                    x.MaritalStatus = rd["MaritalStatus"].ToString();
                    x.HealthStatus = rd["HealthStatus"].ToString();
                    x.StudyStatus = rd["StudyStatus"].ToString();
                    if (!(rd["IsNursemaid"] is DBNull))
                        x.IsNursemaid = System.Boolean.Parse(rd["IsNursemaid"].ToString());
                    x.Notes = rd["Notes"].ToString();
                }
                rd.Close();
            }
            catch
            {
                x = new Father();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
    }
}
