public class Auction
{
    // Basic Information
    public int AuctionId { get; set; }           // ( Unique number for this auction eg 1, 2, 3...)
    public string Title { get; set; }            
    public string Description { get; set; }      
    public string Location { get; set; }         
    public string Category { get; set; }         // (type of property like flat, house etc)
    
    // Money info
    public decimal StartingPrice { get; set; }      
    public decimal CurrentHighestBid { get; set; }  
    
    // auction time info
    public DateTime StartTime { get; set; }      
    public DateTime EndTime { get; set; }       
    
    // Status info
    public AuctionStatus Status { get; set; }    // active, ended, cancelled ..
    
    // Constructor to create new empty auction
    public Auction()
    {
        Title = "";
        Description = "";
        Location = "";
        Category = "";
        Status = AuctionStatus.Pending;
    }
    //checks if auction is currently active and returns true if its is and false otherwise
    
    public bool IsActive()
    {
        DateTime now = DateTime.Now;
        
        // auction is active if:
        // 1, status is Active AND
        // 2, current time is after start time AND
        // 3, current time is before end time
        return Status == AuctionStatus.Active && 
               now >= StartTime && 
               now <= EndTime;
    }
}


// The different states an auction can be in
public enum AuctionStatus
{
    Pending,    
    Active,     
    Ended,      
    Cancelled   
}