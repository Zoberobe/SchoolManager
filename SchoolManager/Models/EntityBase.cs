namespace SchoolManager.Models
{
    public abstract class EntityBase
    {
        public int Id { get; private set; }
        public Guid Uuid { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public bool IsDeleted { get; init; }
    }
}
