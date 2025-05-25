using Autodesk.Revit.DB;

namespace Craftify.Revit.Extensions.Geometry;

public static class Vector
{
    public static XYZ ByTwoPoints(XYZ start, XYZ end)
    {
        return end - start;
    }
}
