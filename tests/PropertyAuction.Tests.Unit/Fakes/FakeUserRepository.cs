using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyAuction.Core.Interfaces;
using PropertyAuction.Core.Models;

namespace PropertyAuction.Tests.Unit.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<User?> GetByUsernameAsync(string username)
        {
            var user = _users.FirstOrDefault(u =>
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult<User?>(user);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u =>
                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult<User?>(user);
        }

        public Task AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            var existing = _users.FirstOrDefault(u => u.Username == user.Username);
            if (existing != null)
            {
                _users.Remove(existing);
                _users.Add(user);
            }
            return Task.CompletedTask;
        }
    }
}