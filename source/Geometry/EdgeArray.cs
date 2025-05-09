namespace Craftify.Revit.Extensions.Geometry;

public static partial class EdgeArrayExtensions
{
    public static IEnumerable<Curve> AsCurves(this EdgeArray edgeArray) =>
        edgeArray.OfType<Edge>().Select(edge => edge.AsCurve());
}
