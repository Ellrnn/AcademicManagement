namespace AcademicManagement.Communication.Request
{
    public class RequestStudentJson
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateBirth { get; set; }
    }
}
