using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Communication.Request;
using AcademicManagement.Communication.Response;
using AcademicManagement.Exception;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Students
{
    public class UpdateStudentUseCase
    {
        private readonly DataBaseContext _dbContext;

        public UpdateStudentUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ResponseStudentJson> Execute(Guid studentId, RequestUpdateStudentJson request)
        {

            await Validate(request, studentId);

            var entity = await _dbContext.Students
                .FirstOrDefaultAsync(student => student.Id == studentId)
                ?? throw new NotFoundException("Aluno não encontrado.");

            if (request.Name != null && !string.IsNullOrWhiteSpace(request.Name))
                entity.Name = request.Name;

            if (request.Email != null && !string.IsNullOrWhiteSpace(request.Email))
                entity.Email = request.Email;

            if (request.DateBirth.HasValue)
                entity.DateBirth = DateTime.SpecifyKind(request.DateBirth.Value, DateTimeKind.Utc);

            entity.DateBirth = DateTime.SpecifyKind(entity.DateBirth, DateTimeKind.Utc);

            _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new ResponseStudentJson
            {
                Name = entity.Name,
                Email = entity.Email,
                DateBirth = entity.DateBirth
            };
        }

        private async Task Validate(RequestUpdateStudentJson request, Guid studentId)
        {
            var validator = new UpdateStudentValidator(); 
            var result = validator.Validate(request);

            if (request.Email != null)
            {
                var userWithExistingEmail = await _dbContext.Students
                    .AnyAsync(student => student.Email.Equals(request.Email) && student.Id != studentId);
                if (userWithExistingEmail)
                    result.Errors.Add(new ValidationFailure("email", "Email já existe."));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}