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
        _nextAuctionId = 5;
        LoadSampleData();
        
        _bidService = new BidService(_tree);
        LoadSampleBids();
    }
    
    public int SubmitAuction(Auction auction)
    {
        auction.AuctionId = _nextAuctionId++;
        auction.Status = AuctionStatus.Pending;
        _tree.Insert(auction);
        Console.WriteLine($"Added auction #{auction.AuctionId} '{auction.Title}' with status Pending");
        return auction.AuctionId;
    }
    
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
            Description = "Charming Victorian terraced house with original period features, modern kitchen and a landscaped rear garden. Walking distance to transport links.",
            Location = "London",
            Category = "houses",
            Bedrooms = 3,
            Bathrooms = 1,
            ImageUrl = "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=600&q=80",
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
            Description = "Stylish 2 bedroom flat on the 8th floor with stunning city views, open plan living and a private balcony. Gym and concierge included.",
            Location = "Manchester",
            Category = "flats",
            Bedrooms = 2,
            Bathrooms = 1,
            ImageUrl = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=600&q=80",
            StartingPrice = 150000,
            CurrentHighestBid = 180000,
            StartTime = DateTime.Now.AddDays(-2),
            EndTime = DateTime.Now.AddDays(5),
            Status = AuctionStatus.Active
        });
        
        _tree.Insert(new Auction
        {
            AuctionId = 3,
            Title = "Commercial shop unit",
            Description = "Prime retail space on a busy high street in the city centre. Ideal for food, retail or office use. High footfall location.",
            Location = "Birmingham",
            Category = "shops",
            Bedrooms = 0,
            Bathrooms = 1,
            ImageUrl = "https://images.unsplash.com/photo-1604709177225-055f99402ea3?w=600&q=80",
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
            Description = " top floor penthouse with panoramic views across the city. Features 3 bedrooms, designer kitchen and a wraparound terrace.",
            Location = "London",
            Category = "flats",
            Bedrooms = 3,
            Bathrooms = 2,
            ImageUrl = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=600&q=80",
            StartingPrice = 800000,
            CurrentHighestBid = 850000,
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = DateTime.Now.AddDays(10),
            Status = AuctionStatus.Active
        });

        _tree.Insert(new Auction
        {
            AuctionId = 5,
            Title = "Semi detached family home",
            Description = "Spacious 4 bedroom semi detached house with a large garden, driveway and garage. Located in a quiet residential area close to good schools.",
            Location = "Birmingham",
            Category = "houses",
            Bedrooms = 4,
            Bathrooms = 2,
            ImageUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=600&q=80",
            StartingPrice = 250000,
            CurrentHighestBid = 265000,
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = DateTime.Now.AddDays(6),
            Status = AuctionStatus.Active
        });

        _tree.Insert(new Auction
        {
            AuctionId = 6,
            Title = "Studio Apartment City Centre",
            Description = "Compact and well designed studio apartment in a sought after location. Perfect for first time buyers or investors. Close to universities and transport.",
            Location = "Manchester",
            Category = "flats",
            Bedrooms = 0,
            Bathrooms = 1,
            ImageUrl = "https://images.unsplash.com/photo-1536376072261-38c75010e6c9?w=600&q=80",
            StartingPrice = 90000,
            CurrentHighestBid = 90000,
            StartTime = DateTime.Now.AddDays(-1),
            EndTime = DateTime.Now.AddDays(4),
            Status = AuctionStatus.Active
        });

        _nextAuctionId = 8;
    }
}