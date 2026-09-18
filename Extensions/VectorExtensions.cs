
using Godot;

namespace Godex.Extensions;

public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector4 vec)
    {
        return new Vector3(vec.X, vec.Y, vec.Z);
    }
    public static Vector4 ToVector4(this Vector3 vec)
    {
        return new Vector4(vec.X, vec.Y, vec.Z, 0);
    }

    public static Vector2I ToVector2I(this Vector2 vec)
    {
        return new Vector2I((int) vec.X, (int) vec.Y);
    }

    public static System.Numerics.Vector3 ToSystemVector3(this Godot.Vector3 vec)
    {
        return new System.Numerics.Vector3(vec.X, vec.Y, vec.Z);
    }
    public static System.Numerics.Vector4 ToSystemVector4(this Godot.Vector4 vec)
    {
        return new System.Numerics.Vector4(vec.X, vec.Y, vec.Z, vec.W);
    }

    public static Vector3 Multiply(this Vector3 vector1, Vector3 vector2)
    {
        return new Vector3(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z);
    }
    
    public static Vector3 OmitX(this Vector3 vector)
    {
        return new Vector3(0, vector.Y, vector.Z);
    }
    public static Vector3 OmitY(this Vector3 vector)
    {
        return new Vector3(vector.X, 0, vector.Z);
    }
    public static Vector3 OmitZ(this Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, 0);
    }
    public static Vector3 WithX(this Vector3 vector, float x)
    {
        return new Vector3(x, vector.Y, vector.Z);
    }
    public static Vector3 WithY(this Vector3 vector, float y)
    {
        return new Vector3(vector.X, y, vector.Z);
    }
    public static Vector2 WithX(this Vector2 vector, float x)
    {
        return new Vector2(x, vector.Y);
    }
    public static Vector2 WithY(this Vector2 vector, float y)
    {
        return new Vector2(vector.X, y);
    }
    public static Vector3 WithZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.X, vector.Y, z);
    }
    public static Vector3 OnlyX(this Vector3 vector)
    {
        return new Vector3(vector.X, 0, 0);
    }
    public static Vector3 OnlyY(this Vector3 vector)
    {
        return new Vector3(0, vector.Y, 0);
    }
    public static Vector3 OnlyZ(this Vector3 vector)
    {
        return new Vector3(0, 0, vector.Z);
    }

    public static Vector2 OnlyX(this Vector2 vector)
    {
        return new Vector2(vector.X, 0);
    }
    public static Vector2 OnlyY(this Vector2 vector)
    {
        return new Vector2(0, vector.Y);
    }

    public static bool IsParallel(this Vector3 vector, Vector3 otherVector)
    {
        var crossProduct = vector.Cross(otherVector);
        return crossProduct.LengthSquared() < Mathf.Epsilon;
    }

    public static unsafe float* ToFloatPointer(this Vector3 vector)
    {
        float[] vec = new float[3];
        vec[0] = vector.X;
        vec[1] = vector.Y;
        vec[2] = vector.Z;

        fixed (float* ptr = vec)
        {
            return ptr;
        }
    }

    /// <summary>
    /// Like Max but when X or Y of the limit vector is 0 that factor will be ignored
    /// </summary>
    public static Vector2 Limit(this Vector2 vector, Vector2 limit)
    {
        var x = vector.X;
        var y = vector.Y;
        if (limit.X != 0) x = Mathf.Min(x, limit.X);
        if (limit.Y != 0) y = Mathf.Min(y, limit.Y);

        return new Vector2(x, y);
    }

    public static Vector3 WithMinLength(this Vector3 vector, float minLength)
    {
        if (vector.Length() >= minLength) return vector;

        return vector.Normalized() * minLength;
    }
}