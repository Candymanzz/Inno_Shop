using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Exceptions;
using FluentValidation;

namespace ProductService.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
                });
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                });
            }
        }
    }

}
