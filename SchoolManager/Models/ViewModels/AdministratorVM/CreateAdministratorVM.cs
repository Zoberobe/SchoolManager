using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.AdministratorVM
{
    public class CreateAdministratorVM
    {
        
        public Guid Uuid { get; init; } 
        public DateTime CreatedAt { get; init; } 

        [Display(Name = "Nome completo")]
        [StringLength(60)]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Name { get; init; }

        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        public required DateOnly Birth { get; init; }
        [Display(Name = "Capital")]
        public required decimal Capital { get; init; }


    }
}
