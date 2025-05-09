#pragma warning disable

namespace Craftify.Revit.Extensions.Common;

public static partial class XYZExtensions
{
    public static int GetHashCode(this XYZ xyz, double tolerance = Defaults.Tolerance) =>
        xyz.X.RoundToTolerance(tolerance).GetHashCode()
        ^ xyz.Y.RoundToTolerance(tolerance).GetHashCode()
        ^ xyz.Z.RoundToTolerance(tolerance).GetHashCode();
}
