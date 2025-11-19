using SchoolManager.Models.Enums;

namespace SchoolManager.Models
{
    public class Teacher : Person
    {
        public Matter Matter { get; init; }
        public decimal Salary { get; private set; }
    }
}
