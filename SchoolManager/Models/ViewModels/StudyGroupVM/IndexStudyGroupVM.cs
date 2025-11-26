namespace SchoolManager.Models.ViewModels.StudyGroupVM
{
    public class IndexStudyGroupVM
    {
        public Guid Uuid { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; } 

        public Guid Uuid { get; set; }
        public string Name { get; set; }    
        public string SchoolName { get; set; }  
        public string TeacherName { get; set; } 
        public int StudentsCount { get; set; }   
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }


    }
}