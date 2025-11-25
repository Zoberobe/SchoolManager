namespace SchoolManager.Models
{
    public abstract class Person : EntityBase
    {
        public string? Name { get; private set; }
        public DateOnly Birth { get; set; }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Nome não Pode Ser Vazio!!", nameof(name));
            }

            Name = name;
        }
    }
}
