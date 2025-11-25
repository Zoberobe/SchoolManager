using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.SchoolVM
{
    public class EditSchoolVM
    {
        public Guid Uuid { get; init; }
        public string Name { get; init; }
        public string City { get; init; }
    }
}