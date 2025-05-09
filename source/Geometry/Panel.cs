namespace Craftify.Revit.Extensions.Geometry;

public static partial class PanelExtensions
{
    public static IEnumerable<GeometryObject> FlattenGeometry(
        this Panel panel,
        Action<Options>? optionsBuilder = default,
        Transform? transform = default
    )
    {
        var options = new Options();
        optionsBuilder?.Invoke(options);

        var nestedElements = panel.AllNestedElements();

        return nestedElements
            .SelectMany(nestedElement =>
                nestedElement switch
                {
                    Wall wall => wall.CurtainGrid switch
                    {
                        CurtainGrid curtainGrid => curtainGrid
                            .GetPanelIds()
                            .Select(panelId => panel.Document.GetElement(panelId))
                            .OfType<Panel>()
                            .SelectMany(panel => panel.FlattenGeometry(optionsBuilder, transform)),
                        _ => wall.Geometry(transform, optionsBuilder),
                    },
                    _ => nestedElement.Geometry(transform, optionsBuilder),
                }
            )
            .Concat(
                panel
                    .GetSubComponentIds()
                    .Select(elementId => panel.Document.GetElement(elementId))
                    .OfType<FamilyInstance>()
                    .SelectMany(familyInstance =>
                        familyInstance
                            .InstanceGeometry(optionsBuilder, transform)
                            .Concat(familyInstance.Geometry(transform, optionsBuilder))
                    )
            )
            .Concat(panel.InstanceGeometry(optionsBuilder, transform));
    }
}
