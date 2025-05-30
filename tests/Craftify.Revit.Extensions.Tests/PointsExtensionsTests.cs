using Autodesk.Revit.DB;
using Craftify.Revit.Extensions.Geometry;
using Shouldly;

namespace Craftify.Revit.Extensions.Tests;

public class PointsExtensionsTests
{
    [Test]
    public void MaxByCoordinates_ShouldReturnPointWithMaximumCoordinates()
    {
        var points = new List<XYZ>
        {
            new XYZ(1, 2, 3),
            new XYZ(4, 0, 2),
            new XYZ(2, 5, 1),
            new XYZ(0, 3, 6),
        };

        var result = points.MaxByCoordinates();

        var expected = new XYZ(4, 5, 6);
        result.IsAlmostEqualTo(expected).ShouldBeTrue();
    }

    [Test]
    public void MaxByCoordinates_WithSinglePoint_ShouldReturnThatPoint()
    {
        var points = new List<XYZ> { new XYZ(1, 2, 3) };

        var result = points.MaxByCoordinates();

        result.IsAlmostEqualTo(points[0]).ShouldBeTrue();
    }

    [Test]
    public void MinByCoordinates_ShouldReturnPointWithMinimumCoordinates()
    {
        var points = new List<XYZ>
        {
            new XYZ(1, 2, 3),
            new XYZ(4, 0, 2),
            new XYZ(2, 5, 1),
            new XYZ(0, 3, 6),
        };

        var result = points.MinByCoordinates();

        var expected = new XYZ(0, 0, 1);
        result.IsAlmostEqualTo(expected).ShouldBeTrue();
    }

    [Test]
    public void MinByCoordinates_WithSinglePoint_ShouldReturnThatPoint()
    {
        var points = new List<XYZ> { new XYZ(1, 2, 3) };

        var result = points.MinByCoordinates();

        result.IsAlmostEqualTo(points[0]).ShouldBeTrue();
    }

    [Test]
    public void MinByCoordinates_WithNegativeValues_ShouldReturnCorrectMinimum()
    {
        var points = new List<XYZ> { new XYZ(-1, 2, 3), new XYZ(4, -5, 2), new XYZ(2, 5, -7) };

        var result = points.MinByCoordinates();

        var expected = new XYZ(-1, -5, -7);
        result.IsAlmostEqualTo(expected).ShouldBeTrue();
    }

    [Test]
    public void MaxByCoordinates_WithNegativeValues_ShouldReturnCorrectMaximum()
    {
        var points = new List<XYZ> { new XYZ(-1, 2, 3), new XYZ(4, -5, 2), new XYZ(2, 5, -7) };

        var result = points.MaxByCoordinates();

        var expected = new XYZ(4, 5, 3);
        result.IsAlmostEqualTo(expected).ShouldBeTrue();
    }
}
