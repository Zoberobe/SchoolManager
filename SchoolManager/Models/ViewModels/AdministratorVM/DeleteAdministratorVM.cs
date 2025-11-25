using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.AdministratorVM
{
    public class DeleteAdministratorVM
    {
        public Guid Uuid { get; set; }
        public string Name { get; private set; } 
    }
}
