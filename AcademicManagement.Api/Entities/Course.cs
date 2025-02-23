namespace AcademicManagement.Api.Entities
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
