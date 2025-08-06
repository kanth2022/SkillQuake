namespace SkillQuakeAPI.Models
{
    namespace SkillQuakeAPI.Models
    {
        public class RatingDto
        {
            public int UserId { get; set; }
            public int CourseId { get; set; }
            public int Stars { get; set; }
            public string? Review { get; set; }
        }
    }

}
