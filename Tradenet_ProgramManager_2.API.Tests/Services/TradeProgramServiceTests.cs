using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tradenet_ProgramManager_2.API.Services;
using Tradenet_ProgramManager_2.API.Repositories;
using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Models.ViewModels;

namespace Tradenet_ProgramManager_2.API.Tests.Services
{
    [TestFixture]
    public class TradeProgramServiceTests
    {
        private Mock<ITradeProgramRepository> _mockTradeRepo;
        private Mock<ITransactionRepository> _mockTransactionRepo;
        private TradeProgramService _service;

        [SetUp]
        public void Setup()
        {
            // ARRANGE: Initialize mocks for both repository dependencies
            _mockTradeRepo = new Mock<ITradeProgramRepository>();
            _mockTransactionRepo = new Mock<ITransactionRepository>();

            // ARRANGE: Inject mocked dependencies into service
            _service = new TradeProgramService(_mockTradeRepo.Object, _mockTransactionRepo.Object);
        }

        #region GetAllPrograms Tests

        [Test]
        public async Task GetAllPrograms_WithValidData_ReturnsAllPrograms()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Budget = 100000m, Status = "Active" },
                new TradeProgram { Id = 2, Title = "Program 2", Budget = 150000m, Status = "Active" },
                new TradeProgram { Id = 3, Title = "Program 3", Budget = 200000m, Status = "Inactive" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            // ACT
            var result = await _service.GetAllPrograms();

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(3));
            _mockTradeRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetAllPrograms_WithEmptyRepository_ReturnsEmpty()
        {
            // ARRANGE
            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<TradeProgram>());

            // ACT
            var result = await _service.GetAllPrograms();

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
            _mockTradeRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        #endregion

        #region GetProgramById Tests

        [Test]
        public async Task GetProgramById_WithValidId_ReturnsProgram()
        {
            // ARRANGE
            int programId = 1;
            var mockProgram = new TradeProgram
            {
                Id = 1,
                Title = "Test Program",
                Budget = 250000m,
                Status = "Active"
            };

            _mockTradeRepo.Setup(repo => repo.GetById(programId))
                .ReturnsAsync(mockProgram);

            // ACT
            var result = await _service.GetProgramById(programId);

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Test Program"));
            Assert.That(result.Budget, Is.EqualTo(250000m));
            _mockTradeRepo.Verify(repo => repo.GetById(programId), Times.Once);
        }

        [Test]
        public async Task GetProgramById_WithInvalidId_ReturnsNull()
        {
            // ARRANGE
            int invalidId = 999;
            _mockTradeRepo.Setup(repo => repo.GetById(invalidId))
                .ReturnsAsync((TradeProgram)null);

            // ACT
            var result = await _service.GetProgramById(invalidId);

            // ASSERT
            Assert.That(result, Is.Null);
            _mockTradeRepo.Verify(repo => repo.GetById(invalidId), Times.Once);
        }

        #endregion

        #region AddProgram Tests

        [Test]
        public async Task AddProgram_WithValidBudget_ReturnsTrueAndCallsRepository()
        {
            // ARRANGE
            var validProgram = new TradeProgram
            {
                Title = "Export Program A",
                Budget = 150000m,
                Status = "Active"
            };

            _mockTradeRepo.Setup(repo => repo.Add(It.IsAny<TradeProgram>()))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _service.AddProgram(validProgram);

            // ASSERT
            Assert.That(result, Is.True);
            _mockTradeRepo.Verify(repo => repo.Add(It.IsAny<TradeProgram>()), Times.Once);
        }

        [Test]
        public async Task AddProgram_WithZeroBudget_ReturnsTrueAndCallsRepository()
        {
            // ARRANGE
            var programWithZeroBudget = new TradeProgram
            {
                Title = "Zero Budget Program",
                Budget = 0m,
                Status = "Inactive"
            };

            _mockTradeRepo.Setup(repo => repo.Add(It.IsAny<TradeProgram>()))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _service.AddProgram(programWithZeroBudget);

            // ASSERT
            Assert.That(result, Is.True);
            _mockTradeRepo.Verify(repo => repo.Add(It.IsAny<TradeProgram>()), Times.Once);
        }

        [Test]
        public async Task AddProgram_WithNegativeBudget_ReturnsFalseAndDoesNotCallRepository()
        {
            // ARRANGE
            var invalidProgram = new TradeProgram
            {
                Title = "Invalid Program",
                Budget = -50000m,
                Status = "Active"
            };

            // ACT
            var result = await _service.AddProgram(invalidProgram);

            // ASSERT
            Assert.That(result, Is.False);
            _mockTradeRepo.Verify(repo => repo.Add(It.IsAny<TradeProgram>()), Times.Never);
        }

        [Test]
        public async Task AddProgram_WithLargeValidBudget_ReturnsTrueAndCallsRepository()
        {
            // ARRANGE
            var largeProgram = new TradeProgram
            {
                Title = "Large Budget Program",
                Budget = 999999999.99m,
                Status = "Active"
            };

            _mockTradeRepo.Setup(repo => repo.Add(It.IsAny<TradeProgram>()))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _service.AddProgram(largeProgram);

            // ASSERT
            Assert.That(result, Is.True);
            _mockTradeRepo.Verify(repo => repo.Add(It.IsAny<TradeProgram>()), Times.Once);
        }

