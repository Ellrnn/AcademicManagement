using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Request;
using AcademicManagement.Communication.Response;
using AcademicManagement.Exception;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Courses
{
    public class UpdateCourseUseCase
    {
        private readonly DataBaseContext _dbContext;

        public UpdateCourseUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ResponseCourseJson> Execute(Guid courseId, RequestCourseJson request)
        {
            var entity = await _dbContext.Courses
                .FirstOrDefaultAsync(course => course.Id == courseId)
                ?? throw new NotFoundException("Curso não encontrado.");

            if (request.Name != null && !string.IsNullOrWhiteSpace(request.Name))
                entity.Name = request.Name;
            
            if (request.Description != null && !string.IsNullOrWhiteSpace(request.Description))
                entity.Description = request.Description;

            _dbContext.Courses.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new ResponseCourseJson
            {
                Name = entity.Name,
                Description = entity.Description,
            };
        }
    }
}
