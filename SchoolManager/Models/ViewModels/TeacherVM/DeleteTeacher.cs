using SchoolManager.Models.Enums;

namespace SchoolManager.Models.ViewModels.TeacherVM
{
    public class DeleteTeacher
    {
        public Guid Uuid { get; set; }
        public string? Name { get; set; }
        public Matter Matter { get; set; }
        public DateOnly Birth { get; set; }
        public decimal Salary { get; set; }
    }
}
