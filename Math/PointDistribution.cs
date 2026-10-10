using Godot;

namespace Godex.Math;

public static class PointDistribution
{
    public static Vector3[] DistributePointsOnDisc(
        Vector3 discCenter,
        Vector3 discForwardNormal,
        float discRadius,
        int pointsCount,
        RandomNumberGenerator rng
    )
    {
        Vector3 normalTangentReference = Mathf.Abs(discForwardNormal.Y) < 0.99f ? Vector3.Up : Vector3.Right;
        Vector3 tangent = discForwardNormal.Cross(normalTangentReference).Normalized();
        Vector3 bitangent = discForwardNormal.Cross(tangent).Normalized();

        Vector3[] points = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            float randomAngle = rng.RandfRange(0.0f, Mathf.Tau);
            float randomDistance = discRadius * Mathf.Sqrt(rng.Randf());
            float x = Mathf.Cos(randomAngle) * randomDistance;
            float y = Mathf.Sin(randomAngle) * randomDistance;
            points[i] = discCenter + tangent * x + bitangent * y;
        }

        return points;
    }
}