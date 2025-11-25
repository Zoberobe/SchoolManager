using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.SchoolVM
{
    public class DetailsSchoolVM
    {
        public Guid Uuid { get; init; }
        public required string Name { get; init; }
        public string City { get; set; } = string.Empty;
    }
}
