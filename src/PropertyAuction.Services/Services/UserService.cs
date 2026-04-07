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
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
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
                CreatedAt = DateTime.UtcNow,
                IsVerified = false,
                VerificationCode = Guid.NewGuid().ToString("N").Substring(0,6)
            };

            user.SetPassword(password);
            await _userRepository.AddAsync(user);
            await _emailService.SendVerificationEmail(user.Email, user.VerificationCode);
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
            if (!user.IsVerified)
                throw new InvalidOperationException("Account not verified. Please check your email.");
            // Generates login 2fa code
            user.LoginCode = new Random().Next(100000, 999999).ToString();
            user.LoginCodeExpiry = DateTime.UtcNow.AddMinutes(5);

            await _userRepository.UpdateAsync(user);

            await _emailService.SendLoginCode(user.Email, user.LoginCode);

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

        public async Task<bool> VerifyRegistrationCodeAsync(string email, string code)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return false;



            if (user.VerificationCode != code)
                return false;

            user.IsVerified = true;
            user.VerificationCode = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> VerifyLoginCodeAsync(string email, string code)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return false;

            if (user.LoginCode != code || user.LoginCodeExpiry < DateTime.UtcNow)
                return false;

            user.LoginCode = null;
            user.LoginCodeExpiry = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
