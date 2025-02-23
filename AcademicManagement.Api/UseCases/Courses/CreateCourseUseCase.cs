using AcademicManagement.Api.Entities;
using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Request;
using AcademicManagement.Communication.Response;

namespace AcademicManagement.Api.UseCases.Courses
{
    public class CreateCourseUseCase
    {
        private readonly DataBaseContext _dbContext;

        public CreateCourseUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<ResponseCourseJson> Execute(RequestCourseJson request)
        {
            var entity = new Course
            {
                Name = request.Name,
                Description = request.Description
            };

            _dbContext.Courses.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new ResponseCourseJson
            {
                Name = entity.Name,
                Description = entity.Description
            };
        }
    }
}
