using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.Enums
{
    public enum SchoolShift
    {
        [Display(Name = "Manhã")] Morning,
        [Display(Name = "Tarde")] Afternoon,
        [Display(Name = "Noite")] Night
    }
}
