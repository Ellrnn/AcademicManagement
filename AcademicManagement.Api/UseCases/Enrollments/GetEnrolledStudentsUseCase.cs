using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Response;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Enrollments
{
    public class GetEnrolledStudentsUseCase
    {
        private readonly DataBaseContext _dbContext;

        public GetEnrolledStudentsUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<ResponseStudentJson>> Execute()
        {
            var enrolledStudents = await _dbContext.Students
                .OrderBy(student => student.Name)
                .Where(student => student.Enrollments.Any())
                .Select(student => new ResponseStudentJson
                {
                    Name = student.Name,
                    Email = student.Email,
                    DateBirth = student.DateBirth 
                })
                .ToListAsync();

            return enrolledStudents;
        }

    }
}
