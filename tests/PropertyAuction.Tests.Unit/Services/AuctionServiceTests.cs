using PropertyAuction.Services.Services;

namespace PropertyAuction.Tests.Unit.Services
{
	[TestFixture]
	public class AuctionServiceTests
	{
		private AuctionService _auctionService;

		[SetUp]
		public void SetUp()
		{
			_auctionService = new AuctionService();
		}

		// GetAll Tests (Returns the sample data loaded in auction.cs)

		[Test]
		public void GetAll_ReturnsAtLeastFourAuctions()
		{
			var result = _auctionService.GetAll();
			Assert.That(result.Count, Is.GreaterThanOrEqualTo(4));
		}

		[Test]
		public void GetAll_ReturnsSortedByAuctionId()
		{
			var result = _auctionService.GetAll();
			for (int i = 0; i < result.Count - 1; i++)
				Assert.That(result[i].AuctionId, Is.LessThan(result[i + 1].AuctionId));
		}

		// AddAuction Tests

		[Test]
		public void AddAuction_NewAuction_AppearsInGetAll()
		{
			var auction = new Auction
			{
				AuctionId = 99,
				Title = "Test Property",
				StartingPrice = 100000,
				StartTime = DateTime.Now.AddDays(-1),
				EndTime = DateTime.Now.AddDays(5),
				Status = AuctionStatus.Active
			};

			_auctionService.AddAuction(auction);
			var result = _auctionService.GetAll();

			Assert.That(result.Any(a => a.AuctionId == 99), Is.True);
		}

		[Test]
		public void AddAuction_IncreasesTotalCount()
		{
			var countBefore = _auctionService.GetAll().Count;

			_auctionService.AddAuction(new Auction
			{
				AuctionId = 100,
				Title = "New Auction",
				StartingPrice = 50000,
				StartTime = DateTime.Now.AddDays(-1),
				EndTime = DateTime.Now.AddDays(3),
				Status = AuctionStatus.Active
			});

			Assert.That(_auctionService.GetAll().Count, Is.EqualTo(countBefore + 1));
		}

		// GetById Tests

		[Test]
		public void GetById_ExistingId_ReturnsCorrectAuction()
		{
			var result = _auctionService.GetById(1);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.AuctionId, Is.EqualTo(1));
			Assert.That(result.Title, Is.EqualTo("3 bed Victorian house"));
		}

		[Test]
		public void GetById_NonExistentId_ReturnsNull()
		{
			var result = _auctionService.GetById(999);
			Assert.That(result, Is.Null);
		}

		// GetActive Tests

		[Test]
		public void GetActive_ReturnsOnlyActiveAuctions()
		{
			var result = _auctionService.GetActive();
			Assert.That(result, Is.Not.Empty);
			Assert.That(result.All(a => a.Status == AuctionStatus.Active), Is.True);
		}

		[Test]
		public void GetActive_DoesNotReturnEndedAuctions()
		{
			_auctionService.AddAuction(new Auction
			{
				AuctionId = 101,
				Title = "Ended Auction",
				StartingPrice = 50000,
				StartTime = DateTime.Now.AddDays(-5),
				EndTime = DateTime.Now.AddDays(-1),
				Status = AuctionStatus.Ended
			});

			var result = _auctionService.GetActive();
			Assert.That(result.Any(a => a.AuctionId == 101), Is.False);
		}

		[Test]
		public void GetActive_DoesNotReturnCancelledAuctions()
		{
			_auctionService.AddAuction(new Auction
			{
				AuctionId = 102,
				Title = "Cancelled Auction",
				StartingPrice = 50000,
				StartTime = DateTime.Now.AddDays(-1),
				EndTime = DateTime.Now.AddDays(5),
				Status = AuctionStatus.Cancelled
			});

			var result = _auctionService.GetActive();
			Assert.That(result.Any(a => a.AuctionId == 102), Is.False);
		}

		// IsActive Tests

		[Test]
		public void IsActive_ActiveStatusWithinTimeRange_ReturnsTrue()
		{
			var auction = new Auction
			{
				AuctionId = 200,
				Status = AuctionStatus.Active,
				StartTime = DateTime.Now.AddDays(-1),
				EndTime = DateTime.Now.AddDays(1)
			};

			Assert.That(auction.IsActive(), Is.True);
		}

		[Test]
		public void IsActive_ActiveStatusButExpired_ReturnsFalse()
		{
			var auction = new Auction
			{
				AuctionId = 201,
				Status = AuctionStatus.Active,
				StartTime = DateTime.Now.AddDays(-5),
				EndTime = DateTime.Now.AddDays(-1)
			};

			Assert.That(auction.IsActive(), Is.False);
		}

		[Test]
		public void IsActive_ActiveStatusButNotStarted_ReturnsFalse()
		{
			var auction = new Auction
			{
				AuctionId = 202,
				Status = AuctionStatus.Active,
				StartTime = DateTime.Now.AddDays(1),
				EndTime = DateTime.Now.AddDays(5)
			};

			Assert.That(auction.IsActive(), Is.False);
		}

		[Test]
		public void IsActive_PendingStatus_ReturnsFalse()
		{
			var auction = new Auction
			{
				AuctionId = 203,
				Status = AuctionStatus.Pending,
				StartTime = DateTime.Now.AddDays(-1),
				EndTime = DateTime.Now.AddDays(5)
			};

			Assert.That(auction.IsActive(), Is.False);
		}

		// TO DO:
		// Add tests for PlaceBid when implemented
		// Add tests for UpdateAuction when implemented
		// Add tests for GetByCategory and GetByLocation when implemented
	}
}