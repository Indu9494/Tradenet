using Moq;
using TradeNet11.Interfaces;
using TradeNet11.API.Controllers;
using TradeNet11.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this
using TradeNet11.API.DTOs;        // Add this for CompleteAuditRequest/CreateAuditRequest

namespace TradeNet11.Tests.Controllers
{
    public class AuditControllerTests
    {
        private readonly Mock<IAuditService> _mockAuditService;
        private readonly Mock<ILogger<AuditsController>> _mockLogger; // Added logger mock
        private readonly AuditsController _controller;

        public AuditControllerTests()
        {
            _mockAuditService = new Mock<IAuditService>();
            _mockLogger = new Mock<ILogger<AuditsController>>(); // Initialize logger mock

            // Fix for CS7036: Passing both dependencies to the constructor
            _controller = new AuditsController(_mockAuditService.Object, _mockLogger.Object);
        }

        #region Complete Tests

        [Fact]
        public async Task Complete_WithValidData_RedirectsToIndex()
        {
            // Arrange
            int auditId = 1;
            var request = new CompleteAuditRequest { Findings = "No critical issues found" }; // Fix for CS1503

            _mockAuditService.Setup(s => s.CompleteAuditAsync(auditId, request.Findings)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Complete(auditId, request);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AuditsController.Index), redirectResult.ActionName);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Create_Post_WithValidRequest_RedirectsToIndex()
        {
            // Arrange
            // Fix for CS1503: Using CreateAuditRequest instead of Audit model
            var request = new CreateAuditRequest { AuditTitle = "Annual Compliance Audit" };

            _mockAuditService.Setup(s => s.CreateAuditAsync(It.IsAny<Audit>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            // Fix for CS0103: Use nameof(AuditsController...) instead of AuditController
            Assert.Equal(nameof(AuditsController.Index), redirectResult.ActionName);
        }

        #endregion
    }
}