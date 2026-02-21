using System;
using PropertyAuction.Core.Enums;

namespace PropertyAuction.Core.Models
{
	public class User
	{
		public int Id { get; set; }
		public required string Username { get; set; }
        public required string Email { get; set; }
		public required string PasswordHash { get; set; }

		// When the account was created
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// User roles (Bidder, Seller, Admin)
		public UserRole Role { get; set; } = UserRole.Bidder;

		// Security tracking
		public int FailedLoginAttempts { get; set; } = 0;
		public DateTime? LastLogin { get; set; }
		public bool IsLocked { get; set; } = false;

		private const int MaxFailedLoginAttempts = 5;

		// Hash password using a secure algorithm (bcrypt)
		public void SetPassword(string password)
		{
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
		}

		// Verify password
		public bool VerifyPassword(string password)
		{
			return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
		}

		// Record a failed login attempt and lock account if necessary
		public void RecordFailedLogin()
		{
			FailedLoginAttempts++;

			if (FailedLoginAttempts >= MaxFailedLoginAttempts)
			{
				IsLocked = true;
			}
		}

		// Reset failed login attempts after successful login
		public void ResetFailedLogin()
		{
			FailedLoginAttempts = 0;
			LastLogin = DateTime.UtcNow;
			IsLocked = false;
		}

		// Unlock account after time-based lockout (e.g., 15 minutes)
		public void UnlockAccount()
		{
			if (IsLocked && LastLogin.HasValue && (DateTime.UtcNow - LastLogin.Value).TotalMinutes >= 15)
			{
				IsLocked = false;
				FailedLoginAttempts = 0;
			}
		}
	}
}