#pragma warning disable

namespace Craftify.Revit.Extensions.Geometry;

public static partial class XYZExtensions
{
    public static XYZ Translate(this XYZ point, XYZ vector) =>
        new(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
}
