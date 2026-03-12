using PropertyAuction.Core.Models;
using PropertyAuction.DataStructures.BST;

namespace PropertyAuction.Services.Services;

/// <summary>
/// Service for managing bids using custom linked list
/// Time Complexity: PlaceBid O(1), GetBidHistory O(n), GetHighestBid O(n)
/// </summary>
public class BidService
{
    private readonly Dictionary<int, BidList> _auctionBids;
    private readonly AuctionBST _auctionTree;
    private int _nextBidId;
    
    public BidService(AuctionBST auctionTree)
    {
        _auctionBids = new Dictionary<int, BidList>();
        _auctionTree = auctionTree;
        _nextBidId = 1;
    }
    
    /// <summary>
    /// Place a bid on an auction
    /// Time Complexity: O(log n) for BST search + O(1) for linked list insert
    /// </summary>
    public (bool success, string message) PlaceBid(int auctionId, int bidderId, decimal amount)
    {
        // Find auction in BST
        var auction = _auctionTree.Search(auctionId);
        
        if (auction == null)
            return (false, "Auction not found");
            
        if (!auction.IsActive())
            return (false, "Auction is not active");
            
        if (amount <= auction.CurrentHighestBid)
            return (false, $"Bid must be higher than £{auction.CurrentHighestBid}");
            
        if (amount < auction.StartingPrice)
            return (false, $"Bid must be at least £{auction.StartingPrice}");
        
        // Create bid
        var bid = new Bid
        {
            BidId = _nextBidId++,
            AuctionId = auctionId,
            BidderId = bidderId,
            Amount = amount,
            Timestamp = DateTime.Now
        };
        
        // Add to bid history (linked list - O(1))
        if (!_auctionBids.ContainsKey(auctionId))
        {
            _auctionBids[auctionId] = new BidList();
        }
        
        _auctionBids[auctionId].Add(bid);
        
        // Update auction's current highest bid
        auction.CurrentHighestBid = amount;
        
        return (true, "Bid placed successfully!");
    }
    
    /// <summary>
    /// Get bid history for an auction
    /// Time Complexity: O(n) where n is number of bids
    /// </summary>
    public List<Bid> GetBidHistory(int auctionId)
    {
        if (_auctionBids.ContainsKey(auctionId))
        {
            return _auctionBids[auctionId].GetAll();
        }
        
        return new List<Bid>();
    }
    
    /// <summary>
    /// Get highest bid for an auction
    /// Time Complexity: O(n)
    /// </summary>
    public Bid? GetHighestBid(int auctionId)
    {
        if (_auctionBids.ContainsKey(auctionId))
        {
            return _auctionBids[auctionId].GetHighest();
        }
        
        return null;
    }
    
    /// <summary>
    /// Get all bids placed by a specific user
    /// Time Complexity: O(m * n) where m is auctions, n is bids per auction
    /// </summary>
    public List<Bid> GetUserBids(int bidderId)
    {
        var userBids = new List<Bid>();
        
        foreach (var bidList in _auctionBids.Values)
        {
            var bids = bidList.GetAll();
            userBids.AddRange(bids.Where(b => b.BidderId == bidderId));
        }
        
        return userBids;
    }
    
    /// <summary>
    /// Get bid count for an auction
    /// Time Complexity: O(1)
    /// </summary>
    public int GetBidCount(int auctionId)
    {
        if (_auctionBids.ContainsKey(auctionId))
        {
            return _auctionBids[auctionId].Count();
        }
        
        return 0;
    }
    public void LoadSampleBids()
    {
        PlaceBid(1, 101, 460000);
        PlaceBid(1, 102, 470000);
        PlaceBid(1, 103, 480000);
        PlaceBid(2, 101, 190000);
        PlaceBid(2, 104, 195000);
    }
}