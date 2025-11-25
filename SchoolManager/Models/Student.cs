namespace SchoolManager.Models
{
    public class Student : Person
    {
        public bool IsScholarshipRecipient { get; private set; }
        public decimal MonthlyFee { get; private set; }
        public int SchoolId { get; private set; }
        public int StudyGroupId { get; private set; }

        public virtual School? School { get; set; }
        public virtual StudyGroup? StudyGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public Student(string name, decimal monthlyFee, bool isScholarshipRecipient, int schoolId, int studyGroupId)
        : base(name)
        {
            IsScholarshipRecipient = isScholarshipRecipient;
            MonthlyFee = IsScholarshipRecipient ? 0 : monthlyFee;
            SchoolId = schoolId;
            StudyGroupId = studyGroupId;
        }

        protected Student() {  }

        public void TransferToSchool(int newSchoolId, int newStudyGroupId)
        {
            if (SchoolId == newSchoolId && StudyGroupId == newStudyGroupId) return;

            SchoolId = newSchoolId;
            StudyGroupId = newStudyGroupId;
        }

        public void ChangeStudyGroup(int newStudyGroupId)
        {
            if (StudyGroupId == newStudyGroupId) return;

            StudyGroupId = newStudyGroupId;
        }


        public void UpdateFinancials(decimal newFee, bool isScholarship)
        {
            IsScholarshipRecipient = isScholarship;

            if (IsScholarshipRecipient)
            {
                MonthlyFee = 0;
            }
            else
            {
                if (newFee < 500) throw new ArgumentException("Mensalidade mínima é 500.");
                MonthlyFee = newFee;
            }
        }

        public void UpdateFullProfile(string newName, decimal newFee, int newSchoolId, int newStudyGroupId, bool isScholarship)
        {
            UpdateName(newName);

            if (SchoolId != newSchoolId)
            {
                TransferToSchool(newSchoolId, newStudyGroupId);
            }

            else if (StudyGroupId != newStudyGroupId)
            {
                ChangeStudyGroup(newStudyGroupId);
            }

            UpdateFinancials(newFee, isScholarship);
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
