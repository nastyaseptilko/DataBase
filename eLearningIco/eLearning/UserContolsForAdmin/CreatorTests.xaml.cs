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

            string sqlExpression1 = "SELECT * FROM Themes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand sqlCommand1 = new SqlCommand(sqlExpression1, connection);
                    SqlDataReader reader1 = sqlCommand1.ExecuteReader();

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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Classes.Theme currentTheme = themes.Where(t => t.NameTheme.ToString() == SelectedTheme).First();
                SqlCommand command = new SqlCommand(
                    $"SELECT * FROM Tests " +
                    $"WHERE IdTheme = " +
                    $"{currentTheme.IdTheme}", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem test = new ListViewItem();
                    test.Content = reader.GetValue(1).ToString();
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

                    SqlCommand command = connection.CreateCommand();
                    bool flagTheme = false;

                    command.CommandText = "SELECT * FROM Tests";
                    SqlDataReader readerTest = command.ExecuteReader();

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

                                    //Объект с которым буду работать и флаг(есть ли такая тема в бд)
                                    Classes.Theme theme = new Classes.Theme();
                                    bool flagTheme = false;

                                    command.CommandText = "SELECT * FROM Themes";
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

                                                                
                                    command.CommandText = "SELECT * FROM Tests";
                                    SqlDataReader readerTest1 = command.ExecuteReader();

                                    Classes.Test test = new Classes.Test();
                                    bool flagTest = false;

                                    if (readerTest1.HasRows)
                                    {
                                        while (readerTest1.Read())
                                        {
                                            if (SelectedTest == readerTest1.GetValue(1).ToString())
                                            {
                                                flagTest = true;
                                                test.idTest = readerTest1.GetValue(0);
                                                test.Name = readerTest1.GetValue(1).ToString();
                                                test.IdTheme = readerTest1.GetValue(2);
                                                break;
                                            }
                                        }
                                    }
                                    readerTest1.Close();

                                    if (!flagTest)
                                    {
                                        numberQuestion = 1;
                                        command.CommandText = $"INSERT INTO Tests ([Name], [IdTheme]) VALUES ('{testInBD.Name}', {theme.IdTheme})";
                                        command.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        MessageBox.Show("К существующему тесту добавляем информацию!");
                                    }                                

                                    


                                    command.CommandText = "SELECT * FROM Tests";
                                    SqlDataReader readerTest2 = command.ExecuteReader();

                                    if (readerTest2.HasRows)
                                    {
                                        while (readerTest2.Read())
                                        {
                                            if (SelectedTest == readerTest2.GetValue(1).ToString())
                                            {
                                                test.idTest = readerTest2.GetValue(0);
                                                test.Name = readerTest2.GetValue(1).ToString();
                                                test.IdTheme = readerTest2.GetValue(2);
                                                break;
                                            }
                                        }
                                    }
                                    readerTest2.Close();

//--------------------------------------------------------------------------------------------------------------------------


                                    command.CommandText = "SELECT * FROM Questions";
                                    SqlDataReader readerQuestion1 = command.ExecuteReader();

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
                                        command.CommandText = $"INSERT INTO Questions ([IdTest], [NumberQuestion], [Question]) VALUES ({test.idTest}, {numberQuestion}, '{questionInDB.SomeQuestion}')";
                                        command.ExecuteNonQuery();
                                        numberQuestion++;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Такой вопрос уже есть!");
                                        return;
                                    }


                                    command.CommandText = "SELECT * FROM Questions";
                                    SqlDataReader readerQuestion2 = command.ExecuteReader();

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
                                        command.CommandText = $"INSERT INTO Answer ([Answer], [IsRight], [IdQuestion]) VALUES ('{listAnswerInDB[0].SomeAnswer}', 1, {question.IdQuestion})";
                                        command.ExecuteNonQuery();
                                        command.CommandText = $"INSERT INTO Answer ([Answer], [IsRight], [IdQuestion]) VALUES ('{listAnswerInDB[1].SomeAnswer}', 0, {question.IdQuestion})";
                                        command.ExecuteNonQuery();
                                        command.CommandText = $"INSERT INTO Answer ([Answer], [IsRight], [IdQuestion]) VALUES ('{listAnswerInDB[2].SomeAnswer}', 0, {question.IdQuestion})";
                                        command.ExecuteNonQuery();

                                        //
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
                command.CommandText = $@"
                                        SELECT Questions.Question FROM Questions
                                        JOIN Tests ON Questions.IdTest = Tests.IdTest
                                        WHERE Tests.Name = '{listTests.SelectedItem}';";

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
