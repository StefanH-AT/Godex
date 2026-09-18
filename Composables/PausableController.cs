using Godex.Logger;

namespace Godex.Composables;

public class PausableController
{
    private List<Pausable> _pausables;

    public PausableController()
    {
        _pausables = new();
    }

    public void PauseAll()
    {
        foreach (var pausable in _pausables)
        {
            pausable.Pause();
        }
    }

    public void ResumeAll(uint? remainingTicks = null)
    {
        foreach (var pausable in _pausables)
        {
            pausable.Resume(remainingTicks);
        }
    }

    public void Attach(Pausable pausable)
    {
        if (_pausables.Contains(pausable))
        {
            GoLogger.GoLog.Warning($"{nameof(Pausable)} {pausable.ToString()} already attached to {nameof(PausableController)} {this.ToString()}");
            return;
        }
    
        _pausables.Add(pausable);
    }

    public void Detach(Pausable pausable)
    {
        if (!_pausables.Contains(pausable))
        {
            GoLogger.GoLog.Warning($"Cannot detach {nameof(Pausable)} {pausable.ToString()} from {nameof(PausableController)} {this.ToString()} as it isn't attached");
            return;
        }
    
        _pausables.Remove(pausable);
    }
}