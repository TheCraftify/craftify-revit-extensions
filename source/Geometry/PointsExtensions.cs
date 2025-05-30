using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

/// <summary>
/// Provides extension methods for working with collections of 3D points.
/// </summary>
public static class PointsExtensions
{
    /// <summary>
    /// Creates a new point with the maximum X, Y, and Z coordinates from a collection of points.
    /// </summary>
    /// <param name="points">The collection of points to analyze.</param>
    /// <returns>
    /// A new point where each coordinate (X, Y, Z) is the maximum value found for that coordinate
    /// across all points in the collection.
    /// </returns>
    /// <remarks>
    /// This method does not return an existing point from the collection, but rather creates a new point
    /// with the maximum values for each coordinate. The resulting point may not exist in the original collection.
    /// </remarks>
    /// <example>
    /// For points [(1,2,3), (4,0,2), (2,5,1)], the result would be (4,5,3).
    /// </example>
    public static XYZ MaxByCoordinates(this IEnumerable<XYZ> points)
    {
        var maxX = double.MinValue;
        var maxY = double.MinValue;
        var maxZ = double.MinValue;
        foreach (var point in points)
        {
            maxX = Math.Max(maxX, point.X);
            maxY = Math.Max(maxY, point.Y);
            maxZ = Math.Max(maxZ, point.Z);
        }
        return new XYZ(maxX, maxY, maxZ);
    }

    /// <summary>
    /// Creates a new point with the minimum X, Y, and Z coordinates from a collection of points.
    /// </summary>
    /// <param name="points">The collection of points to analyze.</param>
    /// <returns>
    /// A new point where each coordinate (X, Y, Z) is the minimum value found for that coordinate
    /// across all points in the collection.
    /// </returns>
    /// <remarks>
    /// This method does not return an existing point from the collection, but rather creates a new point
    /// with the minimum values for each coordinate. The resulting point may not exist in the original collection.
    /// </remarks>
    /// <example>
    /// For points [(1,2,3), (4,0,2), (2,5,1)], the result would be (1,0,1).
    /// </example>
    public static XYZ MinByCoordinates(this IEnumerable<XYZ> points)
    {
        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var minZ = double.MaxValue;
        foreach (var point in points)
        {
            minX = Math.Min(minX, point.X);
            minY = Math.Min(minY, point.Y);
            minZ = Math.Min(minZ, point.Z);
        }
        return new XYZ(minX, minY, minZ);
    }
}
