using Microsoft.Extensions.Logging;

namespace Development.Suite.Logging;

public interface IDevelopmentSuiteLogger<out TClass> : IDevelopmentSuiteLogger, ILogger<TClass> where TClass : class
{
}

public interface IDevelopmentSuiteLogger : ILogger
{
    void LogDebug(Exception exception, string message, params object[] args);
    void LogDebug(string message, params object[] args);
    void LogTrace(Exception exception, string message, params object[] args);
    void LogTrace(string message, params object[] args);
    void LogInformation(Exception exception, string message, params object[] args);
    void LogInformation(string message, params object[] args);
    void LogWarning(Exception exception, string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogCritical(Exception exception, string message, params object[] args);
    void LogCritical(string message, params object[] args);
}

public class DevelopmentSuiteLogger<TClass> : IDevelopmentSuiteLogger<TClass> where TClass : class
{
    private readonly ILogger<TClass> _logger;

    public DevelopmentSuiteLogger(ILogger<TClass> logger)
    {
        _logger = logger;
    }

    public void LogDebug(Exception exception, string message, params object[] args)
    {
        _logger.Log(LogLevel.Debug, exception, message, args);
    }

    public void LogDebug(string message, params object[] args)
    {
        _logger.Log(LogLevel.Debug, message, args);
    }

    public void LogTrace(Exception exception, string message, params object[] args)
    {
        _logger.Log(LogLevel.Trace, exception, message, args);
    }

    public void LogTrace(string message, params object[] args)
    {
        _logger.Log(LogLevel.Trace, message, args);
    }

    public void LogInformation(Exception exception, string message, params object[] args)
    {
        _logger.Log(LogLevel.Information, exception, message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.Log(LogLevel.Information, message, args);
    }

    public void LogWarning(Exception exception, string message, params object[] args)
    {
        _logger.Log(LogLevel.Warning, exception, message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.Log(LogLevel.Warning, message, args);
    }

    public void LogError(Exception exception, string message, params object[] args)
    {
        _logger.Log(LogLevel.Error, exception, message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.Log(LogLevel.Error, message, args);
    }

    public void LogCritical(Exception exception, string message, params object[] args)
    {
        _logger.Log(LogLevel.Critical, exception, message, args);
    }

    public void LogCritical(string message, params object[] args)
    {
        _logger.Log(LogLevel.Critical, message, args);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return _logger.BeginScope(state);
    }
}