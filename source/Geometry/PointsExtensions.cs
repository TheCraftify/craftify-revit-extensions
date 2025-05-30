using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class PointsExtensions
{
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
