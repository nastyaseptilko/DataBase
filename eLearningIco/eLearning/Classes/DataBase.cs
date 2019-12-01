using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace eLearning.Classes
{
    public static class DataBase
    {   
        public static string UserConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=eLEARNING; Integrated Security=False;User ID=Default_User;Password=password";
        public static string AdminConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=eLEARNING; Integrated Security=False;User ID=Admin_User;Password=password";
        public static string DefaultConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=eLEARNING; Integrated Security=True";

        //создать строку коннекшн админ и коннекшен юзер
        public static string data = DefaultConnectionString;


        public static void ApplyAdminPrivileges()
        {
            data = AdminConnectionString;
        }

        public static void ApplyUserPrivileges()
        {
            data = UserConnectionString;
        }


        private const string ConnectionStringWithoutDb = @"Data Source=.\SQLEXPRESS; Integrated Security=True";
        private const string CREATE_DB_QUERY = "Use master; CREATE DATABASE SAA_DB;";
        private static readonly string SQL_SCRIPT_FILE_PATH = Directory.GetCurrentDirectory() + @"\CreatingDbScript.sql";

        //созданы ли таблицы? проверка
        private static bool IsDbCreated = false;

        public static void CreateDb()
        {
            if (IsDbCreated)
                return;

            IsDbCreated = true;

            using (SqlConnection connection = new SqlConnection(ConnectionStringWithoutDb))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(CREATE_DB_QUERY, connection);
                    command.ExecuteNonQuery();

                    CreateTables(connection);
                }
                catch (Exception e) { }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void CreateTables(SqlConnection connection)
        {
            string script = File.ReadAllText(SQL_SCRIPT_FILE_PATH, Encoding.Default);
            IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (string commandString in commandStrings)
            {
                if (commandString.Trim() != string.Empty)
                {
                    SqlCommand command = new SqlCommand(commandString, connection);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
