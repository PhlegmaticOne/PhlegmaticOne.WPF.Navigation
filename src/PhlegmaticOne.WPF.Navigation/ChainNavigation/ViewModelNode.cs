using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.ChainNavigation;

internal class ViewModelNode
{
    internal ViewModelNode? Previous { get; set; }
    internal ViewModelNode? Next { get; set; }
    internal ApplicationBaseViewModel ViewModel { get; set; }

    internal ViewModelNode(ApplicationBaseViewModel viewModel)
    {
        ViewModel = viewModel;
    }
}