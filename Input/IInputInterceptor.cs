using Godot;

namespace Godex.Input;

public interface IInputInterceptor
{
    public bool OnInput(InputEvent input);
}