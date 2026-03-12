using PropertyAuction.Core.Models;
using PropertyAuction.DataStructures.BST;

namespace PropertyAuction.Services.Services;

public class AdminService
{
    private readonly AuctionBST _tree;
    
    public AdminService(AuctionBST tree)
    {
        _tree = tree;
    }
    
    //// <summary>
    /// Get all pending auctions awaiting approval
    /// Time Complexity: O(n) - must check all auctions
    /// </summary>
    public List<Auction> GetPendingAuctions()
    {
        var all = _tree.GetAllSorted();
        Console.WriteLine($"AdminService: Total auctions in BST: {all.Count}");
    
        foreach (var a in all)
        {
            Console.WriteLine($"  Auction #{a.AuctionId}: {a.Title} - Status: {a.Status}");
        }
    
        var pending = all.Where(a => a.Status == AuctionStatus.Pending).ToList();
        Console.WriteLine($"AdminService: Found {pending.Count} pending auctions");
    
        return pending;
    }
    
    /// <summary>
    /// Approve an auction (changes status to Active)
    /// Time Complexity: O(log n) - BST search
    /// </summary>
    public bool ApproveAuction(int auctionId)
    {
        var auction = _tree.Search(auctionId);
        if (auction == null) return false;
        
        auction.Status = AuctionStatus.Active;
        auction.StartTime = DateTime.Now;
        return true;
    }
    
    /// <summary>
    /// Reject an auction
    /// Time Complexity: O(log n) - BST search
    /// </summary>
    public bool RejectAuction(int auctionId)
    {
        var auction = _tree.Search(auctionId);
        if (auction == null) return false;
        
        auction.Status = AuctionStatus.Rejected;
        return true;
    }
    
    /// <summary>
    /// Delete an auction completely
    /// Time Complexity: O(log n) - BST delete
    /// </summary>
    public bool DeleteAuction(int auctionId)
    {
        return _tree.Delete(auctionId);
    }
}