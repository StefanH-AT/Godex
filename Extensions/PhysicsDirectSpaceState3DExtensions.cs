using Godot;

namespace Godex.Extensions;

public static class PhysicsDirectSpaceState3DExtensions
{
    public static IntersectRayResult? IntersectRayExt(this PhysicsDirectSpaceState3D state, PhysicsRayQueryParameters3D param)
    {
        var dictionary = state.IntersectRay(param);
        if (dictionary.Count == 0)
        {
            return null;
        }

        // physics_server_3d.cpp 358
        var result = new IntersectRayResult()
        {
            Rid = dictionary["rid"].AsRid(),
            Position = dictionary["position"].AsVector3(),
            Normal = dictionary["normal"].AsVector3(),
            Collider = dictionary["collider"].AsGodotObject(),
            ColliderId = dictionary["collider_id"].AsUInt64(),
            Shape = dictionary["shape"].AsInt32(),
            FaceIndex = dictionary["face_index"].AsInt32(),
        };

        return result;
    }

    public static IntersectShapeResult[]? IntersectShapeExt(this PhysicsDirectSpaceState3D state, PhysicsShapeQueryParameters3D param)
    {
        var array = state.IntersectShape(param);
        if (array is null) return null;

        IntersectShapeResult[] results = new IntersectShapeResult[array.Count];

        for (int i = 0; i < array.Count; i++)
        {
            // physics_server_3d.cpp 164
            var result = new IntersectShapeResult()
            {
                Rid = array[i]["rid"].AsRid(),
                Collider = array[i]["collider"].AsGodotObject(),
                ColliderId = array[i]["collider_id"].AsUInt64(),
                Shape = array[i]["shape"].AsInt32(),
            };
            results[i] = result;
        }

        return results;
    }
}

public class IntersectRayResult
{
    public Rid Rid;
    public Vector3 Position;
    public Vector3 Normal;
    public GodotObject? Collider;
    public ulong ColliderId;
    public int Shape;
    public int FaceIndex;
}

public class IntersectShapeResult
{
    public Rid Rid;
    public GodotObject? Collider;
    public ulong ColliderId;
    public int Shape;
}