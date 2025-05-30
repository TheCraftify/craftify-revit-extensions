namespace Craftify.Revit.Extensions.Geometry;

public enum ApplyTransform
{
    Yes,
    No,
}

public enum BoundingBoxSide
{
    Length,
    Width,
    Height,
}

public record BoundingBoxDimensions(double Length, double Width, double Height);
