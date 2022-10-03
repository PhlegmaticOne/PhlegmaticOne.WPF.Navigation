using PhlegmaticOne.WPF.Core.ViewModels;
using System.Reflection;

namespace PhlegmaticOne.WPF.Navigation.Helpers;

internal class TypesGetter
{
    internal static List<Type> GetApplicationViewModels(Assembly assembly)
    {
        var viewModelTypes = assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(ApplicationBaseViewModel)) &&
                        x.GetInterfaces().All(i => !i.IsGenericType || i.GetGenericTypeDefinition() != typeof(IEntityContainingViewModel<>)) &&
                        x.IsAbstract == false)
            .ToList();
        return viewModelTypes;
    }
    internal static List<Type> GetEntityContainingViewModels(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(ApplicationBaseViewModel)) &&
                        x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityContainingViewModel<>)) &&
                        x.IsAbstract == false)
            .ToList();
    }

    internal static List<Type> GetTypesWithAttribute<TBase, TAttr>(Assembly assembly)
        where TAttr : Attribute
        where TBase : class
    {
        return assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(TBase)) && x.IsAbstract == false && x.GetCustomAttribute<TAttr>() is not null)
            .ToList();
    }
}
