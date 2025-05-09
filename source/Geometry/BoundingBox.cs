namespace Craftify.Revit.Extensions.Geometry;

public static partial class BoundingBoxExtensions
{
    public static IEnumerable<Line> Explode(this BoundingBoxXYZ boundingBoxXYZ)
    {
        var min = boundingBoxXYZ.Min;
        var max = boundingBoxXYZ.Max;

        var buttomFrontLeft = new XYZ(min.X, max.Y, min.Z);
        var buttomFrontRight = new XYZ(max.X, max.Y, min.Z);
        var buttomBackRight = new XYZ(max.X, min.Y, min.Z);

        var topBackLeft = new XYZ(min.X, min.Y, max.Z);
        var topFrontLeft = new XYZ(min.X, max.Y, max.Z);
        var topBackRight = new XYZ(max.X, min.Y, max.Z);

        yield return Line.CreateBound(min, buttomFrontLeft);
        yield return Line.CreateBound(buttomFrontLeft, buttomFrontRight);
        yield return Line.CreateBound(buttomFrontRight, buttomBackRight);
        yield return Line.CreateBound(buttomBackRight, min);
        yield return Line.CreateBound(max, topBackRight);
        yield return Line.CreateBound(topBackRight, topBackLeft);
        yield return Line.CreateBound(topBackLeft, topFrontLeft);
        yield return Line.CreateBound(topFrontLeft, max);
        yield return Line.CreateBound(min, topBackLeft);
        yield return Line.CreateBound(buttomFrontLeft, topFrontLeft);
        yield return Line.CreateBound(buttomFrontRight, max);
        yield return Line.CreateBound(buttomBackRight, topBackRight);
    }
}
