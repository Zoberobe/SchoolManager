namespace SchoolManager.Models
{
    public class Student : Person
    {
        public bool IsScholarshipRecipient { get; init; }
        public decimal MonthlyFee { get; private set; }
    }
}
