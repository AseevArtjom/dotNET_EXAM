using dotNET_EXAM.Models;
using dotNET_EXAM.Models.Db;
using dotNET_EXAM.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dotNET_EXAM.Views.CRUD.Admin.AdminCreateTest
{
    public partial class QuestionsPage : UserControl
    {
        public event Action TestCreated;

        private ObservableCollection<Question> Questions = new ObservableCollection<Question>();

        private string TestName;
        private string TestDescription;
        public QuestionsPage(string testname,string testDescription)
        {
            InitializeComponent();
            TestName = testname;
            TestDescription = testDescription;
        }

        public QuestionsPage()
        {
            InitializeComponent();
        }

        private void AddTextBox_Click(object sender, RoutedEventArgs e)
        {
            if (DynamicTextBoxes.Items.Count < 5)
            {
                StackPanel panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                TextBox newTextBox = new TextBox
                {
                    Style = (Style)FindResource("RoundedTextBox"),
                    Background = (Brush)FindResource("AccentColor"),
                    Height = 40,
                    Width = 600,
                    Margin = new Thickness(70, 10, 10, 0),
                    FontSize = 22,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontFamily = new FontFamily("Cascadia Code"),
                    Padding = new Thickness(15, 0, 15, 0),
                };

                Button newButton = new Button
                {
                    Height = 30,
                    Width = 30,
                    Background = Brushes.Red,
                    Margin = new Thickness(0, 10, 0, 0),
                    Tag = "Unselected"
                };

                newButton.SetValue(ButtonAssist.CornerRadiusProperty, new CornerRadius(50));
                newButton.Click += SelectAnswerButton_Click;

                panel.Children.Add(newTextBox);
                panel.Children.Add(newButton);

                DynamicTextBoxes.Items.Add(panel);
            }
            else
            {
                AnswersErrorLabel.Content = "Max 5 answers";
            }
        }

        private void SelectAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in DynamicTextBoxes.Items)
            {
                if (item is StackPanel panel)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is Button button)
                        {
                            button.Background = Brushes.Red;
                            button.Tag = "Unselected";
                        }
                    }
                }
            }

            Button clickedButton = sender as Button;
            clickedButton.Background = Brushes.Green;
            clickedButton.Tag = "Selected";
        }

        private bool CheckAnswersIsNullOrWhiteSpaces()
        {
            foreach (var item in DynamicTextBoxes.Items)
            {
                if (item is StackPanel panel)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is TextBox textBox)
                        {
                            if (string.IsNullOrWhiteSpace(textBox.Text))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            QuestionErrorLabel.Content = "";
            AnswersErrorLabel.Content = "";

            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
            {
                QuestionErrorLabel.Content = "Required field";
            }
            else if (DynamicTextBoxes.Items.Count < 2)
            {
                AnswersErrorLabel.Content = "Minimum 2 answers";
            }
            else if (CheckAnswersIsNullOrWhiteSpaces())
            {
                AnswersErrorLabel.Content = "Fill out all the answers";
            }
            else
            {
                bool isRightAnswerSelected = false;
                ObservableCollection<Answer> answers = new ObservableCollection<Answer>();

                foreach (var item in DynamicTextBoxes.Items)
                {
                    if (item is StackPanel panel)
                    {
                        string answerText = string.Empty;
                        bool IsRight = false;

                        foreach (var child in panel.Children)
                        {
                            if (child is TextBox textBox)
                            {
                                answerText = textBox.Text.Trim();
                            }
                            else if (child is Button button && button.Tag.ToString() == "Selected")
                            {
                                IsRight = true;
                                isRightAnswerSelected = true;
                            }
                        }

                        answers.Add(new Answer
                        {
                            Text = answerText,
                            IsRight = IsRight
                        });
                    }
                }

                if (!isRightAnswerSelected)
                {
                    AnswersErrorLabel.Content = "Select the correct answer";
                }
                else
                {
                    Question newQuestion = new Question
                    {
                        Text = QuestionTextBox.Text.Trim(),
                        Answers = answers
                    };

                    Questions.Add(newQuestion);

                    QuestionTextBox.Clear();
                    DynamicTextBoxes.Items.Clear();
                    QuestionErrorLabel.Content = "";
                    AnswersErrorLabel.Content = "";
                }
            }
        }

        private async void Complete_Click(object sender, RoutedEventArgs e)
        {
            QuestionErrorLabel.Content = "";

            if (Questions.Count < 1)
            {
                QuestionErrorLabel.Content = "Minimum 1 question";
            }
            else
            {
                using (var context = new ProgramContext())
                {
                    Test newTest = new Test
                    {
                        Name = TestName,
                        Description = TestDescription,
                        Questions = new ObservableCollection<Question>(Questions)
                    };
                    await context.Tests.AddAsync(newTest);
                    await context.Questions.AddRangeAsync(Questions);
                    await context.SaveChangesAsync();
                }

                Questions.Clear();
                TestCreated?.Invoke();
            }
        }

    }
}
