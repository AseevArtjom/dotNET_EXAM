using Microsoft.AspNetCore.Identity;
using dotNET_EXAM.Models;
using dotNET_EXAM.Models.Db;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

public class UserManager
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }

    public async Task ChangePasswordAsync(User user, string newPassword)
    {
        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
        using (var context = new ProgramContext())
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddUserAsync(string username, string login, string password, Role role)
    {
        using (var context = new ProgramContext())
        {
            var user = new User { Username = username, Login = login };
            user.PasswordHash = HashPassword(user, password);

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var userRole = new UserRole { UserId = user.Id, RoleId = role.Id };

            await context.UserRoles.AddAsync(userRole);
            await context.SaveChangesAsync();
        }
    }
    public async Task<bool> CheckIfUserExistsAsync(string login)
    {
        using (var context = new ProgramContext())
        {
            return await context.Users.AnyAsync(u => u.Login == login);
        }
    }


    public async Task<ObservableCollection<Role>> GetRolesAsync()
    {
        using (var context = new ProgramContext())
        {
            var rolesList = await context.Roles.ToListAsync();

            return new ObservableCollection<Role>(rolesList);
        }
    }

}
