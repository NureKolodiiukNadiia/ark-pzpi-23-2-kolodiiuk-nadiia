using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace SpotRent.Api.Controllers;

public abstract class BaseController<TController> : ControllerBase
{
    protected readonly ILogger<TController> Logger;

    protected BaseController(ILogger<TController> logger)
    {
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
