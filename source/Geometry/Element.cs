namespace Craftify.Revit.Extensions.Geometry;

public static partial class ElementExtensions
{
    public static IEnumerable<GeometryObject> Geometry(
        this Element element,
        Transform? transform = default,
        Action<Options>? optionsBuilder = default
    )
    {
        var options = new Options();
        optionsBuilder?.Invoke(options);
        transform ??= Transform.Identity;

        return element.get_Geometry(options).GetTransformed(transform).OfType<GeometryObject>();
    }

    public static BoundingBoxXYZ BoundingBox(this Element element, View? activeView = default) =>
        element.get_BoundingBox(activeView);

    public static IEnumerable<XYZ> LocationPoints(this Element element) =>
        element.Location switch
        {
            LocationPoint locationPoint => [locationPoint.Point],
            LocationCurve locationCurve => locationCurve.Curve.EndPoints(),
            _ => [],
        };
}
