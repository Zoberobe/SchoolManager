using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.AdministratorVM
{
    public class DetailsAdministratorVM
    {
        public Guid Uuid { get; set; }

        public required string Name { get; init; }

        [DataType(DataType.Date)]
        public DateOnly Birth { get; set; }
        public decimal Capital { get; set; }
    }
}
