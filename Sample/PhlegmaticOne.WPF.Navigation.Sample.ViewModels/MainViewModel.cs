using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.Sample.ViewModels;

public class MainViewModel : ApplicationBaseViewModel
{
	public MainViewModel(NavigationViewModel navigationViewModel)
	{
		NavigationViewModel = navigationViewModel;
	}

	public NavigationViewModel NavigationViewModel { get; }
}
