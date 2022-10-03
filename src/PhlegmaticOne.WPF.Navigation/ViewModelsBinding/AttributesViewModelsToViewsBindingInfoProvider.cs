using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Exceptions;
using PhlegmaticOne.WPF.Navigation.Helpers;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Attributes;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Base;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Models;
using System.Reflection;
using System.Windows.Controls;

namespace PhlegmaticOne.WPF.Navigation.ViewModelsBinding;

/// <summary>
/// Gets ViewModels to Views binding info scanning ViewModels with HasView attributes
/// </summary>
public class AttributesViewModelsToViewsBindingInfoProvider : IViewModelsToViewsBindingInfoProvider
{
    public IEnumerable<ViewModelToViewBindingInfo> GetViewModelToViewBindingInfo(Assembly viewModelsAssembly, Assembly viewsAssembly)
    {
        var viewModelWithAttributesTypes = TypesGetter
            .GetTypesWithAttribute<ApplicationBaseViewModel, HasViewAttribute>(viewModelsAssembly);

        var viewsTypes = viewsAssembly.GetTypes().Where(x => x.IsAssignableTo(typeof(UserControl))).ToList();
        var result = new List<ViewModelToViewBindingInfo>();

        foreach (var viewModelType in viewModelWithAttributesTypes)
        {
            var attribute = viewModelType.GetCustomAttribute<HasViewAttribute>()!;

            var viewNameStart = (attribute.ViewTypeNameStart is not null) ?
                attribute.ViewTypeNameStart :
                GetViewModelNamePart(viewModelType.Name);

            var viewType = viewsTypes.FirstOrDefault(x => x.Name.StartsWith(viewNameStart));

            if (viewType is null)
            {
                throw new NavigationException($"Cannot find view in assembly {viewsAssembly} for ViewModel" +
                    $"{viewModelType} with attribute {nameof(attribute.ViewTypeNameStart)} value: " +
                    $"{attribute.ViewTypeNameStart}");
            }
            result.Add(new(viewModelType, viewType));
        }
        return result;
    }

    private string GetViewModelNamePart(string viewModelName)
    {
        var splitted = viewModelName.SplitByUppercase().SkipLast(2);
        return string.Concat(splitted);
    }
}
