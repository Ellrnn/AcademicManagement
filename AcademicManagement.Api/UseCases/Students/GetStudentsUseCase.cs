using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Response;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Students
{
    public class GetStudentsUseCase
    {
        private readonly DataBaseContext _dbContext;

        public GetStudentsUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<ResponseStudentJson>> Execute()
        {
            return await _dbContext.Students
                .OrderBy(student => student.Name)
                .Select(student => new ResponseStudentJson
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    DateBirth = student.DateBirth
                })
                .ToListAsync();
        }
    }
}
