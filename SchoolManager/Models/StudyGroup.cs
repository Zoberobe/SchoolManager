    namespace SchoolManager.Models
    {
        public class StudyGroup : EntityBase
        {
            public DateTime InitialDate { get; init; }
            public DateTime FinalDate { get; init; }
            public required Teacher Teacher { get; init; }
            public virtual ICollection<Student> Students { get; private set; } = new List<Student>();
            public int SchoolId { get; set; }

        public StudyGroup() { }


    }
}
