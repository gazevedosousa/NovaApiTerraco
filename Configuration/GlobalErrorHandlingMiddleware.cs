using System.Net;
using System.Text.Json;
using TerracoDaCida.Exceptions;

namespace TerracoDaCida.Configuration
{
    public class GlobalErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Type exceptionType = exception.GetType();

            int status = (int)HttpStatusCode.InternalServerError;

            if (exceptionType == typeof(NotFoundException))
            {
                status = (int)HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = (int)HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(BadRequestException))
            {
                status = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(ForbiddenException))
            {
                status = (int)HttpStatusCode.Forbidden;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;

            string exceptionResult = JsonSerializer.Serialize(new CustomResponse(status, false, exception.Message));

            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
