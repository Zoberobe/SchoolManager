namespace SchoolManager.Models
{
    public class Administrator : Person
    {
        public decimal Capital { get; private set; }


        public Administrator() { }

        public Administrator(string name, DateOnly birth, decimal capital)
            : base(name, birth)
        {
            Capital = capital;
        }

        public string GetName()
        {
            return Name;
        }


        public void UpdateProperties(string name, DateOnly birth, decimal capital)
        {
            if (capital < 0)
                throw new ArgumentOutOfRangeException(nameof(capital), "Capital não pode ser negativo.");

            Capital = capital;
            MarkUpdatedAt();
            UpdateName(name);
            UpdateBirth(birth);
        }






    }


}
