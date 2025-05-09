namespace Craftify.Revit.Extensions.Visualization;

#pragma warning disable S101 // Types should be named in PascalCase
public static partial class XYZExtensions
#pragma warning restore S101 // Types should be named in PascalCase
{
    public static void VisualizeAsPoint(
        this XYZ xyz,
        Document document,
        BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel
    ) => document.CreateDirectShape([Point.Create(xyz)], builtInCategory);
}
