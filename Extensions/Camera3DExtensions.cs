using Godot;
using Godot.Collections;

namespace Godex.Extensions;

public static class Camera3DExtensions
{
    public static Vector3? ProjectPlane(this Camera3D camera, Vector2 mouseXy, Plane plane)
    {
        var origin = camera.ProjectRayOrigin(mouseXy);
        var normal = camera.ProjectRayNormal(mouseXy);
        return plane.IntersectsRay(origin, normal);
    }

    public static IntersectRayResult? TraceRay(this Camera3D camera, Vector2 mouseXy, uint mask = 1, bool collideWithAreas = false, float? selectionDistance = null)
    {
        var from = camera.ProjectRayOrigin(mouseXy);
        var to = camera.ProjectRayNormal(mouseXy);
        var ray = PhysicsRayQueryParameters3D.Create(from, to * (selectionDistance ?? camera.Far), mask);
        ray.CollideWithAreas = collideWithAreas;
        
        var spaceState = PhysicsServer3D.SpaceGetDirectState(camera.GetWorld3D().Space);
        return spaceState.IntersectRayExt(ray);
    }

    public static IntersectRayResult? TraceRayForward(this Camera3D camera, uint mask = 1, bool collideWithAreas = false, float? selectionDistance = null, Array<Rid>? exclude = null)
    {
        var from = camera.GlobalPosition;
        var to = from + camera.GlobalTransform.Basis.Z * -(selectionDistance ?? camera.Far);
        var ray = PhysicsRayQueryParameters3D.Create(from, to, mask);
        ray.CollideWithAreas = collideWithAreas;
        if (exclude is not null)
        {
            ray.Exclude = exclude;
        }

        var spaceState = PhysicsServer3D.SpaceGetDirectState(camera.GetWorld3D().Space);
        return spaceState.IntersectRayExt(ray);
    }

    public static Vector2 GetCenterMousePosition(this Camera3D camera)
    {
        return camera.GetViewport().GetWindow().Size / 2;
    }
}