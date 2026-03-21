using PropertyAuction.Core.Models;

namespace PropertyAuction.DataStructures.BST
{
    /// <summary>
    /// Custom Binary Search Tree implementation for storing Auctions
    /// Key: AuctionId (integer)
    /// Value: Auction object
    /// 
    /// Time complexity analysis:
    /// - Insert: O(log n) average, O(n) worst case
    /// - Search: O(log n) average, O(n) worst case
    /// - Delete: O(log n) average, O(n) worst case
    /// - InOrder Traversal: O(n)
    /// 
    /// Space complexity: O(n) where n is number of nodes
    /// </summary>
    public class AuctionBST
    {
        private class TreeNode
        {
            public Auction Data { get; set; }
            public TreeNode? Left { get; set; }
            public TreeNode? Right { get; set; }
            
            public TreeNode(Auction auction)
            {
                Data = auction;
                Left = null;
                Right = null;
            }
        }
        
        private TreeNode? root;
        private int size;
        
        public AuctionBST()
        {
            root = null;
            size = 0;
        }
        
        /// <summary>
        /// insert an auction into the BST
        /// Time Complexity: O(log n) average, O(n) worst case
        /// </summary>
        public void Insert(Auction auction)
        {
            bool isNew = Search(auction.AuctionId) == null;
            root = InsertRecursive(root, auction);
            if (isNew) size++;
        }
        
        private TreeNode InsertRecursive(TreeNode? node, Auction auction)
        {
            // Base case: found the insertion point
            if (node == null)
            {
                return new TreeNode(auction);
            }
            
            // Recursive case: traverse left or right
            if (auction.AuctionId < node.Data.AuctionId)
            {
                node.Left = InsertRecursive(node.Left, auction);
            }
            else if (auction.AuctionId > node.Data.AuctionId)
            {
                node.Right = InsertRecursive(node.Right, auction);
            }
            
            return node;
        }
        
        /// <summary>
        /// Search for an auction by ID
        /// Time Complexity: O(log n) average, O(n) worst case
        /// </summary>
        public Auction? Search(int auctionId)
        {
            return SearchRecursive(root, auctionId);
        }
        
        private Auction? SearchRecursive(TreeNode? node, int auctionId)
        {
            if (node == null)
            {
                return null;
            }
            
            if (auctionId == node.Data.AuctionId)
            {
                return node.Data;
            }
            
            if (auctionId < node.Data.AuctionId)
            {
                return SearchRecursive(node.Left, auctionId);
            }
            else
            {
                return SearchRecursive(node.Right, auctionId);
            }
        }
        
        /// <summary>
        /// Delete an auction from the BST
        /// Time Complexity: O(log n) average, O(n) worst case
        /// </summary>
        public bool Delete(int auctionId)
        {
            int initialSize = size;
            root = DeleteRecursive(root, auctionId);
            return size < initialSize;
        }
        
        private TreeNode? DeleteRecursive(TreeNode? node, int auctionId)
        {
            if (node == null)
            {
                return null;
            }
            
            if (auctionId < node.Data.AuctionId)
            {
                node.Left = DeleteRecursive(node.Left, auctionId);
            }
            else if (auctionId > node.Data.AuctionId)
            {
                node.Right = DeleteRecursive(node.Right, auctionId);
            }
            else
            {
                size--;
                
                // Case 1: No children
                if (node.Left == null && node.Right == null)
                {
                    return null;
                }
                
                // Case 2: One child
                if (node.Left == null)
                {
                    return node.Right;
                }
                if (node.Right == null)
                {
                    return node.Left;
                }
                
                // Case 3: Two children
                TreeNode successor = FindMin(node.Right);
                node.Data = successor.Data;
                node.Right = DeleteRecursive(node.Right, successor.Data.AuctionId);
            }
            
            return node;
        }
        
        private TreeNode FindMin(TreeNode node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }
        
        /// <summary>
        /// Get all auctions in sorted order (by ID)
        /// Time Complexity: O(n)
        /// </summary>
        public List<Auction> GetAllSorted()
        {
            List<Auction> result = new List<Auction>();
            InOrderTraversal(root, result);
            return result;
        }
        
        private void InOrderTraversal(TreeNode? node, List<Auction> result)
        {
            if (node == null) return;
            
            InOrderTraversal(node.Left, result);
            result.Add(node.Data);
            InOrderTraversal(node.Right, result);
        }
        
        /// <summary>
        /// Search auctions by price range
        /// Time Complexity: O(n)
        /// </summary>
        public List<Auction> SearchByPriceRange(decimal minPrice, decimal maxPrice)
        {
            List<Auction> results = new List<Auction>();
            SearchByPriceRangeRecursive(root, minPrice, maxPrice, results);
            return results;
        }
        
        private void SearchByPriceRangeRecursive(TreeNode? node, decimal minPrice, 
                                                  decimal maxPrice, List<Auction> results)
        {
            if (node == null) return;
            
            if (node.Data.CurrentHighestBid >= minPrice && 
                node.Data.CurrentHighestBid <= maxPrice)
            {
                results.Add(node.Data);
            }
            
            SearchByPriceRangeRecursive(node.Left, minPrice, maxPrice, results);
            SearchByPriceRangeRecursive(node.Right, minPrice, maxPrice, results);
        }
        
        /// <summary>
        /// Get size of the tree
        /// Time Complexity: O(1)
        /// </summary>
        public int Size()
        {
            return size;
        }
        
        /// <summary>
        /// Check if tree is empty
        /// Time Complexity: O(1)
        /// </summary>
        public bool IsEmpty()
        {
            return root == null;
        }
        
        /// <summary>
        /// Clear all nodes from the tree
        /// Time Complexity: O(1)
        /// </summary>
        public void Clear()
        {
            root = null;
            size = 0;
        }
    }
}