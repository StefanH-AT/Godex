using System.Globalization;
using Godex.Logger;
using Godot;

namespace Godex.Composables;

public class LoadingStages
{
    private LoadingStage[] _stages;

    public uint CurrentStageNumber;
    public LoadingStage CurrentStage => _stages[CurrentStageNumber];
    public uint TotalStageCount => (uint) _stages.Length;

    public bool IsComplete => CurrentStageNumber >= TotalStageCount;
    
    private double _currentStageStartedTime;
    
    public LoadingStages(LoadingStage[] stages)
    {
        _stages = stages;
    }

    public void Start()
    {
        _currentStageStartedTime = Time.GetTicksUsec();
    }
    
    /// <returns>True when successfully advanced to next stage. False when already complete</returns>
    public bool NextStage(GoLogger.LogFunction? logFunction = null)
    {
        if (IsComplete) return false;
        double currentTime = Time.GetTicksUsec();
        _stages[CurrentStageNumber].LoadTime = TimeSpan.FromMicroseconds(currentTime - _currentStageStartedTime);
        if(logFunction is not null) _stages[CurrentStageNumber].PrintLog(logFunction);

        CurrentStageNumber++;
        _currentStageStartedTime = currentTime;

        return true;
    }

    public LoadingStage[] GetStages()
    {
        return _stages;
    }

    public TimeSpan GetTotalTime()
    {
        double totalMicroseconds = 0;
        foreach (var stage in _stages)
        {
            if (stage.LoadTime is null) continue;
            totalMicroseconds += stage.LoadTime.Value.TotalMicroseconds;
        }
        
        return TimeSpan.FromMicroseconds(totalMicroseconds);
    }

    public void PrintSummary(GoLogger.LogFunction log)
    {
        int longestName = _stages.Max(s => s.Name.Length);
        foreach (var stage in _stages)
        {
            log("- ? took ?s", stage.Name.PadRight(longestName), FormatTime(stage.LoadTime!.Value));
        }

        log("Total time: ?s", FormatTime(GetTotalTime()));
    }

    public static string FormatTime(TimeSpan span)
    {
        return span.TotalSeconds.ToString("F", CultureInfo.InvariantCulture);
    }
}

public record struct LoadingStage(string Name)
{

    public string Name = Name;
    public TimeSpan? LoadTime = null;

    public void PrintLog(GoLogger.LogFunction log)
    {
        if (LoadTime is not null)
        {
            log("(?) finished in ?s", Name, LoadingStages.FormatTime(LoadTime!.Value));
        }
        else
        {
            log("? has not finished yet", Name);
        }
    }
}
