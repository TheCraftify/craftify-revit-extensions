namespace Craftify.Revit.Extensions.Mappings;

public static partial class OutlineExtensions
{
    public static BoundingBoxXYZ ToBoundingBoxXYZ(this Outline outline) =>
        new() { Min = outline.MinimumPoint, Max = outline.MaximumPoint };
}
