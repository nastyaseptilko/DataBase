using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearning.Classes
{
    public class Admin
    {
        public string login = "admin";
        public string password = "admin";


        private Admin() { }

        // Реализация паттерна Singleton
        public static Admin admin;

        public static Admin getInstance()
        {
            if (admin == null)
                admin = new Admin();
            return admin;
        }
    }
}
