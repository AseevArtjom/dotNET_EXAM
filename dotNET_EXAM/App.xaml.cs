using System.Linq;
using System.Windows;
using dotNET_EXAM.Models;
using dotNET_EXAM.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace dotNET_EXAM
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AddTestUser();
        }

        private void AddTestUser()
        {
            using (var context = new ProgramContext())
            {
                if (!context.Users.Any())
                {
                    var user = new User { Username = "testuser", Login = "testlogin" };
                    var userManager = new UserManager();
                    user.PasswordHash = userManager.HashPassword(user, "testpassword");
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
        }
    }
}
