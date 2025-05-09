namespace Craftify.Revit.Extensions.Selection;

public static partial class UIDocumentExtensions
{
    public static Element SelectElement(
        this UIDocument uiDocument,
        Func<Element, bool> selectionElementPredicate,
        bool isLinkedElement = false,
        string? statusPrompt = default,
        Func<Reference, bool>? selectionReferencePredicate = null
    )
    {
        var objectType = isLinkedElement ? ObjectType.LinkedElement : ObjectType.Element;
        var elementSelectionFilter = selectionReferencePredicate is null
            ? new ElementSelectionFilter(selectionElementPredicate)
            : new ElementSelectionFilter(selectionElementPredicate, selectionReferencePredicate);

        return uiDocument.Document.GetElement(
            uiDocument.PerformSingleSelection(
                objectType,
                elementSelectionFilter,
                statusPrompt ?? string.Empty
            )
        );
    }

    public static IEnumerable<Element> SelectElementsInOrder(
        this UIDocument uiDocument,
        Func<Element, bool> selectionElementPredicate,
        bool isLinkedElement = false,
        string? statusPrompt = default,
        Func<Reference, bool>? selectionReferencePredicate = null
    )
    {
        var objectType = isLinkedElement ? ObjectType.LinkedElement : ObjectType.Element;
        var elementSelectionFilter = selectionReferencePredicate is null
            ? new ElementSelectionFilter(selectionElementPredicate)
            : new ElementSelectionFilter(selectionElementPredicate, selectionReferencePredicate);

        var isOperationCancelled = false;
        var references = new List<Reference>();

        while (!isOperationCancelled)
        {
            try
            {
                references.Add(
                    uiDocument.PerformSingleSelection(
                        objectType,
                        elementSelectionFilter,
                        statusPrompt ?? string.Empty
                    )
                );
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                isOperationCancelled = true;
            }
        }

        return references.Select(reference => uiDocument.Document.GetElement(reference));
    }

    public static IEnumerable<Element> SelectElements(
        this UIDocument uiDocument,
        Func<Element, bool> selectionElementPredicate,
        bool isLinkedElement = false,
        string? statusPrompt = default,
        Func<Reference, bool>? selectionReferencePredicate = null
    )
    {
        var objectType = isLinkedElement ? ObjectType.LinkedElement : ObjectType.Element;
        var elementSelectionFilter = selectionReferencePredicate is null
            ? new ElementSelectionFilter(selectionElementPredicate)
            : new ElementSelectionFilter(selectionElementPredicate, selectionReferencePredicate);

        var references = uiDocument.PerformMultiSelection(
            objectType,
            elementSelectionFilter,
            statusPrompt ?? string.Empty
        );

        return references.Select(reference => uiDocument.Document.GetElement(reference));
    }

    static Reference PerformSingleSelection(
        this UIDocument uiDocument,
        ObjectType objectType,
        ElementSelectionFilter elementSelectionFilter,
        string statusPrompt
    ) =>
        !string.IsNullOrEmpty(statusPrompt)
            ? uiDocument.Selection.PickObject(objectType, elementSelectionFilter, statusPrompt)
            : uiDocument.Selection.PickObject(objectType, elementSelectionFilter);

    static IList<Reference> PerformMultiSelection(
        this UIDocument uiDocument,
        ObjectType objectType,
        ElementSelectionFilter elementSelectionFilter,
        string statusPrompt
    ) =>
        !string.IsNullOrEmpty(statusPrompt)
            ? uiDocument.Selection.PickObjects(objectType, elementSelectionFilter, statusPrompt)
            : uiDocument.Selection.PickObjects(objectType, elementSelectionFilter);
}
