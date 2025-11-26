using SchoolManager.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.ViewModels.StudyGroupVM
{
    public class StudyGroupEditViewModel
    {
        public Guid Uuid { get; set; }

        [Display(Name = "Nome Atual")]
        public string? Name { get; set; } // Apenas leitura visual

        // Enums para regenerar o nome (caso queira mudar)
        [Display(Name = "Série / Ano")]
        public GradeLevel Grade { get; set; }

        [Display(Name = "Turma")]
        public ClassSection Section { get; set; }

        [Display(Name = "Turno")]
        public SchoolShift Shift { get; set; }

        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime InitialDate { get; set; }

        [Display(Name = "Data de Encerramento")]
        [DataType(DataType.Date)]
        public DateTime FinalDate { get; set; }

        [Display(Name = "Escola")]
        [Required(ErrorMessage = "A escola é obrigatória")]
        public Guid SchoolUuid { get; set; }

        [Display(Name = "Professor Responsável")]
        [Required(ErrorMessage = "O professor é obrigatório")]
        public Guid TeacherUuid { get; set; }

        // Listas para os Dropdowns
        public SelectList? SchoolsList { get; set; }
        public SelectList? TeachersList { get; set; }
    }
}