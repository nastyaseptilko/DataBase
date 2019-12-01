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

namespace eLearning.UserContolsForAdmin
{
    /// <summary>
    /// Логика взаимодействия для CreatorTests.xaml
    /// </summary>
    public partial class CreatorTests : UserControl
    {
        string connectionString = DataBase.data;


        Classes.Theme themeInBD = new Classes.Theme();
        List<Classes.Theme> themes = new List<Classes.Theme>();
        Classes.Test testInBD = new Classes.Test();
        Classes.Question questionInDB = new Classes.Question();
        List<Classes.Answer> listAnswerInDB;
        

        public CreatorTests()
        {
            InitializeComponent();

            string getThemesForTests = "GET_THEME_FOR_TEST ";
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand getThemes = new SqlCommand(getThemesForTests, connection);
                    getThemes.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader1 = getThemes.ExecuteReader();

                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            Classes.Theme newTheme = new Classes.Theme();
                            newTheme.IdTheme = reader1.GetValue(0);
                            newTheme.NameTheme = reader1.GetValue(1).ToString();

                            themes.Add(newTheme);


                            ListViewItem theme = new ListViewItem();
                            theme.Content = newTheme.NameTheme;
                            theme.Selected += NewTheme_Selected;
                            listThemes.Items.Add(theme);
                        }
                    }
                    reader1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
           

