using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class BoundingBox
{
    public static BoundingBoxXYZ Empty() => new BoundingBoxXYZ();

    public static BoundingBoxXYZ ByCorners(XYZ min, XYZ max, Transform? transform = null) =>
        new BoundingBoxXYZ()
        {
            Min = min,
            Max = max,
            Transform = transform,
        };
}
