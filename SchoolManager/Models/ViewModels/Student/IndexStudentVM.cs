using System.ComponentModel.DataAnnotations;

namespace SchoolManager.ViewModels
{
    public class StudentViewModel
    {

        [Required(ErrorMessage = "O nome é obrigatório")] 
        [Display(Name = "Nome do Aluno")]
        public required string Name { get; set; }
            
        public int Id { get; set; }

        [Display(Name = "É Bolsista?")]
        public bool IsScholarshipRecipient { get; set; }

        [Display(Name = "Mensalidade")]
        [DataType(DataType.Currency)] 
        public decimal MonthlyFee { get; set; }

        
        [Display(Name = "Status da Matrícula")]
        public string StatusDescricao => IsScholarshipRecipient ? "Bolsista Integral" : "Pagante";
    }
}