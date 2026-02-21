using System;
using System.Threading.Tasks;
using PropertyAuction.Core.Models;
using PropertyAuction.Core.Enums;

namespace PropertyAuction.Core.Interfaces
{
	public interface IUserService
	{
		Task<User> RegisterAsync(string username, string email, string password, UserRole role = UserRole.Bidder);
        // Login with either username or email
        Task<User> LoginAsync(string usernameOrEmail, string password);
		Task<User> GetByUsernameAsync(string username);
		Task LoadSampleUsersAsync();
    }
}