namespace Craftify.Revit.Extensions.Geometry;

public static partial class FaceExtensions
{
    public static IEnumerable<Edge> Edges(this Face face) =>
        face.EdgeLoops.OfType<EdgeArray>().SelectMany(edgeArray => edgeArray.OfType<Edge>());

    public static IEnumerable<Curve> Curves(this Face face) =>
        face.GetEdgesAsCurveLoops().SelectMany(curveLoop => curveLoop.OfType<Curve>());
}
