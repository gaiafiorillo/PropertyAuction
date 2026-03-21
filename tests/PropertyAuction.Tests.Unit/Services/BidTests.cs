using PropertyAuction.Core.Models;
using PropertyAuction.DataStructures.BST;
using PropertyAuction.Services.Services;

namespace PropertyAuction.Tests.Unit.Services
{
    [TestFixture]
    public class BidTests
    {
        private AuctionBST _auctionTree;
        private BidService _bidService;

        [SetUp]
        public void SetUp()
        {
            _auctionTree = new AuctionBST();
            _bidService = new BidService(_auctionTree);

            _auctionTree.Insert(new Auction
            {
                AuctionId = 1,
                Title = "3 bed Victorian house",
                StartingPrice = 450_000m,
                CurrentHighestBid = 0,
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = DateTime.Now.AddDays(6),
                Status = AuctionStatus.Active
            });

            _auctionTree.Insert(new Auction
            {
                AuctionId = 2,
                Title = "City centre flat",
                StartingPrice = 185_000m,
                CurrentHighestBid = 0,
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = DateTime.Now.AddDays(4),
                Status = AuctionStatus.Active
            });
        }

        // BidList Core Behaviour
        
        [Test]
        public void BidList_NewList_IsEmptyAndCountIsZero()
        {
            var list = new BidList();

            Assert.That(list.IsEmpty(), Is.True);
            Assert.That(list.Count(), Is.EqualTo(0));
        }

        [Test]
        public void BidList_AfterAddMultipleBids_CountMatchesNumberAdded()
        {
            var list = new BidList();
            list.Add(new Bid { BidId = 1, Amount = 460_000m });
            list.Add(new Bid { BidId = 2, Amount = 470_000m });
            list.Add(new Bid { BidId = 3, Amount = 480_000m });

            Assert.That(list.Count(), Is.EqualTo(3));
        }

        [Test]
        public void BidList_GetAll_ReturnsMostRecentlyAddedBidFirst()
        {
            // Add is O(1) prepend so last-added appears at index 0
            var list = new BidList();
            var bid1 = new Bid { BidId = 1, Amount = 460_000m };
            var bid2 = new Bid { BidId = 2, Amount = 470_000m };
            list.Add(bid1);
            list.Add(bid2);

            var all = list.GetAll();

            Assert.That(all[0], Is.EqualTo(bid2));
            Assert.That(all[1], Is.EqualTo(bid1));
        }

        [Test]
        public void BidList_GetHighest_ReturnsCorrectHighestBid()
        {
            var list = new BidList();
            list.Add(new Bid { BidId = 1, Amount = 460_000m });
            list.Add(new Bid { BidId = 2, Amount = 480_000m });
            list.Add(new Bid { BidId = 3, Amount = 470_000m });

            Assert.That(list.GetHighest().Amount, Is.EqualTo(480_000m));
        }

        [Test]
        public void BidList_GetHighest_EmptyList_ReturnsNull()
        {
            var list = new BidList();

            Assert.That(list.GetHighest(), Is.Null);
        }

        // PlaceBid Failures
        
        [Test]
        public void PlaceBid_AuctionNotFound_ReturnsFalseWithMessage()
        {
            var (success, message) = _bidService.PlaceBid(auctionId: 999, bidderId: 101, amount: 500_000m);

            Assert.That(success, Is.False);
            Assert.That(message, Does.Contain("not found").IgnoreCase);
        }

        [Test]
        public void PlaceBid_AuctionNotActive_ReturnsFalseWithMessage()
        {
            _auctionTree.Insert(new Auction
            {
                AuctionId = 3,
                StartingPrice = 100_000m,
                StartTime = DateTime.Now.AddDays(-5),
                EndTime = DateTime.Now.AddDays(-1),
                Status = AuctionStatus.Ended
            });

            var (success, message) = _bidService.PlaceBid(auctionId: 3, bidderId: 101, amount: 100_000m);

            Assert.That(success, Is.False);
            Assert.That(message, Does.Contain("not active").IgnoreCase);
        }

