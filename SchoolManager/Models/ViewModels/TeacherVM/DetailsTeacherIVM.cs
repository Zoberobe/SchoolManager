
using SchoolManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.TeacherVM
{
    public class DetailsTeacherIVM
    {
        public Guid Uuid { get;  set; }
        [Display(Name = "Name")]
        public string? Name { get;  set; }
        [Display(Name = "Birth")]
        public DateOnly Birth { get;  set; }
        [Display(Name = "Matter")]
        public Matter Matter { get; set; }
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }
        [Display(Name = "School Name")]
        public string? SchoolName { get; set; }
    }
}
