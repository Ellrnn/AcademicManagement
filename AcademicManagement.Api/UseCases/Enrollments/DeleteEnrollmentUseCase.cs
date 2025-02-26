using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Exception;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Enrollments
{
    public class DeleteEnrollmentUseCase
    {
        private readonly DataBaseContext _dbContext;

        public DeleteEnrollmentUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task Execute(Guid enrollmentId)
        {
            var enrollment = await _dbContext.Enrollments
                .FirstOrDefaultAsync(e => e.Id == enrollmentId)
                ?? throw new NotFoundException("Matrícula não encontrada para o aluno e curso especificados.");

            _dbContext.Enrollments.Remove(enrollment);
            await _dbContext.SaveChangesAsync(); 
        }
    }
}