        [Test]
        public void PlaceBid_AmountBelowStartingPrice_ReturnsFalse()
        {
            var (success, _) = _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 100m);

            Assert.That(success, Is.False);
        }

        [Test]
        public void PlaceBid_AmountEqualToOrBelowCurrentHighest_ReturnsFalse()
        {
            _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 460_000m);

            var (equalResult, _) = _bidService.PlaceBid(auctionId: 1, bidderId: 102, amount: 460_000m);
            var (belowResult, _) = _bidService.PlaceBid(auctionId: 1, bidderId: 102, amount: 455_000m);

            Assert.That(equalResult, Is.False);
            Assert.That(belowResult, Is.False);
        }

        // PlaceBid Success
        
        [Test]
        public void PlaceBid_ValidBid_ReturnsTrueAndUpdatesCurrentHighest()
        {
            var (success, _) = _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 460_000m);

            var auction = _auctionTree.Search(1);
            Assert.That(success, Is.True);
            Assert.That(auction.CurrentHighestBid, Is.EqualTo(460_000m));
        }

        [Test]
        public void PlaceBid_SetsCorrectBidDetailsOnStoredBid()
        {
            _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 460_000m);

            var bid = _bidService.GetBidHistory(1).Single();
            Assert.That(bid.AuctionId, Is.EqualTo(1));
            Assert.That(bid.BidderId, Is.EqualTo(101));
            Assert.That(bid.Amount, Is.EqualTo(460_000m));
        }

        // GetBidHistory / GetHighestBid / GetBidCount
        
        [Test]
        public void GetBidHistory_NoAuctionBids_ReturnsEmptyList()
        {
            Assert.That(_bidService.GetBidHistory(999), Is.Empty);
        }

        [Test]
        public void GetBidHistory_DifferentAuctions_DoNotShareHistory()
        {
            _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 460_000m);
            _bidService.PlaceBid(auctionId: 2, bidderId: 101, amount: 190_000m);
            _bidService.PlaceBid(auctionId: 2, bidderId: 102, amount: 195_000m);

            Assert.That(_bidService.GetBidHistory(1).Count, Is.EqualTo(1));
            Assert.That(_bidService.GetBidHistory(2).Count, Is.EqualTo(2));
        }

        [Test]
        public void GetHighestBid_AfterSeveralBids_ReturnsHighestAmount()
        {
            _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 460_000m);
            _bidService.PlaceBid(auctionId: 1, bidderId: 102, amount: 470_000m);
            _bidService.PlaceBid(auctionId: 1, bidderId: 103, amount: 480_000m);

            Assert.That(_bidService.GetHighestBid(1).Amount, Is.EqualTo(480_000m));
        }

        [Test]
        public void GetBidCount_MatchesGetBidHistoryCount()
        {
            _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 460_000m);
            _bidService.PlaceBid(auctionId: 1, bidderId: 102, amount: 470_000m);

            Assert.That(_bidService.GetBidCount(1), Is.EqualTo(_bidService.GetBidHistory(1).Count));
        }

        // GetUserBids
        
        [Test]
        public void GetUserBids_ReturnsOnlyBidsForThatUserAcrossAllAuctions()
        {
            _bidService.PlaceBid(auctionId: 1, bidderId: 101, amount: 460_000m);
            _bidService.PlaceBid(auctionId: 2, bidderId: 101, amount: 190_000m);
            _bidService.PlaceBid(auctionId: 2, bidderId: 102, amount: 195_000m);

            var user101Bids = _bidService.GetUserBids(bidderId: 101);

            Assert.That(user101Bids.Count, Is.EqualTo(2));
            Assert.That(user101Bids.All(b => b.BidderId == 101), Is.True);
        }
    }
}