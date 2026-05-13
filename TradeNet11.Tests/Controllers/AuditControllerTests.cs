using Moq;
using TradeNet11.Controllers;
using TradeNet11.Interfaces;
using TradeNet11.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeNet11.Tests.Controllers
{
    public class AuditControllerTests
    {
        private readonly Mock<IAuditService> _mockAuditService;
        private readonly AuditController _controller;

        public AuditControllerTests()
        {
            _mockAuditService = new Mock<IAuditService>();
            _controller = new AuditController(_mockAuditService.Object);
        }

        #region Index Tests

        [Fact]
        public async Task Index_ReturnsViewResult_WithAuditList()
        {
            // Arrange
            var audits = new List<Audit>
            {
                new Audit { Id = 1, Status = "Pending" },
                new Audit { Id = 2, Status = "In Progress" }
            };
            _mockAuditService.Setup(s => s.GetAllAuditsAsync()).ReturnsAsync(audits);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            _mockAuditService.Verify(s => s.GetAllAuditsAsync(), Times.Once);
        }

        #endregion

        #region Details Tests

        [Fact]
        public async Task Details_WithValidId_ReturnsViewResult()
        {
            // Arrange
            int auditId = 1;
            var audit = new Audit { Id = auditId, Status = "Pending" };
            _mockAuditService.Setup(s => s.GetAuditDetailAsync(auditId)).ReturnsAsync(audit);

            // Act
            var result = await _controller.Details(auditId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            _mockAuditService.Verify(s => s.GetAuditDetailAsync(auditId), Times.Once);
        }

        [Fact]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int auditId = 999;
            _mockAuditService.Setup(s => s.GetAuditDetailAsync(auditId)).ReturnsAsync((Audit?)null);

            // Act
            var result = await _controller.Details(auditId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Start Tests

        [Fact]
        public async Task Start_WithValidId_RedirectsToDetails()
        {
            // Arrange
            int auditId = 1;
            _mockAuditService.Setup(s => s.StartAuditAsync(auditId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Start(auditId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AuditController.Details), redirectResult.ActionName);
            Assert.Equal(auditId, redirectResult.RouteValues?["id"]);
            _mockAuditService.Verify(s => s.StartAuditAsync(auditId), Times.Once);
        }

        #endregion

        #region Complete Tests

        [Fact]
        public async Task Complete_WithValidData_RedirectsToIndex()
        {
            // Arrange
            int auditId = 1;
            string findings = "No critical issues found";
            _mockAuditService.Setup(s => s.CompleteAuditAsync(auditId, findings)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Complete(auditId, findings);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AuditController.Index), redirectResult.ActionName);
            _mockAuditService.Verify(s => s.CompleteAuditAsync(auditId, findings), Times.Once);
        }

        #endregion

        #region Create Tests

        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_WithValidAudit_RedirectsToIndex()
        {
            // Arrange
            var audit = new Audit { AuditTitle = "Annual Compliance Audit" };
            _mockAuditService.Setup(s => s.CreateAuditAsync(It.IsAny<Audit>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(audit);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AuditController.Index), redirectResult.ActionName);
            Assert.Equal(1, audit.AssignedOfficerId);
            _mockAuditService.Verify(s => s.CreateAuditAsync(audit), Times.Once);
        }

        #endregion
    }
}
