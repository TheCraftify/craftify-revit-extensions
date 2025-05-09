namespace Craftify.Revit.Extensions.Common;

public static partial class DoubleExtensions
{
    public static double RadiansToDegrees(this double radians) => radians * (180.0 / Math.PI);

    public static double DegreesToRadians(this double degrees) => degrees * (Math.PI / 180.0);

    public static double MillimetersToFeet(this double millimeters) => millimeters / 304.8d;

    public static double FeetToMillimeters(this double feets) => feets * 304.8d;
}
