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


            string sqlExpression = $@"
            Select Tests.Name, Questions.IdQuestion, Questions.NumberQuestion, Questions.Question FROM Questions
            join Tests ON Questions.IdTest = Tests.IdTest
            where Tests.IdTest = {test.idTest}
";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression, sqlConnection);
                    SqlDataReader readerQuestion = sqlCommand.ExecuteReader();


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
                    string sqlExpressionAnswer = $@"
                                                SELECT Answer.Answer FROM Answer
                                                join Questions on Answer.IdQuestion = Questions.IdQuestion
                                                where Questions.Question = '{createTests[i].Question}'";


                    List<string> answers = new List<string>();
                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();

                            SqlCommand sqlCommand2 = new SqlCommand(sqlExpressionAnswer, sqlConnection);
                            SqlDataReader readerAnswer = sqlCommand2.ExecuteReader();

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
            string sqlExpression = $@"
        SELECT Questions.NumberQuestion, Questions.Question, Answer.Answer FROM Questions
        JOIN Answer ON Questions.IdQuestion = Answer.IdQuestion
        JOIN Tests ON Questions.IdTest = Tests.IdTest
        WHERE Answer.IsRight = 1 AND Tests.Name = '{txbNameTest.Text}'
";
            List<Classes.RightAnswer> rightAnswers = new List<Classes.RightAnswer>();
            try
            {


                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression, sqlConnection);
                    SqlDataReader readerQuestion = sqlCommand.ExecuteReader();

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

                string sqlExpressionProgressWin = $"INSERT INTO PROGRESS (IdUser, IdTest, NameTest, DateTest, IsRight, CountRightAnswer) VALUES ({user.idUser}, {test.idTest}, '{test.Name}', '{DateTime.Now}', 1, {countRightAnswer})";

                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        SqlCommand sqlCommandProgressWin = new SqlCommand(sqlExpressionProgressWin, sqlConnection);
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

                string sqlExpressionProgressBad = $"INSERT INTO PROGRESS (IdUser, IdTest, NameTest, DateTest, IsRight, CountRightAnswer) VALUES ({user.idUser}, {test.idTest}, '{test.Name}', '{DateTime.Now}', 0, {countRightAnswer})";

                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        SqlCommand sqlCommandProgressBad = new SqlCommand(sqlExpressionProgressBad, sqlConnection);
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
