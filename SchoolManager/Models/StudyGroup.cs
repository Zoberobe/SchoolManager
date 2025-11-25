namespace SchoolManager.Models
{
    public class StudyGroup : EntityBase
    {
        public string? Name { get; private set; }
        public DateTime InitialDate { get; init; }
        public DateTime FinalDate { get; init; }
        public int TeacherId { get; set; }
        public int SchoolId { get; set; }
        public required Teacher Teacher { get; init; }
        public virtual ICollection<Student> Students { get; private set; } = new List<Student>();

        public StudyGroup() { }
    }
}
