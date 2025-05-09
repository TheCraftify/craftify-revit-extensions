namespace Craftify.Revit.Extensions.Visualization;

public static partial class BoundingBoxExtensions
{
    public static void VisualizeTransparent(
        this BoundingBoxXYZ boundingBoxXYZ,
        Document document,
        BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel
    ) =>
        document.CreateDirectShape(
            boundingBoxXYZ.ToSolid().Edges.AsCurves().Cast<GeometryObject>().ToList(),
            builtInCategory
        );

    public static void VisualizeSolid(
        this BoundingBoxXYZ boundingBoxXYZ,
        Document document,
        BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel
    ) => document.CreateDirectShape([boundingBoxXYZ.ToSolid()], builtInCategory);
}
