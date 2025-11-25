using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManager.Models.Enums;
using System.ComponentModel.DataAnnotations;
namespace SchoolManager.Models.ViewModels.TeacherVM
{
    public class CreateTeacherIVM
    {
       
        public  string Name { get; init; }
        public Matter Matter { get; set; }
        public DateOnly Birth { get; set; }
        public decimal Salary { get; set; }
        
        public  string SchoolName { get; init; }

       public IEnumerable<SelectListItem> Schoollist { get; set; } = [];
        public IEnumerable<SelectListItem> Matterlists { get; set; } = [];
    }
}
