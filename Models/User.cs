namespace SkillQuakeAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }

        public string Phone { get; set; }

        public ICollection<Course>? CreatedCourses { get; set; } //AS COACH
        public ICollection<Enrollment>? Enrollments { get; set; } //AS LEARNER
    }
}
