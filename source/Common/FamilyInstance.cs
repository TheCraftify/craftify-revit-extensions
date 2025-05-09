namespace Craftify.Revit.Extensions.Common;

public static partial class FamilyInstanceExtensions
{
    private static readonly ElementClassFilter _tagFilter = new(typeof(IndependentTag));
    private static readonly ElementClassFilter _dimensionFilter = new(typeof(Dimension));

    public static IEnumerable<IndependentTag> IndependentTags(
        this FamilyInstance familyInstance,
        Document document
    ) =>
        familyInstance
            .GetDependentElements(_tagFilter)
            .Select(document.GetElement)
            .Cast<IndependentTag>();

    public static IEnumerable<Dimension> Dimensions(
        this FamilyInstance familyInstance,
        Document document
    ) =>
        familyInstance
            .GetDependentElements(_dimensionFilter)
            .Select(document.GetElement)
            .Cast<Dimension>();
}
