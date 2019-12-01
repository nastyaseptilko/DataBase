using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eLearning.Classes
{
    public class User
    {
        public object idUser { get; set; }
        public object idAdmin { get; set; }
        public object login { get; set; }
        public object password { get; set; }
        //шифрование
        public static string getHash(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return "-1";
            }
            else
            {
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash).Substring(0, 15);
            }
        }
    }
}
