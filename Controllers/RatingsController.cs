using Microsoft.AspNetCore.Mvc;
using SkillQuakeAPI.Data;
using SkillQuakeAPI.Models;
using SkillQuakeAPI.Models.SkillQuakeAPI.Models;

namespace SkillQuakeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddRating(RatingDto dto)
        {
            // Check if user exists
            var user = _context.Users.FirstOrDefault(u => u.Id == dto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // ✅ Allow only Learners to rate
            if (user.Role != "Learner")
            {
                return Forbid("Only learners are allowed to submit ratings.");
            }

            // Optional: check if user already rated this course
            var existing = _context.Ratings
                .FirstOrDefault(r => r.UserId == dto.UserId && r.CourseId == dto.CourseId);

            if (existing != null)
            {
                return BadRequest("User already rated this course.");
            }

            var rating = new Rating
            {
                UserId = dto.UserId,
                CourseId = dto.CourseId,
                Stars = dto.Stars,
                Review = dto.Review,
                RatedOn = DateTime.Now
            };

            _context.Ratings.Add(rating);
            _context.SaveChanges();

            return Ok("Rating submitted successfully.");
        }

        [HttpGet("course/{courseId}")]
        public IActionResult GetRatingsForCourse(int courseId)
        {
            var ratings = _context.Ratings
                .Where(r => r.CourseId == courseId)
                .ToList();

            return Ok(ratings);
        }
        [HttpGet("coach/{coachId}/summary")]
        public IActionResult GetCoachRatingSummary(int coachId)
        {
            // Get all course IDs created by this coach
            var courseIds = _context.Courses
                .Where(c => c.CoachId == coachId)
                .Select(c => c.Id)
                .ToList();

            if (!courseIds.Any())
            {
                return NotFound("No courses found for this coach.");
            }

            // Get ratings for all those courses
            var ratings = _context.Ratings
                .Where(r => courseIds.Contains(r.CourseId))
                .ToList();

            // Group by Stars and count
            var grouped = ratings
                .GroupBy(r => r.Stars)
                .Select(g => new
                {
                    Stars = g.Key,
                    Count = g.Count(),
                    Label = g.Key switch
                    {
                        1 => "Poor",
                        2 => "Fair",
                        3 => "Average",
                        4 => "Good",
                        5 => "Excellent",
                        _ => "Unknown"
                    }
                })
                .OrderBy(g => g.Stars)
                .ToList();

            return Ok(grouped);
        }

        [HttpDelete("{userId}/{courseId}")]
        public IActionResult DeleteRating(int userId, int courseId)
        {
            var rating = _context.Ratings
                .FirstOrDefault(r => r.UserId == userId && r.CourseId == courseId);

            if (rating == null)
            {
                return NotFound("Rating not found.");
            }

            _context.Ratings.Remove(rating);
            _context.SaveChanges();

            return Ok("Rating deleted successfully.");
        }

    }
}
