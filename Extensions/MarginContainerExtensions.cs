using Godot;

namespace Godex.Extensions;

public static class MarginContainerExtensions
{
    public static void SetMargin(this MarginContainer container, int margin)
    {
        container.AddThemeConstantOverride("margin_left", margin);
        container.AddThemeConstantOverride("margin_right", margin);
        container.AddThemeConstantOverride("margin_top", margin);
        container.AddThemeConstantOverride("margin_bottom", margin);
    }
}