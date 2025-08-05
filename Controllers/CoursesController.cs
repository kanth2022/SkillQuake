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
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _context.Courses.ToListAsync();

            if (courses == null || !courses.Any())
                return NotFound(new { message = "No courses available." });

            return Ok(courses);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the given CoachId belongs to a real user and is actually a coach
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.VideoUrl = dto.VideoUrl;
            course.Price = dto.Price;
            course.CoachId = dto.CoachId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Course updated successfully!" });
        }
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedCourses(string sortBy = "title", bool descending = false)
        {
            var query = _context.Courses.AsQueryable();

            query = sortBy.ToLower() switch
            {
                "price" => descending ? query.OrderByDescending(c => c.Price) : query.OrderBy(c => c.Price),
                "title" => descending ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
                "id" => descending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                // You can add "createdDate" if you include that property in the Course model.
                _ => query.OrderBy(c => c.Title)
            };

            var sortedCourses = await query.ToListAsync();
            return Ok(sortedCourses);
        }

    }
}
