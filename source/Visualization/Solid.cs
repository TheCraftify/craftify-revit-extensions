namespace Craftify.Revit.Extensions.Visualization;

public static partial class SolidExtensions
{
    public static void Visualize(
        this Solid solid,
        Document document,
        BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel
    ) => document.CreateDirectShape([solid], builtInCategory);
}
