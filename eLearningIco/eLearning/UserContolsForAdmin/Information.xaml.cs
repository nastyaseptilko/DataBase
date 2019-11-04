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

            txbUser.Text = admin.login;

            string connectionString = DataBase.data;
            string sqlExpressionUsers = $@"SELECT * FROM USERS";

            string sqlExpressionThemes = $@"
SELECT Themes.Name, Tests.Name, Questions.Question FROM Themes
JOIN Tests ON Themes.IdTheme = Tests.IdTheme
JOIN Questions ON Tests.IdTest = Questions.IdTest";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommandUser = new SqlCommand(sqlExpressionUsers, sqlConnection);
                    SqlDataReader readerUser = sqlCommandUser.ExecuteReader();


                    List<Classes.User> users = new List<Classes.User>();
                    if (readerUser.HasRows)
                    {
                        while (readerUser.Read())
                        {
                            Classes.User user = new Classes.User();
                            user.idUser = readerUser.GetValue(0);
                            user.login = readerUser.GetValue(1);
                            user.password = readerUser.GetValue(2);
                            users.Add(user);
                        }
                    }
                    readerUser.Close();

                    usersGrid.ItemsSource = users;

                    SqlCommand sqlCommandTheme = new SqlCommand(sqlExpressionThemes, sqlConnection);
                    SqlDataReader readerTheme = sqlCommandTheme.ExecuteReader();

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


                var data_grid = (DataGrid)sender;
                if (Key.Delete == e.Key)
                    foreach (Theme theme in data_grid.SelectedItems)
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(DataBase.data))
                        {
                            sqlConnection.Open();
                            string questionName = theme.NameQuestion.ToString();
                            SqlCommand command = new SqlCommand(
                                $"DELETE a FROM Answer a INNER JOIN " +
                                $"Questions q ON a.IdQuestion = q.IdQuestion " +
                                $"WHERE q.Question = '{questionName}'", sqlConnection);
                            command.ExecuteNonQuery();

                            command.CommandText = $"DELETE FROM Questions WHERE Question = '{questionName}'";
                            command.ExecuteNonQuery();
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
