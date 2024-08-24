using dotNET_EXAM.Models;
using dotNET_EXAM.Models.Db;
using dotNET_EXAM.Services;
using dotNET_EXAM.Views.CRUD.Admin.AdminCreateTest;
using Microsoft.EntityFrameworkCore;
using SweetAlertSharp;
using SweetAlertSharp.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace dotNET_EXAM.Views.CRUD.Admin
{
    public partial class AdminHomePage : UserControl
    {
        public ObservableCollection<User> UsersList { get; set; }
        public ObservableCollection<Test> TestsList { get; set; }
        public Test SelectedTest { get; set; }

        private int currentQuestionIndex = 0;
        private readonly Dictionary<int, List<int>> selectedAnswers = new();
        private int correctAnswersCount = 0;

        private DispatcherTimer timer;
        private TimeSpan elapsedTime;


        public AdminHomePage()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            UserLabel.Content = SessionManager.CurrentUser.Username;
            UsersList = LoadUsersFromDB();
            TestsList = LoadTestsFromDB();
            LVTest.ItemsSource =
            LVTest.ItemsSource = TestsList;
            DataContext = this;
        }

        private ObservableCollection<User> LoadUsersFromDB()
        {
            using var context = new ProgramContext();
            return new ObservableCollection<User>(context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToList());
        }

        private ObservableCollection<Test> LoadTestsFromDB()
        {
            using var context = new ProgramContext();
            return new ObservableCollection<Test>(context.Tests.Include(t => t.Questions).ThenInclude(q => q.Answers).ToList());
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            NavigatorObject.Switch(new LoginPage());
        }

        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                if (selectedUser.Id == SessionManager.CurrentUser.Id)
                {
                    SweetAlert.Show("Caution", "You can't delete your account", SweetAlertButton.OK, SweetAlertImage.INFORMATION);
                    return;
                }

                using var context = new ProgramContext();
                UsersList.Remove(selectedUser);
                context.Users.Remove(selectedUser);
                await context.SaveChangesAsync();

                UsersDataGrid.Items.Refresh();
                SweetAlert.Show("Success", "Successful user deletion", SweetAlertButton.OK, SweetAlertImage.SUCCESS);
            }
            else
            {
                SweetAlert.Show("Error", "Failed to delete user", SweetAlertButton.OK, SweetAlertImage.ERROR);
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is not User selectedUser) return;

            var resetPasswordWindow = new AdminResetPasswordWindow(selectedUser);
            resetPasswordWindow.ShowDialog();

            if (resetPasswordWindow.DialogResult == true)
            {
                SweetAlert.Show("Success", "Successful password change", SweetAlertButton.OK, SweetAlertImage.SUCCESS);
            }
            else
            {
                SweetAlert.Show("Error", "Failed to reset password", SweetAlertButton.OK, SweetAlertImage.ERROR);
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AdminAddUserWindow();
            addUserWindow.ShowDialog();

            if (addUserWindow.DialogResult == true)
            {
                SweetAlert.Show("Success", "Successful user addition", SweetAlertButton.OK, SweetAlertImage.SUCCESS);

                var newUsers = LoadUsersFromDB();
                foreach (var user in newUsers)
                {
                    if (!UsersList.Any(u => u.Id == user.Id))
                    {
                        UsersList.Add(user);
                    }
                }
            }
            else
            {
                SweetAlert.Show("Error", "Failed to add user", SweetAlertButton.OK, SweetAlertImage.ERROR);
            }
        }


        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is ListViewItem item && item.DataContext is Test test)
            {
                SelectedTest = test;
            }
        }

        private void SearchUsersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = SearchUserTextBox.Text.ToLower();

            var collectionView = CollectionViewSource.GetDefaultView(UsersDataGrid.ItemsSource);

            if (collectionView != null)
            {
                collectionView.Filter = item =>
                {
                    if (item is User user)
                    {
                        return user.Username.ToLower().Contains(filterText) ||
                               user.Login.ToLower().Contains(filterText);
                    }
                    return false;
                };
            }
        }


        // TEST
        private void PassTest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTest == null) return;

            LVTest.Visibility = Visibility.Collapsed;
            TestGrid.Visibility = Visibility.Visible;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            TestName.Text = SelectedTest.Name;
            currentQuestionIndex = 0;
            correctAnswersCount = 0;
            UpdateQuestion();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            TimeText.Text = $"Time: {elapsedTime.ToString(@"hh\:mm\:ss")}";
        }


        private void UpdateQuestion()
        {
            if (SelectedTest == null || SelectedTest.Questions.Count == 0) return;

            var currentQuestion = SelectedTest.Questions.ElementAt(currentQuestionIndex);
            QuestionCounter.Text = $"{currentQuestionIndex + 1} of {SelectedTest.Questions.Count}";
            TestQuestion.Text = currentQuestion.Text;

            QuestionOptionsStackPanel.Children.Clear();

            foreach (var answer in currentQuestion.Answers)
            {
                var button = new Button
                {
                    Content = answer.Text,
                    Height = 50,
                    Width = 900,
                    FontSize = 20,
                    Foreground = Brushes.White,
                    Background = FindResource("SecondaryColor") as Brush,
                    Margin = new Thickness(0, 10, 0, 0),
                    Tag = answer.Id
                };

                button.Click += OptionButton_Click;
                QuestionOptionsStackPanel.Children.Add(button);
            }
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button clickedButton || clickedButton.Tag is not int selectedAnswerId) return;

            var currentQuestion = SelectedTest.Questions.ElementAt(currentQuestionIndex);

            if (currentQuestion.Answers.Any(a => a.Id == selectedAnswerId && a.IsRight))
            {
                correctAnswersCount++;
            }

            if (currentQuestionIndex < SelectedTest.Questions.Count - 1)
            {
                currentQuestionIndex++;
                UpdateQuestion();
            }
            else
            {
                FinishTest_Click(null, null);
            }
        }

        private void FinishTest_Click(object sender, RoutedEventArgs e)
        {
            TestGrid.Visibility = Visibility.Collapsed;
            ResultGrid.Visibility = Visibility.Visible;

            ResultText.Text = $"Your result: {correctAnswersCount} of {SelectedTest.Questions.Count}";
            timer.Stop();
        }

        private void CloseResult_Click(object sender, RoutedEventArgs e)
        {
            ResultGrid.Visibility = Visibility.Collapsed;
            LVTest.Visibility = Visibility.Visible;
        }

        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            var AddTestWindow = new AddTestWindow();
            AddTestWindow.ShowDialog();

            if (AddTestWindow.DialogResult == true)
            {
                SweetAlert.Show("Success", "Successful test addition", SweetAlertButton.OK, SweetAlertImage.SUCCESS);

                
            }
            else
            {
                SweetAlert.Show("Error", "Failed to add test", SweetAlertButton.OK, SweetAlertImage.ERROR);
            }
        }

        private void DeleteTest_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = LVTest.SelectedItem as Test;

            if (selectedItem == null)
            {
                return;
            }

            var result = SweetAlert.Show("Delete test?", $"Are you sure you want to delete {selectedItem.Name} test?", SweetAlertButton.YesNo, SweetAlertImage.WARNING);

            if (result == SweetAlertResult.YES)
            {
                try
                {
                    using (var context = new ProgramContext())
                    {
                        context.Tests.Remove(selectedItem);
                        context.SaveChangesAsync();
                    }

                    TestsList.Remove(selectedItem);
                    SweetAlert.Show("Success", "Test successfully removed!", SweetAlertButton.OK, SweetAlertImage.SUCCESS);
                }
                catch (Exception ex)
                {
                    SweetAlert.Show("Error", $"Failed to remove test! {ex.Message}", SweetAlertButton.OK, SweetAlertImage.ERROR);
                }
            }
        }



    }

}

