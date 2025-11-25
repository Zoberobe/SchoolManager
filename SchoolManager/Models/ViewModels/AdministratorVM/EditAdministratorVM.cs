using System.ComponentModel.DataAnnotations;


namespace SchoolManager.Models.ViewModels.AdministratorVM
{
    public class EditAdministratorVM
    {


           
        public Guid Uuid { get; set; }

        public  string Name { get;  set; }

        [DataType(DataType.Date)]
        public  DateOnly Birth { get; set; }
        public  decimal Capital { get;  set; }

        public DateTime UpdatedAt { get; set; }


    }
}
