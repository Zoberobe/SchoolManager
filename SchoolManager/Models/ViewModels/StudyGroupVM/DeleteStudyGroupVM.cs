using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.StudyGroupVM
{
    public class DeleteStudyGroupVM
    {
        public Guid Uuid { get; init; }
        public int SchoolId { get; set; }
        [Display(Name = "Nome do Grupo")]
        public string? Name { get; set; }
        [Display(Name = "Data Inicial")]
        [DataType(DataType.Date)]
        public DateTime InitialDate { get; init; }
        [Display(Name = "Data Final")]
        [DataType(DataType.Date)]
        public DateTime FinalDate { get; set; }
        [Display(Name = "Professor")]
          public required string TeacherName { get; init; }
        [Display(Name = "Qtd. Alunos")]
        public int StudentCount { get; set; }
        [Display(Name = "Escola")]
        public required string SchoolName { get; set; }
    }
}