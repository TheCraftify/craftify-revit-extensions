using Autodesk.Revit.DB;
using Craftify.Revit.Extensions.Geometry;
using Shouldly;

namespace Craftify.Revit.Extensions.Tests;

public class BoundingBoxMutatationTests
{
    [Test]
    public void MoveBy_ShouldReturnNewMovedBoundingBox()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var translation = new XYZ(2, 3, 4);
        var movedBox = box.MoveBy(translation);
        movedBox.Origin().IsAlmostEqualTo(translation).ShouldBeTrue();
    }

    [Test]
    public void MoveBy_ShouldThrowException_WhenTranslationIsNull()
    {
        var boundingBox = BoundingBox.ByCorners(new XYZ(0, 0, 0), new XYZ(2, 3, 0));

        var moveBoundingBox = () => boundingBox.MoveBy(null);

        Should.Throw<ArgumentNullException>(moveBoundingBox);
    }

    [Test]
    public void SetMin_ShouldReturnNewBoundingBoxWithUpdatedMin()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var newMin = new XYZ(1, 1, 1);

        var updatedBox = box.SetMin(newMin);

        updatedBox.Min.IsAlmostEqualTo(newMin).ShouldBeTrue();
        updatedBox.Max.IsAlmostEqualTo(box.Max).ShouldBeTrue();
    }

    [Test]
    public void SetMax_ShouldReturnNewBoundingBoxWithUpdatedMax()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var newMax = new XYZ(6, 6, 6);

        var updatedBox = box.SetMax(newMax);

        updatedBox.Max.IsAlmostEqualTo(newMax).ShouldBeTrue();
        updatedBox.Min.IsAlmostEqualTo(box.Min).ShouldBeTrue();
    }

    [Test]
    public void SetTransform_ShouldReturnNewBoundingBoxWithUpdatedTransform()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var newTransform = Transform.CreateTranslation(new XYZ(3, 4, 5));

        var updatedBox = box.SetTransform(newTransform);

        updatedBox.Transform.AlmostEqual(newTransform).ShouldBeTrue();
        updatedBox.Min.IsAlmostEqualTo(box.Min).ShouldBeTrue();
        updatedBox.Max.IsAlmostEqualTo(box.Max).ShouldBeTrue();
    }

    [Test]
    public void SetOrigin_ShouldReturnNewBoundingBoxWithUpdatedOrigin()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var newOrigin = new XYZ(3, 4, 5);

        var updatedBox = box.SetOrigin(newOrigin);

        updatedBox.Origin().IsAlmostEqualTo(newOrigin).ShouldBeTrue();
    }

    [Test]
    public void SetNewBounds_ShouldReturnNewBoundingBoxWithUpdatedMinAndMax()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var newMin = new XYZ(1, 1, 1);
        var newMax = new XYZ(6, 6, 6);

        var updatedBox = box.SetNewBounds(newMin, newMax);

        updatedBox.Min.IsAlmostEqualTo(newMin).ShouldBeTrue();
        updatedBox.Max.IsAlmostEqualTo(newMax).ShouldBeTrue();
        updatedBox.Transform.AlmostEqual(box.Transform).ShouldBeTrue();
    }

    [Test]
    public void ExtrudeUpwards_ShouldIncreaseMaxZCoordinate()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var extrusionValue = 3.0;

        var extrudedBox = box.ExtrudeUpwards(extrusionValue);

        var expectedMax = new XYZ(box.Max.X, box.Max.Y, box.Max.Z + extrusionValue);
        extrudedBox.Max.IsAlmostEqualTo(expectedMax).ShouldBeTrue();
        extrudedBox.Min.IsAlmostEqualTo(box.Min).ShouldBeTrue();
    }

    [Test]
    public void ExtrudeFront_ShouldIncreaseMaxYCoordinate()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var extrusionValue = 3.0;

        var extrudedBox = box.ExtrudeFront(extrusionValue);

        var expectedMax = new XYZ(box.Max.X, box.Max.Y + extrusionValue, box.Max.Z);
        extrudedBox.Max.IsAlmostEqualTo(expectedMax).ShouldBeTrue();
        extrudedBox.Min.IsAlmostEqualTo(box.Min).ShouldBeTrue();
    }

    [Test]
    public void ExtrudeBack_ShouldDecreaseMinYCoordinate()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var extrusionValue = 3.0;

        var extrudedBox = box.ExtrudeBack(extrusionValue);

        var expectedMin = new XYZ(box.Min.X, box.Min.Y - extrusionValue, box.Min.Z);
        extrudedBox.Min.IsAlmostEqualTo(expectedMin).ShouldBeTrue();
        extrudedBox.Max.IsAlmostEqualTo(box.Max).ShouldBeTrue();
    }

    [Test]
    public void ExtrudeRight_ShouldIncreaseMaxXCoordinate()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var extrusionValue = 3.0;

        var extrudedBox = box.ExtrudeRight(extrusionValue);

        var expectedMax = new XYZ(box.Max.X + extrusionValue, box.Max.Y, box.Max.Z);
        extrudedBox.Max.IsAlmostEqualTo(expectedMax).ShouldBeTrue();
        extrudedBox.Min.IsAlmostEqualTo(box.Min).ShouldBeTrue();
    }

    [Test]
    public void ExtrudeLeft_ShouldDecreaseMinXCoordinate()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var extrusionValue = 3.0;

        var extrudedBox = box.ExtrudeLeft(extrusionValue);

        var expectedMin = new XYZ(box.Min.X - extrusionValue, box.Min.Y, box.Min.Z);
        extrudedBox.Min.IsAlmostEqualTo(expectedMin).ShouldBeTrue();
        extrudedBox.Max.IsAlmostEqualTo(box.Max).ShouldBeTrue();
    }

    [Test]
    public void ExtrudeDownwards_ShouldDecreaseMinZCoordinate()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var extrusionValue = 3.0;

        var extrudedBox = box.ExtrudeDownwards(extrusionValue);

        var expectedMin = new XYZ(box.Min.X, box.Min.Y, box.Min.Z - extrusionValue);
        extrudedBox.Min.IsAlmostEqualTo(expectedMin).ShouldBeTrue();
        extrudedBox.Max.IsAlmostEqualTo(box.Max).ShouldBeTrue();
    }

    [Test]
    public void Clone_ShouldReturnNewBoundingBoxWithSameProperties()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var transform = Transform.CreateTranslation(new XYZ(1, 2, 3));
        box.Transform = transform;

        var clonedBox = box.Clone();

        clonedBox.Min.IsAlmostEqualTo(box.Min).ShouldBeTrue();
        clonedBox.Max.IsAlmostEqualTo(box.Max).ShouldBeTrue();
        clonedBox.Transform.AlmostEqual(box.Transform).ShouldBeTrue();
        clonedBox.ShouldNotBeSameAs(box);
    }
}
