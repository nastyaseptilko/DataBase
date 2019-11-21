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
    /// Логика взаимодействия для Dictionary.xaml
    /// </summary>
    public partial class Dictionary : UserControl
    {
        MainWindow mainWindow;

        public List<Classes.PodTheme> listPodThemes = new List<Classes.PodTheme>();
        public List<Classes.ThemesForDictionary> listThemseForDictionaries = new List<Classes.ThemesForDictionary>();
        Classes.User user;
        public Dictionary(MainWindow mainWindow, Classes.User user)
        {
            this.user = user;
            this.mainWindow = mainWindow;
            InitializeComponent();

            string connectionString = DataBase.data;
            string getThemeForDictionary = "GET_THEME_FOR_DICTIONARY";
            string getPodThemes = "GET_POD_THEMES";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand getThemeForDictionaryCommand = new SqlCommand(getThemeForDictionary, sqlConnection);
                    getThemeForDictionaryCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = getThemeForDictionaryCommand.ExecuteReader();

                    
                    if (reader.HasRows)
                    {
                        
                        while (reader.Read())
                        {
                            Classes.ThemesForDictionary themesForDictionary = new Classes.ThemesForDictionary();
                            themesForDictionary.IdTheme = reader.GetValue(0);
                            themesForDictionary.IdAdmin = reader.GetValue(1);
                            themesForDictionary.NameThemeForDictionary = reader.GetValue(2);
                            listThemseForDictionaries.Add(themesForDictionary);
                        }
                    }
                    reader.Close();

                    for(int i = 0; i < listThemseForDictionaries.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                txbThemeDictionary1.Text = listThemseForDictionaries[i].NameThemeForDictionary.ToString();
                                break;
                            case 1:
                                txbThemeDictionary2.Text = listThemseForDictionaries[i].NameThemeForDictionary.ToString();
                                break;
                            case 2:
                                txbThemeDictionary3.Text = listThemseForDictionaries[i].NameThemeForDictionary.ToString();
                                break;
                            case 3:
                                txbThemeDictionary4.Text = listThemseForDictionaries[i].NameThemeForDictionary.ToString();
                                break;
                        }
                    }
                    /////////////////////////////////////////////////////////////////

                    SqlCommand getPodThemesCommand = new SqlCommand(getPodThemes, sqlConnection);
                    getPodThemesCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader2 = getPodThemesCommand.ExecuteReader();


                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            Classes.PodTheme podTheme = new Classes.PodTheme();
                            podTheme.IdTheme = reader2.GetValue(0);
                            podTheme.IdThemeForDictionary = reader2.GetValue(1);
                            podTheme.NameTheme = reader2.GetValue(2);
                            listPodThemes.Add(podTheme);
                        }
                    }
                    reader2.Close();

                    List<string> listThemesDict1 = new List<string>();
                    List<string> listThemesDict2 = new List<string>();
                    List<string> listThemesDict3 = new List<string>();
                    List<string> listThemesDict4 = new List<string>();
                    for (int i = 0; i < listPodThemes.Count; i++)
                    {
                        if ((int)listPodThemes[i].IdThemeForDictionary == 1)
                        {
                            listThemesDict1.Add(listPodThemes[i].NameTheme.ToString());
                        }
                        if ((int)listPodThemes[i].IdThemeForDictionary == 2)
                        {
                            listThemesDict2.Add(listPodThemes[i].NameTheme.ToString());
                        }
                        if ((int)listPodThemes[i].IdThemeForDictionary == 3)
                        {
                            listThemesDict3.Add(listPodThemes[i].NameTheme.ToString());
                        }
                        if ((int)listPodThemes[i].IdThemeForDictionary == 4)
                        {
                            listThemesDict4.Add(listPodThemes[i].NameTheme.ToString());
                        }
                    }
                    
                    this.listFormThemeDict1.ItemsSource = listThemesDict1;
                    this.listFormThemeDict2.ItemsSource = listThemesDict2;
                    this.listFormThemeDict3.ItemsSource = listThemesDict3;
                    this.listFormThemeDict4.ItemsSource = listThemesDict4;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listFormThemeDict1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < listPodThemes.Count; i++)
                {
                    if (listPodThemes[i].NameTheme.ToString() == listFormThemeDict1.SelectedItem.ToString())
                    {
                        mainWindow.GridMain.Children.Clear();
                        mainWindow.GridMain.Children.Add(new TestsUserControl.TestDictionary(listPodThemes[i], user));
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listFormThemeDict2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < listPodThemes.Count; i++)
                {
                    if (listPodThemes[i].NameTheme.ToString() == listFormThemeDict2.SelectedItem.ToString())
                    {
                        mainWindow.GridMain.Children.Clear();
                        mainWindow.GridMain.Children.Add(new TestsUserControl.TestDictionary(listPodThemes[i], user));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listFormThemeDict3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < listPodThemes.Count; i++)
                {
                    if (listPodThemes[i].NameTheme.ToString() == listFormThemeDict3.SelectedItem.ToString())
                    {
                        mainWindow.GridMain.Children.Clear();
                        mainWindow.GridMain.Children.Add(new TestsUserControl.TestDictionary(listPodThemes[i], user));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listFormThemeDict4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < listPodThemes.Count; i++)
                {
                    if (listPodThemes[i].NameTheme.ToString() == listFormThemeDict4.SelectedItem.ToString())
                    {
                        mainWindow.GridMain.Children.Clear();
                        mainWindow.GridMain.Children.Add(new TestsUserControl.TestDictionary(listPodThemes[i], user));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