        private void NewTheme_Selected(object sender, RoutedEventArgs e)
        {
            listTests.Items.Clear();
            SelectedTheme = ((ListBoxItem)sender).Content.ToString();
            string getTestsTests = "GET_TESTS_FOR_TEST";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Classes.Theme currentTheme = themes.Where(t => t.NameTheme.ToString() == SelectedTheme).First();
                SqlCommand getTestsTestsCommand = new SqlCommand(getTestsTests, connection);
                getTestsTestsCommand.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter themeIdParameter = new SqlParameter
                {
                    ParameterName = "@theme_Id",
                    Value = currentTheme.IdTheme
                };
                getTestsTestsCommand.Parameters.Add(themeIdParameter);
                SqlDataReader reader = getTestsTestsCommand.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem test = new ListViewItem();
                    test.Content = reader.GetValue(2).ToString();
                    test.Selected += NewTest_Selected;
                    listTests.Items.Add(test);
                }
                reader.Close();
            }
        }

        private void btnAddTest_Click(object sender, RoutedEventArgs e)
        {
            if (txbTest.Text != String.Empty)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    string getTestsTests = "GET_TESTS_TEST";

                    SqlCommand getTests = new SqlCommand(getTestsTests, connection);
                    getTests.CommandType = System.Data.CommandType.StoredProcedure;
                    bool flagTheme = false;
                    SqlDataReader readerTest = getTests.ExecuteReader();


                    if (readerTest.HasRows)
                    {
                        while (readerTest.Read())
                        {
                            if (txbTest.Text == readerTest.GetValue(1).ToString())
                            {
                                flagTheme = true;
                                break;
                            }
                        }
                    }
                    readerTest.Close();
                    
                    if(!flagTheme)
                    {
                        SelectedTest = txbTest.Text;

                        ListBoxItem newItem = new ListBoxItem();
                        newItem.Content = txbTest.Text;
                        newItem.Selected += NewTest_Selected;

                        testInBD.Name = txbTest.Text;
                        listTests.Items.Add(newItem);
                        txbTest.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Такая тема уже есть, просмотрите темы!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Введите тест!");
            }
        }

        private void NewTest_Selected(object sender, RoutedEventArgs e) => SelectedTest = ((ListBoxItem)sender).Content.ToString();

        private string SelectedQuestion = string.Empty;
        private string SelectedTest = string.Empty;
        private string SelectedTheme = string.Empty;

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (txbQuestion.Text != string.Empty)
            {
                SelectedQuestion = txbQuestion.Text;
                questionInDB.SomeQuestion = txbQuestion.Text;

                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = txbQuestion.Text;
                newItem.Selected += NewItem_Selected;

                listQuestions.Items.Add(newItem);
                txbQuestion.Text = "";
            }
            else
            {
                MessageBox.Show("Введите вопрос!");
            }
        }

        private void NewItem_Selected(object sender, RoutedEventArgs e)
        {
            SelectedQuestion = ((ListBoxItem)sender).Content.ToString();
        }

        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            listAnswerInDB = new List<Classes.Answer>();
            //если выбрал вопрос для которого буду создавать ответы
            if(listQuestions.SelectedIndex != -1)
            {
                if(txbAnswer1.Text != String.Empty && txbAnswer2.Text != String.Empty && txbAnswer3.Text != String.Empty)
                {
                    if (txbAnswer1.Text == txbAnswer2.Text || txbAnswer1.Text == txbAnswer3.Text || txbAnswer2.Text == txbAnswer1.Text || txbAnswer2.Text == txbAnswer3.Text || txbAnswer3.Text == txbAnswer1.Text || txbAnswer3.Text == txbAnswer2.Text)
                    {
                        MessageBox.Show("Ответы повторяются, измените!");                        
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Classes.Answer answer = new Classes.Answer();
                            switch (i)
                            {
                                case 0:
                                    answer.SomeAnswer = txbAnswer1.Text;
                                    listAnswerInDB.Add(answer);
                                    break;
                                case 1:
                                    answer.SomeAnswer = txbAnswer2.Text;
                                    listAnswerInDB.Add(answer);
                                    break;
                                case 2:
                                    answer.SomeAnswer = txbAnswer3.Text;
                                    listAnswerInDB.Add(answer);
                                    break;
                            }
                        }
                        txbAnswer1.Text = "";
                        txbAnswer2.Text = "";
                        txbAnswer3.Text = "";
                        MessageBox.Show("Можете приступать к сохранению!");
                    }
                }               
                else
                {
                    MessageBox.Show("Введите все ответы!");
                }
            }
            else
            {
                MessageBox.Show("Выберите вопрос, для которого хотите создать ответы!");
            }          
        }

        
        int numberQuestion = 1;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (themeInBD != null && testInBD != null && questionInDB != null && listAnswerInDB != null)
                {
                    //если не ввожу значения, то беру из выбранных
                    if (themeInBD.NameTheme == null || testInBD.Name == null)
                    {
                        themeInBD.NameTheme = SelectedTheme;
                        testInBD.Name = SelectedTest;
                    }

                    bool flagSave = false; //проверка, можно ли сохранить в бд!
                    int countAnswer = 0; //проверка, есть ли все ответы!


                    if (themeInBD.NameTheme.ToString() != String.Empty && testInBD.Name.ToString() != String.Empty && questionInDB.SomeQuestion.ToString() != String.Empty && listAnswerInDB.Count == 3)
                    {
                        for (int i = 0; i < listAnswerInDB.Count; i++)
                        {
                            if (listAnswerInDB[i].SomeAnswer.ToString() != String.Empty)
                            {
                                countAnswer++;
                            }
                        }
                        if (countAnswer == 3)
                        {
                            flagSave = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Все составляющие долны быть заполнены!");
                        return;
                    }

                    //если всё классно, все объекты заполнены, то я начинаю их сохранять в бд
                    if (flagSave)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();

                                
                                SqlTransaction transaction = connection.BeginTransaction();//Транзакции 
                                try
                                {                                    
                                    SqlCommand command = connection.CreateCommand();
                                    command.Transaction = transaction;
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    //Объект с которым буду работать и флаг(есть ли такая тема в бд)
                                    Classes.Theme theme = new Classes.Theme();
                                    bool flagTheme = false;


                                    command.CommandText = "GET_THEME_FOR_TEST";
                                    
                                    SqlDataReader readerTheme1 = command.ExecuteReader();

                                    if (readerTheme1.HasRows)
                                    {
                                        while (readerTheme1.Read())
                                        {
                                            if (SelectedTheme == readerTheme1.GetValue(1).ToString())
                                            {
                                                //заполняю выше созданный объект и устанавливаю флаг(что есть такая тема в бд)
                                                flagTheme = true;
                                                theme.IdTheme = readerTheme1.GetValue(0);
                                                theme.NameTheme = readerTheme1.GetValue(1);
                                                break;
                                            }
                                        }
                                    }
                                    readerTheme1.Close();

                                    
                                    if (flagTheme)
                                        MessageBox.Show("К существующей теме добавляется информация!");
                                    else
                                    {
                                        MessageBox.Show("Выберите одну из тем");
                                        return;
                                    }
                                    
                                    command.CommandText = "GET_TESTS_TEST";
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    SqlDataReader readerTest1 = command.ExecuteReader();

                                    Classes.Test test = new Classes.Test();
                                    bool flagTest = false;

                                    if (readerTest1.HasRows)
                                    {
                                        while (readerTest1.Read())
                                        {
                                            if (SelectedTest == readerTest1.GetValue(2).ToString())
                                            {
                                                flagTest = true;
                                                test.idTest = readerTest1.GetValue(0);
                                                test.idAdmin = readerTest1.GetValue(1);
                                                test.Name = readerTest1.GetValue(2).ToString();
                                                test.IdTheme = readerTest1.GetValue(3);
                                                break;
                                            }
                                        }
                                    }
                                    readerTest1.Close();

                                    if (!flagTest)
                                    {
                                        // Добавить команду добавления теста
                                        numberQuestion = 1;
                                        SqlCommand addTestsCommand = new SqlCommand("Add_TESTS", connection);
                                        addTestsCommand.Transaction = transaction;
                                        addTestsCommand.CommandType = System.Data.CommandType.StoredProcedure;
                                        
                                        // Передаем параметры и значения
                                        SqlParameter nameTestParameter = new SqlParameter
                                        {
                                            ParameterName = "@name_test",
                                            Value = testInBD.Name
                                        };

                                        SqlParameter adminIdParameter = new SqlParameter
                                        {
                                            ParameterName = "@admin_id",
                                            Value = Admin.getInstance().Id
                                        };

                                        SqlParameter themeIdParameter = new SqlParameter
                                        {
                                            ParameterName = "@theme_Id",
                                            Value = theme.IdTheme
                                        };

                                        // Добавляем парраметры
                                        addTestsCommand.Parameters.Add(nameTestParameter);
                                        addTestsCommand.Parameters.Add(adminIdParameter);
                                        addTestsCommand.Parameters.Add(themeIdParameter);
                                        addTestsCommand.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        MessageBox.Show("К существующему тесту добавляем информацию!");
                                    }


                                    SqlCommand getAllTestsCommand = new SqlCommand("GET_TESTS_TEST", connection);
                                    getAllTestsCommand.Transaction = transaction;
                                    getAllTestsCommand.CommandType = System.Data.CommandType.StoredProcedure;
                                    SqlDataReader readerTest2 = getAllTestsCommand.ExecuteReader();

                                    if (readerTest2.HasRows)
                                    {
                                        while (readerTest2.Read())
                                        {
                                            if (SelectedTest == readerTest2.GetValue(2).ToString())
                                            {
                                                test.idTest = readerTest2.GetValue(0);
                                                test.idAdmin = readerTest2.GetValue(1);
                                                test.Name = readerTest2.GetValue(2).ToString();
                                                test.IdTheme = readerTest2.GetValue(3);
                                                break;
                                            }
                                        }
                                    }
                                    readerTest2.Close();

                                    //--------------------------------------------------------------------------------------------------------------------------
                                    
                                    SqlCommand getTestQuestionsCommand = new SqlCommand("GET_QUESTIONS", connection);
                                    getTestQuestionsCommand.Transaction = transaction;
                                    getTestQuestionsCommand.CommandType = System.Data.CommandType.StoredProcedure;
                                    SqlDataReader readerQuestion1 = getTestQuestionsCommand.ExecuteReader();

                                    Classes.Question question = new Classes.Question();
                                    bool flagQuestion = false; //проверка, есть ли вопросы в бд

                                    if (readerQuestion1.HasRows)
                                    {
                                        while (readerQuestion1.Read())
                                        {
                                            if (SelectedQuestion == readerQuestion1.GetValue(2).ToString())
                                            {
                                                flagQuestion = true;
                                                question.IdQuestion = readerQuestion1.GetValue(0);
                                                question.IdTest = readerQuestion1.GetValue(1);
                                                question.NumberQuestion = readerQuestion1.GetValue(2);
                                                question.SomeQuestion = readerQuestion1.GetValue(3);
                                                break;
                                            }
                                        }
                                    }
                                    readerQuestion1.Close();

                                    if (!flagQuestion)
                                    {
                                        SqlCommand addQuestionsCommand = new SqlCommand("Add_QUESTIONS", connection);
                                        addQuestionsCommand.Transaction = transaction;
                                        addQuestionsCommand.CommandType = System.Data.CommandType.StoredProcedure;

                                        // Передаем параметры и значения
                                        SqlParameter testIdParameter = new SqlParameter
                                        {
                                            ParameterName = "@test_Id",
                                            Value = (int) test.idTest
                                        };

                                        SqlParameter numberQuestionParameter = new SqlParameter
                                        {
                                            ParameterName = "@number_question",
                                            Value = numberQuestion
                                        };

                                        SqlParameter questionParameter = new SqlParameter
                                        {
                                            ParameterName = "@question",
                                            Value = questionInDB.SomeQuestion
                                        };
                                        // Добавляем парраметры
                                        addQuestionsCommand.Parameters.Add(testIdParameter);
                                        addQuestionsCommand.Parameters.Add(numberQuestionParameter);
                                        addQuestionsCommand.Parameters.Add(questionParameter);
                                        addQuestionsCommand.ExecuteNonQuery();
                                        numberQuestion++;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Такой вопрос уже есть!");
                                        return;
                                    }
                                    
                                    SqlDataReader readerQuestion2 = getTestQuestionsCommand.ExecuteReader();

                                    bool isQuestion = false;
                                    if (readerQuestion2.HasRows)
                                    {
                                        while (readerQuestion2.Read())
                                        {
                                            if (SelectedQuestion == readerQuestion2.GetValue(3).ToString())
                                            {
                                                isQuestion = true;
                                                question.IdQuestion = readerQuestion2.GetValue(0);
                                                question.IdTest = readerQuestion2.GetValue(1);
                                                question.NumberQuestion = readerQuestion2.GetValue(2);
                                                question.SomeQuestion = readerQuestion2.GetValue(3);
                                                break;
                                            }
                                        }
                                    }
                                    readerQuestion2.Close();

                                    //--------------------------------------------------------------------------------------------------------------------------
                                    
                                    if (isQuestion)
                                    {
                                    
                                        SqlCommand addAnswerCommandOne = new SqlCommand("Add_ANSWER ", connection);
                                        addAnswerCommandOne.Transaction = transaction;
                                        addAnswerCommandOne.CommandType = System.Data.CommandType.StoredProcedure;
                                        // Передаем параметры и значения
                                        SqlParameter answerParameter = new SqlParameter
                                        {
                                            ParameterName = "@answer",
                                            Value = listAnswerInDB[0].SomeAnswer
                                        };

                                        SqlParameter isRightParameter = new SqlParameter
                                        {
                                            
                                            ParameterName = "@is_right",
                                            Value = 1
                                        };

                                        SqlParameter questionIdParameter = new SqlParameter
                                        {
                                            ParameterName = "@question_Id",
                                            Value = question.IdQuestion
                                        };
                                        // Добавляем парраметры
                                        addAnswerCommandOne.Parameters.Add(answerParameter);
                                        addAnswerCommandOne.Parameters.Add(isRightParameter);
                                        addAnswerCommandOne.Parameters.Add(questionIdParameter);

                                        addAnswerCommandOne.ExecuteNonQuery();

                                        answerParameter.Value = listAnswerInDB[1].SomeAnswer;
                                        isRightParameter.Value = 0;

                                        addAnswerCommandOne.ExecuteNonQuery();
                                        
                                        answerParameter.Value = listAnswerInDB[2].SomeAnswer;

                                        addAnswerCommandOne.ExecuteNonQuery();
                                        
                                        transaction.Commit();

                                    }

                                    MessageBox.Show("Добавление прошло успешно!");
                                    listQuestions.Items.Remove(listQuestions.SelectedItem);

                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                    transaction.Rollback();
                                }
                            
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Все составляющие должны быть заполнены!");
                }
            
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void listTests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<string> questions = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "JOIN_QUESTION ";

                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Передаем параметры и значения
                SqlParameter nameTestParameter = new SqlParameter
                {
                    ParameterName = "@name_test",
                    Value = listTests.SelectedItem
              
                };
                command.Parameters.Add(nameTestParameter);


                SqlDataReader readerTest = command.ExecuteReader();
                if (readerTest.HasRows)
                {
                    while (readerTest.Read())
                    {
                        questions.Add(readerTest.GetValue(0).ToString());
                    }
                }
                readerTest.Close();

                DialogWindow dialogWindow = new DialogWindow(questions);
                dialogWindow.ShowDialog();
            }
        }
    }
}
