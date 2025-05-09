namespace Craftify.Revit.Extensions.Common;

public static partial class TransformExtensions
{
    public static BoundingBoxXYZ OfBoundingBoxXYZ(
        this Transform transform,
        BoundingBoxXYZ boundingBoxXYZ
    ) =>
        new()
        {
            Min = transform.OfPoint(boundingBoxXYZ.Min),
            Max = transform.OfPoint(boundingBoxXYZ.Max),
        };
}
