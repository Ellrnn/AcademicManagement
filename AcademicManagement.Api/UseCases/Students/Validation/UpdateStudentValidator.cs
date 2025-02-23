using AcademicManagement.Communication.Request;
using FluentValidation;

namespace AcademicManagement.Api.UseCases.Students
{
    public class UpdateStudentValidator : AbstractValidator<RequestUpdateStudentJson>
    {
        private const int MinimumLegalAge = 18;

        public UpdateStudentValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().When(r => r.Name != null && !string.IsNullOrWhiteSpace(r.Name))
                .WithMessage("O nome não pode ser vazio.");

            RuleFor(request => request.Email)
                .EmailAddress().When(r => r.Email != null && !string.IsNullOrWhiteSpace(r.Email))
                .WithMessage("O email é inválido.");

            RuleFor(request => request.DateBirth)
                .Must(date => BeOfLegalAge(date))
                .When(r => r.DateBirth.HasValue)
                .WithMessage("O aluno deve ter no mínimo 18 anos.");
        }

        private bool BeOfLegalAge(DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
                return true;

            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Value.Year;

            if (dateOfBirth.Value.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= MinimumLegalAge;
        }
    }
}