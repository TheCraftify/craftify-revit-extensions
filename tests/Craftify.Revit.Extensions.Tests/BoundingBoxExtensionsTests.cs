using Autodesk.Revit.DB;
using Craftify.Revit.Extensions.Geometry;
using Shouldly;

namespace Craftify.Revit.Extensions.Tests;

public class BoundingBoxExtensionsTests
{
    [Test]
    public void AllDimensions_ShouldReturnCorrectDimensions()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 4, 3));

        var dimensions = box.AllDimensions();

        dimensions.Length.ShouldBe(5);
        dimensions.Width.ShouldBe(4);
        dimensions.Height.ShouldBe(3);
    }

    [Test]
    public void DimensionAlongSide_ShouldReturnCorrectDimension()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 4, 3));

        var length = box.DimensionAlongSide(BoundingBoxSide.Length);
        var width = box.DimensionAlongSide(BoundingBoxSide.Width);
        var height = box.DimensionAlongSide(BoundingBoxSide.Height);

        length.ShouldBe(5);
        width.ShouldBe(4);
        height.ShouldBe(3);
    }

    [Test]
    public void CornerVertices_WithoutTransform_ShouldReturnMinAndMax()
    {
        var min = new XYZ(1, 2, 3);
        var max = new XYZ(4, 5, 6);
        var box = BoundingBox.ByCorners(min, max);

        var vertices = box.CornerVertices().ToList();

        vertices.Count.ShouldBe(2);
        vertices[0].IsAlmostEqualTo(min).ShouldBeTrue();
        vertices[1].IsAlmostEqualTo(max).ShouldBeTrue();
    }

    [Test]
    public void CornerVertices_WithTransform_ShouldReturnTransformedVertices()
    {
        var min = new XYZ(1, 2, 3);
        var max = new XYZ(4, 5, 6);
        var transform = Transform.CreateTranslation(new XYZ(10, 10, 10));
        var box = BoundingBox.ByCorners(min, max, transform);

        var vertices = box.CornerVertices(ApplyTransform.Yes).ToList();

        vertices.Count.ShouldBe(2);
        vertices[0].IsAlmostEqualTo(transform.OfPoint(min)).ShouldBeTrue();
        vertices[1].IsAlmostEqualTo(transform.OfPoint(max)).ShouldBeTrue();
    }

    [Test]
    public void Center_WithoutTransform_ShouldReturnCenterPoint()
    {
        var box = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(10, 10, 10));

        var center = box.Center();

        var expected = new XYZ(5, 5, 5);
        center.IsAlmostEqualTo(expected).ShouldBeTrue();
    }

    [Test]
    public void Center_WithTransform_ShouldReturnTransformedCenterPoint()
    {
        var transform = Transform.CreateTranslation(new XYZ(5, 5, 5));
        var box = BoundingBox.ByCorners(
            min: new XYZ(0, 0, 0),
            max: new XYZ(10, 10, 10),
            transform: transform
        );

        var center = box.Center(ApplyTransform.Yes);

        var expected = new XYZ(10, 10, 10);
        center.IsAlmostEqualTo(expected).ShouldBeTrue();
    }

    [Test]
    public void Origin_ShouldReturnTransformOrigin()
    {
        var origin = new XYZ(3, 4, 5);
        var transform = Transform.CreateTranslation(origin);
        var box = BoundingBox.ByCorners(
            min: new XYZ(0, 0, 0),
            max: new XYZ(10, 10, 10),
            transform: transform
        );

        var result = box.Origin();

        result.IsAlmostEqualTo(origin).ShouldBeTrue();
    }

    [Test]
    public void Merge_ShouldCombineMultipleBoundingBoxes()
    {
        var box1 = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var box2 = BoundingBox.ByCorners(min: new XYZ(3, 3, 3), max: new XYZ(8, 8, 8));
        var box3 = BoundingBox.ByCorners(min: new XYZ(-2, -2, -2), max: new XYZ(2, 2, 2));
        var boxes = new[] { box1, box2, box3 };

        var merged = boxes.Merge();

        merged.Min.IsAlmostEqualTo(new XYZ(-2, -2, -2)).ShouldBeTrue();
        merged.Max.IsAlmostEqualTo(new XYZ(8, 8, 8)).ShouldBeTrue();
    }

    [Test]
    public void Merge_WithNullCollection_ShouldThrowArgumentNullException()
    {
        IEnumerable<BoundingBoxXYZ> boxes = null;

        Should.Throw<ArgumentNullException>(() => boxes.Merge());
    }

    [Test]
    public void Merge_WithEmptyCollection_ShouldThrowInvalidOperationException()
    {
        var boxes = Enumerable.Empty<BoundingBoxXYZ>();

        Should.Throw<InvalidOperationException>(() => boxes.Merge());
    }

    [Test]
    public void MergeWith_ShouldCombineTwoBoundingBoxes()
    {
        var box1 = BoundingBox.ByCorners(min: new XYZ(0, 0, 0), max: new XYZ(5, 5, 5));
        var box2 = BoundingBox.ByCorners(min: new XYZ(3, 3, 3), max: new XYZ(8, 8, 8));

        var merged = box1.MergeWith(box2, ApplyTransform.No);

        merged.Min.IsAlmostEqualTo(new XYZ(0, 0, 0)).ShouldBeTrue();
        merged.Max.IsAlmostEqualTo(new XYZ(8, 8, 8)).ShouldBeTrue();
    }

    [Test]
    public void MergeWith_WithTransform_ShouldCombineTransformedBoundingBoxes()
    {
        var box1 = BoundingBox.ByCorners(
            min: new XYZ(0, 0, 0),
            max: new XYZ(5, 5, 5),
            transform: Transform.CreateTranslation(new XYZ(10, 0, 0))
        );
        var box2 = BoundingBox.ByCorners(
            min: new XYZ(0, 0, 0),
            max: new XYZ(5, 5, 5),
            transform: Transform.CreateTranslation(new XYZ(0, 10, 0))
        );

        var merged = box1.MergeWith(box2, ApplyTransform.Yes);

        merged.Min.IsAlmostEqualTo(new XYZ(0, 0, 0)).ShouldBeTrue();
        merged.Max.IsAlmostEqualTo(new XYZ(15, 15, 5)).ShouldBeTrue();
    }
}
