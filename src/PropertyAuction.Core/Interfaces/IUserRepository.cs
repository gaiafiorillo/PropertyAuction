using System;

namespace PropertyAuction.Core.Interfaces
{
	using PropertyAuction.Core.Models;
	using System.Threading.Tasks;

	public interface IUserRepository
	{
		Task<User> GetByEmailAsync(string email);
		Task<User> GetByUsernameAsync(string username);
		Task AddAsync(User user);
	}
}
