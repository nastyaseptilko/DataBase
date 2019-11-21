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

namespace eLearning.TestsUserControl
{
    /// <summary>
    /// Логика взаимодействия для TestArticles.xaml
    /// </summary>
    public partial class TestArticles : UserControl
    {
        string connectionString = DataBase.data;

        MainWindow mainWindow;
        Classes.User user;
        Classes.Test test;
        //public UserControls.Progress formProgress;
        List<Classes.CreateTests> createTests = new List<Classes.CreateTests>();

        public TestArticles(MainWindow mainWindow, Classes.User user, Classes.Test test)
        {
            this.mainWindow = mainWindow;
            this.user = user;
            this.test = test;

            InitializeComponent();
            
            txbNameTest.Text = test.Name.ToString();
            txbNameTest.TextWrapping = TextWrapping.Wrap;

            string testArticles = "TEST_ARTICLES_TESTS";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand testArticlesCommand = new SqlCommand(testArticles, sqlConnection);
                    testArticlesCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter testIdParameter = new SqlParameter
                    {
                        ParameterName = "@test_Id",
                        Value = test.idTest
                    };
                    testArticlesCommand.Parameters.Add(testIdParameter);
                    SqlDataReader readerQuestion = testArticlesCommand.ExecuteReader();
                    
                    if (readerQuestion.HasRows)
                    {
                        while (readerQuestion.Read())
                        {
                            Classes.CreateTests createTest = new Classes.CreateTests();
                            createTest.NameTest = readerQuestion.GetValue(0);
                            createTest.IdQuestion = readerQuestion.GetValue(1);
                            createTest.NumberQuestion = readerQuestion.GetValue(2);
                            createTest.Question = readerQuestion.GetValue(3);
                            createTests.Add(createTest);
                        }
                    }
                    readerQuestion.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            try
            {
                for (int i = 0; i < createTests.Count; i++)
                {
                    Grid grid = new Grid();

                    grid.Height = 170;

                    TextBlock txbNumberQuestion = new TextBlock();
                    txbNumberQuestion.Text = createTests[i].NumberQuestion.ToString() + ".";
                    txbNumberQuestion.FontSize = 30;
                    txbNumberQuestion.Margin = new Thickness(10, 20, 10, 20);

                    grid.Children.Add(txbNumberQuestion);

                    Border border = new Border();
                    border.Height = 90;
                    border.Margin = new Thickness(60, 20, 60, 20);
                    border.VerticalAlignment = VerticalAlignment.Top;
                    border.CornerRadius = new CornerRadius(15);
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = new SolidColorBrush(Colors.Black);

                    TextBlock txbQuestion = new TextBlock();
                    txbQuestion.Text = createTests[i].Question.ToString();
                    txbQuestion.Margin = new Thickness(5, 0, 0, 0);
                    txbQuestion.FontSize = 30;
                    txbQuestion.TextWrapping = TextWrapping.Wrap;

                    border.Child = txbQuestion;

                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Margin = new Thickness(60, 130, 0, 0);

                    //получаем ответы по вопросу
                    string articlesAnswer = "TEST_ARTICLES_ANSWERS ";


                    List<string> answers = new List<string>();
                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();

                            SqlCommand articlesAnswerCommand= new SqlCommand(articlesAnswer, sqlConnection);
                            articlesAnswerCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter qustionParameter = new SqlParameter
                            {
                                ParameterName = "@question",
                                Value = createTests[i].Question
                            };
                            articlesAnswerCommand.Parameters.Add(qustionParameter);

                            SqlDataReader readerAnswer = articlesAnswerCommand.ExecuteReader();

                            if (readerAnswer.HasRows)
                            {
                                while (readerAnswer.Read())
                                {
                                    answers.Add(readerAnswer.GetValue(0).ToString());
                                }
                            }
                            readerAnswer.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }



                    for (int j = 0; j < answers.Count; j++)
                    {
                        RadioButton radioButton1 = new RadioButton();
                        radioButton1.Content = answers[j].ToString();
                        radioButton1.FontSize = 18;
                        radioButton1.Margin = new Thickness(20, 0, 0, 0);
                        radioButton1.Padding = new Thickness(10, -3, 0, 0);
                        stackPanel.Children.Add(radioButton1);
                    }

                    grid.Children.Add(border);
                    grid.Children.Add(stackPanel);

                    stkPQuestion.Children.Add(grid);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void Close()
        {
            throw new NotImplementedException();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            string articlesQuestion = "TEST_ARTICLES_QUESTIONS ";
            List<Classes.RightAnswer> rightAnswers = new List<Classes.RightAnswer>();
            try
            {


                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand articlesQuestionCommand = new SqlCommand(articlesQuestion, sqlConnection);
                    articlesQuestionCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter nameTestParameter = new SqlParameter
                    {
                        ParameterName = "@name_test",
                        Value = txbNameTest.Text
                    };
                    articlesQuestionCommand.Parameters.Add(nameTestParameter);

                    SqlDataReader readerQuestion = articlesQuestionCommand.ExecuteReader();

                    if (readerQuestion.HasRows)
                    {
                        while (readerQuestion.Read())
                        {
                            Classes.RightAnswer rightAnswer = new Classes.RightAnswer();
                            rightAnswer.NumberQuestion = readerQuestion.GetValue(0);
                            rightAnswer.Question = readerQuestion.GetValue(1);
                            rightAnswer.Answer = readerQuestion.GetValue(2);
                            rightAnswers.Add(rightAnswer);
                        }
                    }
                    readerQuestion.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            int countRightAnswer = 0;


            foreach (Grid grid in stkPQuestion.Children)
            {
                TextBlock txbNumberQuestion = new TextBlock();
                TextBlock txbQuestion = new TextBlock();
                foreach (var stkP in grid.Children)
                {

                    if (stkP.GetType() == typeof(TextBlock))
                    {
                        txbNumberQuestion = (TextBlock)stkP;
                    }

                    if (stkP.GetType() == typeof(Border))
                    {
                        Border border = (Border)stkP;
                        var q = border.Child;
                        txbQuestion = (TextBlock)q;
                    }

                    if (stkP.GetType() == typeof(StackPanel))
                    {
                        StackPanel stackPanel = (StackPanel)stkP;
                        foreach (RadioButton rButt in stackPanel.Children)
                        {

                            for (int i = 0; i < rightAnswers.Count; i++)
                            {
                                if (rButt.IsChecked == true && rButt.Content.ToString() == rightAnswers[i].Answer.ToString() && rightAnswers[i].NumberQuestion.ToString() + "." == txbNumberQuestion.Text.ToString() && rightAnswers[i].Question.ToString() == txbQuestion.Text.ToString())
                                {
                                    countRightAnswer++;
                                }
                            }
                        }
                    }
                }
            }
            
            if(countRightAnswer == rightAnswers.Count)
            {
                MessageBox.Show("Тест пройден. Просмотрите вкладку достижений!");

                //string sqlExpressionProgressWin = $"INSERT INTO PROGRESS_FOR_TEST(IdUser, IdTest, DateTest, IsRight, CountRightAnswer) VALUES 
                //({user.idUser}, {test.idTest}, '{DateTime.Now}', 1, {countRightAnswer})";
                string addProgressForTests = "ADD_PROGRESS_FOR_TESTS";
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        SqlCommand sqlCommandProgressWin = new SqlCommand(addProgressForTests, sqlConnection);
                        sqlCommandProgressWin.CommandType = System.Data.CommandType.StoredProcedure;
                        // ДОБАВИТЬ ВСЕ ПАРАМЕТРЫ К КОМАНДЕ
                        SqlParameter userIdParameter = new SqlParameter
                        {
                            ParameterName = "@user_Id",
                            Value = user.idUser
                        };
                        SqlParameter testIdParameter = new SqlParameter
                        {
                            ParameterName = "@test_Id",
                            Value = test.idTest
                        };
                        SqlParameter dateTestParameter = new SqlParameter
                        {
                            ParameterName = "@date_test",
                            Value = DateTime.Now
                        };
                        SqlParameter isRightParameter = new SqlParameter
                        {
                            ParameterName = "@is_right",
                            Value = 1
                        };
                        SqlParameter countRightParameter = new SqlParameter
                        {
                            ParameterName = "@count_right_answer",
                            Value = countRightAnswer
                        };
                        sqlCommandProgressWin.Parameters.Add(userIdParameter);
                        sqlCommandProgressWin.Parameters.Add(testIdParameter);
                        sqlCommandProgressWin.Parameters.Add(dateTestParameter);
                        sqlCommandProgressWin.Parameters.Add(isRightParameter);
                        sqlCommandProgressWin.Parameters.Add(countRightParameter);
                        sqlCommandProgressWin.ExecuteNonQuery();

                        mainWindow.GridMain.Children.Clear();
                        mainWindow.GridMain.Children.Add(new UserControls.Topics(mainWindow, user));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show($"Тест не пройден. Количество правильных ответов: {countRightAnswer.ToString()} из {rightAnswers.Count.ToString()}");

                //string sqlExpressionProgressBad = $"INSERT INTO PROGRESS_FOR_TEST(IdUser, IdTest, DateTest, IsRight, CountRightAnswer) 
                //VALUES ({user.idUser}, {test.idTest}, '{DateTime.Now}', 0, {countRightAnswer})";
                string addProgressBadForTest = "NO_ADD_PROGRESS_FOR_TESTS";
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        SqlCommand sqlCommandProgressBad = new SqlCommand(addProgressBadForTest, sqlConnection);
                        // СДЕЛАТЬ КОМАНДУ ДЛЯ ВЫПОЛНЕНИЯ ПРОЦЕДУРЫ
                        // ДОБАВИТЬ ВСЕ ПАРАМЕТРЫ К КОМАНДЕ
                        sqlCommandProgressBad.CommandType = System.Data.CommandType.StoredProcedure;
                        // ДОБАВИТЬ ВСЕ ПАРАМЕТРЫ К КОМАНДЕ
                        SqlParameter userIdParameter = new SqlParameter
                        {
                            ParameterName = "@user_Id",
                            Value = user.idUser
                        };
                        SqlParameter testIdParameter = new SqlParameter
                        {
                            ParameterName = "@test_Id",
                            Value = test.idTest
                        };
                        SqlParameter dateTestParameter = new SqlParameter
                        {
                            ParameterName = "@date_test",
                            Value = DateTime.Now
                        };
                        SqlParameter isRightParameter = new SqlParameter
                        {
                            ParameterName = "@is_right",
                            Value = 0
                        };
                        SqlParameter countRightParameter = new SqlParameter
                        {
                            ParameterName = "@count_right_answer",
                            Value = countRightAnswer
                        };
                        sqlCommandProgressBad.Parameters.Add(userIdParameter);
                        sqlCommandProgressBad.Parameters.Add(testIdParameter);
                        sqlCommandProgressBad.Parameters.Add(dateTestParameter);
                        sqlCommandProgressBad.Parameters.Add(isRightParameter);
                        sqlCommandProgressBad.Parameters.Add(countRightParameter);
                        sqlCommandProgressBad.ExecuteNonQuery();
                        sqlCommandProgressBad.ExecuteNonQuery();

                        mainWindow.GridMain.Children.Clear();
                        mainWindow.GridMain.Children.Add(new UserControls.Topics(mainWindow, user));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                mainWindow.GridMain.Children.Clear();
                mainWindow.GridMain.Children.Add(new UserControls.Topics(mainWindow, user));
            }
        }
    }
}
