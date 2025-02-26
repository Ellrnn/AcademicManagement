using AcademicManagement.Api.Entities;
using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Response;
using AcademicManagement.Exception;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Courses
{
    public class GetCourseStudentsUseCase
    {
        private readonly DataBaseContext _dbContext;

        public GetCourseStudentsUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<ResponseStudentJson>> Execute(Guid courseId)
        {
            var students = await _dbContext.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => new ResponseStudentJson
                {
                    Id = e.Student.Id,
                    EnrollmentId = e.Id,
                    Name = e.Student.Name,
                    Email = e.Student.Email,
                    DateBirth = e.Student.DateBirth 
                })
                .OrderBy(student => student.Name)
                .ToListAsync();

            if (!students.Any())
            {
                throw new NotFoundException("Nenhum aluno matriculado encontrado para este curso.");
            }

            return students;
        }
    }
}
