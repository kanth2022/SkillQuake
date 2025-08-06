using System.ComponentModel.DataAnnotations;

namespace SkillQuakeAPI.Models.DTO
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
