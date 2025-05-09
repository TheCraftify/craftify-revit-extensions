namespace Craftify.Revit.Extensions.Common;

public static partial class ElementExtensions
{
    public static long Id(this Element element)
    {
#if Revit2020 || Revit2021 || Revit2022 || Revit2023
        return element.Id.IntegerValue;
#else
        return element.Id.Value;
#endif
    }
}
