public class Auction
{
    // Basic Information
    public int AuctionId { get; set; }
    public string Title { get; set; }            
    public string Description { get; set; }      
    public string Location { get; set; }         
    public string Category { get; set; }
    public string ImageUrl { get; set; }

    
    // Property details
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    
    // Money info
    public decimal StartingPrice { get; set; }      
    public decimal CurrentHighestBid { get; set; }  
    
    // auction time info
    public DateTime StartTime { get; set; }      
    public DateTime EndTime { get; set; }       
    
    // Status info
    public AuctionStatus Status { get; set; }
    
    // Constructor to create new empty auction
    public Auction()
    {
        Title = "";
        Description = "";
        Location = "";
        Category = "";
        Status = AuctionStatus.Pending;
        Bedrooms = 0;
        Bathrooms = 0;
        ImageUrl = "";

    }

    public bool IsActive()
    {
        DateTime now = DateTime.Now;
        return Status == AuctionStatus.Active && 
               now >= StartTime && 
               now <= EndTime;
    }
}

public enum AuctionStatus
{
    Pending,    
    Active, 
    Rejected,
    Ended,      
    Cancelled   
}