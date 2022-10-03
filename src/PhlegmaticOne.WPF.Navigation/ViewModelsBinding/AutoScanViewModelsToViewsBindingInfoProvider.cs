using PhlegmaticOne.WPF.Navigation.Helpers;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Base;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Models;
using System.Reflection;
using System.Windows.Controls;

namespace PhlegmaticOne.WPF.Navigation.ViewModelsBinding;

/// <summary>
/// Gets ViewModels to Views binding info scanning type names of ViewModels and Views
/// </summary>
public class AutoScanViewModelsToViewsBindingInfoProvider : IViewModelsToViewsBindingInfoProvider
{
    private readonly string _viewsNameEnd;
    private readonly string _viewsNamespaceEnd;

    public AutoScanViewModelsToViewsBindingInfoProvider(string viewsNameEnd, string viewsNamespaceEnd)
    {
        _viewsNameEnd = viewsNameEnd;
        _viewsNamespaceEnd = viewsNamespaceEnd;
    }

    public static AutoScanViewModelsToViewsBindingInfoProvider Default =>
        new(Constants.DEFAULT_VIEW_NAME_END, Constants.DEFAULT_VIEWS_NAMESPACE_END);

    public IEnumerable<ViewModelToViewBindingInfo> GetViewModelToViewBindingInfo(Assembly viewModelsAssembly, Assembly viewsAssembly)
    {
        var viewModelTypes = TypesGetter.GetApplicationViewModels(viewModelsAssembly);
        var viewTypes = viewsAssembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(UserControl)) && x.Namespace is not null && x.Namespace.EndsWith(_viewsNamespaceEnd));

        foreach (var viewType in viewTypes)
        {
            var typeName = viewType.Name;
            var viewPartIndex = typeName.IndexOf(_viewsNameEnd);
            var mainNamePart = typeName.Substring(0, viewPartIndex);

            var viewModelType = viewModelTypes.FirstOrDefault(x => x.Name.Contains(mainNamePart,
                StringComparison.InvariantCultureIgnoreCase));

            if (viewModelType is null)
            {
                throw new ArgumentException($"Cannot find ViewModel in assembly {viewModelsAssembly} for view {viewType}");
            }

            yield return new(viewModelType, viewType);
        }
    }
}
