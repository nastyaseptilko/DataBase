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
        public static string data = @"Data Source=.\SQLEXPRESS;Initial Catalog=eLEARNING; Integrated Security=True";

        private const string ConnectionStringWithoutDb = @"Data Source=.\SQLEXPRESS; Integrated Security=True";
        private const string CREATE_DB_QUERY = "Use master; CREATE DATABASE SAA_MyBase;";
        private static readonly string SQL_SCRIPT_FILE_PATH = Directory.GetCurrentDirectory() + @"\CreatingDbScript.sql";

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
