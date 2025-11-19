namespace SchoolManager.Models
{
    public class School : EntityBase
    {
        public required string Name { get; init; }
        public required string City { get; init; }
        public required Administrator Administrator { get; init; }
        public ICollection<Teacher> Teachers { get; set; } = [];
        public ICollection<Student> Students { get; set; } = [];
        public ICollection<StudyGroup> StudyGroups { get; private set; } = [];
    }
}
