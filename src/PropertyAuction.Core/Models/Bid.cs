namespace PropertyAuction.Core.Models;

//Represents one bid on an auction
public class Bid
{
    public int BidId { get; set; }
    public int AuctionId { get; set; }
    public int BidderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    
    public Bid()
    {
        Timestamp = DateTime.Now;
    }
}

//Linked list for storing bid history
//add O(1), GetAll O(n), GetHighest O(n)
public class BidList
{
    private class Node
    {
        public Bid Data;
        public Node? Next;
        
        public Node(Bid bid)
        {
            Data = bid;
            Next = null;
        }
    }
    
    private Node? head;
    private int count;
    
    public BidList()
    {
        head = null;
        count = 0;
    }
    
    
    /// Add bid to front of list O(1)
   
    public void Add(Bid bid)
    {
        Node newNode = new Node(bid);
        newNode.Next = head;
        head = newNode;
        count++;
    }
    
    /// Get all bids  O(n)
    public List<Bid> GetAll()
    {
        List<Bid> result = new List<Bid>();
        Node? current = head;
        
        while (current != null)
        {
            result.Add(current.Data);
            current = current.Next;
        }
        
        return result;
    }
    
    /// Find highest bid O(n)
    public Bid? GetHighest()
    {
        if (head == null) return null;
        
        Bid highest = head.Data;
        Node? current = head.Next;
        
        while (current != null)
        {
            if (current.Data.Amount > highest.Amount)
            {
                highest = current.Data;
            }
            current = current.Next;
        }
        
        return highest;
    }
    
    public int Count() => count;
    public bool IsEmpty() => head == null;
}