using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class BoundingBoxMutations
{
    public static BoundingBoxXYZ MoveBy(this BoundingBoxXYZ boundingBox, XYZ translation)
    {
        if (translation is null)
        {
            throw new ArgumentNullException(nameof(translation));
        }
        var transform = boundingBox.Transform;
        var origin = transform.Origin.Add(translation);
        transform.Origin = origin;
        return boundingBox.SetTransform(transform);
    }

    public static BoundingBoxXYZ SetMin(this BoundingBoxXYZ boundingBox, XYZ min)
    {
        var clonedBox = boundingBox.Clone();
        clonedBox.Min = min;
        return clonedBox;
    }

    public static BoundingBoxXYZ SetMax(this BoundingBoxXYZ boundingBox, XYZ max)
    {
        var clonedBox = boundingBox.Clone();
        clonedBox.Max = max;
        return clonedBox;
    }

    public static BoundingBoxXYZ SetTransform(this BoundingBoxXYZ boundingBox, Transform transform)
    {
        var box = boundingBox.Clone();
        box.Transform = transform;
        return box;
    }

    public static BoundingBoxXYZ SetOrigin(this BoundingBoxXYZ boundingBox, XYZ origin)
    {
        var vectorToMoveBy = Vector.ByTwoPoints(boundingBox.Origin(), origin);
        return boundingBox.MoveBy(vectorToMoveBy);
    }

    public static BoundingBoxXYZ SetNewBounds(this BoundingBoxXYZ boundingBox, XYZ min, XYZ max)
    {
        var clonedBox = boundingBox.Clone();
        clonedBox.Min = min;
        clonedBox.Max = max;
        return clonedBox;
    }

    public static BoundingBoxXYZ ExtrudeUpwards(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMax = boundingBox.Max.Add(XYZ.BasisZ * value);
        return boundingBox.SetMax(newMax);
    }

    public static BoundingBoxXYZ ExtrudeFront(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMax = boundingBox.Max.Add(XYZ.BasisY * value);
        return boundingBox.SetMax(newMax);
    }

    public static BoundingBoxXYZ ExtrudeBack(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMin = boundingBox.Min.Add(XYZ.BasisY.Negate() * value);
        return boundingBox.SetMin(newMin);
    }

    public static BoundingBoxXYZ ExtrudeRight(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMax = boundingBox.Max.Add(XYZ.BasisX * value);
        return boundingBox.SetMax(newMax);
    }

    public static BoundingBoxXYZ ExtrudeLeft(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMin = boundingBox.Min.Add(XYZ.BasisX.Negate() * value);
        return boundingBox.SetMin(newMin);
    }

    public static BoundingBoxXYZ ExtrudeDownwards(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMin = boundingBox.Min.Add(XYZ.BasisZ.Negate() * value);
        return boundingBox.SetMin(newMin);
    }

    public static BoundingBoxXYZ Clone(this BoundingBoxXYZ boundingBox)
    {
        return new BoundingBoxXYZ()
        {
            Min = boundingBox.Min,
            Max = boundingBox.Max,
            Transform = boundingBox.Transform,
        };
    }
}
