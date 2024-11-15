using Meowgic.Data.Models.Response;
using Meowgic.Shares.Enum;
using Meowgic.Shares.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Meowgic.API.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                //if (token != null)
                //{
                //    context.Items["User"] = token;
                //}
                await _next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong while processing {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorResponse
            {
                ErrorType = ErrorType.InternalServerError.ToString(),
                ErrorMessage = exception.Message,
                StackTrace = exception.StackTrace
            };

            switch (exception)
            {
                case NotFoundException _:
                    errorResponse.ErrorType = ErrorType.NotFound.ToString();
                    errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException _:
                    errorResponse.ErrorType = ErrorType.BadRequest.ToString();
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedException _:
                    errorResponse.ErrorType = ErrorType.Unauthorized.ToString();
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ForbiddenMethodException _:
                    errorResponse.ErrorType = ErrorType.ForbiddenMethod.ToString();
                    errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                default:
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var response = JsonConvert.SerializeObject(errorResponse);
            context.Response.StatusCode = errorResponse.StatusCode;

            return context.Response.WriteAsync(response);
        }
    }

}
