using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Models.Enums
{
    public enum GradeLevel
    {
        [Display(Name = "1º Ano - Fundamental")] Fundamental1 = 1,
        [Display(Name = "2º Ano - Fundamental")] Fundamental2 = 2,
        [Display(Name = "3º Ano - Fundamental")] Fundamental3 = 3,
        [Display(Name = "4º Ano - Fundamental")] Fundamental4 = 4,
        [Display(Name = "5º Ano - Fundamental")] Fundamental5 = 5,
        [Display(Name = "6º Ano - Fundamental")] Fundamental6 = 6,
        [Display(Name = "7º Ano - Fundamental")] Fundamental7 = 7,
        [Display(Name = "8º Ano - Fundamental")] Fundamental8 = 8,
        [Display(Name = "9º Ano - Fundamental")] Fundamental9 = 9,

        [Display(Name = "1º Ano - Médio")] Medio1 = 10,
        [Display(Name = "2º Ano - Médio")] Medio2 = 11,
        [Display(Name = "3º Ano - Médio")] Medio3 = 12
    }
}
