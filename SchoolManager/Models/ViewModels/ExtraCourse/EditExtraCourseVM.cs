using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.ExtraCourse
{
    public sealed class EditExtraCourseVM
    {
        public Guid Uuid { get; init; }
        [Required]
        public required string Name { get; init; }
        [Required]
        [Range(1, 300)]
        public int Hours { get; init; }
    }
}
