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

            string connectionString = DataBase.data; 
            // Пройденные тесты
            string PassedForTests = "PASSED_FOR_TEST";
            // Пройденные тесты
            string noPassedForTests = "NO_PASSED_FOR_TEST" ;
            // Пройденные тесты по словарю
            string passedForDictionary = "PASSED_FOR_DICTIONARY ";
            // Не пройденные тесты по словарю
            string noPassedForDictionary = "NO_PASSED_FOR_DICTIONARY"; 
           

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //Этот класс инкапсулирует sql-выражение, которое должно быть выполнено.
                    SqlCommand PassedForTestsProgress = new SqlCommand(PassedForTests, sqlConnection);
                    // Задаем тип команды
                    PassedForTestsProgress.CommandType = System.Data.CommandType.StoredProcedure;

                    // Передаем параметры и значения
                    SqlParameter ProgressParameter = new SqlParameter
                    {
                        ParameterName = "@user_Id",
                        Value = user.idUser
                    };

                    PassedForTestsProgress.Parameters.Add(ProgressParameter);
                    SqlDataReader readerWinTest = PassedForTestsProgress.ExecuteReader();

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


                    SqlCommand noPassedForTestsProgress = new SqlCommand(noPassedForTests, sqlConnection);
                    // Задаем тип команды
                    noPassedForTestsProgress.CommandType = System.Data.CommandType.StoredProcedure;
                    // Передаем параметры и значения
                    SqlParameter noProgressParameter = new SqlParameter
                    {
                        ParameterName = "@user_Id",
                        Value = user.idUser
                    };

                    noPassedForTestsProgress.Parameters.Add(noProgressParameter);

                    SqlDataReader readerNotPassedTest = noPassedForTestsProgress.ExecuteReader();


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


                    SqlCommand passedForDictionaryProgress = new SqlCommand(passedForDictionary , sqlConnection);
                    passedForDictionaryProgress.CommandType = System.Data.CommandType.StoredProcedure;
                    // Передаем параметры и значения
                    SqlParameter progressParameterDictionary = new SqlParameter
                    {
                        ParameterName = "@user_Id",
                        Value = user.idUser
                    };

                    passedForDictionaryProgress.Parameters.Add(progressParameterDictionary);
                    SqlDataReader readerWinTestDict = passedForDictionaryProgress.ExecuteReader();

                    List<Classes.TestDictionary> listWinTestsDict = new List<Classes.TestDictionary>();
                    if (readerWinTestDict.HasRows)
                    {
                        while (readerWinTestDict.Read())
                        {
                            Classes.TestDictionary testDictionary = new Classes.TestDictionary();
                            testDictionary.NameTest = readerWinTestDict.GetValue(9);
                            testDictionary.DateTest = readerWinTestDict.GetValue(3);
                            testDictionary.CountRightAnswer = readerWinTestDict.GetValue(5);
                            testDictionary.CountQuestion = readerWinTestDict.GetValue(6);
                            listWinTestsDict.Add(testDictionary);
                        }
                    }
                    readerWinTestDict.Close();

                    winDictTestGrid.ItemsSource = listWinTestsDict;


                    SqlCommand noPassedForDictionaryProgress = new SqlCommand(noPassedForDictionary, sqlConnection); //позволяет выполнять операции с данными из БД
                    noPassedForDictionaryProgress.CommandType = System.Data.CommandType.StoredProcedure;
                    // Передаем параметры и значения
                    SqlParameter noProgressParameterDictionary = new SqlParameter
                    {
                        ParameterName = "@user_Id",
                        Value = user.idUser
                    };

                    noPassedForDictionaryProgress.Parameters.Add(noProgressParameterDictionary);
                    
                    SqlDataReader readerBadTestDict = noPassedForDictionaryProgress.ExecuteReader();

                    List<Classes.TestDictionary> listBadTestsDict = new List<Classes.TestDictionary>();
                    if (readerBadTestDict.HasRows)
                    {
                        while (readerBadTestDict.Read())
                        {
                            Classes.TestDictionary testDictionary = new Classes.TestDictionary();
                            testDictionary.NameTest = readerBadTestDict.GetValue(9);
                            testDictionary.DateTest = readerBadTestDict.GetValue(3);
                            testDictionary.CountRightAnswer = readerBadTestDict.GetValue(5);
                            testDictionary.CountQuestion = readerBadTestDict.GetValue(6);
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
