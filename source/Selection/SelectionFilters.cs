namespace Craftify.Revit.Extensions.Selection;

public abstract class BaseSelectionFilter(Func<Element, bool> selectionElementPredicate)
    : ISelectionFilter
{
    protected readonly Func<Element, bool> SelectionElementPredicate = selectionElementPredicate;

    public abstract bool AllowElement(Element elem);
    public abstract bool AllowReference(Reference reference, XYZ position);
}

public class ElementSelectionFilter : BaseSelectionFilter
{
    private readonly Func<Reference, bool>? _validateReference;

    public ElementSelectionFilter(Func<Element, bool> selectionElementPredicate)
        : base(selectionElementPredicate) { }

    public ElementSelectionFilter(
        Func<Element, bool> selectionElementPredicate,
        Func<Reference, bool> validateReference
    )
        : base(selectionElementPredicate) => _validateReference = validateReference;

    public override bool AllowElement(Element elem) => SelectionElementPredicate(elem);

    public override bool AllowReference(Reference reference, XYZ position) =>
        _validateReference?.Invoke(reference) ?? true;
}

public class LinkableSelectionFilter(
    Document document,
    Func<Element, bool> selectionElementPredicate
) : BaseSelectionFilter(selectionElementPredicate)
{
    public override bool AllowElement(Element elem) => true;

    public override bool AllowReference(Reference reference, XYZ position)
    {
        if (document.GetElement(reference.ElementId) is RevitLinkInstance revitLinkInstance)
        {
            var element = revitLinkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);
            return SelectionElementPredicate(element);
        }

        return SelectionElementPredicate(document.GetElement(reference.ElementId));
    }
}
