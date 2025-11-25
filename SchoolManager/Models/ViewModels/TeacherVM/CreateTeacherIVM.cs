using SchoolManager.Models.Enums;
using System.ComponentModel.DataAnnotations;
namespace SchoolManager.Models.ViewModels.TeacherVM
{
    public class CreateTeacherIVM
    {
       
        public required string Name { get; init; }
        public Matter Matter { get; set; }
        public DateOnly Birth { get; set; }
        public decimal Salary { get; set; }
        
        public required string SchoolName { get; init; }
        

    }
}
