using MovieTicketBooking.Exceptions;

namespace MovieTicketBooking.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ForbiddenAccessException ex)
        {
            context.Response.StatusCode =
                StatusCodes.Status403Forbidden;

            await context.Response.WriteAsJsonAsync(new
            {
                message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                message = ex.Message
            });
        }
    }
}
