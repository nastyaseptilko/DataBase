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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eLearning.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Topics.xaml
    /// </summary>
    public partial class Topics : UserControl
    {
        string connectionString = DataBase.data;

        MainWindow mainWindow;
        Classes.User user;


        List<Classes.Theme> themes = new List<Classes.Theme>();
        List<Classes.Test> tests = new List<Classes.Test>();

        public Topics(MainWindow mainWindow, Classes.User user)
        {
            this.user = user;
            this.mainWindow = mainWindow;
            InitializeComponent();

            string sqlExpressionTheme = "GET_THEME_FOR_TEST";
            string sqlExpressionTest = "GET_TESTS_TEST ";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommandTheme = new SqlCommand(sqlExpressionTheme, sqlConnection);
                sqlCommandTheme.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader readerTheme = sqlCommandTheme.ExecuteReader();

                if (readerTheme.HasRows)
                {
                    while (readerTheme.Read())
                    {
                        Classes.Theme theme = new Classes.Theme();
                        theme.IdTheme = readerTheme.GetValue(0);
                        theme.NameTheme = readerTheme.GetValue(1);
                        themes.Add(theme);
                    }
                }
                readerTheme.Close();

                SqlCommand sqlCommand = new SqlCommand(sqlExpressionTest, sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader readerTest = sqlCommand.ExecuteReader();


                if (readerTest.HasRows)
                {
                    while (readerTest.Read())
                    {
                        Classes.Test test = new Classes.Test();
                        test.idTest = readerTest.GetValue(0);
                        test.Name = readerTest.GetValue(2).ToString();
                        test.IdTheme = readerTest.GetValue(3);
                        tests.Add(test);
                    }
                }
                readerTest.Close();
                for (int i = 0; i < themes.Count; i++)
                {
                    Expander expander = new Expander();
                    expander.Header = themes[i].NameTheme;
                    expander.Background = new SolidColorBrush(Colors.White);
                    expander.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    
                  

                    ListView listView = new ListView();
                    listView.SelectionChanged += Test_SelectionChanged;
                    for (int j = 0; j < tests.Count; j++)
                    {
                        if (themes[i].IdTheme.ToString() == tests[j].IdTheme.ToString())
                        {
                            listView.FontSize = 15;
                            listView.Margin = new Thickness(10);
                           
                            listView.Background = new SolidColorBrush(Colors.White);
                            listView.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                            listView.Items.Add(tests[j].Name.ToString());
                        }
                    }
                    expander.Content = listView;
                    stkP.Children.Add(expander);
                }
            }
        }

        private void Test_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender as ListView;
            for(int i = 0; i < tests.Count; i++)
            {
                if (item.SelectedItem.ToString() == tests[i].Name.ToString())
                {
                    mainWindow.GridMain.Children.Clear();
                    mainWindow.GridMain.Children.Add(new TestsUserControl.TestArticles(mainWindow, user, tests[i]));
                    break;
                }
            }
            
        }
    }
}
