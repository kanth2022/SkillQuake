using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SkillQuakeAPI.Controllers;
using SkillQuakeAPI.Data;
using SkillQuakeAPI.Models;
using SkillQuakeAPI.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillQuakeAPI.Tests.Controllers
{
    public class CoursesControllerTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
        }

        [Test]
        public async Task CreateCourse_ShouldReturnOk_WhenCoachIsValid()
        {
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            context.Users.Add(new User
            {
                Id = 1,
                Role = "Coach",
                Name = "Farhana",
                Email = "farhana@example.com",
                PasswordHash = "hashedpassword",
                ConfirmPassword = "hashedpassword",
                Phone = "1234567890"
            });
            context.SaveChanges();

            var controller = new CoursesController(context);

            var dto = new CourseCreateDto
            {
                Title = "New Course",
                Description = "Course description",
                VideoUrl = "http://video.com",
                Price = 100,
                CoachId = 1
            };

            var result = await controller.CreateCourse(dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteCourse_ShouldReturnOk_WhenCourseExists()
        {
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            context.Users.Add(new User
            {
                Id = 1,
                Role = "Coach",
                Name = "Farhana",
                Email = "farhana@example.com",
                PasswordHash = "hashedpassword",
                ConfirmPassword = "hashedpassword",
                Phone = "1234567890"
            });

            context.Courses.Add(new Course
            {
                Id = 1,
                Title = "Course to Delete",
                Description = "Desc",
                VideoUrl = "http://video.com",
                Price = 50,
                CoachId = 1
            });

            context.SaveChanges();

            var controller = new CoursesController(context);
            var result = await controller.DeleteCourse(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteCourse_ShouldReturnNotFound_WhenCourseDoesNotExist()
        {
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            var controller = new CoursesController(context);
            var result = await controller.DeleteCourse(999);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetCoursesByCoach_ShouldReturnCourses_WhenCoachExists()
        {
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            context.Users.Add(new User
            {
                Id = 1,
                Role = "Coach",
                Name = "Farhana",
                Email = "farhana@example.com",
                PasswordHash = "hashedpassword",
                ConfirmPassword = "hashedpassword",
                Phone = "1234567890"
            });

            context.Courses.Add(new Course
            {
                Id = 1,
                Title = "Test Course",
                Description = "Desc",
                VideoUrl = "http://video.com",
                Price = 100,
                CoachId = 1
            });

            context.SaveChanges();

            var controller = new CoursesController(context);
            var result = await controller.GetCoursesByCoach(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var courses = okResult?.Value as IEnumerable<CourseSummaryDto>;
            Assert.That(courses?.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetCoursesByCoach_ShouldReturnBadRequest_WhenCoachIsInvalid()
        {
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            var controller = new CoursesController(context);
            var result = await controller.GetCoursesByCoach(999);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
