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
using System.Windows.Shapes;

namespace eLearning.UserContolsForAdmin
{
    /// <summary>
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        
        public DialogWindow(List<string> question)
        {
            InitializeComponent();
            for(int i = 0; i < question.Count; i++)
            {
                listQuestions.Items.Add(question[i].ToString());
            }
        }
    }
}
