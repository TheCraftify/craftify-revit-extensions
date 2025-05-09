using System.Collections.Generic;

namespace Craftify.Revit.Extensions.Common;

public static partial class HostObjectExtensions
{
    public static IEnumerable<Face> AllSideFaces(this HostObject hostObject, Document document)
    {
        foreach (var face in hostObject.SideFaces(document, ShellLayerType.Interior))
            yield return face;
        foreach (var face in hostObject.SideFaces(document, ShellLayerType.Exterior))
            yield return face;
    }

    public static IEnumerable<Face> TopFaces(this HostObject hostObject, Document document) =>
        HostObjectUtils
            .GetTopFaces(hostObject)
            .Select(reference =>
                document.GetElement(reference).GetGeometryObjectFromReference(reference)
            )
            .OfType<Face>();

    public static IEnumerable<Face> ButtomFaces(this HostObject hostObject, Document document) =>
        HostObjectUtils
            .GetBottomFaces(hostObject)
            .Select(reference =>
                document.GetElement(reference).GetGeometryObjectFromReference(reference)
            )
            .OfType<Face>();

    public static IEnumerable<Face> SideFaces(
        this HostObject hostObject,
        Document document,
        ShellLayerType shellLayerType = ShellLayerType.Interior
    ) =>
        HostObjectUtils
            .GetSideFaces(hostObject, shellLayerType)
            .Select(reference =>
                document.GetElement(reference).GetGeometryObjectFromReference(reference)
            )
            .OfType<Face>();

    public static IEnumerable<T> JoinedElements<T>(this HostObject hostObject, Document document)
        where T : Element =>
        JoinGeometryUtils
            .GetJoinedElements(document, hostObject)
            .Select(elementId => document.GetElement(elementId))
            .OfType<T>();
}
