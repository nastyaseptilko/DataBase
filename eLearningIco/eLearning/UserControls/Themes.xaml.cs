using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Themes.xaml
    /// </summary>
    public partial class Themes : UserControl
    {
        MainWindow mainWindow;

        public Themes()
        {
        }

        public Themes(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Theme1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            mainWindow.GridMain.Children.Add(new ThemesUserControl.Tences());
        }

        private void Theme2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            mainWindow.GridMain.Children.Add(new ThemesUserControl.Articles());
        }

        private void Theme3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            mainWindow.GridMain.Children.Add(new ThemesUserControl.Noun());
        }

        private void Theme4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            mainWindow.GridMain.Children.Add(new ThemesUserControl.Adjectives());
        }

        internal void Show()
        {
            throw new NotImplementedException();
        }
    }
}
