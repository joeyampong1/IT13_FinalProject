using System;
using System.Collections.Generic;
using System.Linq;

namespace IT_13FinalProject.Services
{
    public class UserAccount
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public interface IUserAccountService
    {
        bool UserExists(string userName);
        void AddUser(UserAccount user);
        UserAccount? ValidateUser(string userName, string password);
    }

    public class InMemoryUserAccountService : IUserAccountService
    {
        private readonly List<UserAccount> _users = new();

        public InMemoryUserAccountService()
        {
            _users.Add(new UserAccount { UserName = "Admin", Password = "12345", Role = "Admin" });
            _users.Add(new UserAccount { UserName = "Nurse", Password = "12345", Role = "Nurse" });
            _users.Add(new UserAccount { UserName = "Doctor", Password = "12345", Role = "Doctor" });
            _users.Add(new UserAccount { UserName = "Billing", Password = "12345", Role = "Billing Staff" });
            _users.Add(new UserAccount { UserName = "Inventory", Password = "12345", Role = "Inventory Staff" });
        }

        public bool UserExists(string userName)
        {
            return _users.Any(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase));
        }

        public void AddUser(UserAccount user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (UserExists(user.UserName))
            {
                throw new InvalidOperationException("User already exists.");
            }

            _users.Add(user);
        }

        public UserAccount? ValidateUser(string userName, string password)
        {
            return _users.FirstOrDefault(u =>
                string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }
    }
}
