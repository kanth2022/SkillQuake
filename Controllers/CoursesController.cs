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
            try
            {
                var courses = await _context.Courses.Include(c => c.Coach).ToListAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
                    

            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            var coach = await _context.Users.FindAsync(dto.CoachId);
            if (coach == null || coach.Role.ToLower() != "coach")
                return BadRequest("Invalid coach Id");

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

            return Ok(new { message = "Course created successfully" });
        }


    }
}
