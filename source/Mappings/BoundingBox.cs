namespace Craftify.Revit.Extensions.Mappings;

public static partial class BoundingBoxExtensions
{
    public static Outline ToOutline(this BoundingBoxXYZ boundingBoxXYZ) =>
        new(boundingBoxXYZ.Min, boundingBoxXYZ.Max);

    public static Solid ToSolid(this BoundingBoxXYZ boundingBoxXYZ)
    {
        var min = boundingBoxXYZ.Min;
        var max = boundingBoxXYZ.Max;
        var transform = boundingBoxXYZ.Transform;

        var buttomLeftNear = transform.OfPoint(min);
        var buttomLeftFar = transform.OfPoint(new XYZ(min.X, max.Y, min.Z));
        var buttomRightFar = transform.OfPoint(new XYZ(max.X, max.Y, min.Z));
        var buttomRightNear = transform.OfPoint(new XYZ(max.X, min.Y, min.Z));

        var profileLoops = new List<CurveLoop>
        {
            CurveLoop.Create(
                [
                    Line.CreateBound(buttomLeftNear, buttomLeftFar),
                    Line.CreateBound(buttomLeftFar, buttomRightFar),
                    Line.CreateBound(buttomRightFar, buttomRightNear),
                    Line.CreateBound(buttomRightNear, buttomLeftNear),
                ]
            ),
        };

        var extrusionDistance = boundingBoxXYZ.Max.Z - boundingBoxXYZ.Min.Z;

        return GeometryCreationUtilities.CreateExtrusionGeometry(
            profileLoops,
            XYZ.BasisZ,
            extrusionDistance
        );
    }
}
