using dotNET_EXAM.Services;
using System;
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


        private void Next_Click(object sender, RoutedEventArgs e)
        {
            TestNameErrorLabel.Content = "";
            TestDescriptionErrorLabel.Content = "";
            string TestName = TestNameTextBox.Text;
            string TestDescription = TestDescriptionTextBox.Text;
            if (string.IsNullOrWhiteSpace(TestName))
            {
                TestNameErrorLabel.Content = "Required field";
                return;

            }
            else if (string.IsNullOrWhiteSpace(TestDescription))
            {
                TestDescriptionErrorLabel.Content = "Required field";
                return;
            }
            else
            {
                TestNavigatorObject.Switch(new QuestionsPage(TestName,TestDescription));
            }
        }

        
    }
}
