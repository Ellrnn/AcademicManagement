namespace AcademicManagement.Communication.Request
{
    public class RequestEnrollStudentJson
    {
            public Guid StudentId { get; set; }
            public List<Guid> CourseIds { get; set; } = new List<Guid>(); 
    }
}
