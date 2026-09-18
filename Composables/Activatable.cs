namespace Godex.Composables;

public class Activatable
{
    public bool IsActive { get; protected set; }

    public delegate void OnChange(bool active);
    
    private readonly OnChange? _changeAction;

    public Activatable(bool startActive = false)
    {
        IsActive = startActive;
    }
    
    public Activatable(OnChange changeAction)
    {
        _changeAction = changeAction;
    }
    
    public Activatable(bool startActive, OnChange changeAction)
    {
        IsActive = startActive;
        _changeAction = changeAction;
    }

    public void Activate()
    {
        if (IsActive) return;
        
        IsActive = true;
        if (_changeAction is not null)
        {
            _changeAction(true);
        }
    }

    public void Deactivate()
    {
        if (!IsActive) return;
        
        IsActive = false;
        if (_changeAction is not null)
        {
            _changeAction(false);
        }
    }

    public void Toggle()
    {
        if (IsActive)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

}