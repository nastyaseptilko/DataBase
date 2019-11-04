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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eLearning.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Progress.xaml
    /// </summary>
    public partial class Progress : UserControl
    {
        Classes.User user;
        public Progress(Classes.User user)
        {
            this.user = user;
            InitializeComponent();

            txbUser.Text = user.login.ToString();

            string connectionString = DataBase.data; //строкa подключения
                                                                                                                                     //пройденные тесты

            string sqlExpressionWin = $@"
            SELECT Themes.Name, Progress.NameTest, Progress.DateTest, Progress.CountRightAnswer
            FROM Progress
            JOIN Tests ON Progress.IdTest = Tests.IdTest
            JOIN Themes ON Tests.IdTheme = Themes.IdTheme
            WHERE Progress.IsRight = 1 AND IdUser = {user.idUser}";
                        //не пройденные тесты
                        string sqlExpressionNotPassed = $@"
            SELECT Themes.Name, Progress.NameTest, Progress.DateTest, Progress.CountRightAnswer
            FROM Progress
            JOIN Tests ON Progress.IdTest = Tests.IdTest
            JOIN Themes ON Tests.IdTheme = Themes.IdTheme
            WHERE Progress.IsRight = 0 AND IdUser = {user.idUser}";
                        //пройденные тесты по словарю
                        string sqlExpressionWinTestDictionary = $@"
            SELECT * FROM ProgressDictionary
            WHERE IdUser = {user.idUser} AND IsRight = 1;
            ";
                        //не пройденные тесты по словарю
                        string sqlExpressionBadTestDictionary = $@"
            SELECT * FROM ProgressDictionary
            WHERE IdUser = {user.idUser} AND IsRight = 0;
            ";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //Этот класс инкапсулирует sql-выражение, которое должно быть выполнено.
                    SqlCommand sqlCommandWin = new SqlCommand(sqlExpressionWin, sqlConnection);
                    SqlDataReader readerWinTest = sqlCommandWin.ExecuteReader();

                    List<Classes.WinTestDictionary> listWinTests = new List<Classes.WinTestDictionary>();
                    if (readerWinTest.HasRows)
                    {
                        while (readerWinTest.Read())
                        {
                            Classes.WinTestDictionary winTest = new Classes.WinTestDictionary();
                            winTest.NameTheme = readerWinTest.GetValue(0);
                            winTest.NameTest = readerWinTest.GetValue(1);
                            winTest.DateTest = readerWinTest.GetValue(2);
                            winTest.CountRightAnswer = readerWinTest.GetValue(3);
                            listWinTests.Add(winTest);
                        }
                    }
                    readerWinTest.Close();

                    winGrid.ItemsSource = listWinTests;


                    SqlCommand sqlCommandNotPassed = new SqlCommand(sqlExpressionNotPassed, sqlConnection);
                    SqlDataReader readerNotPassedTest = sqlCommandNotPassed.ExecuteReader();


                    List<Classes.NotPassedTestDictionary> listNotPassedTests = new List<Classes.NotPassedTestDictionary>();
                    if (readerNotPassedTest.HasRows)
                    {
                        while (readerNotPassedTest.Read())
                        {
                            Classes.NotPassedTestDictionary notPassedTest = new Classes.NotPassedTestDictionary();
                            notPassedTest.NameTheme = readerNotPassedTest.GetValue(0);
                            notPassedTest.NameTest = readerNotPassedTest.GetValue(1);
                            notPassedTest.DateTest = readerNotPassedTest.GetValue(2);
                            notPassedTest.CountRightAnswer = readerNotPassedTest.GetValue(3);
                            listNotPassedTests.Add(notPassedTest);
                        }
                    }
                    readerNotPassedTest.Close();

                    notPassedGrid.ItemsSource = listNotPassedTests;


                    SqlCommand sqlCommandWinTestDict = new SqlCommand(sqlExpressionWinTestDictionary, sqlConnection);
                    SqlDataReader readerWinTestDict = sqlCommandWinTestDict.ExecuteReader();

                    List<Classes.TestDictionary> listWinTestsDict = new List<Classes.TestDictionary>();
                    if (readerWinTestDict.HasRows)
                    {
                        while (readerWinTestDict.Read())
                        {
                            Classes.TestDictionary testDictionary = new Classes.TestDictionary();
                            testDictionary.NameTest = readerWinTestDict.GetValue(1);
                            testDictionary.DateTest = readerWinTestDict.GetValue(2);
                            testDictionary.CountRightAnswer = readerWinTestDict.GetValue(4);
                            testDictionary.CountQuestion = readerWinTestDict.GetValue(5);
                            listWinTestsDict.Add(testDictionary);
                        }
                    }
                    readerWinTestDict.Close();

                    winDictTestGrid.ItemsSource = listWinTestsDict;


                    SqlCommand sqlCommandBadTestDict = new SqlCommand(sqlExpressionBadTestDictionary, sqlConnection); //позволяет выполнять операции с данными из БД
                    SqlDataReader readerBadTestDict = sqlCommandBadTestDict.ExecuteReader();//считывает полученные в результате запроса данные

                    List<Classes.TestDictionary> listBadTestsDict = new List<Classes.TestDictionary>();
                    if (readerBadTestDict.HasRows)
                    {
                        while (readerBadTestDict.Read())
                        {
                            Classes.TestDictionary testDictionary = new Classes.TestDictionary();
                            testDictionary.NameTest = readerBadTestDict.GetValue(1);
                            testDictionary.DateTest = readerBadTestDict.GetValue(2);
                            testDictionary.CountRightAnswer = readerBadTestDict.GetValue(4);
                            testDictionary.CountQuestion = readerBadTestDict.GetValue(5);
                            listBadTestsDict.Add(testDictionary);
                        }
                    }
                    readerBadTestDict.Close();

                    notPassedDictTestGrid.ItemsSource = listBadTestsDict;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }                    
    }
}
