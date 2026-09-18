using System.Text;

namespace Godex.Logger;

public class GoLogger
{
#if DEBUG
    public static readonly GoLogger GoLog = new GoLogger(GoLoggerLevel.Trace);
#else
    public static readonly GoLogger GoLog = new GoLogger();
#endif

    private GoLoggerColorScheme _colorScheme;

    public LogSink Sink { get; set; } = (in string message) => Console.WriteLine(message);
    public bool UseBbCode { get; set; }

    public delegate void LogFunction(in string message, params string[] args);
    public delegate void LogSink(in string message);
    public delegate void LogListener(string message);
    public event LogListener LogEvent = _ => {};
    
    public GoLoggerLevel LogLevel { get; set; }
    
    public GoLogger(GoLoggerColorScheme colorScheme, GoLoggerLevel logLevel = GoLoggerLevel.Info)
    {
        LogLevel = logLevel;
        _colorScheme = colorScheme;
    }

    public GoLogger(GoLoggerLevel logLevel = GoLoggerLevel.Info)
    {
        LogLevel = logLevel;
        _colorScheme = new GoLoggerColorScheme();
    }

    /// <summary>
    /// Trace logs give verbose information about what's happening in the engine like inputs or detailed loading progress
    /// </summary>
    public void Trace(in string message, params Object[] args)
    {
        Log(GoLoggerLevel.Trace, message, args);
    }

    /// <summary>
    /// Use this for development/debugging. Never leave debug logs around in the code! Keep them commented out
    /// </summary>
    public void Debug(in string message, params Object[] args)
    {
        Log(GoLoggerLevel.Debug, message, args);
    }

    /// <summary>
    /// General info log.
    /// Use this only to log important information that is relevant to users like announcing loading screens.
    /// Don't spam the console with tons of these every frame
    /// </summary>
    public void Info(in string message, params Object[] args)
    {
        Log(GoLoggerLevel.Info, message, args);
    }

    /// <summary>
    /// Something executed successfully.
    /// Prefer to use this rarely and mainly for engine systems like loading levels or initializing game services.
    /// Could also be used for downloads or anything else that is expected to succeed or fail, like any IO device
    /// </summary>
    public void Okay(in string message, params Object[] args)
    {
        Log(GoLoggerLevel.Success, message, args);
    }

    /// <summary>
    /// Warn when something goes wrong that the application can recover from or disable some features.
    /// For example if a required exported property isn't defined or has an invalid value, log a warning and disable whatever features need that content
    /// </summary>
    public void Warning(in string message, params Object[] args)
    {
        Log(GoLoggerLevel.Warning, message, args);
    }

    /// <summary>
    /// Only create error logs when the application cannot recover and must exit.
    /// Do not log errors if the application can keep running even if content won't work as expected
    /// </summary>
    public void Error(in string message, params Object[] args)
    {
        Log(GoLoggerLevel.Error, message, args);
    }

    public void Log(in GoLoggerLevel level, in string message, params Object[] args)
    {
        if (level < LogLevel) return;

        string log = BuildLog(level, message, args, UseBbCode);
        Sink.Invoke(log);
        LogEvent.Invoke(log);
    }

    public string BuildLog(in GoLoggerLevel level, in string message, in Object[]? args, in bool useBbCode)
    {
        StringBuilder builder = new StringBuilder();

        if (useBbCode)
        {
            builder.Append("[color=");
            builder.Append(_colorScheme.GetTagColor(level));
            builder.Append(']');
        }
        
        builder.Append('[');
        builder.Append(GetLogLevelLabel(level));
        builder.Append(']');
        
        if (useBbCode)
        {
            builder.Append("[/color]");
        }
        
        builder.Append(' ');


        if (args is null || args.Length == 0)
        {
            builder.Append(message);
            return builder.ToString();
        }

        ResolveArgs(level, builder, message, args, useBbCode);
        return builder.ToString();
    }

    private void ResolveArgs(in GoLoggerLevel level, in StringBuilder stringBuilder, in string message, in Object[] args, in bool useBbCode)
    {
        int placeholderIndex = 0;
        bool escaped = false;
        
        foreach (var c in message)
        {
            if (c == '\\')
            {
                escaped = !escaped;
                if (escaped) continue;
            }

            if (c == '?' && placeholderIndex < args.Length)
            {
                if (useBbCode)
                {
                    stringBuilder.Append("[color=");
                    stringBuilder.Append(_colorScheme.GetArgColor(level));
                    stringBuilder.Append(']');
                }
                stringBuilder.Append(args[placeholderIndex]);
                if (useBbCode)
                {
                    stringBuilder.Append("[/color]");
                }
                placeholderIndex++;
                continue;
            }

            stringBuilder.Append(c);
        }
    }

    public bool IsEnabled(in GoLoggerLevel logLevel)
    {
        return (int) logLevel >= (int) LogLevel;
    }

    private string GetLogLevelLabel(in GoLoggerLevel level)
    {
        switch (level)
        {
            case GoLoggerLevel.Trace: return "TRCE";
            case GoLoggerLevel.Debug: return "DEBG";
            case GoLoggerLevel.Info: return "INFO";
            case GoLoggerLevel.Success: return " OK ";
            case GoLoggerLevel.Warning: return "WARN";
            case GoLoggerLevel.Error: return "ERR!";
            default: return "INVL";
        }
    }
    
    // Special case logs

    /// <summary>
    /// Writes warning message and returns what you give it. Useful for chaining commands
    /// </summary>
    public T WarningReturn<T>(in string message, T ret, params string[] args)
    {
        Log(GoLoggerLevel.Warning, message, args);
        return ret;
    }
}
