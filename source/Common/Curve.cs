namespace Craftify.Revit.Extensions.Common;

public static partial class CurveExtensions
{
    public static IEnumerable<XYZ> EndPoints(this Curve curve)
    {
        yield return curve.Start();
        yield return curve.End();
    }

    public static XYZ Start(this Curve curve) => curve.GetEndPoint(0);

    public static XYZ End(this Curve curve) => curve.GetEndPoint(1);
}
