using Microsoft.EntityFrameworkCore;
using IT_13FinalProject.Data;
using IT_13FinalProject.Models;

namespace IT_13FinalProject.Services
{
    public class DatabaseUserAccountService : IUserAccountService
    {
        private readonly ApplicationDbContext _context;

        public DatabaseUserAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool UserExists(string userName)
        {
            return _context.Users.Any(u => string.Equals(u.Username, userName, StringComparison.OrdinalIgnoreCase));
        }

        public void AddUser(UserAccount user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (UserExists(user.UserName))
            {
                throw new InvalidOperationException("User already exists.");
            }

            var dbUser = new User
            {
                Username = user.UserName,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email ?? "",
                FullName = user.Name ?? ""
            };

            _context.Users.Add(dbUser);
            _context.SaveChanges();
        }

        public UserAccount? ValidateUser(string userName, string password)
        {
            var user = _context.Users.FirstOrDefault(u =>
                string.Equals(u.Username, userName, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user == null) return null;

            return new UserAccount
            {
                UserName = user.Username,
                Password = user.Password,
                Role = user.Role,
                Name = user.FullName,
                Email = user.Email
            };
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
    }
}
