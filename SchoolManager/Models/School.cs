namespace SchoolManager.Models
{
    public class School : EntityBase
    {
        public string Name { get; private set; }
        public string City { get; private set; }
        public Administrator Administrator { get; init; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<StudyGroup> StudyGroups { get; private set; } = new List<StudyGroup>();

        private School() { }
        public School(string name, string city, Administrator administrator)
        {
            Name = name;
            City = city;
            Administrator = administrator;
        }

        // Método público de edição usado pelo controller
        public void Edit(string name, string city)
        {
            UpdateName(name);
            UpdateCity(city);

            // Atualize timestamps se sua EntityBase expor setters

        }

        // Validações/atribuições internas
        private void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser nulo ou vazio.", nameof(name));

            Name = name.Trim();
        }

        private void UpdateCity(string? city)
        {
            City = string.IsNullOrWhiteSpace(city) ? string.Empty : city.Trim();
        }
    }
}