namespace Craftify.Revit.Extensions.Mappings;

public static partial class ElementExtensions
{
    public static Reference ToReference(this Element element) => new(element);
}
