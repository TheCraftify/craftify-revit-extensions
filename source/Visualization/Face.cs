namespace Craftify.Revit.Extensions.Visualization;

public static partial class FaceExtensions
{
    public static void Visualize(
        this Face face,
        Document document,
        BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel
    ) =>
        document.CreateDirectShape(
            face.Edges().Select(edge => edge.AsCurve()).Cast<GeometryObject>().ToList(),
            builtInCategory
        );
}
