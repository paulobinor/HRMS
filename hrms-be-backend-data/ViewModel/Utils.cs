using System.Data;
using System.Reflection;
using System.Text;

namespace hrms_be_backend_data.ViewModel
{
    public static class Utils
    {
        private static readonly Random _random = new Random();
        public static bool DoesPasswordMatch(string hashedPwdFromDatabase, string userEnteredPassword)
        {
            return BCrypt.Net.BCrypt.Verify(userEnteredPassword, hashedPwdFromDatabase);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }
        
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public static string UniqueRef(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(5, false));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(6, false));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(5, false));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(6, false));
            builder.Append(RandomNumber(1000, 9999));
            return builder.ToString();
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string GenerateDefaultPassword(int length = 15)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public static string GenerateReferralCode(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);           
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; 

            for (var i = 0; i < size; i++)
            {
                var code = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(code);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = string.Empty;
            string sTempChars = string.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }
       
        public static (bool IsValid, DateTime ValidDate) ValidateDate(string date)
        {
            try
            {
                // for US, alter to suit if splitting on hyphen, comma, etc.
                string[] dateParts = date.Split('/');

                // create new date from the parts; if this does not fail
                // the method will return true and the date is valid 
                DateTime resp = new DateTime(Convert.ToInt32(dateParts[0]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]));

                //DateTime resp = DateTime.Parse($"{Convert.ToInt32(dateParts[2])}/{Convert.ToInt32(dateParts[1])}/{Convert.ToInt32(dateParts[0])}");

                return (true, resp);
            }
            catch(Exception ex)
            {
                var eerr = ex.Message;
                // if a test date cannot be created, the
                // method will return false
                DateTime temp = new DateTime(); //value = {01/01/0001 00:00:00}
                return (false, temp);
            }
        }
    }

   
}
