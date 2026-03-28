using System.Threading.Tasks;
using PropertyAuction.Core.Interfaces;
using PropertyAuction.Core.Models;
using PropertyAuction.DataStructures.HashTable;

namespace PropertyAuction.DataStructures
{
    public class UserRepository : IUserRepository
    {
        private readonly UserHashTable _hashTable = new UserHashTable();

        public Task<User?> GetByUsernameAsync(string username)
        {
            return Task.FromResult(_hashTable.Search(username));
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            // Hash table is keyed by username, so scan all users for email
            var all = _hashTable.GetAllUsers();
            var user = all.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task AddAsync(User user)
        {
            _hashTable.Insert(user.Username, user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            // Insert handles updates (overwrites if key exists)
            _hashTable.Insert(user.Username, user);
            return Task.CompletedTask;
        }
    }
}