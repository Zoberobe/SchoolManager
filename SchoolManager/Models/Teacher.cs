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
        public void SetBirth(DateOnly birth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birth.Year;
            if (birth > today.AddYears(-age)) age--;
            if (age < 18)
                throw new Exception("Professor deve ser maior de idade");
            UpdateBirth(birth);
        }
        public Teacher(string name, DateOnly birth)
            : base(name, birth)
        {
        }
        private Teacher() : base()
        {
        }
    }
}
