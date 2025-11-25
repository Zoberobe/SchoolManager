namespace SchoolManager.Models
{
    public abstract class EntityBase
    {
        public int Id { get; private set; }
        public Guid Uuid { get; init; } = Guid.NewGuid();
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        public void MarkAsDeleted()
        {
           IsDeleted = true;
        }

        public void MarkUpdatedAt()
        {
           UpdatedAt= DateTime.UtcNow;
        }

    }
}
