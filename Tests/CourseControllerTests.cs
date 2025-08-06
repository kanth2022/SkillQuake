using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SkillQuakeAPI.Controllers;
using SkillQuakeAPI.Data;
using SkillQuakeAPI.Models;
using SkillQuakeAPI.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillQuakeAPI.Tests
{
    [TestFixture]
    public class CourseControllerTests
    {
        private ApplicationDbContext _context;
        private CoursesController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SkillQuakeTestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            // Seed a coach user
            _context.Users.Add(new User
            {
                Id = 1,
                Name = "Coach User",
                Email = "coach@example.com",
                Role = "Coach",
                PasswordHash = "hash",
                ConfirmPassword = "hash",
                Phone = "1234567890"
            });
            _context.SaveChanges();

            _controller = new CoursesController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllCourses_ReturnsOkResult()
        {
            // Arrange
            _context.Courses.Add(new Course
            {
                Title = "Test Course",
                Description = "Desc",
                VideoUrl = "http://video",
                Price = 100,
                CoachId = 1
            });
            _context.SaveChanges();

            // Act
            var result = await _controller.GetAllCourses();

            // Assert
            var okResult = result as OkObjectResult; // Declare 'okResult' before using it
            Console.WriteLine(okResult.Value.GetType().FullName);
        }

        [Test]
        public async Task CreateCourse_WithValidCoach_ReturnsOk()
        {
            // Arrange
            var dto = new CourseCreateDto
            {
                Title = "New Course",
                Description = "Desc",
                VideoUrl = "http://video",
                Price = 50,
                CoachId = 1
            };

            // Act
            var result = await _controller.CreateCourse(dto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
             Console.WriteLine(result.GetType().FullName);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value.ToString(), Does.Contain("Course created successfully"));
        }

        [Test]
        public async Task CreateCourse_WithInvalidCoach_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CourseCreateDto
            {
                Title = "New Course",
                Description = "Desc",
                VideoUrl = "http://video",
                Price = 50,
                CoachId = 999 // Non-existent
            };

            // Act
            var result = await _controller.CreateCourse(dto);

            // Assert
            Console.WriteLine(result.GetType().FullName);
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value.ToString(), Does.Contain("Invalid coach Id"));
        }
    }
}