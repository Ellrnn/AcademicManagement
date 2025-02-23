using AcademicManagement.Api.Entities;
using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Request;
using AcademicManagement.Exception;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Enrollments
{
    public class EnrollStudentInCoursesUseCase
    {
        private readonly DataBaseContext _dbContext;

        public EnrollStudentInCoursesUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Execute(RequestEnrollStudentJson request)
        {
            var student = await _dbContext.Students
                .FirstOrDefaultAsync(student => student.Id == request.StudentId)
                ?? throw new NotFoundException("Aluno não encontrado.");

            foreach (var courseId in request.CourseIds)
            {
                var course = await _dbContext.Courses
                    .FirstOrDefaultAsync(course => course.Id == courseId)
                    ?? throw new NotFoundException("Curso não encontrado.");

                var existingEnrollment = await _dbContext.Enrollments
                    .FirstOrDefaultAsync(enroll => enroll.StudentId == request.StudentId && enroll.CourseId == courseId);

                if (existingEnrollment != null)
                {
                    throw new ErrorOnValidationException(["O aluno já está matriculado neste curso."]);
                }

                var enrollment = new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = request.StudentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow
                };

                _dbContext.Enrollments.Add(enrollment);
            }
            
            await _dbContext.SaveChangesAsync();
        }
    }
}
