using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MainWPF
{
    public class House : ModelViewContext
    {
        private int? houseid;
        public int? HouseID
        {
            get
            { return houseid; }
            set
            { houseid = value; }
        }

        private string area;
        public string Area
        {
            get
            { return area; }
            set
            { area = value; }
        }



        private string _value;
        public string Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }



        private int? sectorpartnum;
        public int? SectorPartNum
        {
            get
            { return sectorpartnum; }
            set
            {
                sectorpartnum = value;
                this.ClearError("SectorPartNum");
                this.NotifyPropertyChanged("SectorPartNum");
            }
        }

        private string houseSection;
        public string HouseSection
        {
            get
            { return houseSection; }
            set
            {
                if (value != houseSection)
                {
                    houseSection = value;
                    this.ClearError("HouseSection");
                    if (string.IsNullOrEmpty(HouseSection))
                    {
                        this.SetError("HouseSection", "يجب إدخال منطقة السكن");
                    }
                    this.NotifyPropertyChanged("HouseSection");
                }
            }
        }

        private string houseStreet;
        public string HouseStreet
        {
            get
            { return houseStreet; }
            set
            {
                houseStreet = value;
                this.ClearError("HouseStreet");
                this.NotifyPropertyChanged("HouseStreet");
            }
        }

        private string houseBuildingNumber;
        public string HouseBuildingNumber
        {
            get
            { return houseBuildingNumber; }
            set
            {
                houseBuildingNumber = value;
                this.ClearError("HouseBuildingNumber");
                this.NotifyPropertyChanged("HouseBuildingNumber");
            }
        }

        private string houseFloor;
        public string HouseFloor
        {
            get
            { return houseFloor; }
            set
            {
                houseFloor = value;
                this.ClearError("HouseFloor");
                this.NotifyPropertyChanged("HouseFloor");
            }
        }

        private string address;
        public string Address
        {
            get
            { return address; }
            set
            {
                address = value;
                this.ClearError("Address");
                this.NotifyPropertyChanged("Address");
            }
        }

        private string oldAddress;
        public string OldAddress
        {
            get
            { return oldAddress; }
            set
            {
                oldAddress = value;
                this.ClearError("OldAddress");
                this.NotifyPropertyChanged("OldAddress");
            }
        }


        private string photo;
        public string Photo
        {
            get
            { return photo; }
            set
            {
                if (photo != value)
                {
                    photo = value;
                    this.NotifyPropertyChanged("Photo");
                }
            }
        }



        private string housesituation;
        public string HouseSituation
        {
            get
            { return housesituation; }
            set
            { housesituation = value; }
        }



        private string furnituresituation;
        public string FurnitureSituation
        {
            get
            { return furnituresituation; }
            set
            { furnituresituation = value; }
        }



        private string roomssize;
        public string RoomsSize
        {
            get
            { return roomssize; }
            set
            { roomssize = value; }
        }



        private string lights;
        public string Lights
        {
            get
            { return lights; }
            set
            { lights = value; }
        }



        private string airing;
        public string Airing
        {
            get
            { return airing; }
            set
            { airing = value; }
        }



        private int? roomscount;
        public int? RoomsCount
        {
            get
            { return roomscount; }
            set
            { roomscount = value; }
        }



        private string neaistschools;
        public string NeaistSchools
        {
            get
            { return neaistschools; }
            set
            { neaistschools = value; }
        }



        private string nearistjawamea;
        public string NearistJawamea
        {
            get
            { return nearistjawamea; }
            set
            { nearistjawamea = value; }
        }



        private string housetype;
        public string HouseType
        {
            get
            { return housetype; }
            set
            {
                housetype = value;
                this.ClearError("HouseType");
                this.NotifyPropertyChanged("HouseType");
            }
        }



        private string owner;
        public string Owner
        {
            get
            { return owner; }
            set
            { owner = value; }
        }



        private string phone;
        public string Phone
        {
            get
            { return phone; }
            set
            { phone = value; }
        }

        private int? familyid;
        public int? FamilyID
        {
            get
            { return familyid; }
            set
            { familyid = value; }
        }



        private int? orphanid;
        public int? OrphanID
        {
            get
            { return orphanid; }
            set
            { orphanid = value; }
        }



        private DateTime? date;
        public DateTime? Date
        {
            get
            { return date; }
            set
            {
                date = value;
                this.ClearError("Date");
                this.NotifyPropertyChanged("Date");
            }
        }



        private DateTime? enddate;
        public DateTime? EndDate
        {
            get
            { return enddate; }
            set
            { enddate = value; }
        }

        private string houseStatus;
        public string HouseStatus
        {
            get
            { return houseStatus; }
            set
            { houseStatus = value; }
        }

        private string digitalAddress;
        public string DigitalAddress
        {
            get
            { return digitalAddress; }
            set
            { digitalAddress = value; }
        }

        private string notes;
        public string Notes
        {
            get
            { return notes; }
            set
            { notes = value; }
        }

        public static House GetHouseByID(int? id)
        {
            House x = new House();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetHouseByID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@HouseID", id);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["HouseID"] is DBNull))
                        x.HouseID = System.Int32.Parse(rd["HouseID"].ToString());
                    x.Area = rd["Area"].ToString();
                    x.Value = rd["Value"].ToString();
                    if (!(rd["SectorPartNum"] is DBNull))
                        x.SectorPartNum = System.Int32.Parse(rd["SectorPartNum"].ToString());
                    x.OldAddress = rd["OldAddress"].ToString();
                    x.HouseSection = rd["HouseSection"].ToString();
                    x.HouseStreet = rd["HouseStreet"].ToString();
                    x.Address = rd["Address"].ToString();
                    x.HouseBuildingNumber = rd["HouseBuildingNumber"].ToString();
                    x.HouseFloor = rd["HouseFloor"].ToString();
                    x.Photo = rd["Photo"].ToString();
                    x.HouseSituation = rd["HouseSituation"].ToString();
                    x.FurnitureSituation = rd["FurnitureSituation"].ToString();
                    x.RoomsSize = rd["RoomsSize"].ToString();
                    x.Lights = rd["Lights"].ToString();
                    x.Airing = rd["Airing"].ToString();
                    if (!(rd["RoomsCount"] is DBNull))
                        x.RoomsCount = System.Int32.Parse(rd["RoomsCount"].ToString());
                    x.NeaistSchools = rd["NeaistSchools"].ToString();
                    x.NearistJawamea = rd["NearistJawamea"].ToString();
                    x.HouseType = rd["HouseType"].ToString();
                    x.Owner = rd["Owner"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.HouseStatus = rd["HouseStatus"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.DigitalAddress = rd["DigitalAddress"].ToString();
                    }
                rd.Close();
            }
            catch
            {
                x = new House();
            }
            finally
            {
                con.Close();
            }
            return x;
        }
        public static List<House> GetHouseAllByFamilyID(int? FamilyID)
        {
            List<House> hs = new List<House>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetHouseAllByFamilyID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@FamilyID", FamilyID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    House x = new House();
                    if (!(rd["HouseID"] is DBNull))
                        x.HouseID = System.Int32.Parse(rd["HouseID"].ToString());
                    x.Area = rd["Area"].ToString();
                    x.Value = rd["Value"].ToString();
                    if (!(rd["SectorPartNum"] is DBNull))
                        x.SectorPartNum = System.Int32.Parse(rd["SectorPartNum"].ToString());
                    x.OldAddress = rd["OldAddress"].ToString();
                    x.HouseSection = rd["HouseSection"].ToString();
                    x.HouseStreet = rd["HouseStreet"].ToString();
                    x.Address = rd["Address"].ToString();
                    x.HouseBuildingNumber = rd["HouseBuildingNumber"].ToString();
                    x.HouseFloor = rd["HouseFloor"].ToString();
                    x.Photo = rd["Photo"].ToString();
                    x.HouseSituation = rd["HouseSituation"].ToString();
                    x.FurnitureSituation = rd["FurnitureSituation"].ToString();
                    x.RoomsSize = rd["RoomsSize"].ToString();
                    x.Lights = rd["Lights"].ToString();
                    x.Airing = rd["Airing"].ToString();
                    if (!(rd["RoomsCount"] is DBNull))
                        x.RoomsCount = System.Int32.Parse(rd["RoomsCount"].ToString());
                    x.NeaistSchools = rd["NeaistSchools"].ToString();
                    x.NearistJawamea = rd["NearistJawamea"].ToString();
                    x.HouseType = rd["HouseType"].ToString();
                    x.Owner = rd["Owner"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.HouseStatus = rd["HouseStatus"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.DigitalAddress = rd["DigitalAddress"].ToString();

                    hs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                hs = new List<House>();
            }
            finally
            {
                con.Close();
            }
            return hs;
        }
        public static List<House> GetHouseAllByOrphanID(int? OrphanID)
        {
            List<House> hs = new List<House>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetHouseAllByOrphanID", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@OrphanID", OrphanID);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    House x = new House();
                    if (!(rd["HouseID"] is DBNull))
                        x.HouseID = System.Int32.Parse(rd["HouseID"].ToString());
                    x.Area = rd["Area"].ToString();
                    x.Value = rd["Value"].ToString();
                    if (!(rd["SectorPartNum"] is DBNull))
                        x.SectorPartNum = System.Int32.Parse(rd["SectorPartNum"].ToString());
                    x.OldAddress = rd["OldAddress"].ToString();
                    x.HouseSection = rd["HouseSection"].ToString();
                    x.HouseStreet = rd["HouseStreet"].ToString();
                    x.Address = rd["Address"].ToString();
                    x.HouseBuildingNumber = rd["HouseBuildingNumber"].ToString();
                    x.HouseFloor = rd["HouseFloor"].ToString();
                    x.Photo = rd["Photo"].ToString();
                    x.HouseSituation = rd["HouseSituation"].ToString();
                    x.FurnitureSituation = rd["FurnitureSituation"].ToString();
                    x.RoomsSize = rd["RoomsSize"].ToString();
                    x.Lights = rd["Lights"].ToString();
                    x.Airing = rd["Airing"].ToString();
                    if (!(rd["RoomsCount"] is DBNull))
                        x.RoomsCount = System.Int32.Parse(rd["RoomsCount"].ToString());
                    x.NeaistSchools = rd["NeaistSchools"].ToString();
                    x.NearistJawamea = rd["NearistJawamea"].ToString();
                    x.HouseType = rd["HouseType"].ToString();
                    x.Owner = rd["Owner"].ToString();
                    x.Phone = rd["Phone"].ToString();
                    x.Notes = rd["Notes"].ToString();
                    x.HouseStatus = rd["HouseStatus"].ToString();
                    if (!(rd["FamilyID"] is DBNull))
                        x.FamilyID = System.Int32.Parse(rd["FamilyID"].ToString());
                    if (!(rd["OrphanID"] is DBNull))
                        x.OrphanID = System.Int32.Parse(rd["OrphanID"].ToString());
                    if (!(rd["Date"] is DBNull))
                        x.Date = System.DateTime.Parse(rd["Date"].ToString());
                    if (!(rd["EndDate"] is DBNull))
                        x.EndDate = System.DateTime.Parse(rd["EndDate"].ToString());
                    x.DigitalAddress = rd["DigitalAddress"].ToString();

                    hs.Add(x);
                }
                rd.Close();
            }
            catch
            {
                hs = new List<House>();
            }
            finally
            {
                con.Close();
            }
            return hs;
        }


        public static List<string> GetHouseSections
        {
            get {
                return BaseDataBase.GetAllStrings("Select distinct HouseSection from House where HouseSection is not null order by HouseSection");
            }
        }
        public static List<string> GetHouseSectionsWithAll
        {
            get
            {
                return BaseDataBase.GetAllStringsWithAll("Select distinct HouseSection from House where HouseSection is not null order by HouseSection");
            }
        }
        public static List<string> GetHouseStreets
        {
            get
            {
                return BaseDataBase.GetAllStrings("Select distinct HouseStreet from House where HouseStreet is not null order by HouseStreet");
            }
        }
        public static List<string> GetHouseStreetsWithAll
        {
            get
            {
                return BaseDataBase.GetAllStringsWithAll("Select distinct HouseStreet from House where HouseStreet is not null order by HouseStreet");
            }
        }
        public static List<string> GetOldAddresses
        {
            get
            {
                return BaseDataBase.GetAllStrings("Select distinct OldAddress from House where OldAddress is not null order by OldAddress");
            }
        }
        public static List<string> GetOldAddressesWithAll
        {
            get
            {
                return BaseDataBase.GetAllStringsWithAll("Select distinct OldAddress from House where OldAddress is not null order by OldAddress");
            }
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (string.IsNullOrEmpty(HouseSection))
            {
                isValid = false;
                this.SetError("HouseSection", "يجب إدخال منطقة السكن");
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
