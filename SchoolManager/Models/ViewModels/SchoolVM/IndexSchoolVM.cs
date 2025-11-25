using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.SchoolVM
{
    public class IndexSchoolVM
    {
        public required Guid Uuid { get; init; }
        public string Name { get; init; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
