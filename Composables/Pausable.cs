using Godot;

namespace Godex.Composables;

public class Pausable
{
    private double _fixedDelta;

    private double _lastUpdateTimeMillis;
    public bool Paused { get; set; }

    private uint? _resumedTicksRemaining;
    
    /// <param name="delta">Time in seconds that has passed since the last time this function was called</param>
    public delegate void UpdateFunction(double delta);

    private readonly UpdateFunction _onUnpausedUpdate;

    public Pausable(UpdateFunction onUnpausedUpdate, double fixedDelta = 0)
    {
        _onUnpausedUpdate = onUnpausedUpdate;
        _fixedDelta = fixedDelta;
    }

    public void AttachToController(PausableController pausableController)
    {
        pausableController.Attach(this);
    }
    
    public void DetachFromController(PausableController pausableController)
    {
        pausableController.Detach(this);
    }
    
    public void Update()
    {
        if (!ShouldUpdate()) return;

        if (_fixedDelta > 0)
        {
            _onUnpausedUpdate(_fixedDelta);
            return;
        }
        
        float now = Time.GetTicksMsec();
        double delta = 0;
        if (_lastUpdateTimeMillis != 0) 
        {
            delta = now - _lastUpdateTimeMillis;
        }

        _lastUpdateTimeMillis = now;
        _onUnpausedUpdate(delta / 1000);
    }

    private bool ShouldUpdate()
    {
        if (Paused) return false;

        if (_resumedTicksRemaining is null) return true;

        if (_resumedTicksRemaining.Value > 0)
        {
            _resumedTicksRemaining = _resumedTicksRemaining.Value - 1;
            return true;
        }

        return false;
    }
    
    public void Pause()
    {
        Paused = true;
        _lastUpdateTimeMillis = Time.GetTicksMsec();
    }

    public void Resume(uint? resumeForThisManyTicks = null)
    {
        Paused = false;
        _resumedTicksRemaining = resumeForThisManyTicks;
    }
}