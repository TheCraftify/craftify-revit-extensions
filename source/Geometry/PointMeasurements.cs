using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class PointMeasurements
{
    public static double SignedDistanceAlongVectorTo(
        this XYZ firstPoint,
        XYZ secondPoint,
        XYZ vector
    ) => Vector.ByTwoPoints(firstPoint, secondPoint).DotProduct(vector);

    public static double DistanceAlongVectorTo(this XYZ firstPoint, XYZ secondPoint, XYZ vector) =>
        Math.Abs(firstPoint.SignedDistanceAlongVectorTo(secondPoint, vector));
}