        #endregion

        #region UpdateProgram Tests

        [Test]
        public async Task UpdateProgram_WithValidBudget_ReturnsTrueAndCallsRepository()
        {
            // ARRANGE
            var validProgram = new TradeProgram
            {
                Id = 1,
                Title = "Updated Program",
                Budget = 175000m,
                Status = "Active"
            };

            _mockTradeRepo.Setup(repo => repo.Update(It.IsAny<TradeProgram>()))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _service.UpdateProgram(validProgram);

            // ASSERT
            Assert.That(result, Is.True);
            _mockTradeRepo.Verify(repo => repo.Update(It.IsAny<TradeProgram>()), Times.Once);
        }

        [Test]
        public async Task UpdateProgram_WithZeroBudget_ReturnsTrueAndCallsRepository()
        {
            // ARRANGE
            var programWithZeroBudget = new TradeProgram
            {
                Id = 2,
                Title = "Zero Budget Update",
                Budget = 0m,
                Status = "Inactive"
            };

            _mockTradeRepo.Setup(repo => repo.Update(It.IsAny<TradeProgram>()))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _service.UpdateProgram(programWithZeroBudget);

            // ASSERT
            Assert.That(result, Is.True);
            _mockTradeRepo.Verify(repo => repo.Update(It.IsAny<TradeProgram>()), Times.Once);
        }

        [Test]
        public async Task UpdateProgram_WithNegativeBudget_ReturnsFalseAndDoesNotCallRepository()
        {
            // ARRANGE
            var invalidProgram = new TradeProgram
            {
                Id = 3,
                Title = "Invalid Update",
                Budget = -100000m,
                Status = "Active"
            };

            // ACT
            var result = await _service.UpdateProgram(invalidProgram);

            // ASSERT
            Assert.That(result, Is.False);
            _mockTradeRepo.Verify(repo => repo.Update(It.IsAny<TradeProgram>()), Times.Never);
        }

        #endregion

        #region DeleteProgram Tests

        [Test]
        public async Task DeleteProgram_WithValidId_CallsRepositoryDeleteOnce()
        {
            // ARRANGE
            int programId = 1;
            _mockTradeRepo.Setup(repo => repo.Delete(programId))
                .Returns(Task.CompletedTask);

            // ACT
            await _service.DeleteProgram(programId);

            // ASSERT
            _mockTradeRepo.Verify(repo => repo.Delete(programId), Times.Once);
        }

        #endregion

        #region GetDashboardData Tests

