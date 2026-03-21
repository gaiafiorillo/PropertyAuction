using PropertyAuction.Core.Models;
using PropertyAuction.Core.Enums;
using PropertyAuction.DataStructures.HashTable;

namespace PropertyAuction.Tests.Unit.DataStructures
{
    [TestFixture]
    public class UserHashTableTests
    {
        private UserHashTable _hashTable;

        [SetUp]
        public void SetUp()
        {
            _hashTable = new UserHashTable();
        }

        private static User MakeUser(string username, string email = null)
        {
            return new User
            {
                Username = username,
                Email = email ?? $"{username}@example.com",
                Role = UserRole.Bidder
            };
        }

        // Initial State
        
        [Test]
        public void HashTable_NewTable_IsEmpty()
        {
            Assert.That(_hashTable.IsEmpty(), Is.True);
        }

        [Test]
        public void HashTable_NewTable_SizeIsZero()
        {
            Assert.That(_hashTable.Size(), Is.EqualTo(0));
        }

        // Insert
        
        [Test]
        public void Insert_OneUser_SizeIsOne()
        {
            _hashTable.Insert("alice", MakeUser("alice"));

            Assert.That(_hashTable.Size(), Is.EqualTo(1));
        }

        [Test]
        public void Insert_DuplicateUsername_UpdatesUserAndSizeUnchanged()
        {
            _hashTable.Insert("alice", MakeUser("alice", "alice@example.com"));
            _hashTable.Insert("alice", MakeUser("alice", "newalice@example.com"));

            Assert.That(_hashTable.Size(), Is.EqualTo(1));
            Assert.That(_hashTable.Search("alice").Email, Is.EqualTo("newalice@example.com"));
        }

        [Test]
        public void Insert_NullUsername_ThrowsArgumentException()
        {
            Assert.That(
                () => _hashTable.Insert(null, MakeUser("alice")),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Insert_EmptyUsername_ThrowsArgumentException()
        {
            Assert.That(
                () => _hashTable.Insert("", MakeUser("alice")),
                Throws.TypeOf<ArgumentException>());
        }

        // Search
        
        [Test]
        public void Search_ExistingUsername_ReturnsCorrectUser()
        {
            _hashTable.Insert("alice", MakeUser("alice"));

            var result = _hashTable.Search("alice");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo("alice"));
        }

        [Test]
        public void Search_NonExistentUsername_ReturnsNull()
        {
            Assert.That(_hashTable.Search("nobody"), Is.Null);
        }

        [Test]
        public void Search_IsCaseSensitive()
        {
            _hashTable.Insert("alice", MakeUser("alice"));

            Assert.That(_hashTable.Search("Alice"), Is.Null);
        }

        // Delete
        
        [Test]
        public void Delete_ExistingUsername_RemovesUserAndDecreasesSize()
        {
            _hashTable.Insert("alice", MakeUser("alice"));
            _hashTable.Insert("bob", MakeUser("bob"));

            _hashTable.Delete("alice");

            Assert.That(_hashTable.Search("alice"), Is.Null);
            Assert.That(_hashTable.Size(), Is.EqualTo(1));
        }

        [Test]
        public void Delete_NonExistentUsername_ReturnsFalse()
        {
            Assert.That(_hashTable.Delete("nobody"), Is.False);
        }

        [Test]
        public void Delete_OneOfManyUsers_OthersRemainSearchable()
        {
            _hashTable.Insert("alice", MakeUser("alice"));
            _hashTable.Insert("bob", MakeUser("bob"));
            _hashTable.Insert("carol", MakeUser("carol"));

            _hashTable.Delete("bob");

            Assert.That(_hashTable.Search("alice"), Is.Not.Null);
            Assert.That(_hashTable.Search("carol"), Is.Not.Null);
        }

        // Collision Handling and Resize
        
        [Test]
        public void Insert_BeyondLoadFactor_ResizesAndPreservesAllUsers()
        {
            // Default capacity 16, load factor 0.75 — resize triggers at 13 inserts
            for (int i = 1; i <= 15; i++)
                _hashTable.Insert($"user{i}", MakeUser($"user{i}"));

            for (int i = 1; i <= 15; i++)
                Assert.That(_hashTable.Search($"user{i}"), Is.Not.Null,
                    $"Expected user{i} to survive resize");
        }

        [Test]
        public void Insert_ManyUsers_SizeRemainsAccurate()
        {
            for (int i = 1; i <= 20; i++)
                _hashTable.Insert($"user{i}", MakeUser($"user{i}"));

            Assert.That(_hashTable.Size(), Is.EqualTo(20));
        }

        // Clear
       
        [Test]
        public void Clear_AfterInserts_TableIsEmptyAndSizeIsZero()
        {
            _hashTable.Insert("alice", MakeUser("alice"));
            _hashTable.Insert("bob", MakeUser("bob"));

            _hashTable.Clear();

            Assert.That(_hashTable.IsEmpty(), Is.True);
            Assert.That(_hashTable.Size(), Is.EqualTo(0));
        }

        [Test]
        public void Clear_CanInsertAgainAfterClear()
        {
            _hashTable.Insert("alice", MakeUser("alice"));
            _hashTable.Clear();
            _hashTable.Insert("bob", MakeUser("bob"));

            Assert.That(_hashTable.Size(), Is.EqualTo(1));
            Assert.That(_hashTable.Search("bob").Username, Is.EqualTo("bob"));
        }
    }
}