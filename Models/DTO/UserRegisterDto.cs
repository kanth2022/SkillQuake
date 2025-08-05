namespace SkillQuakeAPI.Models.DTO
{
    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
    }
}
