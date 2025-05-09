#pragma warning disable

using Craftify.Revit.Extensions.Geometry;

namespace Craftify.Revit.Extensions;

public static class EqualityComparers
{
    private static readonly Lazy<XYZEqualityComparer> _xyz = new(() => new XYZEqualityComparer());
    private static readonly Lazy<LineEqualityComparer> _line = new(
        () => new LineEqualityComparer()
    );

    public static XYZEqualityComparer XYZ => _xyz.Value;
    public static LineEqualityComparer Line => _line.Value;
}

public class XYZEqualityComparer(double tolerance = Defaults.Tolerance) : IEqualityComparer<XYZ>
{
    public bool Equals(XYZ x, XYZ y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return x.IsAlmostEqualTo(y, tolerance);
    }

    public int GetHashCode(XYZ obj) => obj.GetHashCode(tolerance);
}

public class LineEqualityComparer(double tolerance = Defaults.Tolerance) : IEqualityComparer<Line>
{
    public bool Equals(Line x, Line y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return (
            x.Start().IsAlmostEqualTo(y.Start(), tolerance)
                && x.End().IsAlmostEqualTo(y.End(), tolerance)
            || (
                x.Start().IsAlmostEqualTo(y.End(), tolerance)
                && x.End().IsAlmostEqualTo(y.Start(), tolerance)
            )
        );
    }

    public int GetHashCode(Line line) =>
        line.Start().GetHashCode(tolerance) ^ line.End().GetHashCode(tolerance);
}
