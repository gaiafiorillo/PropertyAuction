using PropertyAuction.Core.Models;
using PropertyAuction.DataStructures.BST;

namespace PropertyAuction.Services.Services;

public class AuctionService
{
    private readonly AuctionBST _tree;
    private BidService? _bidService;
    private int _nextAuctionId;
    
    public AuctionService()
    {
        _tree = new AuctionBST();
        _nextAuctionId = 5; // Start after sample data
        LoadSampleData();
        
        _bidService = new BidService(_tree);
        LoadSampleBids();
    }
    
    // Add this method
    public int SubmitAuction(Auction auction)
    {
        auction.AuctionId = _nextAuctionId++;
        auction.Status = AuctionStatus.Pending;
        _tree.Insert(auction);
        Console.WriteLine($"Added auction #{auction.AuctionId} '{auction.Title}' with status Pending");
        return auction.AuctionId;
    }
    
    // Update GetActive to only show Active auctions
    public List<Auction> GetActive()
    {
        return GetAll().Where(a => a.Status == AuctionStatus.Active).ToList();
    }

    
    
    public void AddAuction(Auction auction) => _tree.Insert(auction);
    
    public Auction? GetById(int id) => _tree.Search(id);
    
    public List<Auction> GetAll() => _tree.GetAllSorted();
    
    public AuctionBST GetTree() => _tree;
    
    public BidService GetBidService() => _bidService!; 
    
    private void LoadSampleBids()
    {
        _bidService!.PlaceBid(1, 101, 460000);
        _bidService.PlaceBid(1, 102, 470000);
        _bidService.PlaceBid(1, 103, 480000);
        _bidService.PlaceBid(2, 104, 190000);
        _bidService.PlaceBid(2, 105, 195000);
    }
    
    private void LoadSampleData()
    {
        _tree.Insert(new Auction
        {
            AuctionId = 1,
            Title = "3 bed Victorian house",
            Description = "Beautiful property in prime location",
            Location = "London",
            Category = "houses",
            StartingPrice = 400000,
            CurrentHighestBid = 450000,
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = DateTime.Now.AddDays(7),
            Status = AuctionStatus.Active
        });
        
        _tree.Insert(new Auction
        {
            AuctionId = 2,
            Title = "Modern city flat",
            Description = "2 bed flat with stunning views",
            Location = "Manchester",
            Category = "flats",
            StartingPrice = 150000,
            CurrentHighestBid = 180000,
            StartTime = DateTime.Now.AddDays(-2),
            EndTime = DateTime.Now.AddDays(5),
            Status = AuctionStatus.Active
        });
        
        _tree.Insert(new Auction
        {
            AuctionId = 3,
            Title = "Commercial shop",
            Description = "Prime retail space in city center",
            Location = "Birmingham",
            Category = "shops",
            StartingPrice = 300000,
            CurrentHighestBid = 320000,
            StartTime = DateTime.Now.AddDays(-3),
            EndTime = DateTime.Now.AddDays(3),
            Status = AuctionStatus.Active
        });
        
        _tree.Insert(new Auction
        {
            AuctionId = 4,
            Title = "Luxury penthouse",
            Description = "Top floor with panoramic views",
            Location = "London",
            Category = "flats",
            StartingPrice = 800000,
            CurrentHighestBid = 850000,
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = DateTime.Now.AddDays(10),
            Status = AuctionStatus.Active
        });
    }
}