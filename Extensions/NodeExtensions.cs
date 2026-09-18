using Godot;

namespace Godex.Extensions;

public static class NodeExtensions
{
    public static void QueueFreeChildren(this Node node)
    {
        foreach (var child in node.GetChildren())
        {
            child.QueueFree();
        }
    }
}