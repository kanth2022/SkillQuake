namespace SkillQuakeAPI.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public decimal Price { get; set; }

        public int CoachId { get; set; }
        public User? Coach { get; set; } //navigation to coach
        public ICollection<Enrollment>? Enrollments { get; set; } 
    }
}
