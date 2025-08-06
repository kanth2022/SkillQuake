namespace SkillQuakeAPI.Models.DTO
{
    public class CourseCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public decimal Price { get; set; }
        public int CoachId { get; set; }
    }
}