namespace Craftify.Revit.Extensions.Geometry;

public static partial class LineExtensions
{
    public static XYZ Start(this Line line) => line.GetEndPoint(0);

    public static XYZ End(this Line line) => line.GetEndPoint(1);

    public static bool AnyEndPointsEqualTo(
        this Line line,
        XYZ point,
        double tolerance = Defaults.Tolerance
    ) => line.EndPoints().Any(endPoint => endPoint.IsAlmostEqualTo(point, tolerance));
}
