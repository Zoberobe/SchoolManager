namespace SchoolManager.Models.ViewModels.StudyGroupVM
{
    public class DetailsStudyGroupVM
    {
        public Guid Uuid { get; set; }
        public string? Name { get; init; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }

        public int StudentsCount { get; set; }
        public SchoolProjectionVM school { get; set; }
        public TeacherProjectionVM Teacher { get; set; }


        public virtual ICollection<Student> Students { get; init; } = new List<Student>();

        public class SchoolProjectionVM
        {
            public Guid Uuid { get; set; }
            public string Name { get; set; }
        }

        public class TeacherProjectionVM
        {
            public Guid Uuid { get; set; }
            public string Name { get; set;}
        }
    }
}