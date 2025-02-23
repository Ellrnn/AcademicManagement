using System.Net;

namespace AcademicManagement.Exception
{
    public abstract class AcademicManagementException : SystemException
    {
        public AcademicManagementException(string? message) : base(message)
        {
        }

        public abstract List<string> GetErrorMessages();
            public abstract HttpStatusCode GetStatusCode();
    }
}
