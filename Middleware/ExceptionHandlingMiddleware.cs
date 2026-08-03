using System.Net;
using System.Text.Json;
using CinemaTicketBookingAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBookingAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            MovieNotFoundException => (HttpStatusCode.NotFound, "Movie Not Found"),
            BookingNotFoundException => (HttpStatusCode.NotFound, "Booking Not Found"),
            ShowTimeNotFoundException => (HttpStatusCode.NotFound, "ShowTime Not Found"),
            CustomerNotFoundException => (HttpStatusCode.NotFound, "Customer Not Found"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource Not Found"),

            MovieAlreadyExistsException => (HttpStatusCode.Conflict, "Movie Already Exists"),
            InvalidOperationException => (HttpStatusCode.Conflict, "Conflict"),

            InvalidBookingException => (HttpStatusCode.UnprocessableEntity, "Invalid Booking"),

            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception");
        else
            _logger.LogWarning(exception, "Handled exception: {Title}", title);

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{(int)statusCode}"
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}