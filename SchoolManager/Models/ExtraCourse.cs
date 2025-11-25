namespace SchoolManager.Models
{
    public sealed class Schools : EntityBase
    {
        public string Name { get; private set; }
        public int Hours { get; private set; }

        public Schools(string name, int hours)
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
