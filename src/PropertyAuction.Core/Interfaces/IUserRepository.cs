using System;
using System.Threading.Tasks;

namespace PropertyAuction.Core.Interfaces
{
	using PropertyAuction.Core.Models;

	public interface IUserRepository
	{
		Task<User?> GetByEmailAsync(string email);
		Task<User?> GetByUsernameAsync(string username);
		Task AddAsync(User user);
		Task UpdateAsync(User user);
	}
}
