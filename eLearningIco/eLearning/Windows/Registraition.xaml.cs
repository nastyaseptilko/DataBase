using eLearning.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace eLearning.Windows
{
    /// <summary>
    /// Логика взаимодействия для Registraition.xaml
    /// </summary>
    public partial class Registraition : Window
    {
        
        public Registraition()
        {
            InitializeComponent();
        }

        private void formLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = DataBase.data;
           
          
            string addUsers = "ADD_USERS";
            string getUsersProcedure = "GET_USERS";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (txbLogin.Text.ToString() != string.Empty)
                    {
                        if (txbPassword1.Password.Length < 6)
                        {
                            MessageBox.Show("Пароль слишком мал!");
                            return;
                        }

                        if (txbPassword1.Password != txbPassword2.Password)
                        {
                            MessageBox.Show("Пароли не совпадают!");
                            return;
                        }

                        
                        SqlCommand command2 = new SqlCommand(getUsersProcedure, connection);
                        command2.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataReader reader = command2.ExecuteReader();

                        bool flagPerson = false;
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (txbLogin.Text == (string)reader.GetValue(1))
                                {
                                    flagPerson = true;
                                    break;
                                }
                            }
                        }
                        reader.Close();                     
                       

                        if (!flagPerson)
                        {
                            SqlCommand registerUserCommand = new SqlCommand(addUsers, connection);

                            // Задаем тип команды
                            registerUserCommand.CommandType = System.Data.CommandType.StoredProcedure;

                            // Передаем параметры и значения
                            SqlParameter loginParameter = new SqlParameter
                            {
                                ParameterName = "@login",
                                Value =txbLogin.Text
                            };

                            SqlParameter passwordParameter = new SqlParameter
                            {
                                ParameterName = "@password",
                                Value = User.getHash(txbPassword1.Password)
                            };

                            // Добавляем парраметры
                            registerUserCommand.Parameters.Add(loginParameter);
                            registerUserCommand.Parameters.Add(passwordParameter);

                            registerUserCommand.ExecuteNonQuery();
                            MessageBox.Show("Регистрация прошла успешно!");

                            Login login = new Login();
                            login.Show();
                            Close();

                            txbLogin.Text = "";
                            txbPassword1.Password = "";
                            txbPassword2.Password = "";
                        }
                        else
                        {
                            MessageBox.Show("Такой пользователь уже существует!");

                            txbLogin.Text = "";
                            txbPassword1.Password = "";
                            txbPassword2.Password = "";
                        }


                    }
                    else
                    {
                        MessageBox.Show("Введите данные!");
                        txbLogin.Text = "";
                        txbPassword1.Password = "";
                        txbPassword2.Password = "";
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
