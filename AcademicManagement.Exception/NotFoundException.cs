using System.Net;

namespace AcademicManagement.Exception
{
    public class NotFoundException : AcademicManagementException
    {
        public NotFoundException(string? message) : base(message)
        {
        }

        public override List<string> GetErrorMessages() => new List<string> { Message };

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
