using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.AdministratorVM
{
    public class IndexAdministratorVM
    {
        [Key]
        public required Guid Uuid { get; init; }
        public required string Name { get; init; }
        public required DateOnly Birth { get; init; }
        public required decimal Capital { get; init; }

    }
}
