using Godex.Composables;
using Godot;

namespace Godex.Input;

public interface IInputReceiver
{
    public void OnInput(InputEvent input);
    public int InputId { get; set; }
}

public interface IInputHandler : IInputReceiver
{
    public Activatable Activatable { get; set; }
}

