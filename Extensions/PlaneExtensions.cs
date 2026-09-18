using Godot;

namespace Godex.Extensions;

public static class PlaneExtensions
{
    public static bool IsCoplanar(this Plane plane, Plane otherPlane)
    {
        var normal1 = plane.Normal.Normalized();
        var normal2 = otherPlane.Normal.Normalized();

        if (!normal1.IsParallel(normal2))
        {
            return false;
        }

        return Mathf.Abs(plane.D - otherPlane.D) < Mathf.Epsilon;
    }
}