namespace SchoolManager.Models.ViewModels.StudyGroupVM
{
    public class IndexStudyGroupVM
    {

        public required int Id { get; init; }
        public required Guid Uuid { get; init; }
        public required Teacher Teacher { get; init; } 
        public virtual ICollection<Student> Students { get; private set; } = new List<Student>();
        public int SchoolId { get; set; }

  
    }
}
