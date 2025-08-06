using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using SkillQuakeAPI.Data;

using SkillQuakeAPI.Models;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

using SkillQuakeAPI.Models.DTO;

namespace SkillQuakeAPI.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class AuthController : ControllerBase

    {

        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)

        {

            _context = context;

            _config = config;

        }

        // POST: api/auth/register

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)

        {

            // Check if email already exists

            var exists = await _context.Users.AnyAsync(u => u.Email == userDto.Email);

            if (exists)

                return BadRequest("User already exists.");

            var phoneExists = await _context.Users.AnyAsync(u => u.Phone == userDto.Phone);
            if (phoneExists)

                return BadRequest("Phone number already in use.");

            var user = new User

            {

                Name = userDto.Name,

                Email = userDto.Email,

                Role = userDto.Role,

                Phone = userDto.Phone,

                ConfirmPassword = userDto.ConfirmPassword,

                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.PasswordHash)

            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });

        }

        // POST: api/auth/login

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginDto login)

        {

            try

            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

                if (user == null)

                    return NotFound("User not found");

                bool passwordMatch = BCrypt.Net.BCrypt.Verify(login.PasswordHash, user.PasswordHash);

                if (!passwordMatch)

                    return Unauthorized("Incorrect password");


                var tokenHandler = new JwtSecurityTokenHandler();

                var Key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor

                {

                    Subject = new ClaimsIdentity(new[]

                    {

                    new Claim(ClaimTypes.Name,user.Name),

                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),

                    new Claim(ClaimTypes.Role,user.Role)

                }),

                    Expires = DateTime.UtcNow.AddHours(1),

                    Issuer = _config["Jwt:Issuer"],

                    Audience = _config["Jwt:Audience"],

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)

                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var jwt = tokenHandler.WriteToken(token);

                return Ok(new

                {

                    message = "Login successful",

                    token = jwt,

                    user.Id,

                    user.Name,

                    user.Email,

                    user.Role

                });

            }

            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");

            }

        }


        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> DeleteUser(int id)

        {

            var user = await _context.Users.FindAsync(id);

            if (user == null)

                return NotFound("User not found");

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });

        }

        // GET: api/auth/coaches

        [HttpGet("coaches")]

        public async Task<IActionResult> GetCoaches()

        {

            var coaches = await _context.Users

                .Where(u => u.Role.ToLower() == "coach")

                .Select(u => new

                {

                    u.Id,

                    u.Name,

                    u.Email,

                    u.Phone,

                    u.Role

                })

                .ToListAsync();

            return Ok(coaches);

        }

        // GET: api/auth/learners

        [HttpGet("learners")]

        public async Task<IActionResult> GetLearners()

        {

            var learners = await _context.Users

                .Where(u => u.Role.ToLower() == "learner")

                .Select(u => new

                {

                    u.Id,

                    u.Name,

                    u.Email,

                    u.Phone,

                    u.Role

                })

                .ToListAsync();

            return Ok(learners);

      
        }

    }

}
