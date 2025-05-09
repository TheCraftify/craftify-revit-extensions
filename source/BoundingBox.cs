namespace Craftify.Revit.Extensions;

public static class BoundingBox
{
    public static BoundingBoxXYZ FromPoints(IEnumerable<XYZ> points)
    {
        if (!points.Any())
        {
            throw new ArgumentException(
                "There is no point present while creating the Bounding Box"
            );
        }

        var minX = points.Min(point => point.X);
        var minY = points.Min(point => point.Y);
        var minZ = points.Min(point => point.Z);

        var maxX = points.Max(point => point.X);
        var maxY = points.Max(point => point.Y);
        var maxZ = points.Max(point => point.Z);

        return new BoundingBoxXYZ
        {
            Min = new XYZ(minX, minY, minZ),
            Max = new XYZ(maxX, maxY, maxZ),
        };
    }

    public static BoundingBoxXYZ FromSolids(IEnumerable<Solid> solids)
    {
        var boundingBoxes = solids.Select(solid => solid.GetBoundingBox()).ToList();

        if (!boundingBoxes.Any())
        {
            throw new ArgumentException("There is no valid solid while creating the Bounding Box");
        }

        var minX = boundingBoxes.Min(boundingBox => boundingBox.Min.X);
        var minY = boundingBoxes.Min(boundingBox => boundingBox.Min.Y);
        var minZ = boundingBoxes.Min(boundingBox => boundingBox.Min.Z);

        var maxX = boundingBoxes.Max(boundingBox => boundingBox.Max.X);
        var maxY = boundingBoxes.Max(boundingBox => boundingBox.Max.Y);
        var maxZ = boundingBoxes.Max(boundingBox => boundingBox.Max.Z);

        return new BoundingBoxXYZ
        {
            Min = new XYZ(minX, minY, minZ),
            Max = new XYZ(maxX, maxY, maxZ),
        };
    }
}
