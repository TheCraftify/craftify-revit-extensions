using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class BoundingBoxExtensions
{
    /// <summary>
    /// Gets all dimensions (length, width, height) of a bounding box.
    /// </summary>
    /// <param name="boundingBox">The bounding box to measure.</param>
    /// <returns>A record containing the length, width, and height dimensions.</returns>
    public static BoundingBoxDimensions AllDimensions(this BoundingBoxXYZ boundingBox)
    {
        return new BoundingBoxDimensions(
            Length: boundingBox.DimensionAlongSide(BoundingBoxSide.Length),
            Width: boundingBox.DimensionAlongSide(BoundingBoxSide.Width),
            Height: boundingBox.DimensionAlongSide(BoundingBoxSide.Height)
        );
    }

    /// <summary>
    /// Measures the dimension of a bounding box along a specified side.
    /// </summary>
    /// <param name="boundingBox">The bounding box to measure.</param>
    /// <param name="side">The side to measure (Length, Width, or Height).</param>
    /// <returns>The dimension measurement along the specified side.</returns>
    public static double DimensionAlongSide(this BoundingBoxXYZ boundingBox, BoundingBoxSide side)
    {
        return boundingBox.Min.DistanceAlongVectorTo(boundingBox.Max, side.CorrespondingVector());
    }

    /// <summary>
    /// Gets the corner vertices (Min and Max points) of a bounding box.
    /// </summary>
    /// <param name="box">The bounding box.</param>
    /// <param name="applyTransform">Whether to apply the box's transform to the vertices.</param>
    /// <returns>An enumerable of corner vertices.</returns>
    public static IEnumerable<XYZ> CornerVertices(
        this BoundingBoxXYZ box,
        ApplyTransform applyTransform = ApplyTransform.No
    )
    {
        return Enumerable
            .Range(0, 2)
            .Select(i =>
            {
                var vertex = box.get_Bounds(i);
                return applyTransform == ApplyTransform.Yes
                    ? box.Transform.OfPoint(vertex)
                    : vertex;
            });
    }

    /// <summary>
    /// Calculates the center point of a bounding box.
    /// </summary>
    /// <param name="boundingBox">The bounding box.</param>
    /// <param name="applyTransform">Whether to apply the box's transform to the center point.</param>
    /// <returns>The center point of the bounding box.</returns>
    public static XYZ Center(
        this BoundingBoxXYZ boundingBox,
        ApplyTransform applyTransform = ApplyTransform.No
    )
    {
        var center = boundingBox.Min.Add(boundingBox.Max).Multiply(0.5);
        return applyTransform == ApplyTransform.No ? center : boundingBox.Transform.OfPoint(center);
    }

    /// <summary>
    /// Gets the origin of a bounding box's transform.
    /// </summary>
    /// <param name="boundingBox">The bounding box.</param>
    /// <returns>The origin point of the bounding box's transform.</returns>
    public static XYZ Origin(this BoundingBoxXYZ boundingBox) => boundingBox.Transform.Origin;

    /// <summary>
    /// Merges multiple bounding boxes into a single encompassing box.
    /// </summary>
    /// <param name="boundingBoxes">The collection of bounding boxes to merge.</param>
    /// <param name="applyTransform">Whether to apply transforms when calculating the merged box.</param>
    /// <returns>A new bounding box that encompasses all input boxes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when boundingBoxes is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when boundingBoxes is empty.</exception>
    public static BoundingBoxXYZ Merge(
        this IEnumerable<BoundingBoxXYZ> boundingBoxes,
        ApplyTransform applyTransform = ApplyTransform.No
    )
    {
        if (boundingBoxes is null)
        {
            throw new ArgumentNullException(nameof(boundingBoxes));
        }
        return boundingBoxes.Aggregate(
            (previous, next) => previous.MergeWith(next, applyTransform)
        );
    }

    /// <summary>
    /// Merges two bounding boxes into a single encompassing box.
    /// </summary>
    /// <param name="fromBoundingBox">The first bounding box to merge.</param>
    /// <param name="toBoundingBox">The second bounding box to merge.</param>
    /// <param name="applyTransform">Whether to apply transforms when calculating the merged box.</param>
    /// <returns>A new bounding box that encompasses both input boxes.</returns>
    public static BoundingBoxXYZ MergeWith(
        this BoundingBoxXYZ fromBoundingBox,
        BoundingBoxXYZ toBoundingBox,
        ApplyTransform applyTransform
    )
    {
        var allPoints = fromBoundingBox
            .CornerVertices(applyTransform)
            .Concat(toBoundingBox.CornerVertices(applyTransform))
            .ToArray();
        var minPoint = allPoints.MinByCoordinates();
        var maxPoint = allPoints.MaxByCoordinates();
        return BoundingBox.ByCorners(minPoint, maxPoint);
    }

    /// <summary>
    /// Gets the corresponding vector for a bounding box side.
    /// </summary>
    /// <param name="side">The bounding box side.</param>
    /// <returns>The basis vector corresponding to the specified side.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid side is specified.</exception>
    private static XYZ CorrespondingVector(this BoundingBoxSide side) =>
        side switch
        {
            BoundingBoxSide.Length => XYZ.BasisX,
            BoundingBoxSide.Width => XYZ.BasisY,
            BoundingBoxSide.Height => XYZ.BasisZ,
            _ => throw new InvalidOperationException($"Cannot use ${side}"),
        };
}
