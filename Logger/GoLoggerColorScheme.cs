namespace Godex.Logger;

public class GoLoggerColorScheme
{
    public string DefaultColor { get; set; } = GoLoggerColor.White;
    
    public string TraceTagColor { get; set; } = GoLoggerColor.Gray;
    public string TraceArgColor { get; set; } = GoLoggerColor.Gray;
    public string DebugTagColor { get; set; } = GoLoggerColor.Pink;
    public string DebugArgColor { get; set; } = GoLoggerColor.Pink;
    public string InfoTagColor { get; set; } = GoLoggerColor.Cyan;
    public string InfoArgColor { get; set; } = GoLoggerColor.Cyan;
    public string SuccessTagColor { get; set; } = GoLoggerColor.Lime;
    public string SuccessArgColor { get; set; } = GoLoggerColor.Green;
    public string WarningTagColor { get; set; } = GoLoggerColor.Yellow;
    public string WarningArgColor { get; set; } = GoLoggerColor.Orange;
    public string ErrorTagColor { get; set; } = GoLoggerColor.Red;
    public string ErrorArgColor { get; set; } = GoLoggerColor.Orange;
    
    public string GetTagColor(GoLoggerLevel logLevel)
    {
        switch (logLevel)
        {
            case GoLoggerLevel.Trace: return TraceTagColor;
            case GoLoggerLevel.Debug: return DebugTagColor;
            case GoLoggerLevel.Info: return InfoTagColor;
            case GoLoggerLevel.Success: return SuccessTagColor;
            case GoLoggerLevel.Warning: return WarningTagColor;
            case GoLoggerLevel.Error: return ErrorTagColor;
            default: return DefaultColor;
        }
    }
    
    public string GetArgColor(GoLoggerLevel logLevel)
    {
        switch (logLevel)
        {
            case GoLoggerLevel.Trace: return TraceArgColor;
            case GoLoggerLevel.Debug: return DebugArgColor;
            case GoLoggerLevel.Info: return InfoArgColor;
            case GoLoggerLevel.Success: return SuccessArgColor;
            case GoLoggerLevel.Warning: return WarningArgColor;
            case GoLoggerLevel.Error: return ErrorArgColor;
            default: return DefaultColor;
        }
    }
}