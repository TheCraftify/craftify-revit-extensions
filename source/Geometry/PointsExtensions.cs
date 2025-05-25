using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class PointsExtensions
{
    public static XYZ MaxByCoordinates(this ICollection<XYZ> points)
    {
        var maxX = points.Max(x => x.X);
        var maxY = points.Max(x => x.Y);
        var maxZ = points.Max(x => x.Z);
        return new XYZ(maxX, maxY, maxZ);
    }

    public static XYZ MinByCoordinates(this ICollection<XYZ> points)
    {
        var minX = points.Min(x => x.X);
        var minY = points.Min(x => x.Y);
        var minZ = points.Min(x => x.Z);
        return new XYZ(minX, minY, minZ);
    }
}
