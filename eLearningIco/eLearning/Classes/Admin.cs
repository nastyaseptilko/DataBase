using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearning.Classes
{
    public class Admin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }

        private Admin() { }

        // Реализация паттерна Singleton
        private static Admin admin;

        public static Admin getInstance()
        {
            if (admin == null)
                admin = new Admin();
            return admin;
        }
    }
}
