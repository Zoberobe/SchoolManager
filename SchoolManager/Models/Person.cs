using System.Net.Sockets;

namespace SchoolManager.Models
{
    public abstract class Person : EntityBase
    {
        public string Name { get; private set; }
        public DateOnly Birth { get; private set; }

        public Person() { }
        protected Person(string name, DateOnly birth)
        {
            Name = name;
            Birth = birth;
        }

       protected void UpdateName(string name)
        {
            Name = name;
        }

        protected void UpdateBirth(DateOnly birth)
        {
            Birth = birth;
        }
    }
}
