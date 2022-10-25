using System.Text.Json;
using System.Net;

namespace SharedListApi.Configuration
{
    public class ErrorHandlingMiddleware
    {

        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        //handle http error
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        //handle exceptions

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "An error occurred while processing your request.";
            var exceptionType = exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "You are not authorized to access this resource";
            }
            else if (exceptionType == typeof(ArgumentException))
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
            }
            else if (exceptionType == typeof(InvalidOperationException))
            {
                statusCode = HttpStatusCode.NotFound;
                errorMessage = exception.Message;
            }
            else if (exceptionType == typeof(TimeoutException))
            {
                statusCode = HttpStatusCode.RequestTimeout;
                errorMessage = exception.Message;
            }
           
            else if (exceptionType == typeof(NotSupportedException))
            {
                statusCode = HttpStatusCode.NotImplemented;
                errorMessage = exception.Message;
            }
            else if (exceptionType == typeof(InvalidDataException))
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
            }          

            else if (exceptionType == typeof(ArgumentNullException))
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
            }     
           
          
            else if (exceptionType == typeof(InvalidOperationException))
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
            }
            else if (exceptionType == typeof(InvalidCastException))
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
            }
            else if (exceptionType == typeof(FormatException))
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = exception.Message;
            }
         




            var exceptionResult = JsonSerializer.Serialize(new { error = errorMessage });


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(exceptionResult);
        }
    }

}

