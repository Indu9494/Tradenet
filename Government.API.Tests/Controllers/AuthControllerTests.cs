using System.Linq.Expressions;
using System.Security.Claims;
using Government.Models;
using Government.API.Controllers;
using Government.API.Data;
using Government.API.Exceptions;
using Government.API.Models.ViewModels;
using Government.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Government.API.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IJwtTokenService> _mockJwtTokenService;
        private Mock<ILogger<AuthController>> _mockLogger;
        private Mock<AppDbContext> _mockDbContext;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockJwtTokenService = new Mock<IJwtTokenService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _mockDbContext = new Mock<AppDbContext>();

            _controller = new AuthController(
                _mockDbContext.Object,
                _mockJwtTokenService.Object,
                _mockLogger.Object);
        }

        // 1. SUCCESS: LOGIN
        [Test]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var testUser = new User
            {
                UserID = 1,
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                PasswordSalt = "salt"
            };

            var mockSet = new Mock<DbSet<User>>();

            // Mocking FirstOrDefaultAsync requires specific Setup
            mockSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(testUser);

            _mockDbContext.Setup(d => d.Users).Returns(mockSet.Object);

            _mockJwtTokenService.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns("fake-jwt-token");

            // Act
            var result = await _controller.Login(new ApiLoginRequest { Email = "test@example.com", Password = "password" });

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        // 2. FAILURE: LOGIN (Invalid Credentials)
        [Test]
        public void Login_InvalidUser_ThrowsInvalidCredentialsException()
        {
            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((User)null!);

            _mockDbContext.Setup(d => d.Users).Returns(mockSet.Object);

            Assert.ThrowsAsync<InvalidCredentialsException>(async () =>
                await _controller.Login(new ApiLoginRequest { Email = "wrong@test.com", Password = "any" }));
        }

        // 3. SUCCESS: VALIDATE TOKEN
        [Test]
        public void ValidateToken_ValidClaims_ReturnsOk()
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "test@example.com")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };

            var result = _controller.ValidateToken();
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        // 4. FAILURE: VALIDATE TOKEN (Missing Claims)
        [Test]
        public void ValidateToken_MissingUserId_ThrowsInvalidTokenException()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            };

            Assert.Throws<InvalidTokenException>(() => _controller.ValidateToken());
        }
    }
}