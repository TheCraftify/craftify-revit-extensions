using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

/// <summary>
/// Provides factory methods for creating Revit bounding boxes.
/// </summary>
public static class BoundingBox
{
    /// <summary>
    /// Creates an empty bounding box with default properties.
    /// </summary>
    /// <returns>A new empty bounding box.</returns>
    /// <remarks>
    /// The returned bounding box has undefined Min and Max points and the Identity transform.
    /// It should be initialized with proper values before use.
    /// </remarks>
    public static BoundingBoxXYZ Empty() => new BoundingBoxXYZ();

    /// <summary>
    /// Creates a bounding box defined by its minimum and maximum corner points.
    /// </summary>
    /// <param name="min">The minimum corner point (with smallest X, Y, Z coordinates).</param>
    /// <param name="max">The maximum corner point (with largest X, Y, Z coordinates).</param>
    /// <param name="transform">Optional transform to apply to the bounding box. Defaults to Identity transform if not specified.</param>
    /// <returns>A new bounding box with the specified corners and transform.</returns>
    /// <remarks>
    /// The Min point should have coordinates less than or equal to the Max point's coordinates
    /// for the bounding box to be valid.
    /// </remarks>
    public static BoundingBoxXYZ ByCorners(XYZ min, XYZ max, Transform? transform = null) =>
        new BoundingBoxXYZ()
        {
            Min = min,
            Max = max,
            Transform = transform ?? Transform.Identity,
        };
}
