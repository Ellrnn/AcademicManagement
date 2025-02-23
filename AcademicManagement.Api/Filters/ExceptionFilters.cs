using AcademicManagement.Communication.Response;
using AcademicManagement.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AcademicManagement.Api.Filters
{
    public class ExceptionFilters : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AcademicManagementException libraryException)
            {
                context.HttpContext.Response.StatusCode = (int)libraryException.GetStatusCode();
                context.Result = new ObjectResult(new ResponseErrorMessageJson
                {
                    Errors = libraryException.GetErrorMessages()
                });
            }
            else
            {
                var errorMessage = context.Exception.Message;
                if (context.Exception.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + context.Exception.InnerException.Message;
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Result = new ObjectResult(new ResponseErrorMessageJson
                {
                    Errors = [errorMessage]
                });
            }
        }
    }
}