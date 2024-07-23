using dotNET_EXAM.Models;
using dotNET_EXAM.Models.Db;
using dotNET_EXAM.Models.Services;
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

namespace dotNET_EXAM
{
    public partial class MainWindow : Window
    {
        public UserManager userManager = new UserManager();
        public MainWindow()
        {
            InitializeComponent();
        }


        private async void Submit(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                LoginErrorLabel.Content = "Login is a required field";
                return;
            }
            else
            {
                LoginErrorLabel.Content = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordErrorLabel.Content = "Password is a required field";
                return;
            }
            else
            {
                PasswordErrorLabel.Content = string.Empty;
            }

            using (var context = new ProgramContext())
            {
                var currentUser = await context.FindUserByLoginAsync(login);

                if (currentUser == null)
                {
                    LoginErrorLabel.Content = "A user with this login doesn't exist";
                    return;
                }
                else
                {
                    LoginErrorLabel.Content = string.Empty;
                }

                if (!userManager.VerifyPassword(currentUser, password))
                {
                    PasswordErrorLabel.Content = "Invalid password";
                    return;
                }
                else
                {
                    PasswordErrorLabel.Content = string.Empty;
                }
            }
        }



        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }

        public void Navigate(UserControl nextPage, object state)
        {
            this.Content = nextPage;
            INavigator? s = nextPage as INavigator;
            if (s != null)
            {
                s.UtilizeStart(state);
            }
            else
            {
                throw new ArgumentException("NextPage is not Navigator: " + nextPage.Name.ToString());
            }
        }

    }
}
