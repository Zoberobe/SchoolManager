namespace SchoolManager.Models
{
    public abstract class Person : EntityBase
    {
        public string? Name { get; private set; }
        public DateOnly Birth { get; set; }

        protected Person(string name) 
        {
            Name = name;
        }

        protected Person()
        {
            
        }

        public void UpdateName(string newName) 
        {
            if(string.IsNullOrWhiteSpace(newName)) 
                throw new ArgumentException("O nome não pode ser vazio");
            Name = newName;
        }
    }
}
