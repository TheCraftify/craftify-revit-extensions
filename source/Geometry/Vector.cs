using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

/// <summary>
/// Provides utility methods for working with vectors in 3D space.
/// </summary>
public static class Vector
{
    /// <summary>
    /// Creates a vector from a start point to an end point.
    /// </summary>
    /// <param name="start">The starting point of the vector.</param>
    /// <param name="end">The ending point of the vector.</param>
    /// <returns>A vector representing the direction and magnitude from the start point to the end point.</returns>
    /// <remarks>
    /// The resulting vector is calculated by subtracting the start point from the end point.
    /// The length of the vector represents the distance between the two points.
    /// </remarks>
    public static XYZ ByTwoPoints(XYZ start, XYZ end)
    {
        return end - start;
    }
}
