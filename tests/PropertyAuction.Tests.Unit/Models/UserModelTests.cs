using PropertyAuction.Core.Models;
using PropertyAuction.Core.Enums;

namespace PropertyAuction.Tests.Unit.Models
{
    [TestFixture]
    public class UserModelTests
    {
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _user = new User
            {
                Username = "alice",
                Email = "alice@example.com"
            };
        }

        // Default State
        
        [Test]
        public void User_DefaultRole_IsBidder()
        {
            Assert.That(_user.Role, Is.EqualTo(UserRole.Bidder));
        }

        [Test]
        public void User_DefaultState_NotLockedAndNoFailedAttempts()
        {
            Assert.That(_user.IsLocked, Is.False);
            Assert.That(_user.FailedLoginAttempts, Is.EqualTo(0));
        }

        // SetPassword / VerifyPassword
        
        [Test]
        public void SetPassword_DoesNotStorePasswordInPlainText()
        {
            _user.SetPassword("Password123!");

            Assert.That(_user.PasswordHash, Is.Not.EqualTo("Password123!"));
            Assert.That(_user.PasswordHash, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            _user.SetPassword("Password123!");

            Assert.That(_user.VerifyPassword("Password123!"), Is.True);
        }

        [Test]
        public void VerifyPassword_WrongPassword_ReturnsFalse()
        {
            _user.SetPassword("Password123!");

            Assert.That(_user.VerifyPassword("WrongPassword!"), Is.False);
        }

        // RecordFailedLogin
        
        [Test]
        public void RecordFailedLogin_FourAttempts_AccountNotYetLocked()
        {
            for (int i = 0; i < 4; i++)
                _user.RecordFailedLogin();

            Assert.That(_user.IsLocked, Is.False);
        }

        [Test]
        public void RecordFailedLogin_FiveAttempts_LocksAccount()
        {
            for (int i = 0; i < 5; i++)
                _user.RecordFailedLogin();

            Assert.That(_user.IsLocked, Is.True);
            Assert.That(_user.FailedLoginAttempts, Is.EqualTo(5));
        }

        // ResetFailedLogin
        
        [Test]
        public void ResetFailedLogin_ResetsAttemptsAndUnlocksAccount()
        {
            for (int i = 0; i < 5; i++)
                _user.RecordFailedLogin();

            _user.ResetFailedLogin();

            Assert.That(_user.IsLocked, Is.False);
            Assert.That(_user.FailedLoginAttempts, Is.EqualTo(0));
        }

        [Test]
        public void ResetFailedLogin_SetsLastLoginToUtcNow()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            _user.ResetFailedLogin();
            var after = DateTime.UtcNow.AddSeconds(1);

            Assert.That(_user.LastLogin, Is.InRange(before, after));
        }

        // UnlockAccount
        
        [Test]
        public void UnlockAccount_Before15Minutes_RemainsLocked()
        {
            _user.IsLocked = true;
            _user.FailedLoginAttempts = 5;
            _user.LastLogin = DateTime.UtcNow.AddMinutes(-5);

            _user.UnlockAccount();

            Assert.That(_user.IsLocked, Is.True);
        }

        [Test]
        public void UnlockAccount_After15Minutes_UnlocksAndResetsAttempts()
        {
            _user.IsLocked = true;
            _user.FailedLoginAttempts = 5;
            _user.LastLogin = DateTime.UtcNow.AddMinutes(-20);

            _user.UnlockAccount();

            Assert.That(_user.IsLocked, Is.False);
            Assert.That(_user.FailedLoginAttempts, Is.EqualTo(0));
        }
    }
}