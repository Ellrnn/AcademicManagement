using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Exception;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Courses
{
    public class DeleteCourseUseCase
    {
        private readonly DataBaseContext _dbContext;

        public DeleteCourseUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Execute(Guid courseId)
        {
            var entity = await _dbContext.Courses
                .FirstOrDefaultAsync(course => course.Id == courseId)
                ?? throw new NotFoundException("Curso não encontrado.");

            _dbContext.Courses.Remove(entity);
            await _dbContext.SaveChangesAsync();

        }
    }
}
