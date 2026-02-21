using System;
using System.Net.Mail;
using System.Threading.Tasks;
using PropertyAuction.Core.Interfaces;
using PropertyAuction.Core.Models;
using PropertyAuction.Core.Enums;

namespace PropertyAuction.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Check if email is valid
        private bool IsValidEmail(string email)
        {
            try { new MailAddress(email); return true; }
            catch { return false; }
        }

        public async Task<User> RegisterAsync(string username, string email, string password, UserRole role = UserRole.Bidder)
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email address.");

            if (await _userRepository.GetByUsernameAsync(username) != null)
                throw new InvalidOperationException($"Username '{username}' is already taken.");

            if (await _userRepository.GetByEmailAsync(email) != null)
                throw new InvalidOperationException($"Email '{email}' is already registered.");

            var user = new User
            {
                Username = username,
                Email = email,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            user.SetPassword(password);
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User> LoginAsync(string usernameOrEmail, string password)
        {
            var user = usernameOrEmail.Contains("@")
                ? await _userRepository.GetByEmailAsync(usernameOrEmail)
                : await _userRepository.GetByUsernameAsync(usernameOrEmail);

            if (user == null)
                return null;

            if (user.IsLocked)
                throw new InvalidOperationException("Account is locked due to too many failed login attempts.");

            if (!user.VerifyPassword(password))
            {
                user.RecordFailedLogin();
                await _userRepository.UpdateAsync(user);
                return null;
            }

            user.ResetFailedLogin();
            await _userRepository.UpdateAsync(user);
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task LoadSampleUsersAsync()
        {
            await RegisterAsync("alice_bidder", "alice@example.com", "Password123!", UserRole.Bidder);
            await RegisterAsync("bob_seller", "bob@example.com", "Password123!", UserRole.Seller);
            await RegisterAsync("carol_admin", "carol@example.com", "Password123!", UserRole.Admin);
        }
    }
}
