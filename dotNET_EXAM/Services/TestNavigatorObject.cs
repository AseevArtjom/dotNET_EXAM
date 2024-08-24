using dotNET_EXAM.Views.CRUD.Admin.AdminCreateTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dotNET_EXAM.Services
{
    public class TestNavigatorObject
    {
        public static AddTestWindow? pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            pageSwitcher?.Navigate(newPage);
        }

        public static void Switch(UserControl newPage, object state)
        {
            pageSwitcher?.Navigate(newPage, state);
        }
    }
}
