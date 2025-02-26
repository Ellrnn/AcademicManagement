namespace AcademicManagement.Communication.Response
{
    public class ResponseStudentJson
    {
        public Guid Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateBirth { get; set; }
    }
}
