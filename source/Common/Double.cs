namespace Craftify.Revit.Extensions.Common;

public static partial class DoubleExtensions
{
    public static double RoundToTolerance(
        this double value,
        double tolerance = Defaults.Tolerance
    ) => Math.Round(value / tolerance) * tolerance;
}
