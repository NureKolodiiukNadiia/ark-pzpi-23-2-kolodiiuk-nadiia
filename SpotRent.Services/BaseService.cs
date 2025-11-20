using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using SpotRent.Infrastructure;

namespace SpotRent.Services;

public abstract class BaseService<TService>
{
    protected readonly SpotRentDbContext Context;

    protected readonly ILogger<TService> Logger;

    protected BaseService(SpotRentDbContext context, ILogger<TService> logger)
    {
        Context = context;
        Logger = logger;
    }

    protected void LogMessage(
        LogLevel logLevel,
        EventId eventId,
        [StringSyntax("StructuredLogMessageTemplate")]
        string message)
    {
        var action = LoggerMessage.Define(logLevel, eventId, message);
        action.Invoke(Logger, null);
    }

    protected void LogId(
        LogLevel logLevel,
        EventId eventId,
        int id,
        [StringSyntax("StructuredLogMessageTemplate")]
        string message)
    {
        var action = LoggerMessage.Define<int>(logLevel, eventId, message);
        action.Invoke(Logger, id, null);
    }

    protected void LogIdWithCount(
        LogLevel logLevel,
        EventId eventId,
        int id,
        int count,
        [StringSyntax("StructuredLogMessageTemplate")]
        string message)
    {
        var action = LoggerMessage.Define<int, int>(logLevel, eventId, message);
        action.Invoke(Logger, id, count, null);
    }

    protected void LogIdWithMessage(
        LogLevel logLevel,
        EventId eventId,
        int id,
        string loggedMessage,
        [StringSyntax("StructuredLogMessageTemplate")]
        string message)
    {
        var action = LoggerMessage.Define<int, string>(logLevel, eventId, message);
        action.Invoke(Logger, id, loggedMessage, null);
    }
}
