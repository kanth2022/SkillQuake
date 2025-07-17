namespace SkillQuakeAPI.Models
{
    public class Rating
    {
        public int Id { get; set; } 
        public int UserId {  get; set; }
        public int CourseId {  get; set; }
        public int Stars {  get; set; }

        public string? Review { get; set; }
        public DateTime RatedOn { get; set; }
    }
}
