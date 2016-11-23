using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainWPF
{
    public class DBMain
    {
        private static string ConnectionString
        { get { return BaseDataBase.ConnectionString; } }

        public static string CheckImageFile(string ImagePath, string OldImagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(ImagePath) && ImagePath.Remove(4) != "Data")
                {
                    Random x = new Random();
                    string s = "Data\\" + x.Next(0, 999999999);
                    while (System.IO.File.Exists(s))
                    {
                        s = "Data\\" + x.Next(0, 999999999);
                    }
                    System.IO.File.Copy(ImagePath, s);
                    return s;
                }
                if (!string.IsNullOrEmpty(OldImagePath) && OldImagePath.Remove(4) == "Data")
                {
                    DeleteImageFIle(OldImagePath);
                }
                return ImagePath;
            }
            catch { MessageBox.Show("خطأ في الوصول للملف "); return ImagePath; }
        }
        //=========================================================================================
        public static string CheckImageFile(string ImagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(ImagePath) && ImagePath.Remove(4) != "Data")
                {
                    Random x = new Random();
                    string s = "Data\\" + x.Next(0, 999999999);
                    while (System.IO.File.Exists(s))
                    {
                        s = "Data\\" + x.Next(0, 999999999);
                    }
                    System.IO.File.Copy(ImagePath, s);
                    return s;
                }
                return ImagePath;
            }
            catch { MessageBox.Show("خطأ في الوصول للملف "); return ImagePath; }
        }
        //=========================================================================================
        public static void DeleteImageFIle(string ImagePath)
        {
            try
            {
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
            }
            catch { }
        }
        //==========================================================================================
        private static bool _StoredProcedure(string StoredProcedure, params SqlParameter[] prs)
        {
            SqlConnection MyConn = new SqlConnection(DBMain.ConnectionString);
            try
            {
                MyConn.Open();
                string MyQuery = StoredProcedure;
                SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                MyComm.CommandType = CommandType.StoredProcedure;
                foreach (var item in prs)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                    MyComm.Parameters.Add(item);
                }
                MyComm.ExecuteNonQuery();
                MyConn.Close();
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
            finally { MyConn.Close(); }
        }
        //============================StoredProcedures + Parameters==========================
        private static int? _StoredProcedureReturnable(string StoredProcedureName, params SqlParameter[] prs)
        {
            prs[0].Direction = ParameterDirection.Output;
            SqlConnection MyConn = new SqlConnection(DBMain.ConnectionString);
            try
            {
                MyConn.Open();
                string MyQuery = StoredProcedureName;
                SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                MyComm.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < prs.Length; i++)
                {
                    if (prs[i].Value == null)
                    {
                        prs[i].Value = DBNull.Value;
                    }
                    MyComm.Parameters.Add(prs[i]);
                }
                MyComm.ExecuteNonQuery();
                MyConn.Close();
                return (int?)MyComm.Parameters[0].Value;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
            finally { MyConn.Close(); }
        }
        //============================StoredProcedures + Parameters==========================
        private static bool _StoredProcedureReturnableBool(string StoredProcedureName, params SqlParameter[] prs)
        {
            prs[0].Direction = ParameterDirection.Output;
            SqlConnection MyConn = new SqlConnection(DBMain.ConnectionString);
            try
            {
                MyConn.Open();
                string MyQuery = StoredProcedureName;
                SqlCommand MyComm = new SqlCommand(MyQuery, MyConn);
                MyComm.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < prs.Length; i++)
                {
                    if (prs[i].Value == null)
                    {
                        prs[i].Value = DBNull.Value;
                    }
                    MyComm.Parameters.Add(prs[i]);
                }
                MyComm.ExecuteNonQuery();
                MyConn.Close();
                return (bool)MyComm.Parameters[0].Value;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
            finally { MyConn.Close(); }
        }


        //Family
        public static bool InsertData(Family x)
        {
            x.FamilyReportImage = DBMain.CheckImageFile(x.FamilyReportImage);

            x.FamilyID = DBMain._StoredProcedureReturnable("sp_Add2Family"
                    , new SqlParameter("@FamilyID", System.Data.SqlDbType.Int)
                    , new SqlParameter("@FamilyCode", x.FamilyCode)
                    , new SqlParameter("@SectorID", x.SectorID)
                    , new SqlParameter("@FamilyName", x.FamilyName)
                    , new SqlParameter("@FamilyType", x.FamilyType)
                    , new SqlParameter("@FamilyIdentityID", x.FamilyIdentityID)
                    , new SqlParameter("@ApplyDate", x.ApplyDate)
                    , new SqlParameter("@FamilyReportImage", x.FamilyReportImage)
                    , new SqlParameter("@FamilySalary", x.FamilySalary)
                    , new SqlParameter("@SalarySupport", x.SalarySupport)
                    , new SqlParameter("@SalaryCurrency", x.SalaryCurrency)
                    , new SqlParameter("@DefinedPersonName", x.DefinedPersonName)
                    , new SqlParameter("@DefinedPersonPhone", x.DefinedPersonPhone)
                    , new SqlParameter("@FamilyPersonCount", x.FamilyPersonCount)
                    , new SqlParameter("@FamilyStatus", x.FamilyStatus)
                    , new SqlParameter("@Evaluation", x.Evaluation)
                    , new SqlParameter("@IsRecorded", x.IsRecorded)
                    , new SqlParameter("@HasCard", x.HasCard)
                    , new SqlParameter("@Notes", x.Notes)
                    , new SqlParameter("@CreatePerson", BaseDataBase.CurrentUser.Name)
                    );

            x.FamilyFather.FamilyID = x.FamilyID;
            x.FamilyMother.FamilyID = x.FamilyID;
            return x.FamilyID.HasValue;
        }
        public static bool UpdateData(Family x)
        {
            x.FamilyReportImage = DBMain.CheckImageFile(x.FamilyReportImage, Family.GetFamilyByID(x.FamilyID.Value).FamilyReportImage);

            bool b = DBMain._StoredProcedure("sp_UpdateFamily"
                  , new SqlParameter("@FamilyID", x.FamilyID)
                    , new SqlParameter("@FamilyCode", x.FamilyCode)
                    , new SqlParameter("@SectorID", x.SectorID)
                    , new SqlParameter("@FamilyName", x.FamilyName)
                    , new SqlParameter("@FamilyType", x.FamilyType)
                    , new SqlParameter("@FamilyIdentityID", x.FamilyIdentityID)
                    , new SqlParameter("@ApplyDate", x.ApplyDate)
                    , new SqlParameter("@FamilyReportImage", x.FamilyReportImage)
                    , new SqlParameter("@FamilySalary", x.FamilySalary)
                    , new SqlParameter("@SalarySupport", x.SalarySupport)
                    , new SqlParameter("@SalaryCurrency", x.SalaryCurrency)
                    , new SqlParameter("@DefinedPersonName", x.DefinedPersonName)
                    , new SqlParameter("@DefinedPersonPhone", x.DefinedPersonPhone)
                    , new SqlParameter("@FamilyPersonCount", x.FamilyPersonCount)
                    , new SqlParameter("@FamilyStatus", x.FamilyStatus)
                    , new SqlParameter("@Evaluation", x.Evaluation)
                    , new SqlParameter("@IsRecorded", x.IsRecorded)
                    , new SqlParameter("@HasCard", x.HasCard)
                    , new SqlParameter("@Notes", x.Notes)
                    , new SqlParameter("@LastModifiedPerson", BaseDataBase.CurrentUser.Name)
                    );

            x.FamilyFather.FamilyID = x.FamilyID;
            x.FamilyMother.FamilyID = x.FamilyID;

            return b;
        }
        public static bool UpdateCancelData(Family x)
        {
            return DBMain._StoredProcedure("sp_UpdateFamilyCancelData"
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@IsCanceled", x.IsCanceled)
                , new SqlParameter("@CancelDate", x.CancelDate)
                , new SqlParameter("@CancelReason", x.CancelReason)
                , new SqlParameter("@IsAcquittance", x.IsAcquittance)
                , new SqlParameter("@AcquittanceDate", x.AcquittanceDate));
        }
        public static bool DeleteData(Family x)
        {
            BaseDataBase._NonQuery(string.Format(@"
                                    delete from AdministrationEvaluation where FamilyID = {0};
                                    delete from ExternalFamilySupport where FamilyID = {0};
                                    delete from FamilyNeed where FamilyID ={0};
                                    delete from Orphan where FamilyID ={0};
                                    delete from House where FamilyID ={0};
                                    delete from Parent where FamilyID ={0};
                                    delete from FamilyPerson where FamilyID ={0};
                                    delete from FamilyProperty where FamilyID ={0};
                                    delete from TempChild where TempFamilyID in (select TempFamilyID from TempFamily where FamilyID = {0});
                                    delete from TempFamily where FamilyID ={0};
                                    delete from Lister_ListerGroup where GroupID in (select GroupID from ListerGroup where FamilyID = {0});
                                    delete from FamilyNeed_ListerGroup where ListerGroupID in (select GroupID from ListerGroup where FamilyID = {0});
                                    delete from ListerGroup where FamilyID ={0};
                                    delete from SpecialCardSource where BeneficiaryType = 1 and BeneficiaryID ={0};
                                    delete from Order_Item where OrderID in (select Id from [Order] where IsNull(FamilyID,-1) = {0});
                                    delete from [Order] where IsNull(FamilyID,-1) ={0};", x.FamilyID));
            if (DBMain._StoredProcedure("sp_DeleteFromFamily", new SqlParameter("@FamilyID", x.FamilyID)))
            {
                DBMain.DeleteImageFIle(x.FamilyReportImage);
                return true;
            }
            return false;
        }

        //AdminEvaluation
        public static bool InsertData(AdminEvaluation x)
        {
            x.ID = DBMain._StoredProcedureReturnable("sp_Add2AdministrationEvaluation"
                , new SqlParameter("@ID", System.Data.SqlDbType.Int)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@OrphanID", x.OrphanID)
                , new SqlParameter("@Evaluator", x.Evaluator)
                , new SqlParameter("@Evaluation", x.Evaluation)
                , new SqlParameter("@Reason", x.Reason)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@Notes", x.Notes));
            return x.ID.HasValue;
        }
        public static bool UpdateData(AdminEvaluation x)
        {
            return DBMain._StoredProcedure("sp_UpdateAdministrationEvaluation"
                   , new SqlParameter("@ID", x.ID)
                   , new SqlParameter("@FamilyID", x.FamilyID)
                   , new SqlParameter("@OrphanID", x.OrphanID)
                   , new SqlParameter("@Evaluator", x.Evaluator)
                   , new SqlParameter("@Evaluation", x.Evaluation)
                   , new SqlParameter("@Reason", x.Reason)
                   , new SqlParameter("@Date", x.Date)
                   , new SqlParameter("@Notes", x.Notes));
        }
        public static bool DeleteData(AdminEvaluation x)
        {
            return DBMain._StoredProcedure("sp_DeleteFromAdministrationEvaluation",
                new SqlParameter("@ID", x.ID));
        }

        //FamilyPerson
        public static bool InsertData(FamilyPerson x)
        {
            x.PersonalImage = DBMain.CheckImageFile(x.PersonalImage);
            x.DeathReportImage = DBMain.CheckImageFile(x.DeathReportImage);
            x.IdentityImage = DBMain.CheckImageFile(x.IdentityImage);

            if (!x.FamilyPersonID.HasValue && x.MaritalStatus == "يتيم")
            {
                try
                {
                    Orphan o = new Orphan(x);
                    o.InsertOrphanData();
                }
                catch { }
            }

            x.FamilyPersonID = DBMain._StoredProcedureReturnable("sp_Add2FamilyPerson"
                    , new SqlParameter("@FamilyPersonID", SqlDbType.Int)
                    , new SqlParameter("@FamilyID", x.FamilyID)
                    , new SqlParameter("@FirstName", x.FirstName)
                    , new SqlParameter("@LastName", x.LastName)
                    , new SqlParameter("@Nationality", x.Nationality)
                    , new SqlParameter("@BirthPlace", x.BirthPlace)
                    , new SqlParameter("@DOB", x.DOB)
                    , new SqlParameter("@Gender", x.Gender)
                    , new SqlParameter("@Job", x.Job)
                    , new SqlParameter("@IsStudyCutOff", x.IsStudyCutOff)
                    , new SqlParameter("@Salary", x.Salary)
                    , new SqlParameter("@SalaryCurrency", x.SalaryCurrency)
                    , new SqlParameter("@IsImpeded", x.IsImpeded)
                    , new SqlParameter("@impededType", x.impededType)
                    , new SqlParameter("@ImpededKind", x.ImpededKind)
                    , new SqlParameter("@impededDate", x.impededDate)
                    , new SqlParameter("@IsWantToCompleteStudy", x.IsWantToCompleteStudy)
                    , new SqlParameter("@StudyCuttOffReason", x.StudyCuttOffReason)
                    , new SqlParameter("@LostDate", x.LostDate)
                    , new SqlParameter("@BackDetailes", x.BackDetailes)
                    , new SqlParameter("@BackDate", x.BackDate)
                    , new SqlParameter("@Phone", x.Phone)
                    , new SqlParameter("@Mobile", x.Mobile)
                    , new SqlParameter("@Email", x.Email)
                    , new SqlParameter("@PID", x.PID)
                    , new SqlParameter("@DeathDate", x.DeathDate)
                    , new SqlParameter("@DeathReason", x.DeathReason)
                    , new SqlParameter("@DeathReportID", x.DeathReportID)
                    , new SqlParameter("@DeathReportDate", x.DeathReportDate)
                    , new SqlParameter("@BondPlace", x.BondPlace)
                    , new SqlParameter("@BondNumber", x.BondNumber)
                    , new SqlParameter("@IdentityID", x.IdentityID)
                    , new SqlParameter("@IdentityGivinDate", x.IdentityGivinDate)
                    , new SqlParameter("@Tall", x.Tall)
                    , new SqlParameter("@Wieght", x.Wieght)
                    , new SqlParameter("@PsychicalSituation", x.PsychicalSituation)
                    , new SqlParameter("@EthicsSituation", x.EthicsSituation)
                    , new SqlParameter("@HomePlace", x.HomePlace)
                    , new SqlParameter("@HomeBasePlace", x.HomeBasePlace)
                    , new SqlParameter("@MaritalStatus", x.MaritalStatus)
                    , new SqlParameter("@ChildrenCount", x.ChildrenCount)
                    , new SqlParameter("@Property", x.Property)
                    , new SqlParameter("@PersonalImage", x.PersonalImage)
                    , new SqlParameter("@IdentityImage", x.IdentityImage)
                    , new SqlParameter("@DeathReportImage", x.DeathReportImage)
                    , new SqlParameter("@RelationShip", x.RelationShip)
                    , new SqlParameter("@StudyStatus", x.StudyStatus)
                    , new SqlParameter("@HealthStatus", x.HealthStatus)
                    , new SqlParameter("@IsCancel", x.IsCancel)
                    , new SqlParameter("@CancelReason", x.CancelReason)
                    , new SqlParameter("@Notes", x.Notes));
            return x.FamilyPersonID.HasValue;
        }
        public static bool UpdateData(FamilyPerson x)
        {
            FamilyPerson fp = FamilyPerson.GetFamilyPersonByID(x.FamilyPersonID.Value);
            x.PersonalImage = DBMain.CheckImageFile(x.PersonalImage, fp.PersonalImage);
            x.DeathReportImage = DBMain.CheckImageFile(x.DeathReportImage, fp.DeathReportImage);
            x.IdentityImage = DBMain.CheckImageFile(x.IdentityImage, fp.IdentityImage);

            return BaseDataBase._StoredProcedure("sp_UpdateFamilyPerson"
                        , new SqlParameter("@FamilyPersonID", x.FamilyPersonID)
                        , new SqlParameter("@FamilyID", x.FamilyID)
                        , new SqlParameter("@FirstName", x.FirstName)
                        , new SqlParameter("@LastName", x.LastName)
                        , new SqlParameter("@Nationality", x.Nationality)
                        , new SqlParameter("@BirthPlace", x.BirthPlace)
                        , new SqlParameter("@DOB", x.DOB)
                        , new SqlParameter("@Gender", x.Gender)
                        , new SqlParameter("@Job", x.Job)
                        , new SqlParameter("@IsStudyCutOff", x.IsStudyCutOff)
                        , new SqlParameter("@Salary", x.Salary)
                        , new SqlParameter("@SalaryCurrency", x.SalaryCurrency)
                        , new SqlParameter("@IsImpeded", x.IsImpeded)
                        , new SqlParameter("@impededType", x.impededType)
                        , new SqlParameter("@ImpededKind", x.ImpededKind)
                        , new SqlParameter("@impededDate", x.impededDate)
                        , new SqlParameter("@IsWantToCompleteStudy", x.IsWantToCompleteStudy)
                        , new SqlParameter("@StudyCuttOffReason", x.StudyCuttOffReason)
                        , new SqlParameter("@LostDate", x.LostDate)
                        , new SqlParameter("@BackDetailes", x.BackDetailes)
                        , new SqlParameter("@BackDate", x.BackDate)
                        , new SqlParameter("@Phone", x.Phone)
                        , new SqlParameter("@Mobile", x.Mobile)
                        , new SqlParameter("@Email", x.Email)
                        , new SqlParameter("@PID", x.PID)
                        , new SqlParameter("@DeathDate", x.DeathDate)
                        , new SqlParameter("@DeathReason", x.DeathReason)
                        , new SqlParameter("@DeathReportID", x.DeathReportID)
                        , new SqlParameter("@DeathReportDate", x.DeathReportDate)
                        , new SqlParameter("@BondPlace", x.BondPlace)
                        , new SqlParameter("@BondNumber", x.BondNumber)
                        , new SqlParameter("@IdentityID", x.IdentityID)
                        , new SqlParameter("@IdentityGivinDate", x.IdentityGivinDate)
                        , new SqlParameter("@Tall", x.Tall)
                        , new SqlParameter("@Wieght", x.Wieght)
                        , new SqlParameter("@PsychicalSituation", x.PsychicalSituation)
                        , new SqlParameter("@EthicsSituation", x.EthicsSituation)
                        , new SqlParameter("@HomePlace", x.HomePlace)
                        , new SqlParameter("@HomeBasePlace", x.HomeBasePlace)
                        , new SqlParameter("@MaritalStatus", x.MaritalStatus)
                        , new SqlParameter("@ChildrenCount", x.ChildrenCount)
                        , new SqlParameter("@Property", x.Property)
                        , new SqlParameter("@PersonalImage", x.PersonalImage)
                        , new SqlParameter("@IdentityImage", x.IdentityImage)
                        , new SqlParameter("@DeathReportImage", x.DeathReportImage)
                        , new SqlParameter("@RelationShip", x.RelationShip)
                        , new SqlParameter("@StudyStatus", x.StudyStatus)
                        , new SqlParameter("@HealthStatus", x.HealthStatus)
                        , new SqlParameter("@IsCancel", x.IsCancel)
                        , new SqlParameter("@CancelReason", x.CancelReason)
                        , new SqlParameter("@Notes", x.Notes));
        }
        public static bool DeleteData(FamilyPerson x)
        {
            if (BaseDataBase._StoredProcedure("sp_DeleteFromFamilyPerson"
                    , new SqlParameter("@FamilyPersonID", x.FamilyPersonID)))
            {
                DBMain.DeleteImageFIle(x.IdentityImage);
                DBMain.DeleteImageFIle(x.DeathReportImage);
                DBMain.DeleteImageFIle(x.PersonalImage);
                return true;
            }
            return false;
        }

        //ExternalCourse
        public static bool InsertData(ExternalCourse x)
        {
            x.ExternalCourseID = DBMain._StoredProcedureReturnable("sp_Add2ExternalCourse"
                , new SqlParameter("@ExternalCourseID", SqlDbType.Int)
                , new SqlParameter("@ExternalCourseName", x.ExternalCourseName)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@PersonID", x.PersonID)
                , new SqlParameter("@Type", x.Type)
                , new SqlParameter("@SupportCourse", x.SupportCourse)
                , new SqlParameter("@Notes", x.Notes));
            return x.ExternalCourseID.HasValue;
        }
        public static bool UpdateData(ExternalCourse x)
        {
            return DBMain._StoredProcedure("sp_UpdateExternalCourse"
                , new SqlParameter("@ExternalCourseID", x.ExternalCourseID)
                , new SqlParameter("@ExternalCourseName", x.ExternalCourseName)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@PersonID", x.PersonID)
                , new SqlParameter("@Type", x.Type)
                , new SqlParameter("@SupportCourse", x.SupportCourse)
                , new SqlParameter("@Notes", x.Notes));
        }

        //FamilyNeed
        public static bool InsertData(FamilyNeed x)
        {
            x.FamilyNeedID = DBMain._StoredProcedureReturnable("sp_Add2FamilyNeed"
                    , new SqlParameter("@FamilyNeedID", System.Data.SqlDbType.Int)
                    , new SqlParameter("@FamilyID", x.FamilyID)
                    , new SqlParameter("@NeedType", x.NeedType)
                    , new SqlParameter("@NeedName", x.NeedName)
                    , new SqlParameter("@Value", x.Value)
                    , new SqlParameter("@AskedBy", x.AskedBy)
                    , new SqlParameter("@Date", x.Date)
                    , new SqlParameter("@IsEnsured", x.IsEnsured)
                    , new SqlParameter("@EnsuredSupport", x.EnsuredSupport)
                    , new SqlParameter("@Notes", x.Notes));
            return x.FamilyNeedID.HasValue;
        }
        public static bool UpdateData(FamilyNeed x)
        {
            return DBMain._StoredProcedure("sp_UpdateFamilyNeed"
                    , new SqlParameter("@FamilyNeedID", x.FamilyNeedID)
                    , new SqlParameter("@FamilyID", x.FamilyID)
                    , new SqlParameter("@NeedType", x.NeedType)
                    , new SqlParameter("@NeedName", x.NeedName)
                    , new SqlParameter("@Value", x.Value)
                    , new SqlParameter("@AskedBy", x.AskedBy)
                    , new SqlParameter("@Date", x.Date)
                    , new SqlParameter("@IsEnsured", x.IsEnsured)
                    , new SqlParameter("@EnsuredSupport", x.EnsuredSupport)
                    , new SqlParameter("@Notes", x.Notes));
        }
        public static bool DeleteData(FamilyNeed x)
        {
            return DBMain._StoredProcedure("sp_DeleteFromFamilyNeed"
                , new SqlParameter("@FamilyNeedID", x.FamilyNeedID));
        }

        //FamilyProperty
        public static bool InsertData(FamilyProperty x)
        {
            x.FamilyPropertyID = DBMain._StoredProcedureReturnable("sp_Add2FamilyProperty"
            , new SqlParameter("@FamilyPropertyID", System.Data.SqlDbType.Int)
            , new SqlParameter("@FamilyID", x.FamilyID)
            , new SqlParameter("@PropertyType", x.PropertyType)
            , new SqlParameter("@InvestmentValue", x.InvestmentValue)
            , new SqlParameter("@InvestmentCurrency", x.InvestmentCurrency)
            , new SqlParameter("@RealValue", x.RealValue)
            , new SqlParameter("@ValueCurrency", x.ValueCurrency)
            , new SqlParameter("@Evaluation", x.Evaluation)
            , new SqlParameter("@Date", x.Date)
            , new SqlParameter("@IsExist", x.IsExist)
            , new SqlParameter("@Notes", x.Notes));

            return x.FamilyPropertyID.HasValue;
        }
        public static bool UpdateData(FamilyProperty x)
        {
            return DBMain._StoredProcedure("sp_UpdateFamilyProperty"
            , new SqlParameter("@FamilyPropertyID", x.FamilyPropertyID)
            , new SqlParameter("@FamilyID", x.FamilyID)
            , new SqlParameter("@PropertyType", x.PropertyType)
            , new SqlParameter("@InvestmentValue", x.InvestmentValue)
            , new SqlParameter("@InvestmentCurrency", x.InvestmentCurrency)
            , new SqlParameter("@RealValue", x.RealValue)
            , new SqlParameter("@ValueCurrency", x.ValueCurrency)
            , new SqlParameter("@Evaluation", x.Evaluation)
            , new SqlParameter("@Date", x.Date)
            , new SqlParameter("@IsExist", x.IsExist)
            , new SqlParameter("@Notes", x.Notes));
        }
        public static bool DeleteData(FamilyProperty x)
        {
            return DBMain._StoredProcedure("sp_DeleteFromFamilyProperty"
                , new SqlParameter("@FamilyPropertyID", x.FamilyPropertyID));
        }

        //House
        public static bool InsertData(House x)
        {
            x.Photo = DBMain.CheckImageFile(x.Photo);

            x.HouseID = DBMain._StoredProcedureReturnable("sp_Add2House"
                , new SqlParameter("@HouseID", System.Data.SqlDbType.Int)
                , new SqlParameter("@Area", x.Area)
                , new SqlParameter("@Value", x.Value)
                , new SqlParameter("@SectorPartNum", x.SectorPartNum)
                , new SqlParameter("@OldAddress", x.OldAddress)
                , new SqlParameter("@HouseSection", x.HouseSection)
                , new SqlParameter("@HouseStreet", x.HouseStreet)
                , new SqlParameter("@Address", x.Address)
                , new SqlParameter("@HouseBuildingNumber", x.HouseBuildingNumber)
                , new SqlParameter("@HouseFloor", x.HouseFloor)
                , new SqlParameter("@Photo", x.Photo)
                , new SqlParameter("@HouseSituation", x.HouseSituation)
                , new SqlParameter("@FurnitureSituation", x.FurnitureSituation)
                , new SqlParameter("@RoomsSize", x.RoomsSize)
                , new SqlParameter("@Lights", x.Lights)
                , new SqlParameter("@Airing", x.Airing)
                , new SqlParameter("@RoomsCount", x.RoomsCount)
                , new SqlParameter("@NeaistSchools", x.NeaistSchools)
                , new SqlParameter("@NearistJawamea", x.NearistJawamea)
                , new SqlParameter("@HouseType", x.HouseType)
                , new SqlParameter("@Owner", x.Owner)
                , new SqlParameter("@Phone", x.Phone)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@DigitalAddress", x.DigitalAddress)
                , new SqlParameter("@HouseStatus", x.HouseStatus)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@OrphanID", x.OrphanID)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@EndDate", x.EndDate));
            return x.HouseID.HasValue;
        }
        public static bool UpdateData(House x)
        {
            x.Photo = DBMain.CheckImageFile(x.Photo, House.GetHouseByID(x.HouseID).Photo);

            return DBMain._StoredProcedure("sp_UpdateHouse"
                , new SqlParameter("@HouseID", x.HouseID)
                , new SqlParameter("@Area", x.Area)
                , new SqlParameter("@Value", x.Value)
                , new SqlParameter("@SectorPartNum", x.SectorPartNum)
                , new SqlParameter("@OldAddress", x.OldAddress)
                , new SqlParameter("@HouseSection", x.HouseSection)
                , new SqlParameter("@HouseStreet", x.HouseStreet)
                , new SqlParameter("@Address", x.Address)
                , new SqlParameter("@HouseBuildingNumber", x.HouseBuildingNumber)
                , new SqlParameter("@HouseFloor", x.HouseFloor)
                , new SqlParameter("@Photo", x.Photo)
                , new SqlParameter("@HouseSituation", x.HouseSituation)
                , new SqlParameter("@FurnitureSituation", x.FurnitureSituation)
                , new SqlParameter("@RoomsSize", x.RoomsSize)
                , new SqlParameter("@Lights", x.Lights)
                , new SqlParameter("@Airing", x.Airing)
                , new SqlParameter("@RoomsCount", x.RoomsCount)
                , new SqlParameter("@NeaistSchools", x.NeaistSchools)
                , new SqlParameter("@NearistJawamea", x.NearistJawamea)
                , new SqlParameter("@HouseType", x.HouseType)
                , new SqlParameter("@Owner", x.Owner)
                , new SqlParameter("@Phone", x.Phone)
                , new SqlParameter("@DigitalAddress", x.DigitalAddress)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@HouseStatus", x.HouseStatus)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@OrphanID", x.OrphanID)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@EndDate", x.EndDate));
        }
        public static bool DeleteData(House x)
        {
            if (DBMain._StoredProcedure("sp_DeleteFromHouse"
                , new SqlParameter("@HouseID", x.HouseID)))
            {
                DBMain.DeleteImageFIle(x.Photo);
                return true;
            }
            return false;
        }

        //Parent
        public static bool InsertData(Parent x)
        {
            x.IdentityImage = DBMain.CheckImageFile(x.IdentityImage);
            x.PersonalImage = DBMain.CheckImageFile(x.PersonalImage);

            x.ParentrID = DBMain._StoredProcedureReturnable("sp_Add2Parent"
                , new SqlParameter("@ParentrID", System.Data.SqlDbType.Int)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@FirstName", x.FirstName)
                , new SqlParameter("@LastName", x.LastName)
                , new SqlParameter("@Nickname", x.Nickname)
                , new SqlParameter("@Gender", x.Gender)
                , new SqlParameter("@FatherName", x.FatherName)
                , new SqlParameter("@MotherName", x.MotherName)
                , new SqlParameter("@Nationality", x.Nationality)
                , new SqlParameter("@BirthPlace", x.BirthPlace)
                , new SqlParameter("@DOB", x.DOB)
                , new SqlParameter("@Job", x.Job)
                , new SqlParameter("@IsWorking", x.IsWorking)
                , new SqlParameter("@Salary", x.Salary)
                , new SqlParameter("@SalaryCurrency", x.SalaryCurrency)
                , new SqlParameter("@JobAppointment", x.JobAppointment)
                , new SqlParameter("@IsImpeded", x.IsImpeded)
                , new SqlParameter("@impededType", x.impededType)
                , new SqlParameter("@impededDate", x.impededDate)
                , new SqlParameter("@IsLost", x.IsLost)
                , new SqlParameter("@LostPlace", x.LostPlace)
                , new SqlParameter("@LostDate", x.LostDate)
                , new SqlParameter("@BackDetailes", x.BackDetailes)
                , new SqlParameter("@BackDate", x.BackDate)
                , new SqlParameter("@Phone", x.Phone)
                , new SqlParameter("@Mobile", x.Mobile)
                , new SqlParameter("@Email", x.Email)
                , new SqlParameter("@PID", x.PID)
                , new SqlParameter("@IsDead", x.IsDead)
                , new SqlParameter("@DeathDate", x.DeathDate)
                , new SqlParameter("@DeathReason", x.DeathReason)
                , new SqlParameter("@DeathReportID", x.DeathReportID)
                , new SqlParameter("@DeathReportDate", x.DeathReportDate)
                , new SqlParameter("@Status", x.Status)
                , new SqlParameter("@BondPlace", x.BondPlace)
                , new SqlParameter("@BondNumber", x.BondNumber)
                , new SqlParameter("@IdentityID", x.IdentityID)
                , new SqlParameter("@IdentityGivinDate", x.IdentityGivinDate)
                , new SqlParameter("@IdentityImage", x.IdentityImage)
                , new SqlParameter("@PersonalImage", x.PersonalImage)
                , new SqlParameter("@Tall", x.Tall)
                , new SqlParameter("@Weight", x.Weight)
                , new SqlParameter("@FeetSize", x.FeetSize)
                , new SqlParameter("@WaistSize", x.WaistSize)
                , new SqlParameter("@GeneralWealthySituation", x.GeneralWealthySituation)
                , new SqlParameter("@GeneralEthicalSituation", x.GeneralEthicalSituation)
                , new SqlParameter("@BaseJob", x.BaseJob)
                , new SqlParameter("@FatherSalary", x.FatherSalary)
                , new SqlParameter("@IsFatherAlive", x.IsFatherAlive)
                , new SqlParameter("@IsMotherAlive", x.IsMotherAlive)
                , new SqlParameter("@PsychicalSituation", x.PsychicalSituation)
                , new SqlParameter("@Ethics", x.Ethics)
                , new SqlParameter("@HomePlace", x.HomePlace)
                , new SqlParameter("@MaritalStatus", x.MaritalStatus)
                , new SqlParameter("@IsNursemaid", x.IsNursemaid)
                , new SqlParameter("@HealthStatus", x.HealthStatus)
                , new SqlParameter("@StudyStatus", x.StudyStatus)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@IsPregnant", x.IsPregnant)
                , new SqlParameter("@PregnantDate", x.PregnantDate)
                , new SqlParameter("@ImpededKind", x.ImpededKind));
            return x.ParentrID.HasValue;
        }
        public static bool UpdateData(Parent x)
        {
            Parent p;
            if (x.Gender == "ذكر")
                p = Father.GetFatherByID(x.ParentrID.Value);
            else p = Mother.GetMotherByFamilyID(x.ParentrID.Value);

            x.IdentityImage = DBMain.CheckImageFile(x.IdentityImage, p.IdentityImage);
            x.PersonalImage = DBMain.CheckImageFile(x.PersonalImage, p.PersonalImage);

            return DBMain._StoredProcedure("sp_UpdateParent"
                , new SqlParameter("@ParentrID", x.ParentrID)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@FirstName", x.FirstName)
                , new SqlParameter("@LastName", x.LastName)
                , new SqlParameter("@Nickname", x.Nickname)
                , new SqlParameter("@Gender", x.Gender)
                , new SqlParameter("@FatherName", x.FatherName)
                , new SqlParameter("@MotherName", x.MotherName)
                , new SqlParameter("@Nationality", x.Nationality)
                , new SqlParameter("@BirthPlace", x.BirthPlace)
                , new SqlParameter("@DOB", x.DOB)
                , new SqlParameter("@Job", x.Job)
                , new SqlParameter("@IsWorking", x.IsWorking)
                , new SqlParameter("@Salary", x.Salary)
                , new SqlParameter("@SalaryCurrency", x.SalaryCurrency)
                , new SqlParameter("@JobAppointment", x.JobAppointment)
                , new SqlParameter("@IsImpeded", x.IsImpeded)
                , new SqlParameter("@impededType", x.impededType)
                , new SqlParameter("@impededDate", x.impededDate)
                , new SqlParameter("@IsLost", x.IsLost)
                , new SqlParameter("@LostPlace", x.LostPlace)
                , new SqlParameter("@LostDate", x.LostDate)
                , new SqlParameter("@BackDetailes", x.BackDetailes)
                , new SqlParameter("@BackDate", x.BackDate)
                , new SqlParameter("@Phone", x.Phone)
                , new SqlParameter("@Mobile", x.Mobile)
                , new SqlParameter("@Email", x.Email)
                , new SqlParameter("@PID", x.PID)
                , new SqlParameter("@IsDead", x.IsDead)
                , new SqlParameter("@DeathDate", x.DeathDate)
                , new SqlParameter("@DeathReason", x.DeathReason)
                , new SqlParameter("@DeathReportID", x.DeathReportID)
                , new SqlParameter("@DeathReportDate", x.DeathReportDate)
                , new SqlParameter("@Status", x.Status)
                , new SqlParameter("@BondPlace", x.BondPlace)
                , new SqlParameter("@BondNumber", x.BondNumber)
                , new SqlParameter("@IdentityID", x.IdentityID)
                , new SqlParameter("@IdentityGivinDate", x.IdentityGivinDate)
                , new SqlParameter("@IdentityImage", x.IdentityImage)
                , new SqlParameter("@PersonalImage", x.PersonalImage)
                , new SqlParameter("@Tall", x.Tall)
                , new SqlParameter("@Weight", x.Weight)
                , new SqlParameter("@FeetSize", x.FeetSize)
                , new SqlParameter("@WaistSize", x.WaistSize)
                , new SqlParameter("@GeneralWealthySituation", x.GeneralWealthySituation)
                , new SqlParameter("@GeneralEthicalSituation", x.GeneralEthicalSituation)
                , new SqlParameter("@BaseJob", x.BaseJob)
                , new SqlParameter("@FatherSalary", x.FatherSalary)
                , new SqlParameter("@IsFatherAlive", x.IsFatherAlive)
                , new SqlParameter("@IsMotherAlive", x.IsMotherAlive)
                , new SqlParameter("@PsychicalSituation", x.PsychicalSituation)
                , new SqlParameter("@Ethics", x.Ethics)
                , new SqlParameter("@HomePlace", x.HomePlace)
                , new SqlParameter("@MaritalStatus", x.MaritalStatus)
                , new SqlParameter("@IsNursemaid", x.IsNursemaid)
                , new SqlParameter("@HealthStatus", x.HealthStatus)
                , new SqlParameter("@StudyStatus", x.StudyStatus)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@IsPregnant", x.IsPregnant)
                , new SqlParameter("@PregnantDate", x.PregnantDate)
                , new SqlParameter("@ImpededKind", x.ImpededKind));
        }
        public static bool DeleteData(Parent x)
        {
            if (DBMain._StoredProcedure("sp_DeleteFromParent"
                , new SqlParameter("@ParentrID", x.ParentrID)))
            {
                DBMain.DeleteImageFIle(x.Status);
                DBMain.DeleteImageFIle(x.IdentityImage);
                DBMain.DeleteImageFIle(x.PersonalImage);
                return true;
            }
            return false;
        }

        //ListerGroup
        public static bool InsertData(ListerGroup x)
        {
            x.GroupID = DBMain._StoredProcedureReturnable("sp_Add2ListerGroup"
                    , new SqlParameter("@GroupID", System.Data.SqlDbType.Int)
                    , new SqlParameter("@FamilyID", x.FamilyID)
                    , new SqlParameter("@OrphanID", x.OrphanID)
                    , new SqlParameter("@Evaluation", x.Evaluation)
                    , new SqlParameter("@Date", x.Date)
                    , new SqlParameter("@Notes", x.Notes)
                    , new SqlParameter("@CreatePerson", BaseDataBase.CurrentUser.Name)
                    , new SqlParameter("@CreateDate", BaseDataBase.DateNow)
                    , new SqlParameter("@LastModifiedPerson", x.LastModifiedPerson)
                    );
            if (x.GroupID != null)
            {
                foreach (var l in x.Listers)
                {
                    DBMain._StoredProcedure("sp_Add2Lister_ListerGroup"
                            , new SqlParameter("@ListerID", l.ListerID)
                            , new SqlParameter("@GroupID", x.GroupID));
                }

                foreach (var item in x.FamilyNeeds)
                {
                    item.ListerGroupID = x.GroupID;
                    FamilyNeed_ListerGroup.InsertData(item);
                }
                return true;
            }
            return false;
        }
        public static bool UpdateData(ListerGroup x)
        {
            DBMain._StoredProcedure("sp_DeleteFromLister_ListerGroup", new SqlParameter("@GroupID", x.GroupID));
            foreach (var l in x.Listers)
            {
                DBMain._StoredProcedure("sp_Add2Lister_ListerGroup"
                        , new SqlParameter("@ListerID", l.ListerID)
                        , new SqlParameter("@GroupID", x.GroupID));
            }

            BaseDataBase._StoredProcedure("sp_DeleteFromFamilyNeed_ListerGroupByListerGroupID", new SqlParameter("ListerGroupID", x.GroupID));
            foreach (var item in x.FamilyNeeds)
            {
                item.ListerGroupID = x.GroupID;
                FamilyNeed_ListerGroup.InsertData(item);
            }
            return DBMain._StoredProcedure("sp_UpdateListerGroup"
                , new SqlParameter("@GroupID", x.GroupID)
                , new SqlParameter("@OrphanID", x.OrphanID)
                , new SqlParameter("@Evaluation", x.Evaluation)
                , new SqlParameter("@Date", x.Date)
                , new SqlParameter("@Notes", x.Notes)
                , new SqlParameter("@FamilyID", x.FamilyID)
                , new SqlParameter("@CreatePerson", x.CreatePerson)
                , new SqlParameter("@CreateDate", x.CreateDate)
                , new SqlParameter("@LastModifiedPerson", BaseDataBase.CurrentUser.Name)
                );
        }
        public static bool DeleteData(ListerGroup x)
        {
            BaseDataBase._StoredProcedure("sp_DeleteFromFamilyNeed_ListerGroupByListerGroupID", new SqlParameter("ListerGroupID", x.GroupID));
            return DBMain._StoredProcedure("sp_DeleteFromListerGroup"
                , new SqlParameter("@GroupID", x.GroupID));
        }

        //Lister
        public static bool InsertData(Lister x)
        {
            x.IdentityImage = DBMain.CheckImageFile(x.IdentityImage);

            x.ListerID = DBMain._StoredProcedureReturnable("sp_Add2Lister"
                , new SqlParameter("@ListerID", SqlDbType.Int)
                , new SqlParameter("@FirstName", x.FirstName)
                , new SqlParameter("@LastName", x.LastName)
                , new SqlParameter("@Gender", x.Gender)
                , new SqlParameter("@BirthPlace", x.BirthPlace)
                , new SqlParameter("@DOB", x.DOB)
                , new SqlParameter("@Job", x.Job)
                , new SqlParameter("@Phone", x.Phone)
                , new SqlParameter("@Mobile", x.Mobile)
                , new SqlParameter("@Email", x.Email)
                , new SqlParameter("@MaritalStatus", x.MaritalStatus)
                , new SqlParameter("@ChildCount", x.ChildCount)
                , new SqlParameter("@PlaceAddress", x.PlaceAddress)
                , new SqlParameter("@ScientificQualifier", x.ScientificQualifier)
                , new SqlParameter("@IdentityImage", x.IdentityImage)
                , new SqlParameter("@Notes", x.Notes));
            return x.ListerID.HasValue;
        }
        public static bool UpdateData(Lister x)
        {
            x.IdentityImage = DBMain.CheckImageFile(x.IdentityImage, Lister.GetListerByID(x.ListerID).IdentityImage);

            return DBMain._StoredProcedure("sp_UpdateLister"
                , new SqlParameter("@ListerID", x.ListerID)
                , new SqlParameter("@FirstName", x.FirstName)
                , new SqlParameter("@LastName", x.LastName)
                , new SqlParameter("@Gender", x.Gender)
                , new SqlParameter("@BirthPlace", x.BirthPlace)
                , new SqlParameter("@DOB", x.DOB)
                , new SqlParameter("@Job", x.Job)
                , new SqlParameter("@Phone", x.Phone)
                , new SqlParameter("@Mobile", x.Mobile)
                , new SqlParameter("@Email", x.Email)
                , new SqlParameter("@MaritalStatus", x.MaritalStatus)
                , new SqlParameter("@ChildCount", x.ChildCount)
                , new SqlParameter("@PlaceAddress", x.PlaceAddress)
                , new SqlParameter("@ScientificQualifier", x.ScientificQualifier)
                , new SqlParameter("@IdentityImage", x.IdentityImage)
                , new SqlParameter("@Notes", x.Notes));
        }
        public static bool DeleteData(Lister x)
        {
            if (DBMain._StoredProcedure("sp_DeleteFromLister"
                , new SqlParameter("@ListerID", x.ListerID)))
            {
                DBMain.DeleteImageFIle(x.IdentityImage);
                return true;
            }
            return false;
        }

        public static bool UpdatePregnant(Family x)
        {
            return DBMain._StoredProcedure("sp_UpdatePregnant"
            , new SqlParameter("@FamilyID", x.FamilyID)
            );
        }
        public static async Task<bool> UpdateAllPregnant()
        {
            return (await BaseDataBase._StoredProcedureAsync("sp_UpdateAllPregnant"));
        }
    }
}