        [Test]
        public async Task GetDashboardData_WithValidPrograms_ReturnsCorrectMetrics()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Budget = 100000m, Status = "Active" },
                new TradeProgram { Id = 2, Title = "Program 2", Budget = 150000m, Status = "Active" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(500000m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(300000m);

            // ACT
            var result = await _service.GetDashboardData();

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalPrograms, Is.EqualTo(2));
            Assert.That(result.BudgetUsed, Is.EqualTo(250000m));
            Assert.That(result.TotalSales, Is.EqualTo(500000m));
            Assert.That(result.TotalPurchases, Is.EqualTo(300000m));
            Assert.That(result.NetBalance, Is.EqualTo(200000m));
            _mockTradeRepo.Verify(repo => repo.GetAll(), Times.Once);
            _mockTransactionRepo.Verify(repo => repo.GetTotalByType("Sale"), Times.Once);
            _mockTransactionRepo.Verify(repo => repo.GetTotalByType("Purchase"), Times.Once);
        }

        [Test]
        public async Task GetDashboardData_WithPositiveNetBalance_ReturnsExcellentHealth()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Budget = 50000m, Status = "Active" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(600000m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(400000m);

            // ACT
            var result = await _service.GetDashboardData();

            // ASSERT
            Assert.That(result.NetBalance, Is.GreaterThan(0));
            Assert.That(result.MarketHealth, Is.EqualTo("Excellent"));
        }

        [Test]
        public async Task GetDashboardData_WithZeroNetBalance_ReturnsGoodHealth()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Budget = 50000m, Status = "Active" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(500000m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(500000m);

            // ACT
            var result = await _service.GetDashboardData();

            // ASSERT
            Assert.That(result.NetBalance, Is.EqualTo(0));
            Assert.That(result.MarketHealth, Is.EqualTo("Good"));
        }

        [Test]
        public async Task GetDashboardData_WithNegativeNetBalance_ReturnsNeedsAttentionHealth()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Budget = 50000m, Status = "Active" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(300000m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(500000m);

            // ACT
            var result = await _service.GetDashboardData();

            // ASSERT
            Assert.That(result.NetBalance, Is.LessThan(0));
            Assert.That(result.MarketHealth, Is.EqualTo("Needs Attention"));
        }

        [Test]
        public async Task GetDashboardData_WithEmptyRepositories_ReturnsZeroMetrics()
        {
            // ARRANGE
            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<TradeProgram>());

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(0m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(0m);

            // ACT
            var result = await _service.GetDashboardData();

            // ASSERT
            Assert.That(result.TotalPrograms, Is.EqualTo(0));
            Assert.That(result.BudgetUsed, Is.EqualTo(0m));
            Assert.That(result.TotalSales, Is.EqualTo(0m));
            Assert.That(result.TotalPurchases, Is.EqualTo(0m));
            Assert.That(result.NetBalance, Is.EqualTo(0m));
            Assert.That(result.MarketHealth, Is.EqualTo("Good"));
        }

        #endregion

        #region GetTransactionData Tests

        [Test]
        public async Task GetTransactionData_WithMoreThan20Transactions_Returns20Items()
        {
            // ARRANGE
            var mockTransactions = new List<Transaction>();
            for (int i = 1; i <= 30; i++)
            {
                mockTransactions.Add(new Transaction
                {
                    Id = i,
                    Amount = 10000m * i,
                    Type = i % 2 == 0 ? "Sale" : "Purchase",
                    Date = DateTime.Now.AddDays(-i)
                });
            }

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(150000m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(150000m);

            _mockTransactionRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockTransactions);

            // ACT
            var result = await _service.GetTransactionData();

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SalesVolume, Is.EqualTo(150000m));
            Assert.That(result.PurchaseVolume, Is.EqualTo(150000m));
            Assert.That(result.RecentTransactions, Is.Not.Null);
            Assert.That(result.RecentTransactions.Count(), Is.EqualTo(20));
            _mockTransactionRepo.Verify(repo => repo.GetTotalByType("Sale"), Times.Once);
            _mockTransactionRepo.Verify(repo => repo.GetTotalByType("Purchase"), Times.Once);
            _mockTransactionRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetTransactionData_WithFewerThan20Transactions_ReturnsAllItems()
        {
            // ARRANGE
            var mockTransactions = new List<Transaction>
            {
                new Transaction { Id = 1, Amount = 50000m, Type = "Sale", Date = DateTime.Now },
                new Transaction { Id = 2, Amount = 30000m, Type = "Purchase", Date = DateTime.Now.AddDays(-1) },
                new Transaction { Id = 3, Amount = 45000m, Type = "Sale", Date = DateTime.Now.AddDays(-2) }
            };

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(95000m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(30000m);

            _mockTransactionRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockTransactions);

            // ACT
            var result = await _service.GetTransactionData();

            // ASSERT
            Assert.That(result.RecentTransactions.Count(), Is.EqualTo(3));
            Assert.That(result.SalesVolume, Is.EqualTo(95000m));
            Assert.That(result.PurchaseVolume, Is.EqualTo(30000m));
        }

        [Test]
        public async Task GetTransactionData_WithEmptyTransactions_ReturnsEmptyList()
        {
            // ARRANGE
            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Sale"))
                .ReturnsAsync(0m);

            _mockTransactionRepo.Setup(repo => repo.GetTotalByType("Purchase"))
                .ReturnsAsync(0m);

            _mockTransactionRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<Transaction>());

            // ACT
            var result = await _service.GetTransactionData();

            // ASSERT
            Assert.That(result.RecentTransactions, Is.Not.Null);
            Assert.That(result.RecentTransactions, Is.Empty);
            Assert.That(result.SalesVolume, Is.EqualTo(0m));
            Assert.That(result.PurchaseVolume, Is.EqualTo(0m));
        }

        #endregion

        #region HasNonCompliantPrograms Tests

        [Test]
        public async Task HasNonCompliantPrograms_WithNonCompliantProgram_ReturnsTrue()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Status = "Active" },
                new TradeProgram { Id = 2, Title = "Program 2", Status = "Non-Compliant" },
                new TradeProgram { Id = 3, Title = "Program 3", Status = "Inactive" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            // ACT
            var result = await _service.HasNonCompliantPrograms();

            // ASSERT
            Assert.That(result, Is.True);
            _mockTradeRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public async Task HasNonCompliantPrograms_WithoutNonCompliantProgram_ReturnsFalse()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Status = "Active" },
                new TradeProgram { Id = 2, Title = "Program 2", Status = "Inactive" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            // ACT
            var result = await _service.HasNonCompliantPrograms();

            // ASSERT
            Assert.That(result, Is.False);
            _mockTradeRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public async Task HasNonCompliantPrograms_CaseInsensitive_ReturnsTrue()
        {
            // ARRANGE
            var mockPrograms = new List<TradeProgram>
            {
                new TradeProgram { Id = 1, Title = "Program 1", Status = "non-compliant" },
                new TradeProgram { Id = 2, Title = "Program 2", Status = "ACTIVE" }
            };

            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockPrograms);

            // ACT
            var result = await _service.HasNonCompliantPrograms();

            // ASSERT
            Assert.That(result, Is.True);
            _mockTradeRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public async Task HasNonCompliantPrograms_WithEmptyRepository_ReturnsFalse()
        {
            // ARRANGE
            _mockTradeRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<TradeProgram>());

            // ACT
            var result = await _service.HasNonCompliantPrograms();

            // ASSERT
            Assert.That(result, Is.False);
            _mockTradeRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        #endregion
    }
}