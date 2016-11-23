using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class SystemData
    {
        public static List<string> GetFamilyTypes
        {
            get
            {
                return BaseDataBase.GetAllStrings("Select Name from SystemData where Type like 'FamilyType'");
            }
        }
        public static List<string> GetHouseTypes
        {
            get
            {
                return BaseDataBase.GetAllStrings("Select Name from SystemData where Type like 'HouseType'");
            }
        }
        public static List<string> GetHouseStatuses
        {
            get
            {
                return BaseDataBase.GetAllStrings("Select Name from SystemData where Type like 'HouseStatus'");
            }
        }
    }
}
