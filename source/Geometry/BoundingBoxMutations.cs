using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class BoundingBoxMutations
{
    /// <summary>
    /// Creates a new bounding box moved by the specified translation vector.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="translation">The translation vector to apply.</param>
    /// <returns>A new bounding box with updated position.</returns>
    /// <exception cref="ArgumentNullException">Thrown when translation is null.</exception>
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

    /// <summary>
    /// Creates a new bounding box with an updated Min point.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="min">The new Min point.</param>
    /// <returns>A new bounding box with the updated Min point.</returns>
    public static BoundingBoxXYZ SetMin(this BoundingBoxXYZ boundingBox, XYZ min)
    {
        var clonedBox = boundingBox.Clone();
        clonedBox.Min = min;
        return clonedBox;
    }

    /// <summary>
    /// Creates a new bounding box with an updated Max point.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="max">The new Max point.</param>
    /// <returns>A new bounding box with the updated Max point.</returns>
    public static BoundingBoxXYZ SetMax(this BoundingBoxXYZ boundingBox, XYZ max)
    {
        var clonedBox = boundingBox.Clone();
        clonedBox.Max = max;
        return clonedBox;
    }

    /// <summary>
    /// Creates a new bounding box with an updated transform.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="transform">The new transform to apply.</param>
    /// <returns>A new bounding box with the updated transform.</returns>
    public static BoundingBoxXYZ SetTransform(this BoundingBoxXYZ boundingBox, Transform transform)
    {
        var box = boundingBox.Clone();
        box.Transform = transform;
        return box;
    }

    /// <summary>
    /// Creates a new bounding box with an updated origin point.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="origin">The new origin point.</param>
    /// <returns>A new bounding box with the updated origin.</returns>
    public static BoundingBoxXYZ SetOrigin(this BoundingBoxXYZ boundingBox, XYZ origin)
    {
        var vectorToMoveBy = Vector.ByTwoPoints(boundingBox.Origin(), origin);
        return boundingBox.MoveBy(vectorToMoveBy);
    }

    /// <summary>
    /// Creates a new bounding box with updated Min and Max points.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="min">The new Min point.</param>
    /// <param name="max">The new Max point.</param>
    /// <returns>A new bounding box with the updated bounds.</returns>
    public static BoundingBoxXYZ SetNewBounds(this BoundingBoxXYZ boundingBox, XYZ min, XYZ max)
    {
        var clonedBox = boundingBox.Clone();
        clonedBox.Min = min;
        clonedBox.Max = max;
        return clonedBox;
    }

    /// <summary>
    /// Creates a new bounding box extruded upward along the Z-axis.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="value">The distance to extrude.</param>
    /// <returns>A new bounding box extruded upward by the specified value.</returns>
    public static BoundingBoxXYZ ExtrudeUpwards(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMax = boundingBox.Max.Add(XYZ.BasisZ * value);
        return boundingBox.SetMax(newMax);
    }

    /// <summary>
    /// Creates a new bounding box extruded forward along the Y-axis.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="value">The distance to extrude.</param>
    /// <returns>A new bounding box extruded forward by the specified value.</returns>
    public static BoundingBoxXYZ ExtrudeFront(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMax = boundingBox.Max.Add(XYZ.BasisY * value);
        return boundingBox.SetMax(newMax);
    }

    /// <summary>
    /// Creates a new bounding box extruded backward along the negative Y-axis.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="value">The distance to extrude.</param>
    /// <returns>A new bounding box extruded backward by the specified value.</returns>
    public static BoundingBoxXYZ ExtrudeBack(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMin = boundingBox.Min.Add(XYZ.BasisY.Negate() * value);
        return boundingBox.SetMin(newMin);
    }

    /// <summary>
    /// Creates a new bounding box extruded to the right along the X-axis.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="value">The distance to extrude.</param>
    /// <returns>A new bounding box extruded to the right by the specified value.</returns>
    public static BoundingBoxXYZ ExtrudeRight(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMax = boundingBox.Max.Add(XYZ.BasisX * value);
        return boundingBox.SetMax(newMax);
    }

    /// <summary>
    /// Creates a new bounding box extruded to the left along the negative X-axis.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="value">The distance to extrude.</param>
    /// <returns>A new bounding box extruded to the left by the specified value.</returns>
    public static BoundingBoxXYZ ExtrudeLeft(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMin = boundingBox.Min.Add(XYZ.BasisX.Negate() * value);
        return boundingBox.SetMin(newMin);
    }

    /// <summary>
    /// Creates a new bounding box extruded downward along the negative Z-axis.
    /// </summary>
    /// <param name="boundingBox">The original bounding box.</param>
    /// <param name="value">The distance to extrude.</param>
    /// <returns>A new bounding box extruded downward by the specified value.</returns>
    public static BoundingBoxXYZ ExtrudeDownwards(this BoundingBoxXYZ boundingBox, double value)
    {
        var newMin = boundingBox.Min.Add(XYZ.BasisZ.Negate() * value);
        return boundingBox.SetMin(newMin);
    }

    /// <summary>
    /// Creates a deep copy of a bounding box.
    /// </summary>
    /// <param name="boundingBox">The bounding box to clone.</param>
    /// <returns>A new bounding box with the same Min, Max, and Transform properties.</returns>
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
