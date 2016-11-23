using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class User : ModelViewContext
    {
        private int? id;
        public int? ID
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        private string password;
        public string Password
        {
            get
            { return password; }
            set
            { password = value; }
        }

        private bool isadmin;
        public bool IsAdmin
        {
            get
            { return isadmin; }
            set
            { isadmin = value; }
        }

        private bool canadd;
        public bool CanAdd
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canadd;
            }
            set
            { canadd = value; }
        }

        private bool canupdate;
        public bool CanUpdate
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canupdate;
            }
            set
            { canupdate = value; }
        }

        private bool candelete;
        public bool CanDelete
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return candelete;
            }
            set
            { candelete = value; }
        }

        private bool canpresent;
        public bool CanPresent
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canpresent;
            }
            set
            { canpresent = value; }
        }

        private bool canentertempfamily;
        public bool CanEnterTempFamily
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canentertempfamily;
            }
            set
            { canentertempfamily = value; }
        }

        private bool canenterfamily;
        public bool CanEnterFamily
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canenterfamily;
            }
            set
            { canenterfamily = value; }
        }

        private bool canupdatefamily;
        public bool CanUpdateFamily
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canupdatefamily;
            }
            set
            { canupdatefamily = value; }
        }

        private bool canenterfamilyneed;
        public bool CanEnterFamilyNeed
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canenterfamilyneed;
            }
            set
            { canenterfamilyneed = value; }
        }

        private bool canenterlistergroup;
        public bool CanEnterListerGroup
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canenterlistergroup;
            }
            set
            { canenterlistergroup = value; }
        }

        private bool canmakestatistics;
        public bool CanMakeStatistics
        {
            get
            {
                if (IsAdmin)
                    return true;
                return canmakestatistics;
            }
            set
            { canmakestatistics = value; }
        }

        private bool cancanceltempfamily;
        public bool CanCancelTempFamily
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return cancanceltempfamily;
            }
            set
            { cancanceltempfamily = value; }
        }

        private bool candeletetempfamily;
        public bool CanDeleteTempFamily
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return candeletetempfamily;
            }
            set
            { candeletetempfamily = value; }
        }

        private bool cancancelfamily;
        public bool CanCancelFamily
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return cancancelfamily;
            }
            set
            { cancancelfamily = value; }
        }

        private bool canexport;
        public bool CanExport
        {
            get
            {
                if (IsAdmin)
                    return true;
                return canexport;
            }
            set
            { canexport = value; }
        }

        private bool canprintreports;
        public bool CanPrintReports
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canprintreports;
            }
            set
            { canprintreports = value; }
        }

        private bool canupdatesupport;
        public bool CanUpdateSupport
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return canupdatesupport;
            }
            set
            { canupdatesupport = value; }
        }
        private bool candeletesupport;
        public bool CanDeleteSupport
        {
            get
            {
                if (IsAdmin || PointAdmin)
                    return true;
                return candeletesupport;
            }
            set
            { candeletesupport = value; }
        }

        private bool it;
        public bool IT
        {
            get
            {
                if (IsAdmin)
                    return true;
                return it;
            }
            set
            { it = value; }
        }

        private bool pointAdmin;
        public bool PointAdmin
        {
            get
            {
                if (IsAdmin)
                    return true;
                return pointAdmin;
            }
            set
            { pointAdmin = value; }
        }

        private bool reportCreator;
        public bool ReportCreator
        {
            get
            {
                if (IsAdmin)
                    return true;
                return reportCreator;
            }
            set
            { reportCreator = value; }
        }
        public static bool InsertData(User x)
        {
            x.ID = BaseDataBase._StoredProcedureReturnable("sp_Add2Users"
           , new SqlParameter("@ID", System.Data.SqlDbType.Int)
           , new SqlParameter("@Name", x.Name)
           , new SqlParameter("@Password", User.ComputeHash(x.Password, "SHA512", null))
           , new SqlParameter("@IsAdmin", x.IsAdmin)
           , new SqlParameter("@CanAdd", x.CanAdd)
           , new SqlParameter("@CanUpdate", x.CanUpdate)
           , new SqlParameter("@CanDelete", x.CanDelete)
           , new SqlParameter("@CanPresent", x.CanPresent)
           , new SqlParameter("@CanEnterTempFamily", x.CanEnterTempFamily)
           , new SqlParameter("@CanEnterFamily", x.CanEnterFamily)
           , new SqlParameter("@CanUpdateFamily", x.CanUpdateFamily)
           , new SqlParameter("@CanEnterFamilyNeed", x.CanEnterFamilyNeed)
           , new SqlParameter("@CanEnterListerGroup", x.CanEnterListerGroup)
           , new SqlParameter("@CanMakeStatistics", x.CanMakeStatistics)
           , new SqlParameter("@CanCancelTempFamily", x.CanCancelTempFamily)
           , new SqlParameter("@CanDeleteTempFamily", x.CanDeleteTempFamily)
           , new SqlParameter("@CanCancelFamily", x.CanCancelFamily)
           , new SqlParameter("@CanExport", x.CanExport)
           , new SqlParameter("@CanPrintReports", x.CanPrintReports)
           , new SqlParameter("@CanUpdateSupport", x.CanUpdateSupport)
           , new SqlParameter("@CanDeleteSupport", x.CanDeleteSupport)
           , new SqlParameter("@IT", x.IT)
           , new SqlParameter("@PointAdmin", x.PointAdmin)
           , new SqlParameter("@ReportCreator", x.ReportCreator)
           );

            return x.ID.HasValue;
        }
        public static bool UpdateData(User x)
        {
            x.Password = User.ComputeHash(x.Password, "SHA512", null);
            return BaseDataBase._StoredProcedure("sp_UpdateUsers"
           , new SqlParameter("@ID", x.ID)
           , new SqlParameter("@Name", x.Name)
           , new SqlParameter("@Password", x.Password)
           , new SqlParameter("@IsAdmin", x.IsAdmin)
           , new SqlParameter("@CanAdd", x.CanAdd)
           , new SqlParameter("@CanUpdate", x.CanUpdate)
           , new SqlParameter("@CanDelete", x.CanDelete)
           , new SqlParameter("@CanPresent", x.CanPresent)
           , new SqlParameter("@CanEnterTempFamily", x.CanEnterTempFamily)
           , new SqlParameter("@CanEnterFamily", x.CanEnterFamily)
           , new SqlParameter("@CanUpdateFamily", x.CanUpdateFamily)
           , new SqlParameter("@CanEnterFamilyNeed", x.CanEnterFamilyNeed)
           , new SqlParameter("@CanEnterListerGroup", x.CanEnterListerGroup)
           , new SqlParameter("@CanMakeStatistics", x.CanMakeStatistics)
           , new SqlParameter("@CanCancelTempFamily", x.CanCancelTempFamily)
           , new SqlParameter("@CanDeleteTempFamily", x.CanDeleteTempFamily)
           , new SqlParameter("@CanCancelFamily", x.CanCancelFamily)
           , new SqlParameter("@CanExport", x.CanExport)
           , new SqlParameter("@CanPrintReports", x.CanPrintReports)
           , new SqlParameter("@CanUpdateSupport", x.CanUpdateSupport)
           , new SqlParameter("@CanDeleteSupport", x.CanDeleteSupport)
           , new SqlParameter("@IT", x.IT)
           , new SqlParameter("@PointAdmin", x.PointAdmin)
           , new SqlParameter("@ReportCreator", x.ReportCreator));
        }
        public static bool DeleteData(User x)
        {
            return BaseDataBase._StoredProcedure("sp_DeleteFromUsers"
           , new SqlParameter("@ID", x.ID));
        }

        public static User GetUser(string UserName, string Password)
        {
            User x = new User();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetUserByName", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter pr = new SqlParameter("@Name", UserName);
            com.Parameters.Add(pr);
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.Password = rd["Password"].ToString();
                    if (!(rd["IsAdmin"] is DBNull))
                        x.IsAdmin = System.Boolean.Parse(rd["IsAdmin"].ToString());
                    if (!(rd["CanAdd"] is DBNull))
                        x.CanAdd = System.Boolean.Parse(rd["CanAdd"].ToString());
                    if (!(rd["CanUpdate"] is DBNull))
                        x.CanUpdate = System.Boolean.Parse(rd["CanUpdate"].ToString());
                    if (!(rd["CanDelete"] is DBNull))
                        x.CanDelete = System.Boolean.Parse(rd["CanDelete"].ToString());
                    if (!(rd["CanPresent"] is DBNull))
                        x.CanPresent = System.Boolean.Parse(rd["CanPresent"].ToString());
                    if (!(rd["CanEnterTempFamily"] is DBNull))
                        x.CanEnterTempFamily = System.Boolean.Parse(rd["CanEnterTempFamily"].ToString());
                    if (!(rd["CanEnterFamily"] is DBNull))
                        x.CanEnterFamily = System.Boolean.Parse(rd["CanEnterFamily"].ToString());
                    if (!(rd["CanUpdateFamily"] is DBNull))
                        x.CanUpdateFamily = System.Boolean.Parse(rd["CanUpdateFamily"].ToString());
                    if (!(rd["CanEnterFamilyNeed"] is DBNull))
                        x.CanEnterFamilyNeed = System.Boolean.Parse(rd["CanEnterFamilyNeed"].ToString());
                    if (!(rd["CanEnterListerGroup"] is DBNull))
                        x.CanEnterListerGroup = System.Boolean.Parse(rd["CanEnterListerGroup"].ToString());
                    if (!(rd["CanMakeStatistics"] is DBNull))
                        x.CanMakeStatistics = System.Boolean.Parse(rd["CanMakeStatistics"].ToString());
                    if (!(rd["CanCancelTempFamily"] is DBNull))
                        x.CanCancelTempFamily = System.Boolean.Parse(rd["CanCancelTempFamily"].ToString());
                    if (!(rd["CanDeleteTempFamily"] is DBNull))
                        x.CanDeleteTempFamily = System.Boolean.Parse(rd["CanDeleteTempFamily"].ToString());
                    if (!(rd["CanCancelFamily"] is DBNull))
                        x.CanCancelFamily = System.Boolean.Parse(rd["CanCancelFamily"].ToString());
                    if (!(rd["CanExport"] is DBNull))
                        x.CanExport = System.Boolean.Parse(rd["CanExport"].ToString());
                    if (!(rd["CanPrintReports"] is DBNull))
                        x.CanPrintReports = System.Boolean.Parse(rd["CanPrintReports"].ToString());
                    if (!(rd["CanUpdateSupport"] is DBNull))
                        x.CanUpdateSupport = System.Boolean.Parse(rd["CanUpdateSupport"].ToString());
                    if (!(rd["CanDeleteSupport"] is DBNull))
                        x.CanDeleteSupport = System.Boolean.Parse(rd["CanDeleteSupport"].ToString());
                    if (!(rd["IT"] is DBNull))
                        x.IT = System.Boolean.Parse(rd["IT"].ToString());
                    if (!(rd["PointAdmin"] is DBNull))
                        x.PointAdmin = System.Boolean.Parse(rd["PointAdmin"].ToString());
                    if (!(rd["ReportCreator"] is DBNull))
                        x.ReportCreator = System.Boolean.Parse(rd["ReportCreator"].ToString());
                }
                rd.Close();
            }
            catch
            {
                x = null;
                MyMessageBox.Show("خطأفي الاتصال بقاعدة البيانات");
            }
            finally
            {
                con.Close();
            }
            if (x != null && !User.VerifyHash(Password, "SHA512", x.password))
            {
                MyMessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة");
                x = null;
            }
            return x;
        }

        static List<User> allUsers;
        public static List<User> AllUsers
        {
            get { return allUsers; }
        }
        public static List<User> GetUserAll()
        {
            if (allUsers != null)
                return allUsers;
            List<User> us = new List<User>();
            SqlConnection con = new SqlConnection(BaseDataBase.ConnectionString);
            SqlCommand com = new SqlCommand("sp_GetUserAll", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    User x = new User();
                    if (!(rd["ID"] is DBNull))
                        x.ID = System.Int32.Parse(rd["ID"].ToString());
                    x.Name = rd["Name"].ToString();
                    x.Password = rd["Password"].ToString();
                    if (!(rd["IsAdmin"] is DBNull))
                        x.IsAdmin = System.Boolean.Parse(rd["IsAdmin"].ToString());
                    if (!(rd["CanAdd"] is DBNull))
                        x.CanAdd = System.Boolean.Parse(rd["CanAdd"].ToString());
                    if (!(rd["CanUpdate"] is DBNull))
                        x.CanUpdate = System.Boolean.Parse(rd["CanUpdate"].ToString());
                    if (!(rd["CanDelete"] is DBNull))
                        x.CanDelete = System.Boolean.Parse(rd["CanDelete"].ToString());
                    if (!(rd["CanPresent"] is DBNull))
                        x.CanPresent = System.Boolean.Parse(rd["CanPresent"].ToString());
                    if (!(rd["CanEnterTempFamily"] is DBNull))
                        x.CanEnterTempFamily = System.Boolean.Parse(rd["CanEnterTempFamily"].ToString());
                    if (!(rd["CanEnterFamily"] is DBNull))
                        x.CanEnterFamily = System.Boolean.Parse(rd["CanEnterFamily"].ToString());
                    if (!(rd["CanUpdateFamily"] is DBNull))
                        x.CanUpdateFamily = System.Boolean.Parse(rd["CanUpdateFamily"].ToString());
                    if (!(rd["CanEnterFamilyNeed"] is DBNull))
                        x.CanEnterFamilyNeed = System.Boolean.Parse(rd["CanEnterFamilyNeed"].ToString());
                    if (!(rd["CanEnterListerGroup"] is DBNull))
                        x.CanEnterListerGroup = System.Boolean.Parse(rd["CanEnterListerGroup"].ToString());
                    if (!(rd["CanMakeStatistics"] is DBNull))
                        x.CanMakeStatistics = System.Boolean.Parse(rd["CanMakeStatistics"].ToString());
                    if (!(rd["CanCancelTempFamily"] is DBNull))
                        x.CanCancelTempFamily = System.Boolean.Parse(rd["CanCancelTempFamily"].ToString());
                    if (!(rd["CanDeleteTempFamily"] is DBNull))
                        x.CanDeleteTempFamily = System.Boolean.Parse(rd["CanDeleteTempFamily"].ToString());
                    if (!(rd["CanCancelFamily"] is DBNull))
                        x.CanCancelFamily = System.Boolean.Parse(rd["CanCancelFamily"].ToString());
                    if (!(rd["CanExport"] is DBNull))
                        x.CanExport = System.Boolean.Parse(rd["CanExport"].ToString());
                    if (!(rd["CanPrintReports"] is DBNull))
                        x.CanPrintReports = System.Boolean.Parse(rd["CanPrintReports"].ToString());
                    if (!(rd["CanUpdateSupport"] is DBNull))
                        x.CanUpdateSupport = System.Boolean.Parse(rd["CanUpdateSupport"].ToString());
                    if (!(rd["CanDeleteSupport"] is DBNull))
                        x.CanDeleteSupport = System.Boolean.Parse(rd["CanDeleteSupport"].ToString());
                    if (!(rd["IT"] is DBNull))
                        x.IT = System.Boolean.Parse(rd["IT"].ToString());
                    if (!(rd["PointAdmin"] is DBNull))
                        x.PointAdmin = System.Boolean.Parse(rd["PointAdmin"].ToString());
                    if (!(rd["ReportCreator"] is DBNull))
                        x.ReportCreator = System.Boolean.Parse(rd["ReportCreator"].ToString());

                    us.Add(x);
                }
                rd.Close();
            }
            catch
            {
                us = new List<User>();
            }
            finally
            {
                con.Close();
            }
            allUsers = us;
            return us;
        }



        public static string ComputeHash(string plainText,
                                     string hashAlgorithm,
                                     byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="plainText">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="hashValue">
        /// Base64-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash mathes the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string plainText,
                                      string hashAlgorithm,
                                      string hashValue)
        {
            try
            {
                // Convert base64-encoded hash value into a byte array.
                byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

                // We must know size of hash (without salt).
                int hashSizeInBits, hashSizeInBytes;

                // Make sure that hashing algorithm name is specified.
                if (hashAlgorithm == null)
                    hashAlgorithm = "";

                // Size of hash is based on the specified algorithm.
                switch (hashAlgorithm.ToUpper())
                {
                    case "SHA1":
                        hashSizeInBits = 160;
                        break;

                    case "SHA256":
                        hashSizeInBits = 256;
                        break;

                    case "SHA384":
                        hashSizeInBits = 384;
                        break;

                    case "SHA512":
                        hashSizeInBits = 512;
                        break;

                    default: // Must be MD5
                        hashSizeInBits = 128;
                        break;
                }

                // Convert size of hash from bits to bytes.
                hashSizeInBytes = hashSizeInBits / 8;

                // Make sure that the specified hash value is long enough.
                if (hashWithSaltBytes.Length < hashSizeInBytes)
                    return false;

                // Allocate array to hold original salt bytes retrieved from hash.
                byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                            hashSizeInBytes];

                // Copy salt from the end of the hash to the new array.
                for (int i = 0; i < saltBytes.Length; i++)
                    saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

                // Compute a new hash string.
                string expectedHashString =
                            ComputeHash(plainText, hashAlgorithm, saltBytes);

                // If the computed hash matches the specified hash,
                // the plain text value must be correct.
                return (hashValue == expectedHashString);
            }
            catch { return false; }
        }

        public static string GetUserNameByID(int id)
        {
            return BaseDataBase._Scalar("select Name from Users where id =" + id);
        }
    }
}