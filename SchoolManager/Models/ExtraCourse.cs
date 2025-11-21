namespace SchoolManager.Models
{
    public sealed class ExtraCourse : EntityBase
    {
        public string Name { get; private set; }
        public int Hours { get; private set; }

        public ExtraCourse(string name, int hours)
        {
            Name = name;
            Hours = hours;
        }

        public void Edit(string name, int hours)
        {
            Name = name;
            Hours = hours;
        }
    }
}
