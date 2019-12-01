using eLearning.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            DataBase.CreateDb();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void formRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registraition registraition = new Registraition();
            registraition.Show();
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = DataBase.data;
            string getUsersProcedure = "GET_USERS";     // название хранимой процедуры

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();



                    if (txbLogin.Text != string.Empty)
                    {
                        // В параметре название процедуры и экземпляр коннекшна
                        SqlCommand sqlCommand = new SqlCommand(getUsersProcedure, sqlConnection);

                        // Тип выполняемой команды
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        Classes.User tempUser = new Classes.User();

                        if (reader.HasRows)
                        {
                            // Наличие юзеров
                            bool flagPerson = false;

                            while (reader.Read())
                            {
                                if (txbLogin.Text == (string)reader.GetValue(1) && User.getHash(txbPassword.Password) == (string)reader.GetValue(2))
                                {
                                    flagPerson = true;
                                    
                                    tempUser.idUser = reader.GetValue(0);
                                    tempUser.idAdmin = reader.GetValue(3);
                                    tempUser.login = reader.GetValue(1);
                                    tempUser.password = reader.GetValue(2);
                                    break;
                                }
                            }
                            reader.Close();
                           
                            if (flagPerson)
                            {
                                if (tempUser.idAdmin.ToString() != "" && tempUser.idAdmin != null)
                                {
                                    DataBase.ApplyAdminPrivileges();
                                    // Создаем экземпляр админа
                                    Admin admin = Admin.getInstance();
                                    admin.Id = (int)tempUser.idAdmin;
                                    admin.UserId = (int)tempUser.idUser;
                                    admin.Login = tempUser.login.ToString();
                                    MainWindow mainWindow = new MainWindow(admin);
                                    mainWindow.Show();
                                    Close();
                                }
                                else
                                {
                                    DataBase.ApplyUserPrivileges();
                                    // Передать tempUser
                                    MainWindow mainWindow = new MainWindow(tempUser);
                                    mainWindow.Show();
                                    Close();
                                }
                            }
                            
                            else
                            {
                                MessageBox.Show("Такого пользователя нет!");
                                txbLogin.Text = "";
                                txbPassword.Password = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("В базе еще нет пользователей");
                            txbLogin.Text = "";
                            txbPassword.Password = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите данные");
                        txbLogin.Text = "";
                        txbPassword.Password = "";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        


    }

}
