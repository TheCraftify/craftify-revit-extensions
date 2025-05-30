using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class BoundingBoxExtensions
{
    public static BoundingBoxDimensions AllDimensions(this BoundingBoxXYZ boundingBox)
    {
        return new BoundingBoxDimensions(
            Length: boundingBox.DimensionAlongSide(BoundingBoxSide.Length),
            Width: boundingBox.DimensionAlongSide(BoundingBoxSide.Width),
            Height: boundingBox.DimensionAlongSide(BoundingBoxSide.Height)
        );
    }

    public static double DimensionAlongSide(this BoundingBoxXYZ boundingBox, BoundingBoxSide side)
    {
        return boundingBox.Min.DistanceAlongVectorTo(boundingBox.Max, side.CorrespondingVector());
    }

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

    public static XYZ Center(
        this BoundingBoxXYZ boundingBox,
        ApplyTransform applyTransform = ApplyTransform.No
    )
    {
        var center = boundingBox.Min.Add(boundingBox.Max).Multiply(0.5);
        return applyTransform == ApplyTransform.No ? center : boundingBox.Transform.OfPoint(center);
    }

    public static XYZ Origin(this BoundingBoxXYZ boundingBox) => boundingBox.Transform.Origin;

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

    private static XYZ CorrespondingVector(this BoundingBoxSide side) =>
        side switch
        {
            BoundingBoxSide.Length => XYZ.BasisX,
            BoundingBoxSide.Width => XYZ.BasisY,
            BoundingBoxSide.Height => XYZ.BasisZ,
            _ => throw new InvalidOperationException($"Cannot use ${side}"),
        };
}
