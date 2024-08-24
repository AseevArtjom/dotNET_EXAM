using dotNET_EXAM.Models;
using dotNET_EXAM.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace dotNET_EXAM.Views.CRUD.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminAddUserWindow.xaml
    /// </summary>
    public partial class AdminAddUserWindow : Window
    {
        UserManager userManager = new UserManager();
        public ObservableCollection<Role> Roles { get; set; }
        public AdminAddUserWindow()
        {
            InitializeComponent();
            this.Left = 700;
            this.Top = 140;
            DataContext = this;
            LoadRolesAsync();
        }
        public async Task LoadRolesAsync()
        {
            using (var context = new ProgramContext())
            {
                var rolesFromDb = await context.Roles.ToListAsync();
                Roles = new ObservableCollection<Role>(rolesFromDb);
            }
        }

        private async void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            UserNameErrorLabel.Content = "";
            LoginErrorLabel.Content = "";
            PasswordErrorLabel.Content = "";
            ConfirmPasswordErrorLabel.Content = "";

            // Получение данных из полей ввода
            string username = UserNameTextBox.Text.Trim();
            string login = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();
            string confirmPassword = ConfirmPasswordTextBox.Text.Trim();
            Role selectedRole = RoleComboBox.SelectedItem as Role;

            // Проверка наличия ошибок в полях
            if (string.IsNullOrWhiteSpace(username))
            {
                UserNameErrorLabel.Content = "Required field";
                return;
            }
            if (string.IsNullOrWhiteSpace(login))
            {
                LoginErrorLabel.Content = "Required field";
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordErrorLabel.Content = "Required field";
                return;
            }
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ConfirmPasswordErrorLabel.Content = "Required field";
                return;
            }
            if (password != confirmPassword)
            {
                ConfirmPasswordErrorLabel.Content = "Confirm Password mismatch";
                return;
            }
            var userManager = new UserManager();
            if (await userManager.CheckIfUserExistsAsync(login))
            {
                LoginErrorLabel.Content = "User with this login already exists";
                return;
            }

            await userManager.AddUserAsync(username, login, password, selectedRole);
            this.DialogResult = true;
            this.Close();
        }



        // DESIGN
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserNameTextBox.Text))
            {
                UserNameWaterMarkedText.Visibility = Visibility.Visible;
            }
        }

        private void UserNameWaterMarkGotFocus(object sender, RoutedEventArgs e)
        {
            UserNameWaterMarkedText.Visibility = Visibility.Collapsed;
            UserNameTextBox.Visibility = Visibility.Visible;
            UserNameTextBox.Focus();
        }
        //

        private void LoginLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                LoginWaterMarkedText.Visibility = Visibility.Visible;
            }
        }

        private void LoginWaterMarkGotFocus(object sender, RoutedEventArgs e)
        {
            LoginWaterMarkedText.Visibility = Visibility.Collapsed;
            LoginTextBox.Visibility = Visibility.Visible;
            LoginTextBox.Focus();
        }
        //

        private void PasswordLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                PasswordWaterMarkedText.Visibility = Visibility.Visible;
            }
        }

        private void PasswordWaterMarkGotFocus(object sender, RoutedEventArgs e)
        {
            PasswordWaterMarkedText.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordTextBox.Focus();
        }

        //
        private void ConfirmPasswordLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ConfirmPasswordTextBox.Text))
            {
                ConfirmPasswordWaterMarkedText.Visibility = Visibility.Visible;
            }
        }

        private void ConfirmPasswordWaterMarkGotFocus(object sender, RoutedEventArgs e)
        {
            ConfirmPasswordWaterMarkedText.Visibility = Visibility.Collapsed;
            ConfirmPasswordTextBox.Visibility = Visibility.Visible;
            ConfirmPasswordTextBox.Focus();
        }
    }
}
