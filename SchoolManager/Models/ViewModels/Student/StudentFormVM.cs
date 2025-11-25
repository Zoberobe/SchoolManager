using System.ComponentModel.DataAnnotations;

namespace SchoolManager.ViewModels
{
    public class StudentFormViewModel
    {
        // O ID é necessário para a Edição
        public Guid Uuid { get; set; }

        [Display(Name = "Nome do Aluno")]
        public required string Name { get; set; }

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
    }
}