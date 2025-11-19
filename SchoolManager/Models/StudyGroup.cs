namespace SchoolManager.Models
{
    public class StudyGroup : EntityBase
    {
        public DateTime InitialDate { get; init; }
        public DateTime FinalDate { get; init; }
        public required Teacher Teacher { get; init; }
        public IEnumerable<Student> Students { get; private set; } = [];
    }
}
