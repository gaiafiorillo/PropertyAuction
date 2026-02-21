using PropertyAuction.Core.Models;

namespace PropertyAuction.DataStructures.HashTable
{
    /// <summary>
    /// Custom hash table implementation for storing Users
    /// Used separate chaining for collision resolution
    /// Key: Username (string)
    /// Value: User object
    /// 
    /// Time Complexity Analysis:
    /// - Insert: O(1) average, O(n) worst case
    /// - Search: O(1) average, O(n) worst case
    /// - Delete: O(1) average, O(n) worst case
    /// 
    /// Space Complexity: O(n) where n is number of users
    /// </summary>
    public class UserHashTable
    {
        private class HashNode
        {
            public string Key { get; set; }
            public User Value { get; set; }
            public HashNode? Next { get; set; }
            
            public HashNode(string key, User value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }
        
        private HashNode?[] buckets;
        private int capacity;
        private int size;
        private const double LOAD_FACTOR_THRESHOLD = 0.75;
        
        public UserHashTable(int initialCapacity = 16)
        {
            capacity = initialCapacity;
            buckets = new HashNode[capacity];
            size = 0;
        }
        
        /// <summary>
        /// Hash function to convert string key to array index
        /// Time Complexity: O(k) where k is length of key
        /// </summary>
        private int GetHashCode(string key)
        {
            if (string.IsNullOrEmpty(key))
                return 0;
            
            int hash = 0;
            int prime = 31;
            
            foreach (char c in key)
            {
                hash = hash * prime + c;
            }
            
            return Math.Abs(hash) % capacity;
        }
        
        /// <summary>
        /// Insert or update a user in the hash table
        /// Time Complexity: O(1) average, O(n) worst case
        /// </summary>
        public void Insert(string username, User user)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty");
            }
            
            // Check if we need to resize
            if ((double)size / capacity >= LOAD_FACTOR_THRESHOLD)
            {
                Resize();
            }
            
            int index = GetHashCode(username);
            HashNode? current = buckets[index];
            
            // Check if key already exists (update case)
            while (current != null)
            {
                if (current.Key == username)
                {
                    current.Value = user;
                    return;
                }
                current = current.Next;
            }
            
            // Insert new node at the beginning of the chain
            HashNode newNode = new HashNode(username, user);
            newNode.Next = buckets[index];
            buckets[index] = newNode;
            size++;
        }
        
        /// <summary>
        /// Search for a user by username
        /// Time Complexity: O(1) average, O(n) worst case
        /// </summary>
        public User? Search(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;
            
            int index = GetHashCode(username);
            HashNode? current = buckets[index];
            
            while (current != null)
            {
                if (current.Key == username)
                {
                    return current.Value;
                }
                current = current.Next;
            }
            
            return null;
        }
        
        /// <summary>
        /// Delete a user by username
        /// Time Complexity: O(1) average, O(n) worst case
        /// </summary>
        public bool Delete(string username)
        {
            if (string.IsNullOrEmpty(username))
                return false;
            
            int index = GetHashCode(username);
            HashNode? current = buckets[index];
            HashNode? previous = null;
            
            while (current != null)
            {
                if (current.Key == username)
                {
                    if (previous == null)
                    {
                        buckets[index] = current.Next;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }
                    size--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }
            
            return false;
        }
        
        /// <summary>
        /// Resize the hash table when load factor exceeds threshold
        /// Time Complexity: O(n)
        /// </summary>
        private void Resize()
        {
            int newCapacity = capacity * 2;
            HashNode?[] newBuckets = new HashNode[newCapacity];
            
            for (int i = 0; i < capacity; i++)
            {
                HashNode? current = buckets[i];
                while (current != null)
                {
                    HashNode? next = current.Next;
                    
                    int newIndex = Math.Abs(GetHashCodeForResize(current.Key, newCapacity));
                    
                    current.Next = newBuckets[newIndex];
                    newBuckets[newIndex] = current;
                    
                    current = next;
                }
            }
            
            buckets = newBuckets;
            capacity = newCapacity;
        }
        
        private int GetHashCodeForResize(string key, int newCapacity)
        {
            int hash = 0;
            int prime = 31;
            
            foreach (char c in key)
            {
                hash = hash * prime + c;
            }
            
            return Math.Abs(hash) % newCapacity;
        }
        
        /// <summary>
        /// Get all users as a list
        /// Time Complexity: O(n + m) where n is size, m is capacity
        /// </summary>
        public List<User> GetAllUsers()
        {
            List<User> result = new List<User>();
            
            for (int i = 0; i < capacity; i++)
            {
                HashNode? current = buckets[i];
                while (current != null)
                {
                    result.Add(current.Value);
                    current = current.Next;
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Check if a username exists
        /// Time Complexity: O(1) average
        /// </summary>
        public bool Contains(string username)
        {
            return Search(username) != null;
        }
        
        /// <summary>
        /// Get the number of users
        /// Time Complexity: O(1)
        /// </summary>
        public int Size()
        {
            return size;
        }
        
        /// <summary>
        /// Check if empty
        /// Time Complexity: O(1)
        /// </summary>
        public bool IsEmpty()
        {
            return size == 0;
        }
        
        /// <summary>
        /// Clear all users
        /// Time Complexity: O(m) where m is capacity
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < capacity; i++)
            {
                buckets[i] = null;
            }
            size = 0;
        }
        
        /// <summary>
        /// Get current load factor
        /// </summary>
        public double GetLoadFactor()
        {
            return (double)size / capacity;
        }
    }
}