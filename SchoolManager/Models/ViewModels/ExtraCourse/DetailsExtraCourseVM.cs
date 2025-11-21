namespace SchoolManager.Models.ViewModels.ExtraCourse
{
    public class DetailsExtraCourseVM
    {
        public Guid Uuid { get; init; }
        public required string Name { get; init; }
        public int Hours { get; init; }
    }
}
