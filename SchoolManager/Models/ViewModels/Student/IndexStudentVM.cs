using System.ComponentModel.DataAnnotations;

namespace SchoolManager.ViewModels
{
    public class StudentViewModel
    {

        [Required(ErrorMessage = "O nome é obrigatório")] // Adicione isso
        [Display(Name = "Nome do Aluno")]
        public required string Name { get; set; }
            
        public int Id { get; set; }

        [Display(Name = "É Bolsista?")]
        public bool IsScholarshipRecipient { get; set; }

        [Display(Name = "Mensalidade")]
        [DataType(DataType.Currency)] // Formata como moeda na View automaticamente
        public decimal MonthlyFee { get; set; }

        // Exemplo: Uma propriedade calculada apenas para exibição
        [Display(Name = "Status da Matrícula")]
        public string StatusDescricao => IsScholarshipRecipient ? "Bolsista Integral" : "Pagante";
    }
}