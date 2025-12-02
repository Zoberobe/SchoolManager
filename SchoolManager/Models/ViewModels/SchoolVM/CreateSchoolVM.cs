using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.SchoolVM
{
    public class CreateSchoolVM
    {
        public Guid Uuid { get; init; }
        public DateTime CreatedAt { get; init; }

        [Display(Name = "Digite seu Nome")]
        [StringLength(60)]
        public string Name { get; set; } = string.Empty;
       
        public string City { get; set; } = string.Empty;
    }
}
