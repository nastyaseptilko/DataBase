using eLearning.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для TestDictionary.xaml
    /// </summary>
    public partial class TestDictionary : UserControl
    {
        Classes.PodTheme podTheme;

        public List<Classes.Dictionary> dictionaries = new List<Classes.Dictionary>();
        Classes.User user;
        public TestDictionary(Classes.PodTheme podTheme, Classes.User user)
        {
            this.user = user;
            this.podTheme = podTheme;
            InitializeComponent();

            btnBeginTest.Visibility = Visibility.Visible;

            txbNameTest.Text = podTheme.NameTheme.ToString();

            string connectionString = DataBase.data;
            string sqlExpression = $@"
                SELECT Words.IdWord, Words.EnglishWord, Words.RussianWord
                FROM Words
                JOIN PodThemes ON Words.IdTheme = PodThemes.IdTheme
                LEFT JOIN ThemesForDictionary ON ThemesForDictionary.IdTheme = PodThemes.IdTheme
                WHERE NameTheme = '{txbNameTest.Text}'
                ";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(sqlExpression, sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Dictionary dictionary = new Classes.Dictionary();
                            dictionary.IdWord = reader.GetValue(0);
                            dictionary.EnglishWord = reader.GetValue(1);
                            dictionary.RussianWord = reader.GetValue(2);
                            dictionaries.Add(dictionary);
                        }
                    }
                    reader.Close();

                    for (int i = 0; i < dictionaries.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                txbEnglishWord1.Text = dictionaries[i].EnglishWord.ToString();
                                txbRussianWord1.Text = dictionaries[i].RussianWord.ToString();
                                break;
                            case 1:
                                txbEnglishWord2.Text = dictionaries[i].EnglishWord.ToString();
                                txbRussianWord2.Text = dictionaries[i].RussianWord.ToString();
                                break;
                            case 2:
                                txbEnglishWord3.Text = dictionaries[i].EnglishWord.ToString();
                                txbRussianWord3.Text = dictionaries[i].RussianWord.ToString();
                                break;
                            case 3:
                                txbEnglishWord4.Text = dictionaries[i].EnglishWord.ToString();
                                txbRussianWord4.Text = dictionaries[i].RussianWord.ToString();
                                break;
                            case 4:
                                txbEnglishWord5.Text = dictionaries[i].EnglishWord.ToString();
                                txbRussianWord5.Text = dictionaries[i].RussianWord.ToString();
                                break;
                            case 5:
                                txbEnglishWord6.Text = dictionaries[i].EnglishWord.ToString();
                                txbRussianWord6.Text = dictionaries[i].RussianWord.ToString();
                                break;
                            case 6:
                                txbEnglishWord7.Text = dictionaries[i].EnglishWord.ToString();
                                txbRussianWord7.Text = dictionaries[i].RussianWord.ToString();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPlayRow1_Click(object sender, RoutedEventArgs e) => PlayWord(txbEnglishWord1.Text);

        private void btnPlayRow2_Click(object sender, RoutedEventArgs e) => PlayWord(txbEnglishWord2.Text);

        private void btnPlayRow3_Click(object sender, RoutedEventArgs e) => PlayWord(txbEnglishWord3.Text);

        private void btnPlayRow4_Click(object sender, RoutedEventArgs e) => PlayWord(txbEnglishWord4.Text);

        private void btnPlayRow5_Click(object sender, RoutedEventArgs e) => PlayWord(txbEnglishWord5.Text);

        private void btnPlayRow6_Click(object sender, RoutedEventArgs e) => PlayWord(txbEnglishWord6.Text);

        private void btnPlayRow7_Click(object sender, RoutedEventArgs e) => PlayWord(txbEnglishWord7.Text);

        private void PlayWord(string word)
        {
            PromptBuilder prompt = new PromptBuilder();
            prompt.AppendText(word);
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            //foreach(var voice in synthesizer.GetInstalledVoices())
            //{
            //    MessageBox.Show(voice.VoiceInfo.Description);
            //}
            //synthesizer.SelectVoice("Microsoft David Desktop");
            
            synthesizer.SpeakAsync(prompt);
        }

        //создание теста
        public static int i;
        public List<string> listTestWords = new List<string>();
        int countRightAnswer = 0;

        public Classes.Word word1 = new Classes.Word();
        public Classes.Word word2 = new Classes.Word();
        public Classes.Word word3 = new Classes.Word();
        public Classes.Word word4 = new Classes.Word();

        private void btnBeginTest_Click(object sender, RoutedEventArgs e)
        {
            btnBeginTest.Visibility = Visibility.Hidden;
            btnNextQuestionTest.IsEnabled = false;
            btnNextQuestionTest.Opacity = 0.5;
            try
            {
                i = dictionaries.Count - 2;
                txbCountWord.Text = i.ToString();

                Random random1 = new Random();
                int i1 = random1.Next(1, 8);
                txbAnswer1.Text = dictionaries[i1 - 1].RussianWord.ToString();
                word1.EnglishWord = dictionaries[i1 - 1].EnglishWord.ToString();
                word1.RussianWord = dictionaries[i1 - 1].RussianWord.ToString();

                zz1:
                Random random2 = new Random();
                int i2 = random2.Next(1, 8);
                if (i2 == i1)
                {
                    goto zz1;
                }
                txbAnswer2.Text = dictionaries[i2 - 1].RussianWord.ToString();
                word2.EnglishWord = dictionaries[i2 - 1].EnglishWord.ToString();
                word2.RussianWord = dictionaries[i2 - 1].RussianWord.ToString();

                zz2:
                Random random3 = new Random();
                int i3 = random3.Next(1, 8);
                if (i3 == i2 || i3 == i1)
                {
                    goto zz2;
                }
                txbAnswer3.Text = dictionaries[i3 - 1].RussianWord.ToString();
                word3.EnglishWord = dictionaries[i3 - 1].EnglishWord.ToString();
                word3.RussianWord = dictionaries[i3 - 1].RussianWord.ToString();

                zz3:
                Random random4 = new Random();
                int i4 = random4.Next(1, 8);
                if (i4 == i3 || i4 == i2 || i4 == i1)
                {
                    goto zz3;
                }
                txbAnswer4.Text = dictionaries[i4 - 1].RussianWord.ToString();
                word4.EnglishWord = dictionaries[i4 - 1].EnglishWord.ToString();
                word4.RussianWord = dictionaries[i4 - 1].RussianWord.ToString();

                zz4:
                Random random5 = new Random();
                int tempWord = random5.Next(1, 8);
                if (tempWord == i1 || tempWord == i2 || tempWord == i3 || tempWord == i4)
                {
                    txbQuestion.Text = dictionaries[tempWord - 1].EnglishWord.ToString();
                    listTestWords.Add(txbQuestion.Text);
                }
                else
                {
                    goto zz4;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNextQuestionTest_Click(object sender, RoutedEventArgs e)
        {
            txbAnswer1.Visibility = Visibility.Visible;
            txbAnswer2.Visibility = Visibility.Visible;
            txbAnswer3.Visibility = Visibility.Visible;
            txbAnswer4.Visibility = Visibility.Visible;
            method1();
        }
        //для кнопки далее
        private void method1()
        {
            btnNextQuestionTest.IsEnabled = false;
            btnNextQuestionTest.Opacity = 0.5;

            txbAnswer1.Foreground = new SolidColorBrush(Colors.Black);
            txbAnswer2.Foreground = new SolidColorBrush(Colors.Black);
            txbAnswer3.Foreground = new SolidColorBrush(Colors.Black);
            txbAnswer4.Foreground = new SolidColorBrush(Colors.Black);
            txbAnswer1.TextDecorations = null;
            txbAnswer2.TextDecorations = null;
            txbAnswer3.TextDecorations = null;
            txbAnswer4.TextDecorations = null;

            if (txbQuestion.Text != String.Empty)
            {
                try
                {
                    Random random1 = new Random();
                    int i1 = random1.Next(1, 8);
                    txbAnswer1.Text = dictionaries[i1 - 1].RussianWord.ToString();
                    word1.EnglishWord = dictionaries[i1 - 1].EnglishWord.ToString();
                    word1.RussianWord = dictionaries[i1 - 1].RussianWord.ToString();

                    zz1:
                    Random random2 = new Random();
                    int i2 = random2.Next(1, 8);
                    if (i2 == i1)
                    {
                        goto zz1;
                    }
                    txbAnswer2.Text = dictionaries[i2 - 1].RussianWord.ToString();
                    word2.EnglishWord = dictionaries[i2 - 1].EnglishWord.ToString();
                    word2.RussianWord = dictionaries[i2 - 1].RussianWord.ToString();

                    zz2:
                    Random random3 = new Random();
                    int i3 = random3.Next(1, 8);
                    if (i3 == i2 || i3 == i1)
                    {
                        goto zz2;
                    }
                    txbAnswer3.Text = dictionaries[i3 - 1].RussianWord.ToString();
                    word3.EnglishWord = dictionaries[i3 - 1].EnglishWord.ToString();
                    word3.RussianWord = dictionaries[i3 - 1].RussianWord.ToString();

                    zz3:
                    Random random4 = new Random();
                    int i4 = random4.Next(1, 8);
                    if (i4 == i3 || i4 == i2 || i4 == i1)
                    {
                        goto zz3;
                    }
                    txbAnswer4.Text = dictionaries[i4 - 1].RussianWord.ToString();
                    word4.EnglishWord = dictionaries[i4 - 1].EnglishWord.ToString();
                    word4.RussianWord = dictionaries[i4 - 1].RussianWord.ToString();

                    zz4:
                    Random random5 = new Random();
                    int tempWord = random5.Next(1, 8);

                    for (int i = 0; i < listTestWords.Count; i++)
                    {
                        if (listTestWords[i] == dictionaries[tempWord - 1].EnglishWord.ToString())
                        {
                            goto zz4;
                        }
                    }

                    if (tempWord == i1 || tempWord == i2 || tempWord == i3 || tempWord == i4)
                    {
                        txbQuestion.Text = dictionaries[tempWord - 1].EnglishWord.ToString();
                        listTestWords.Add(txbQuestion.Text);
                    }
                    else
                    {
                        goto zz4;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }



                i--;//уменьшение кол ва
                txbCountWord.Text = i.ToString();

                if (i == 0 && countRightAnswer == 5)
                {
                    btnBeginTest.Visibility = Visibility.Hidden;
                    txbQuestion.Text = "Тест пройден!";
                    txbAnswer1.Text = "";
                    txbAnswer2.Text = "";
                    txbAnswer3.Text = "";
                    txbAnswer4.Text = "";
                    btnNextQuestionTest.IsEnabled = false;
                    btnNextQuestionTest.Opacity = 0.5;

                    int flagWinTest = 1;

                    string connectionString = DataBase.data;
                    string sqlExpression = $@"
                        INSERT INTO ProgressDictionary (IdUser, NameTest, DateTest, IsRight, CountRightAnswer, CountQuestion)
                        VALUES ({user.idUser}, '{txbNameTest.Text}', '{DateTime.Now}', {flagWinTest}, {countRightAnswer}, {dictionaries.Count - 2})";

                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();

                            SqlCommand sqlCommand = new SqlCommand(sqlExpression, sqlConnection);
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if(i == 0 && countRightAnswer != 5)
                {
                    btnBeginTest.Visibility = Visibility.Hidden;
                    txbQuestion.Text = "Тест не пройден!";
                    txbAnswer1.Text = "";
                    txbAnswer2.Text = "";
                    txbAnswer3.Text = "";
                    txbAnswer4.Text = "";
                    btnNextQuestionTest.IsEnabled = false;
                    btnNextQuestionTest.Opacity = 0.5;

                    int flagWinTest = 0;

                    string connectionString = DataBase.data;
                    string sqlExpression = $@"
                        INSERT INTO ProgressDictionary (IdUser, NameTest, DateTest, IsRight, CountRightAnswer, CountQuestion)
                        VALUES ({user.idUser}, '{txbNameTest.Text}', '{DateTime.Now}', {flagWinTest}, {countRightAnswer}, {dictionaries.Count - 2})";

                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();

                            SqlCommand sqlCommand = new SqlCommand(sqlExpression, sqlConnection);
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Начните тест!");
            }
        }

        private void txbAnswer1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            btnNextQuestionTest.IsEnabled = true;
            btnNextQuestionTest.Opacity = 1;
            if(word1.EnglishWord == txbQuestion.Text)
            {
                countRightAnswer++;
                txbAnswer1.Foreground = new SolidColorBrush(Colors.Green);
                txbAnswer1.FontSize = 20;

                txbAnswer2.Visibility = Visibility.Hidden;
                txbAnswer3.Visibility = Visibility.Hidden;
                txbAnswer4.Visibility = Visibility.Hidden;
            }
            else
            {
                txbAnswer1.Foreground = new SolidColorBrush(Colors.Red);
                txbAnswer1.FontSize = 20;

                txbAnswer2.Visibility = Visibility.Hidden;
                txbAnswer3.Visibility = Visibility.Hidden;
                txbAnswer4.Visibility = Visibility.Hidden;
            }
        }

        private void txbAnswer2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            btnNextQuestionTest.IsEnabled = true;
            btnNextQuestionTest.Opacity = 1;
            if (word2.EnglishWord == txbQuestion.Text)
            {
                countRightAnswer++;
                txbAnswer2.Foreground = new SolidColorBrush(Colors.Green);
                txbAnswer2.FontSize = 20;

                txbAnswer1.Visibility = Visibility.Hidden;
                txbAnswer3.Visibility = Visibility.Hidden;
                txbAnswer4.Visibility = Visibility.Hidden;
            }
            else
            {
                txbAnswer2.Foreground = new SolidColorBrush(Colors.Red);
                txbAnswer2.FontSize = 20;

                txbAnswer1.Visibility = Visibility.Hidden;
                txbAnswer3.Visibility = Visibility.Hidden;
                txbAnswer4.Visibility = Visibility.Hidden;
            }
        }

        private void txbAnswer3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            btnNextQuestionTest.IsEnabled = true;
            btnNextQuestionTest.Opacity = 1;
            if (word3.EnglishWord == txbQuestion.Text)
            {
                countRightAnswer++;
                txbAnswer3.Foreground = new SolidColorBrush(Colors.Green);
                txbAnswer3.FontSize = 20;

                txbAnswer1.Visibility = Visibility.Hidden;
                txbAnswer2.Visibility = Visibility.Hidden;
                txbAnswer4.Visibility = Visibility.Hidden;
            }
            else
            {
                txbAnswer3.Foreground = new SolidColorBrush(Colors.Red);
                txbAnswer3.FontSize = 20;

                txbAnswer1.Visibility = Visibility.Hidden;
                txbAnswer2.Visibility = Visibility.Hidden;
                txbAnswer4.Visibility = Visibility.Hidden;
            }
        }

        private void txbAnswer4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            btnNextQuestionTest.IsEnabled = true;
            btnNextQuestionTest.Opacity = 1;
            if (word4.EnglishWord == txbQuestion.Text)
            {
                countRightAnswer++;
                txbAnswer4.Foreground = new SolidColorBrush(Colors.Green);
                txbAnswer4.FontSize = 20;

                txbAnswer1.Visibility = Visibility.Hidden;
                txbAnswer2.Visibility = Visibility.Hidden;
                txbAnswer3.Visibility = Visibility.Hidden;
            }
            else
            {
                txbAnswer4.Foreground = new SolidColorBrush(Colors.Red);
                txbAnswer4.FontSize = 20;

                txbAnswer1.Visibility = Visibility.Hidden;
                txbAnswer2.Visibility = Visibility.Hidden;
                txbAnswer3.Visibility = Visibility.Hidden;
            }
        }
    }
}
