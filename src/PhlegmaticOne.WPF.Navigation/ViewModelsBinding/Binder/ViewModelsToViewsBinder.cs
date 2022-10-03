using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Base;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Models;
using System.Reflection;
using System.Windows;

namespace PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Binder;

internal class ViewModelsToViewsBinder
{
    internal static void BindViewModelsToViews(Application application,
        Assembly viewModelsAssymbly,
        Assembly viewsAssembly,
        IViewModelsToViewsBindingInfoProvider viewModelsToViewsBindingInfoProvider)
    {
        var viewModelToViewsBindings = viewModelsToViewsBindingInfoProvider
            .GetViewModelToViewBindingInfo(viewModelsAssymbly, viewsAssembly);

        foreach (var viewModelBindingInfo in viewModelToViewsBindings)
        {
            Bind(application, viewModelBindingInfo);
        }
    }

    private static void Bind(Application application, ViewModelToViewBindingInfo viewModelToViewBindingInfo)
    {
        var dataTemplate = CreateDataTemplate(viewModelToViewBindingInfo.ViewModelType, viewModelToViewBindingInfo.ViewType);
        application.Resources.Add(new DataTemplateKey(viewModelToViewBindingInfo.ViewModelType), dataTemplate);
    }
    private static DataTemplate CreateDataTemplate(Type viewModelType, Type viewType) => new(viewModelType)
    {
        VisualTree = new(viewType)
    };
}
