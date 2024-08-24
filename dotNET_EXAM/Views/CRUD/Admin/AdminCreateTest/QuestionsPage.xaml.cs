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

namespace dotNET_EXAM.Views.CRUD.Admin.AdminCreateTest
{
    public partial class QuestionsPage : UserControl
    {
        public QuestionsPage()
        {
            InitializeComponent();
        }

        public QuestionsPage(string TestName, string TestDescription)
        {
            InitializeComponent();
        }

        private void AddTextBox_Click(object sender, RoutedEventArgs e)
        {
            if (DynamicTextBoxes.Items.Count < 5)
            {
                TextBox newTextBox = new TextBox
                {
                    Style = (Style)FindResource("RoundedTextBox"),
                    Background = (Brush)FindResource("AccentColor"),
                    Height = 40,
                    Width = 600,
                    Margin = new Thickness(0, 10, 0, 0),
                    FontSize = 22,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontFamily = new FontFamily("Cascadia Code"),
                    Padding = new Thickness(15, 0, 15, 0)
                };

                DynamicTextBoxes.Items.Add(newTextBox);
            }
            else
            {
                AddTextBoxErrorLabel.Content = "Max 5 answers";
            }
        }
    }
}
