using dotNET_EXAM.Models.Db;
using dotNET_EXAM.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace dotNET_EXAM.Views.CRUD.Admin.AdminCreateTest
{
    public partial class AddTestWindow : Window
    {
        public AddTestWindow()
        {
            InitializeComponent();
            TestNavigatorObject.pageSwitcher = this;
        }

        public AddTestWindow(bool isTestCreated)
        {
            if (isTestCreated)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                this.DialogResult = false;
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


        private async Task<bool> TestExistsAsync(string testName)
        {
            using (var context = new ProgramContext())
            {
                var lowerTestName = testName.ToLower();
                return await context.Tests.AnyAsync(t => t.Name.ToLower() == lowerTestName);
            }
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            TestNameErrorLabel.Content = "";
            TestDescriptionErrorLabel.Content = "";
            string testName = TestNameTextBox.Text.Trim();
            string testDescription = TestDescriptionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(testName))
            {
                TestNameErrorLabel.Content = "Required field";
                return;
            }
            else if (string.IsNullOrWhiteSpace(testDescription))
            {
                TestDescriptionErrorLabel.Content = "Required field";
                return;
            }
            else if (await TestExistsAsync(testName))
            {
                TestNameErrorLabel.Content = "A test with this name already exists";
                return;
            }
            else
            {
                var questionsPage = new QuestionsPage(testName, testDescription);
                questionsPage.TestCreated += OnTestCompleted;
                TestNavigatorObject.Switch(questionsPage);
            }
        }



        private void OnTestCompleted()
        {
            this.DialogResult = true;
            this.Close();
        }


    }
}
