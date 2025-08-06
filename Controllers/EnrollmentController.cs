//controller/EnrollmentController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillQuakeAPI.Data;
using SkillQuakeAPI.Models;

[ApiController]

[Route("api/[controller]")]

public class EnrollmentController : ControllerBase

{

    private readonly ApplicationDbContext _context;

    public EnrollmentController(ApplicationDbContext context)

    {

        _context = context;

    }

    [HttpPost("enroll")]
    public async Task<IActionResult> Enroll([FromBody] EnrollDto dto)
    {
        // Check if user exists and fetch their role
        var user = await _context.Users.FindAsync(dto.UserId);
        if (user == null)
            return NotFound("User not found");

        if (!string.Equals(user.Role, "Learner", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only learners can enroll in courses");

        // Check if already enrolled
        var exists = await _context.Enrollments
            .AnyAsync(e => e.UserId == dto.UserId && e.CourseId == dto.CourseId);
        if (exists)
            return BadRequest("Already enrolled");

        // Proceed with enrollment
        var enrollment = new Enrollment
        {
            UserId = dto.UserId,
            CourseId = dto.CourseId,
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Enrollment successful" });
    }


    [HttpGet("user/{userId}")]

    public async Task<IActionResult> GetUserEnrollments(int userId)

    {

        var enrolledCourses = await _context.Enrollments

            .Where(e => e.UserId == userId)

            .Include(e => e.Course)

            .Select(e => e.Course)

            .ToListAsync();

        return Ok(enrolledCourses);

    }

}
