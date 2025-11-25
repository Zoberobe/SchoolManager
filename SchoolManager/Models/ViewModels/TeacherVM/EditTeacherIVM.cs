using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.TeacherVM
{
    public class EditTeacherIVM
    {
        public Guid Uuid { get; set; }
        public string? Name { get; set; }
        public Matter Matter { get; set; }
        public DateOnly Birth { get; set; }
        public decimal Salary { get; set; }

        public int Id { get; set; }


        [Required(ErrorMessage = "O salário é obrigatório.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O salário deve ser maior que zero.")]
        public decimal InputSalary { get; set; }
        [Required(ErrorMessage = "O nome da escola é obrigatório.")]
        public required string SchoolName { get; set; }


        public IEnumerable<SelectListItem> Schoollist { get; set; }
        public IEnumerable<SelectListItem> Matterlists { get; set; }
    }
}
