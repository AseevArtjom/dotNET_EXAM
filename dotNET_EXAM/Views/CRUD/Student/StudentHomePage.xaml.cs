using dotNET_EXAM.Models.Db;
using dotNET_EXAM.Models;
using dotNET_EXAM.Services;
using dotNET_EXAM.Views.CRUD.Admin.AdminCreateTest;
using dotNET_EXAM.Views.CRUD.Admin;
using SweetAlertSharp.Enums;
using SweetAlertSharp;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;

namespace dotNET_EXAM.Views.CRUD.Student
{
    public partial class StudentHomePage : UserControl
    {
        public ObservableCollection<Test> TestsList { get; set; }
        public Test SelectedTest { get; set; }

        private int currentQuestionIndex = 0;
        private readonly Dictionary<int, List<int>> selectedAnswers = new();
        private int correctAnswersCount = 0;

        private DispatcherTimer timer;
        private TimeSpan elapsedTime;


        public StudentHomePage()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            UserLabel.Content = SessionManager.CurrentUser.Username;
            TestsList = LoadTestsFromDB();
            LVTest.ItemsSource = TestsList;
            DataContext = this;
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

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is ListViewItem item && item.DataContext is Test test)
            {
                SelectedTest = test;
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

        

    }
}
