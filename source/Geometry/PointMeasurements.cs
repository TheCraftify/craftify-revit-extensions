using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

/// <summary>
/// Provides extension methods for measuring distances between points in 3D space.
/// </summary>
public static class PointMeasurements
{
    /// <summary>
    /// Calculates the signed distance from the first point to the second point along a specified vector.
    /// </summary>
    /// <param name="firstPoint">The starting point.</param>
    /// <param name="secondPoint">The ending point.</param>
    /// <param name="vector">The direction vector along which to measure the distance.</param>
    /// <returns>
    /// The signed distance value. Positive if the second point is in the direction of the vector
    /// from the first point, negative if it's in the opposite direction.
    /// </returns>
    /// <remarks>
    /// The calculation is performed by creating a vector from the first point to the second point,
    /// then calculating the dot product of this vector with the specified direction vector.
    /// </remarks>
    public static double SignedDistanceAlongVectorTo(
        this XYZ firstPoint,
        XYZ secondPoint,
        XYZ vector
    ) => Vector.ByTwoPoints(firstPoint, secondPoint).DotProduct(vector);

    /// <summary>
    /// Calculates the absolute (positive) distance from the first point to the second point along a specified vector.
    /// </summary>
    /// <param name="firstPoint">The starting point.</param>
    /// <param name="secondPoint">The ending point.</param>
    /// <param name="vector">The direction vector along which to measure the distance.</param>
    /// <returns>
    /// The absolute (always positive) distance value between the points along the specified vector.
    /// </returns>
    /// <remarks>
    /// This method returns the absolute value of the signed distance, making it useful when
    /// only the magnitude of the distance is needed, regardless of direction.
    /// </remarks>
    public static double DistanceAlongVectorTo(this XYZ firstPoint, XYZ secondPoint, XYZ vector) =>
        Math.Abs(firstPoint.SignedDistanceAlongVectorTo(secondPoint, vector));
}
