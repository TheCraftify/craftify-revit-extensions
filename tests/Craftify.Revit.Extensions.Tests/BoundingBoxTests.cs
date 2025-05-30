using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Craftify.Revit.Extensions.Geometry;
using Newtonsoft.Json;
using Shouldly;

namespace Craftify.Revit.Extensions.Tests;

public class BoundingBoxTests
{
    [Test]
    public void Empty_ShouldReturnBoundingBoxEqualToRevitDefaultBox()
    {
        var box = BoundingBox.Empty();

        var expectedBox = new BoundingBoxXYZ();

        box.Min.IsAlmostEqualTo(expectedBox.Min).ShouldBeTrue();
        box.Max.IsAlmostEqualTo(expectedBox.Max).ShouldBeTrue();
        box.Transform.AlmostEqual(expectedBox.Transform).ShouldBeTrue();
    }

    [Test]
    public void ByCorners_WithoutTransform_ShouldCreateBoundingBoxWithMinAndMax()
    {
        var min = new XYZ(1, 2, 3);
        var max = new XYZ(4, 5, 6);

        var box = BoundingBox.ByCorners(min, max);

        box.Min.IsAlmostEqualTo(min).ShouldBeTrue();
        box.Max.IsAlmostEqualTo(max).ShouldBeTrue();
        box.Transform.IsIdentity.ShouldBeTrue();
    }

    [Test]
    public void ByCorners_WithTransform_ShouldCreateBoundingBoxWithTransform()
    {
        var min = new XYZ(1, 2, 3);
        var max = new XYZ(4, 5, 6);
        var transform = Transform.CreateTranslation(new XYZ(10, 10, 10));

        var box = BoundingBox.ByCorners(min, max, transform);

        box.Min.IsAlmostEqualTo(min).ShouldBeTrue();
        box.Max.IsAlmostEqualTo(max).ShouldBeTrue();
        box.Transform.AlmostEqual(transform).ShouldBeTrue();
    }

    [Test]
    public void ByCorners_WithNullTransform_ShouldUseIdentityTransform()
    {
        var min = new XYZ(1, 2, 3);
        var max = new XYZ(4, 5, 6);

        var box = BoundingBox.ByCorners(min, max, null);

        box.Min.IsAlmostEqualTo(min).ShouldBeTrue();
        box.Max.IsAlmostEqualTo(max).ShouldBeTrue();
        box.Transform.IsIdentity.ShouldBeTrue();
    }

    [Test]
    public void ByCorners_ShouldCreateNewBoundingBoxInstance()
    {
        var box1 = BoundingBox.ByCorners(new XYZ(0, 0, 0), new XYZ(1, 1, 1));
        var box2 = BoundingBox.ByCorners(new XYZ(0, 0, 0), new XYZ(1, 1, 1));

        box1.ShouldNotBeSameAs(box2);
    }
}
