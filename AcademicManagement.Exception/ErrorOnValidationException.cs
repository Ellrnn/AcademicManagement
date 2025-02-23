using System.Net;

namespace AcademicManagement.Exception
{
    public class ErrorOnValidationException : AcademicManagementException
    {
        private readonly List<string> _errors;
        public ErrorOnValidationException(List<string> errorMessages) 
            : base(string.Join(", ", errorMessages))
        {
            _errors = errorMessages;
        }
        public override List<string> GetErrorMessages() => _errors;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
