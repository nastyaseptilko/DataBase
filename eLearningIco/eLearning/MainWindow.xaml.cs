using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

namespace eLearning
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isAdmin = false;
        Classes.User user;
        
        //конструктор для пользователя
        public MainWindow(Classes.User user)
        {
            this.user = user;
            InitializeComponent();
            GridMain.Children.Add(new UserControls.Themes(this));
        }
        
        Classes.Admin admin;

        public MainWindow(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();

            GridCursor.Margin = new Thickness(50 + 150, -500, 0, 0);

            GridMain.Children.Clear();
            GridMain.Children.Add(new UserContolsForAdmin.CreatorTests());

            isAdmin = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid); //Source: элемент логического дерева, являющийся источником события.

            GridCursor.Margin = new Thickness(50 + (150 * index), -500, 0, 0);

            if (isAdmin)
            {
                switch (index)
                {
                    case 0:
                        MessageBox.Show("Эта вкладка только для авторизированного пользователя!");

                        GridCursor.Margin = new Thickness(50 + 150, -500, 0, 0);
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserContolsForAdmin.CreatorTests());
                        break;
                    case 1:
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserContolsForAdmin.CreatorTests());
                        break;
                    case 2:
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserContolsForAdmin.Information(admin));
                        break;
                    case 3:
                        MessageBox.Show("Эта вкладка только для авторизированного пользователя!");

                        GridCursor.Margin = new Thickness(50 + 150, -500, 0, 0);
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserContolsForAdmin.CreatorTests());
                        break;
                }
            }

            else
            {

                switch (index)
                {
                    case 0:
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserControls.Themes(this));
                        break;
                    case 1:
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserControls.Topics(this, user));
                        break;
                    case 2:
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserControls.Progress(user));
                        break;
                    case 3:
                        GridMain.Children.Clear();
                        GridMain.Children.Add(new UserControls.Dictionary(this, user));
                        break;
                }
            }
        }

        private void closeForm_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
        private void exitUser_Click(object sender, RoutedEventArgs e)
        {
            Windows.Login login = new Windows.Login();
            login.Show();
            Close();
        }
        //private void Theme1_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    GridMain.Children.Clear();
        //    GridMain.Children.Add(new ThemesUserControl.Tences());
        //}

        //private void Theme2_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    GridMain.Children.Clear();
        //    GridMain.Children.Add(new ThemesUserControl.Articles());
        //}

        //private void Theme3_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    GridMain.Children.Clear();
        //    GridMain.Children.Add(new ThemesUserControl.Noun());
        //}

        //private void Theme4_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    GridMain.Children.Clear();
        //    GridMain.Children.Add(new ThemesUserControl.Adjectives());
        //}

    }
}
