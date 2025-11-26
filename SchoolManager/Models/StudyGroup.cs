using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManager.Models
{
    public class StudyGroup : EntityBase
    {
        public string? Name { get; private set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int TeacherId { get; set; }
        public int SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public virtual School School { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<Student> Students { get; private set; } = new List<Student>();

        public StudyGroup(string name, DateTime initialDate, DateTime finalDate, int schoolId, int teacherId)
        {
            Name = name;
            InitialDate = initialDate;
            FinalDate = finalDate;
            SchoolId = schoolId;
            TeacherId = teacherId;
        }

        public StudyGroup() { }
    }
}