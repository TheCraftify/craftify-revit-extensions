namespace Craftify.Revit.Extensions.Common;

public static partial class CurtainGridExtensions
{
    public static IEnumerable<TElement> AllNestedElements<TElement>(
        this CurtainGrid curtainGrid,
        Document document
    )
        where TElement : Element
    {
        var panels = curtainGrid
            .GetPanelIds()
            .Select(panelId => document.GetElement(panelId))
            .OfType<Panel>();

        foreach (var panel in panels)
        {
            foreach (var nestedElement in panel.NestedElements<TElement>())
            {
                yield return nestedElement;
            }
        }
    }
}
