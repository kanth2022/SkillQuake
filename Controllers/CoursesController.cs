using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillQuakeAPI.Data;
using SkillQuakeAPI.Models;
using SkillQuakeAPI.Models.DTO;

namespace SkillQuakeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new course
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var coach = await _context.Users.FindAsync(dto.CoachId);
            if (coach == null || coach.Role.ToLower() != "coach")
            {
                return BadRequest(new { message = "Invalid Coach ID. Only coaches can create courses." });
            }

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                VideoUrl = dto.VideoUrl,
                Price = dto.Price,
                CoachId = dto.CoachId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Course successfully registered by coach!" });
        }

        // Delete a course
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { message = "Course not found" });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Course deleted successfully!" });
        }

        // ✅ New Functionality: Get all courses by a coach
        [HttpGet("coach/{coachId}")]
        public async Task<IActionResult> GetCoursesByCoach(int coachId)
        {
            var coach = await _context.Users.FindAsync(coachId);
            if (coach == null || coach.Role.ToLower() != "coach")
            {
                return BadRequest(new { message = "Invalid Coach ID." });
            }

            var courses = await _context.Courses
                .Where(c => c.CoachId == coachId)
                .Select(c => new CourseSummaryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    VideoUrl = c.VideoUrl,
                    Price = c.Price
                })
                .ToListAsync();

            return Ok(courses);
        }

    }
}
