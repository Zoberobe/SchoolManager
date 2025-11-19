namespace SchoolManager.Models
{
    public abstract class Person : EntityBase
    {
        public required string Name { get; init; }
        public DateOnly Birth { get; set; }
    }
}
