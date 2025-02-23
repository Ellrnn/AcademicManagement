using AcademicManagement.Communication.Request;
using FluentValidation;

namespace AcademicManagement.Api.UseCases.Students.Validation
{
    public class RegisterStudentValidator : AbstractValidator<RequestStudentJson>
    {
        private const int MinimumLegalAge = 18;
        public RegisterStudentValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("O nome é obrigatório.");
            RuleFor(request => request.Email).EmailAddress().WithMessage("O email é invalido.");
            RuleFor(request => request.DateBirth).NotEmpty().WithMessage("Data de nascimento é Obrigatória.").Must(BeOfLegalAge).WithMessage("O aluno deve ter no mínimo 18 anos."); ;
        }

        private bool BeOfLegalAge(DateTime dateBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateBirth.Year;

            if (dateBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= MinimumLegalAge;
        }

    }

}
