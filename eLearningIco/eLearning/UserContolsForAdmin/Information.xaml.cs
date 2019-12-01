using eLearning.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eLearning.UserContolsForAdmin
{
    /// <summary>
    /// Логика взаимодействия для Information.xaml
    /// </summary>
    public partial class Information : UserControl
    {

        public Information(Classes.Admin admin)
        {
            InitializeComponent();

            txbUser.Text = admin.Login;

            string connectionString = DataBase.data;
            string getUsers = "GET_USERS";
            string getInformation = "GET_INFORMATION";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand getUsersCommand = new SqlCommand(getUsers, sqlConnection);
                    getUsersCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader readerUser = getUsersCommand.ExecuteReader();


                    List<Classes.User> users = new List<Classes.User>();
                    if (readerUser.HasRows)
                    {
                        while (readerUser.Read())
                        {
                            Classes.User user = new Classes.User();
                            user.idUser = readerUser.GetValue(0);
                            user.login = readerUser.GetValue(1);
                            user.password = readerUser.GetValue(2);
                            user.idAdmin = readerUser.GetValue(3);
                            users.Add(user);
                        }
                    }
                    readerUser.Close();

                    usersGrid.ItemsSource = users;

                    SqlCommand getInformationCommand = new SqlCommand(getInformation, sqlConnection);
                    getInformationCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader readerTheme = getInformationCommand.ExecuteReader();

                    List<Theme> themes = new List<Theme>();
                    if (readerTheme.HasRows)
                    {
                        while (readerTheme.Read())
                        {
                            Theme theme = new Theme();
                            theme.NameTheme = readerTheme.GetValue(0);
                            theme.NameTest = readerTheme.GetValue(1);
                            theme.NameQuestion = readerTheme.GetValue(2);
                            themes.Add(theme);
                        }
                    }
                    readerTheme.Close();
                    themsGrid.ItemsSource = themes;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        new private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                string deleteAnswer = "DELETE_ANSWER ";
                string deleteQuestion = "DELETE_QUESTIONS ";
                var data_grid = (DataGrid)sender;
                if (Key.Delete == e.Key)
                    foreach (Theme theme in data_grid.SelectedItems)
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(DataBase.data))
                        {
                            sqlConnection.Open();
                            string questionName = theme.NameQuestion.ToString();

                            SqlCommand deleteAnswerCommand = new SqlCommand(deleteAnswer, sqlConnection);
                            deleteAnswerCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter questionParameter = new SqlParameter
                            {
                                ParameterName = "@question",
                                Value= questionName
                            };
                            deleteAnswerCommand.Parameters.Add(questionParameter);
                            deleteAnswerCommand.ExecuteNonQuery();
                           



                            SqlCommand deleteQuestionCommand = new SqlCommand(deleteQuestion, sqlConnection);
                            deleteQuestionCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter questionsParameter = new SqlParameter
                            {
                                ParameterName = "@question",
                                Value = questionName
                            };
                            deleteQuestionCommand.Parameters.Add(questionsParameter);
                            deleteQuestionCommand.ExecuteNonQuery();
                            //MessageBox.Show("lala");
                        }
                    }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
