using AcademicManagement.Api.Entities;
using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Api.UseCases.Students.Validation;
using AcademicManagement.Communication.Request;
using AcademicManagement.Communication.Response;
using AcademicManagement.Exception;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagement.Api.UseCases.Students
{
    public class RegisterStudentsUseCase
    {
        private readonly DataBaseContext _dbContext;

        public RegisterStudentsUseCase(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ResponseStudentJson> Execute(RequestStudentJson request)
        {
            await Validate(request, _dbContext);

            var dateUtc = DateTime.SpecifyKind(request.DateBirth, DateTimeKind.Utc);

            var entity = new Student
            {
                Name = request.Name,
                DateBirth = dateUtc,
                Email = request.Email
            };

            _dbContext.Students.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new ResponseStudentJson
            {
                Name = entity.Name,
                Email = entity.Email,
                DateBirth = dateUtc
            };
        }

        private async Task Validate(RequestStudentJson request, DataBaseContext dbContext)
        {
            var validator = new RegisterStudentValidator();
            var result = validator.Validate(request);

            var userWithExistingEmail = await dbContext.Students
                .AnyAsync(student => student.Email.Equals(request.Email));
            if (userWithExistingEmail)
                result.Errors.Add(new ValidationFailure("email", "Email já existe."));

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}