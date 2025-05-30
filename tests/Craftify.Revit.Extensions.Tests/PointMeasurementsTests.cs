using Autodesk.Revit.DB;
using Craftify.Revit.Extensions.Geometry;
using Shouldly;

namespace Craftify.Revit.Extensions.Tests;

public class PointMeasurementsTests
{
    [TestCaseSource(nameof(DistanceAlongVectorCases))]
    public void DistanceAlongVectorTo_ShouldReturnPositiveDistanceAlongDirectionVector(
        XYZ fromPoint,
        XYZ toPoint,
        XYZ alongVector,
        double expectedDistance
    )
    {
        var distance = fromPoint.DistanceAlongVectorTo(toPoint, alongVector);
        distance.ShouldBe(expectedDistance);
    }

    [TestCaseSource(nameof(SignedDistanceAlongVectorCases))]
    public void SignedDistanceAlongVectorTo_ShouldReturnSignedDistanceAlongDirectionVector(
        XYZ fromPoint,
        XYZ toPoint,
        XYZ alongVector,
        double expectedDistance
    )
    {
        var distance = fromPoint.SignedDistanceAlongVectorTo(toPoint, alongVector);
        distance.ShouldBe(expectedDistance);
    }

    public static object[] DistanceAlongVectorCases =
    [
        new object[] { new XYZ(0, 0, 0), new XYZ(5, 0, 0), new XYZ(1, 0, 0), 5 },
        new object[] { new XYZ(0, 0, 0), new XYZ(-5, 0, 0), new XYZ(1, 0, 0), 5 },
        new object[] { new XYZ(3, 4, 0), new XYZ(6, 8, 0), new XYZ(0, 1, 0), 4 },
        new object[] { new XYZ(3, 4, 0), new XYZ(6, 8, 0), new XYZ(1, 0, 0), 3 },
        new object[] { new XYZ(3, 4, 5), new XYZ(3, 4, 10), new XYZ(0, 0, 1), 5 },
        new object[] { new XYZ(3, 4, 5), new XYZ(3, 4, 0), new XYZ(0, 0, 1), 5 },
    ];

    public static object[] SignedDistanceAlongVectorCases =
    [
        new object[] { new XYZ(0, 0, 0), new XYZ(5, 0, 0), new XYZ(1, 0, 0), 5 },
        new object[] { new XYZ(0, 0, 0), new XYZ(-5, 0, 0), new XYZ(1, 0, 0), -5 },
        new object[] { new XYZ(3, 4, 0), new XYZ(6, 8, 0), new XYZ(0, 1, 0), 4 },
        new object[] { new XYZ(3, 4, 0), new XYZ(6, 8, 0), new XYZ(1, 0, 0), 3 },
        new object[] { new XYZ(3, 4, 5), new XYZ(3, 4, 10), new XYZ(0, 0, 1), 5 },
        new object[] { new XYZ(3, 4, 5), new XYZ(3, 4, 0), new XYZ(0, 0, 1), -5 },
    ];
}
