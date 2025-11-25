namespace SchoolManager.Models
{
    public abstract class Person : EntityBase
    {
        public required string Name { get; set; }
        public DateOnly Birth { get; set; }
    }
}
