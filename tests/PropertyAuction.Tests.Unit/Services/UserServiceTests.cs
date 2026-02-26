using System;
using System.Threading.Tasks;
using NUnit.Framework;
using PropertyAuction.Tests.Unit.Fakes;
using PropertyAuction.Core.Enums;
using PropertyAuction.Core.Services;

namespace PropertyAuction.Tests.Unit.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private FakeUserRepository _fakeRepo;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _fakeRepo = new FakeUserRepository();
            _userService = new UserService(_fakeRepo);
        }

        // RegisterAsync Tests

        [Test]
        public async Task RegisterAsync_ValidUser_ReturnsUser()
        {
            var result = await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo("alice"));
            Assert.That(result.Email, Is.EqualTo("alice@example.com"));
        }

        [Test]
        public async Task RegisterAsync_PasswordIsHashed()
        {
            var result = await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            Assert.That(result.PasswordHash, Is.Not.EqualTo("Password123!"));
            Assert.That(result.PasswordHash, Is.Not.Null);
        }

        [Test]
        public async Task RegisterAsync_DefaultRole_IsBidder()
        {
            var result = await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            Assert.That(result.Role, Is.EqualTo(UserRole.Bidder));
        }

        [Test]
        public async Task RegisterAsync_InvalidEmail_ThrowsArgumentException()
        {
            Assert.That(
                async () => await _userService.RegisterAsync("alice", "notanemail", "Password123!"),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public async Task RegisterAsync_DuplicateUsername_ThrowsException()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");
            Assert.That(
                async () => await _userService.RegisterAsync("alice", "other@example.com", "Password123!"),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public async Task RegisterAsync_DuplicateEmail_ThrowsException()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");
            Assert.That(
                async () => await _userService.RegisterAsync("bob", "alice@example.com", "Password123!"),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public async Task RegisterAsync_CanAssignSellerRole()
        {
            var result = await _userService.RegisterAsync("bob", "bob@example.com", "Password123!", UserRole.Seller);

            Assert.That(result.Role, Is.EqualTo(UserRole.Seller));
        }

        [Test]
        public async Task RegisterAsync_CanAssignAdminRole()
        {
            var result = await _userService.RegisterAsync("carol", "carol@example.com", "Password123!", UserRole.Admin);

            Assert.That(result.Role, Is.EqualTo(UserRole.Admin));
        }

        // LoginAsync Tests

        [Test]
        public async Task LoginAsync_ValidUsername_ReturnsUser()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            var result = await _userService.LoginAsync("alice", "Password123!");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo("alice"));
        }

        [Test]
        public async Task LoginAsync_ValidEmail_ReturnsUser()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            var result = await _userService.LoginAsync("alice@example.com", "Password123!");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("alice@example.com"));
        }

        [Test]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            var result = await _userService.LoginAsync("alice", "WrongPassword!");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task LoginAsync_WrongPassword_IncrementsFailedAttempts()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            await _userService.LoginAsync("alice", "WrongPassword!");
            await _userService.LoginAsync("alice", "WrongPassword!");

            var user = await _fakeRepo.GetByUsernameAsync("alice");
            Assert.That(user.FailedLoginAttempts, Is.EqualTo(2));
        }

        [Test]
        public async Task LoginAsync_UnknownUser_ReturnsNull()
        {
            var result = await _userService.LoginAsync("nobody", "Password123!");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task LoginAsync_SuccessfulLogin_ResetsFailedAttempts()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");
            await _userService.LoginAsync("alice", "WrongPassword!");

            await _userService.LoginAsync("alice", "Password123!");

            var user = await _fakeRepo.GetByUsernameAsync("alice");
            Assert.That(user.FailedLoginAttempts, Is.EqualTo(0));
        }

        [Test]
        public async Task LoginAsync_FiveFailedAttempts_LocksAccount()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            for (int i = 0; i < 5; i++)
                await _userService.LoginAsync("alice", "WrongPassword!");

            var user = await _fakeRepo.GetByUsernameAsync("alice");
            Assert.That(user.IsLocked, Is.True);
        }

        [Test]
        public async Task LoginAsync_LockedAccount_ThrowsException()
        {
            await _userService.RegisterAsync("alice", "alice@example.com", "Password123!");

            for (int i = 0; i < 5; i++)
                await _userService.LoginAsync("alice", "WrongPassword!");

            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.LoginAsync("alice", "Password123!"));
        }

        // LoadSampleUsersAsync Tests

        [Test]
        public async Task LoadSampleUsersAsync_CreatesAllThreeUsers()
        {
            await _userService.LoadSampleUsersAsync();

            var bidder = await _fakeRepo.GetByUsernameAsync("alice_bidder");
            var seller = await _fakeRepo.GetByUsernameAsync("bob_seller");
            var admin = await _fakeRepo.GetByUsernameAsync("carol_admin");

            Assert.That(bidder, Is.Not.Null);
            Assert.That(seller, Is.Not.Null);
            Assert.That(admin, Is.Not.Null);
        }

        [Test]
        public async Task LoadSampleUsersAsync_UsersHaveCorrectRoles()
        {
            await _userService.LoadSampleUsersAsync();

            var bidder = await _fakeRepo.GetByUsernameAsync("alice_bidder");
            var seller = await _fakeRepo.GetByUsernameAsync("bob_seller");
            var admin = await _fakeRepo.GetByUsernameAsync("carol_admin");

            Assert.That(bidder.Role, Is.EqualTo(UserRole.Bidder));
            Assert.That(seller.Role, Is.EqualTo(UserRole.Seller));
            Assert.That(admin.Role, Is.EqualTo(UserRole.Admin));
        }

        [Test]
        public async Task LoadSampleUsersAsync_CalledTwice_ThrowsException()
        {
            await _userService.LoadSampleUsersAsync();

            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.LoadSampleUsersAsync());
        }
    }
}