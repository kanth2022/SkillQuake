using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SkillQuakeAPI.Controllers;
using SkillQuakeAPI.Data;
using SkillQuakeAPI.Models.DTO;
using SkillQuakeAPI.Models;
using SkillQuakeAPI.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQNunit.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private ApplicationDbContext _context;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            // Setup in-memory DB
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Optional: Clear database before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Fake configuration (you can inject real JWT secret here if needed)
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "this_is_a_test_key_12345"},
                {"Jwt:Issuer", "SkillQuake"},
                {"Jwt:Audience", "SkillQuakeUsers"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new AuthController(_context, configuration);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Register()
        {
            var registerDto = new UserRegisterDto
            {
                Name = "Kanth",
                Email = "kanth@gmail.com",
                PasswordHash = "kanth123",
                Role = "Learner",
                ConfirmPassword = "kanth123",
                Phone = "7815896463"
            };

            var result = await _controller.Register(registerDto) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task DeleteUser()
        {
            // Arrange - Add user to DB
            var user = new User
            {
                Name = "swamy",
                Email = "swamy@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Swamy123"),
                Role = "Coach",
                ConfirmPassword = "Swamy123",
                Phone = "6302368156"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteUser(user.Id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
