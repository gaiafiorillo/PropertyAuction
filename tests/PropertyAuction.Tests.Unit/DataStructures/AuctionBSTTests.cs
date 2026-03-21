using PropertyAuction.Core.Models;
using PropertyAuction.DataStructures.BST;

namespace PropertyAuction.Tests.Unit.DataStructures
{
    [TestFixture]
    public class AuctionBSTTests
    {
        private AuctionBST _bst;

        [SetUp]
        public void SetUp()
        {
            _bst = new AuctionBST();
        }

        private static Auction MakeAuction(int id, decimal currentHighestBid = 0m)
        {
            return new Auction
            {
                AuctionId = id,
                Title = $"Auction {id}",
                StartingPrice = 100_000m,
                CurrentHighestBid = currentHighestBid,
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = DateTime.Now.AddDays(5),
                Status = AuctionStatus.Active
            };
        }

        // Initial State
        
        [Test]
        public void BST_NewTree_IsEmpty()
        {
            Assert.That(_bst.IsEmpty(), Is.True);
        }

        [Test]
        public void BST_NewTree_SizeIsZero()
        {
            Assert.That(_bst.Size(), Is.EqualTo(0));
        }

        // Insert
        
        [Test]
        public void Insert_OneAuction_SizeIsOne()
        {
            _bst.Insert(MakeAuction(1));

            Assert.That(_bst.Size(), Is.EqualTo(1));
        }

        [Test]
        public void Insert_DuplicateId_SizeDoesNotIncrease()
        {
            _bst.Insert(MakeAuction(1));
            _bst.Insert(MakeAuction(1));

            Assert.That(_bst.Size(), Is.EqualTo(1));
        }

        // Search
        
        [Test]
        public void Search_ExistingId_ReturnsCorrectAuction()
        {
            _bst.Insert(MakeAuction(1));

            var result = _bst.Search(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AuctionId, Is.EqualTo(1));
        }

        [Test]
        public void Search_NonExistentId_ReturnsNull()
        {
            _bst.Insert(MakeAuction(1));

            Assert.That(_bst.Search(999), Is.Null);
        }

        [Test]
        public void Search_FindsNodesInBothSubtrees()
        {
            _bst.Insert(MakeAuction(10)); // root
            _bst.Insert(MakeAuction(5));  // left subtree
            _bst.Insert(MakeAuction(15)); // right subtree

            Assert.That(_bst.Search(5).AuctionId, Is.EqualTo(5));
            Assert.That(_bst.Search(15).AuctionId, Is.EqualTo(15));
        }

        // Delete
        
        [Test]
        public void Delete_NonExistentId_ReturnsFalse()
        {
            Assert.That(_bst.Delete(999), Is.False);
        }

        [Test]
        public void Delete_LeafNode_RemovesNodeAndDecreasesSize()
        {
            _bst.Insert(MakeAuction(10));
            _bst.Insert(MakeAuction(5));

            _bst.Delete(5);

            Assert.That(_bst.Search(5), Is.Null);
            Assert.That(_bst.Size(), Is.EqualTo(1));
        }

        [Test]
        public void Delete_NodeWithOneChild_TreeRemainsValid()
        {
            _bst.Insert(MakeAuction(10));
            _bst.Insert(MakeAuction(5));
            _bst.Insert(MakeAuction(3)); // child of 5

            _bst.Delete(5);

            Assert.That(_bst.Search(5), Is.Null);
            Assert.That(_bst.Search(3), Is.Not.Null);
            Assert.That(_bst.Search(10), Is.Not.Null);
        }

        [Test]
        public void Delete_NodeWithTwoChildren_TreeRemainsValid()
        {
            _bst.Insert(MakeAuction(10));
            _bst.Insert(MakeAuction(5));
            _bst.Insert(MakeAuction(15));
            _bst.Insert(MakeAuction(3));
            _bst.Insert(MakeAuction(7));

            _bst.Delete(5); // has two children: 3 and 7

            Assert.That(_bst.Search(5), Is.Null);
            Assert.That(_bst.Search(3), Is.Not.Null);
            Assert.That(_bst.Search(7), Is.Not.Null);
            Assert.That(_bst.Search(10), Is.Not.Null);
        }

        // GetAllSorted
        
        [Test]
        public void GetAllSorted_ReturnsSortedByAuctionIdAscending()
        {
            _bst.Insert(MakeAuction(3));
            _bst.Insert(MakeAuction(1));
            _bst.Insert(MakeAuction(2));

            var result = _bst.GetAllSorted();

            for (int i = 0; i < result.Count - 1; i++)
                Assert.That(result[i].AuctionId, Is.LessThan(result[i + 1].AuctionId));
        }

        // SearchByPriceRange
        
        [Test]
        public void SearchByPriceRange_ReturnsOnlyAuctionsWithinRange()
        {
            _bst.Insert(MakeAuction(1, currentHighestBid: 150_000m));
            _bst.Insert(MakeAuction(2, currentHighestBid: 300_000m));
            _bst.Insert(MakeAuction(3, currentHighestBid: 450_000m));

            var result = _bst.SearchByPriceRange(100_000m, 200_000m);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].AuctionId, Is.EqualTo(1));
        }

        [Test]
        public void SearchByPriceRange_InclusiveOfBoundaries_ReturnsMatching()
        {
            _bst.Insert(MakeAuction(1, currentHighestBid: 100_000m));
            _bst.Insert(MakeAuction(2, currentHighestBid: 200_000m));

            var result = _bst.SearchByPriceRange(100_000m, 200_000m);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        // Clear

        [Test]
        public void Clear_AfterInserts_TreeIsEmptyAndSizeIsZero()
        {
            _bst.Insert(MakeAuction(1));
            _bst.Insert(MakeAuction(2));

            _bst.Clear();

            Assert.That(_bst.IsEmpty(), Is.True);
            Assert.That(_bst.Size(), Is.EqualTo(0));
        }

        [Test]
        public void Clear_CanInsertAgainAfterClear()
        {
            _bst.Insert(MakeAuction(1));
            _bst.Clear();
            _bst.Insert(MakeAuction(2));

            Assert.That(_bst.Size(), Is.EqualTo(1));
            Assert.That(_bst.Search(2).AuctionId, Is.EqualTo(2));
        }
    }
}