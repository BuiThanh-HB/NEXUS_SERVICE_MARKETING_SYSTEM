using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Utils
{
    public class Util
    {
        public static string CreateMD5(string input)
        {
            //bam du lieu
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private static readonly string[] VietNamChar = new string[]
{
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
};
        public static string Converts(string str)
        {
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return str;
        }

        public static string ConvertPhone(string phonenumber)
        {
            if (phonenumber.Contains("+84"))
            {
                int length = phonenumber.Length - 3;
                phonenumber = "0" + phonenumber.Substring(3, length);
            }
            return phonenumber;
        }

        public static Nullable<DateTime> ConvertDate(string date)
        {
            if (!String.IsNullOrEmpty(date))
            {
                try
                {
                    string datetime = date.Replace("-", "/");
                    string[] arr = datetime.Split('/');
                    DateTime dateTime = new DateTime(int.Parse(arr[2]), int.Parse(arr[1]), int.Parse(arr[0]));
                    return dateTime;
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static string GenPass(string pass)
        {
            return BCrypt.Net.BCrypt.HashPassword(pass, 10);
        }

        public static bool CheckPass(string pass, string userPass)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(pass, userPass);
            }
            catch
            {
                return false;
            }
        }
    }
}