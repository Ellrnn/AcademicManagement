using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Exception;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Students
{
    public class DeleteStudentUseCase
    {
        private readonly DataBaseContext _dbContext;

        public DeleteStudentUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task Execute(Guid studentId)
        {
            var entity = await _dbContext.Students
                .FirstOrDefaultAsync(student => student.Id == studentId)
                ?? throw new NotFoundException("Aluno não encontrado.");

            _dbContext.Students.Remove(entity);
            await _dbContext.SaveChangesAsync();
      
        }

    }
}
