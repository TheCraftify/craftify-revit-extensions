namespace Craftify.Revit.Extensions.Common;

public static partial class PanelExtensions
{
    public static BoundingBoxXYZ BoundingBox(this Panel panel, View? activeView = default)
    {
        var panelBoundingBox = panel.get_BoundingBox(activeView);

        if (panelBoundingBox is not null)
        {
            return panelBoundingBox;
        }

        var nestedBoundingBoxes = panel
            .AllNestedElements()
            .Select(nestedElement => nestedElement.BoundingBox())
            .ToArray();

        //TODO: Optional
        if (nestedBoundingBoxes.Length == 0)
        {
            return new BoundingBoxXYZ { Min = XYZ.Zero, Max = XYZ.Zero };
        }

        return Extensions.BoundingBox.FromPoints(
            nestedBoundingBoxes.SelectMany(boundingBox =>
                new[] { boundingBox.Min, boundingBox.Max }
            )
        );
    }

    public static IEnumerable<TElement> NestedElements<TElement>(this Panel panel)
        where TElement : Element => panel.AllNestedElements().OfType<TElement>();

    public static IEnumerable<Element> AllNestedElements(this Panel panel) =>
        panel.Document.GetElement(panel.FindHostPanel()) switch
        {
            Panel nestedPanel => nestedPanel.AllNestedElements(),
            Wall nestedWall => nestedWall.CurtainGrid switch
            {
                CurtainGrid curtainGrid => curtainGrid
                    .GetPanelIds()
                    .Select(panelId => panel.Document.GetElement(panelId))
                    .OfType<Panel>()
                    .SelectMany(panel => panel.AllNestedElements()),
                _ => [nestedWall],
            },
            _ => [],
        };
}
