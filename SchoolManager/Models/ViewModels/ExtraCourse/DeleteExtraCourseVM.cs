
namespace SchoolManager.Models.ViewModels.ExtraCourse
{
    public sealed class DeleteExtraCourseVM
    {
        public Guid Uuid { get; init; }
        public required string Name { get; init; }
        public int Hours { get; init; }
    }
}
