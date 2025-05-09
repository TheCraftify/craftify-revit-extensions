namespace Craftify.Revit.Extensions.Parameters;

public static partial class DocumentExtensions
{
    public static bool AddProjectParameter(
        this Document document,
        Definition definition,
#if Revit2020 || Revit2021
        BuiltInParameterGroup builtInParameterGroup = BuiltInParameterGroup.PG_DATA,
#else
        ForgeTypeId? groupTypeId = default,
#endif
        bool isInstanceParameter = true,
        params BuiltInCategory[] builtInCategories
    )
    {
        var existingProjectParameter = document.ParameterBindings.get_Item(definition);

        if (existingProjectParameter is not null)
        {
            return false;
        }

        var categorySet = new CategorySet();

        foreach (var builtInCategory in builtInCategories)
        {
            categorySet.Insert(Category.GetCategory(document, builtInCategory));
        }

        Binding binding = isInstanceParameter
            ? new InstanceBinding(categorySet)
            : new TypeBinding(categorySet);
#if Revit2020 || Revit2021
        return document.ParameterBindings.Insert(definition, binding, builtInParameterGroup);
#else
        return document.ParameterBindings.Insert(
            definition,
            binding,
            groupTypeId ?? GroupTypeId.Data
        );
#endif
    }
}
