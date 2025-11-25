using SchoolManager.Models.Enums;

namespace SchoolManager.Models
{
    public class Teacher : Person
    {
        public Matter Matter { get; internal set; }
        public decimal Salary { get; private set; }
        public School? School { get; internal set; }
        public int SchoolId { get; set; }
        
        public void SetSalary(decimal value)
        {
            if (value <= 0)
                throw new Exception("Salario não Pode ser menor que zero");
            Salary = value;
        }
    }
}

