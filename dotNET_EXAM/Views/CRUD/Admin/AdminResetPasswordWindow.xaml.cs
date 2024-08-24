using dotNET_EXAM.Models;
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

namespace dotNET_EXAM.Views.CRUD.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminResetPasswordWindow.xaml
    /// </summary>
    public partial class AdminResetPasswordWindow : Window
    {
        User SelectedUser;
        public AdminResetPasswordWindow(User selecteduser)
        {
            InitializeComponent();
            SelectedUser = selecteduser;
            this.Left = 500;
            this.Top = 300;
        }

        private void WaterMarkGotFocus(object sender, RoutedEventArgs e)
        {
            WaterMarkedText.Visibility = Visibility.Collapsed;
            NewPasswordTextBox.Visibility = Visibility.Visible;
            NewPasswordTextBox.Focus();
        }

        private void NewPasswordLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewPasswordTextBox.Text))
            {
                WaterMarkedText.Visibility = Visibility.Visible;
            }
        }

        private void ConfirmPasswordLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewPasswordTextBox.Text))
            {
                WaterMarkedText2.Visibility = Visibility.Visible;
            }
        }

        private void WaterMarkGotFocus2(object sender, RoutedEventArgs e)
        {
            WaterMarkedText2.Visibility = Visibility.Collapsed;
            ConfirmPasswordTextBox.Visibility = Visibility.Visible;
            ConfirmPasswordTextBox.Focus();
        }
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            NewPasswordErrorLabel.Content = "";
            ConfirmPasswordErrorLabel.Content = "";

            if (string.IsNullOrWhiteSpace(NewPasswordTextBox.Text))
            {
                NewPasswordErrorLabel.Content = "Required field";
                return;
            }
            else if (string.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Text))
            {
                ConfirmPasswordErrorLabel.Content = "Required field";
                return;
            }
            else if (NewPasswordTextBox.Text != ConfirmPasswordTextBox.Text)
            {
                ConfirmPasswordErrorLabel.Content = "Confirm password mismatch";
                return;
            }
            UserManager userManager = new UserManager();
            userManager.ChangePasswordAsync(SelectedUser,NewPasswordTextBox.Text);
            this.DialogResult = true;
            this.Close();
        }
    }
}
