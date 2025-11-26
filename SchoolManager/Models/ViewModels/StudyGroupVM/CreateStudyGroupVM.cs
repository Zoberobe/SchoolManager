using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.StudyGroupVM
{
    public class CreateStudyGroupVM
    {
        [Display(Name = "Série / Ano")]
        public GradeLevel Grade { get; set; }

        [Display(Name = "Turma")]
        public ClassSection Section { get; set; }

        [Display(Name = "Turno")]
        public SchoolShift Shift { get; set; }

        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime InitialDate { get; set; } = DateTime.Today; 

        [Display(Name = "Data de Encerramento")]
        [DataType(DataType.Date)]
        public DateTime FinalDate { get; set; } = DateTime.Today.AddMonths(12); 

        [Display(Name = "Escola")]
        public int SchoolId { get; set; }

        [Display(Name = "Professor Responsável")]
        public int TeacherId { get; set; }

        public SelectList? SchoolsList { get; set; }
        public SelectList? TeachersList { get; set; }
    }
}