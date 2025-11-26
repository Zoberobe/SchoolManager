using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolManager.ViewModels
{

    public class StudentFormViewModel
    {
        public Guid Uuid { get; set; }

        [RegularExpression(@"^[a-zA-Z\u00C0-\u00FF\s']+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome do Aluno")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "É Bolsista?")]
        public bool IsScholarshipRecipient { get; set; }

        [Required(ErrorMessage = "O campo Mensalidade é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor inválido")]
        [Display(Name = "Valor da Mensalidade")]
        [DataType(DataType.Currency)]
        public decimal MonthlyFee { get; set; }

        [Required(ErrorMessage = "Selecione uma Escola")]
        [Display(Name = "Escola")]
        public Guid SchoolUuid { get; set; }

        [Required(ErrorMessage = "Selecione uma Turma")]
        [Display(Name = "Turma")]
        public Guid StudyGroupUuid { get; set; }

        public string? SchoolName { get; set; }
        public string? StudyGroupName { get; set; }
        public string? TeacherName { get; set; }
        public string? Origin { get; set;  }
        public IEnumerable<SelectListItem> SchoolsList { get; set; } = [];
        public IEnumerable<SelectListItem> StudyGroupsList { get; set; } = [];
    }
}