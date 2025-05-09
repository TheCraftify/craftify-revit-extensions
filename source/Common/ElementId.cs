namespace Craftify.Revit.Extensions.Common;

public static partial class ElementIdExtensions
{
    public static long Id(this ElementId elementId)
    {
#if Revit2020 || Revit2021 || Revit2022 || Revit2023
        return elementId.IntegerValue;
#else
        return elementId.Value;
#endif
    }
}
