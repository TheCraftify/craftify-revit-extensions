namespace Craftify.Revit.Extensions.Geometry;

public static partial class FamilyInstanceExtensions
{
    public static IEnumerable<GeometryObject> InstanceGeometry(
        this FamilyInstance familyInstance,
        Action<Options>? optionsBuilder = default,
        Transform? transform = default
    )
    {
        var options = new Options();
        optionsBuilder?.Invoke(options);

        return familyInstance
            .get_Geometry(options)
            .OfType<GeometryInstance>()
            .SelectMany(geometryInstance =>
                transform is null
                    ? geometryInstance.GetInstanceGeometry()
                    : geometryInstance.GetInstanceGeometry(transform)
            );
    }
}
