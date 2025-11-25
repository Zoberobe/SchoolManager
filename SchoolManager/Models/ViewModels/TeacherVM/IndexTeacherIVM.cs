using SchoolManager.Models.Enums;
namespace SchoolManager.Models.ViewModels.TeacherVM
{
    public class IndexTeacherIVM
    {
        public Guid Uuid { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public Matter Matter { get; set; }
       

    }
}
