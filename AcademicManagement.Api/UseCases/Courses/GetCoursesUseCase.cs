using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Response;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Courses
{
    public class GetCoursesUseCase
    {
        private readonly DataBaseContext _dbContext;

        public GetCoursesUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<ResponseCourseJson>> Execute()
        {
            return await _dbContext.Courses
                .OrderBy(course => course.Name)
                .Select(course => new ResponseCourseJson
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                })
                .ToListAsync();
        }
    }
}
