using dotNET_EXAM.Services;
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

namespace dotNET_EXAM.Views.CRUD.Student
{
    public partial class StudentHomePage : UserControl
    {
        public StudentHomePage()
        {
            InitializeComponent();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            NavigatorObject.Switch(new LoginPage());
        }
    }
}
