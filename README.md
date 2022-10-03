# PhlegmaticOne.WPF.Navigation

![Logo - Copy](https://user-images.githubusercontent.com/73738250/192877116-bf055039-8220-4ec7-bec0-66c1e269910f.png)
##

## Nuget package
[PhlegmaticOne.WPF.Navigation](https://www.nuget.org/packages/PhlegmaticOne.WPF.Navigation/)
##

## Installation
```
PM> NuGet\Install-Package PhlegmaticOne.WPF.Navigation -Version 2.0.2
```
# Usage

## Setup application structure

In this guide will be shown how to setup application where Models, ViewModels and Views are placed in different projects just to show all moments in navigation setup

## Application initial structure

![image](https://user-images.githubusercontent.com/73738250/193564538-0b3a6c1f-c6b0-4c75-bb8f-c1ecc176bdd5.png)

### What is what
- App - WPF application
- Contracts - All services and other stuff needed for application to work
- Data - Project with data access (for sample there will be no data access)
- Models - Project with Models
- ViewModels - Project with ViewModels
- Views - Project with UserControls

All of this, of course, can be placed in a single WPF application project

Anyway, further here will be shown only ViewModels setup

## Tip

Install in your Models project (if it exists):
- ```PropertyChanged.Fody``` - it allows not to call ```OnPropertyChanged``` method explicitly
- ```PhlegmaticOne.WPF.Core``` - provides base type for all Models - ```EntityViewModelBase```, which implements ```INotifyPropertyChanged``` interface

## ViewModels

### Install this packages

![image](https://user-images.githubusercontent.com/73738250/193579037-89ec9e4f-ef5a-4b85-ab4d-e75b79567098.png)

- PropertyChanged.Fody - it allows not to call ```OnPropertyChanged``` method explicitly
- PhlegmaticOne.WPF.Navigation (this package) - provides WPF navigation

## Typical ViewModel

```csharp
public class AllSchedulesViewModel : ApplicationBaseViewModel
{
    private readonly IScheduleDataService _scheduleDataService;
    
    public AllSchedulesViewModel(IScheduleDataService scheduleDataService)
    {
        _scheduleDataService = scheduleDataService;
       ...
    }
    ...
}
```
Since all ViewModels will be registered in ```IServiceCollection``` it is allowed to inject any registered services in any ViewModel

## EntityContainingViewModel

```csharp
public class ScheduleViewModel : ApplicationBaseViewModel, IEntityContainingViewModel<ScheduleModel>
{
    public ScheduleModel Entity { get; set; }
}
```
Here you also can inject any services you need

## NavigationViewModel

Here is a simple NavigationViewModel

```csharp
public class NavigationViewModel : ApplicationBaseViewModel, IDisposable
{
    private readonly INavigationService _navigationService;

    public NavigationViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.ViewModelChanged += NavigationService_ViewModelChanged;

        NavigateCommand = RelayCommandFactory.CreateRequiredParameterCommand<Type>(Navigate, _ => true);

        Navigate(typeof(HomeViewModel));
    }
    public ApplicationBaseViewModel CurrentViewModel { get; private set; } = null!;
    public IRelayCommand NavigateCommand { get; set; }

    public void Dispose()
    {
        _navigationService.ViewModelChanged -= NavigationService_ViewModelChanged;
    }

    private void Navigate(Type viewModelType)
    {
        _navigationService.NavigateTo(viewModelType);
    }

    private void NavigationService_ViewModelChanged(object? sender, ApplicationBaseViewModel e)
    {
        CurrentViewModel = _navigationService.CurrentViewModel;
    }
}
```

## ChainedNavigationViewModel

If you are registered IChainedNavigationService instead of INavigationService you can write something like that:

```csharp
public class NavigationViewModel : ApplicationBaseViewModel, IDisposable
{
    private readonly IChainNavigationService _chainNavigationService;
    public NavigationViewModel(IChainNavigationService chainNavigationService)
    {
        _chainNavigationService = chainNavigationService;
        _chainNavigationService.DirectionCanMoveChanged += ChainNavigationServiceOnDirectionCanMoveChanged;
        _chainNavigationService.ViewModelChanged += ChainNavigationServiceOnViewModelChanged;

        MoveCommand = RelayCommandFactory
            .CreateRequiredParameterCommand<NavigationMoveDirection>(Move, _ => true);

        NavigateCommand = RelayCommandFactory
            .CreateRequiredParameterCommand<Type>(Navigate, _ => true);

        ResetCommand = RelayCommandFactory.CreateCommand(Reset, _ => true);

        NavigateDefault();
    }
    public bool CanMoveBack { get; private set; }
    public bool CanMoveForward { get; private set; }
    public ApplicationBaseViewModel CurrentViewModel { get; private set; } = null!;
    public IRelayCommand NavigateCommand { get; }
    public IRelayCommand MoveCommand { get; }
    public IRelayCommand ResetCommand { get; }

    private void Move(NavigationMoveDirection navigationMoveDirection)
    {
        _chainNavigationService.Move(navigationMoveDirection);
    }

    private void Navigate(Type parameter)
    {
        _chainNavigationService.NavigateTo(parameter);
    }
    private void Reset(object? _)
    {
        _chainNavigationService.Reset();
        NavigateDefault();
    }
    private void NavigateDefault()
    {
        Navigate(typeof(HomeViewModel));
    }
    private void ChainNavigationServiceOnViewModelChanged(object? sender, ApplicationBaseViewModel e)
    {
        CurrentViewModel = e;
    }

    private void ChainNavigationServiceOnDirectionCanMoveChanged(object? sender, NavigationMoveDirectionChangedArgs e)
    {
        switch (e.NavigationMoveDirection)
        {
            case NavigationMoveDirection.Forward:
            {
                CanMoveForward = e.CanMove;
                break;
            }
            case NavigationMoveDirection.Back:
            {
                CanMoveBack = e.CanMove;
                break;
            }
        }
    }

    public void Dispose()
    {
        _chainNavigationService.DirectionCanMoveChanged -= ChainNavigationServiceOnDirectionCanMoveChanged;
        _chainNavigationService.ViewModelChanged -= ChainNavigationServiceOnViewModelChanged;
    }
}
```

## EntityContainingViewModelsNavigation

In order to use this navigation you need to implement ```NavigationFactory<TFrom, TTo>```, they are used by ```EntityContainingViewModelsNavigationService``` during navigation process. Let's see example.

This example is not very useful, but is show the concept.

Suppose we have ViewModel with list of ScheduleModels and we want to navigate to specified one:

```csharp
public class AllSchedulesViewModel : ApplicationBaseViewModel
{
    private readonly IEntityContainingViewModelsNavigationService _entityContainingViewModelsNavigationService;
    ...
    public AllSchedulesViewModel(...,  
      IEntityContainingViewModelsNavigationService entityContainingViewModelsNavigationService)
    {
      _entityContainingViewModelsNavigationService = entityContainingViewModelsNavigationService;
      Schedules = new();
      ...
      NavigateToScheduleCommand = RelayCommandFactory
        .CreateRequiredParameterAsyncCommand<ScheduleModel>(NavigateToSchedule, _ => true);
    }
    public ObservableCollection<ScheduleModel> Schedules { get; }
    public IRelayCommand NavigateToScheduleCommand { get; }
    private async Task NavigateToSchedule(ScheduleModel scheduleModel)
    {
      await _entityContainingViewModelsNavigationService
        .From<ScheduleModel, ScheduleModel>()
        .NavigateAsync<ScheduleViewModel>(scheduleModel);
    }
    ...
}
```

Navigation process starts here:

```csharp
private async Task NavigateToSchedule(ScheduleModel scheduleModel)
{
  await _entityContainingViewModelsNavigationService
    .From<ScheduleModel, ScheduleModel>()
    .NavigateAsync<ScheduleViewModel>(scheduleModel);
}
```

It means that we want to navigate from ```ScheduleModel``` (first generic type in method ```From```) to ```ApplciationViewModel``` that implements interface ```IEntityContainingViewModel<T>``` (here T is ```ScheduleModel```) (generic type in ```NavigateAsync``` method) that has single ```EntityViewModel``` of type ```ScheduleModel``` (second generic type in method ```From```). During the navigation ```NavigationFactoryBase<ScheduleModel, ScheduleModel>``` will be found and used to create ```ScheduleModel``` from ```ScheduleModel``` object; it means that we need to implement it.

### NavigationFactory

```csharp
public class ScheduleModelToScheduleViewModelNavigationFactory : NavigationFactoryBase<ScheduleModel, ScheduleModel>
{
    private readonly IScheduleDataService _scheduleDataService;

    public ScheduleModelToScheduleViewModelNavigationFactory(IScheduleDataService scheduleDataService)
    {
        _scheduleDataService = scheduleDataService;
    }
    public override Task<ScheduleModel> CreateViewModelAsync(ScheduleModel entityViewModel)
    {
        var result = _scheduleDataService.GetSchedule(entityViewModel.Id);
        if (result.IsOk)
        {
            return Task.FromResult(result.Result.First());
        }
        else
        {
            return Task.FromResult(entityViewModel);
        }
    }
}
```

Since all NavigationFactories will be registered in ```IServiceCollection``` it is allowed to inject any registered services in any NavigationFactory.

In practical case, instead of two equal types, there will be types, for example: ```SchedulePreviewModel``` and ```ScheduleFullModel```. And navigation will look like:

```csharp
private async Task NavigateToSchedule(SchedulePreviewModel schedulePreviewModel)
{
  await _entityContainingViewModelsNavigationService
    .From<SchedulePreviewModel, ScheduleFullModel>()
    .NavigateAsync<ScheduleViewModel>(scheduleModel);
}
```

## Registering NavigationService

### Binding ViewModels to Views

Now supported 3 policies to bind ViewModels to Views:

- Hand binding
- Attributes binding
- Auto binding

Let's see how they works.

### 1. Hand Binding

Bind ViewModels to Views in WPF App project explicitly

```csharp
var bindingPolicy = new HandViewModelsToViewsBindingInfoProvider()
                .Bind<HomeViewModel, HomeView>()
                .Bind<AllSchedulesViewModel, AllSchedulesView>()
                .Bind<CreatingScheduleViewModel, CreatingScheduleView>()
                .Bind<ScheduleViewModel, ScheduleView>();
```

### 2. Attributes bindings

Use ```HasView``` attribute to mark ViewModel as ViewModel that has View. In that case, Views for ViewModels will be found by ViewModel name without 2 last words, for example: ViewModel name - AllSchedulesViewModel, name in finding process - AllSchedules; your View in that case must have name 'AllSchedules...', otherwise View won't be found.

```csharp
[HasView]
public class AllSchedulesViewModel : ApplicationBaseViewModel
{...}
```

To specify View name more explicitly use ```HasView``` attribute with View name. In that case View will be found directly by specified name

```csharp
[HasView("AllSchedulesView")]
public class AllSchedulesViewModel : ApplicationBaseViewModel
{...}
```

### 3. Auto bindings

You don't need to use attributes or hand bindings. Instead of this you only need to specify last word in your View type names and last word in Views namespace. For example, let's see how it works in current guide application: all views (HomeView, SchedulesView, ...) ends with 'View' that means last word in View names is 'View'; all views placed in project with namespace - PhlegmaticOne.WPF.Navigation.Sample.Views - that means last word in namespace is 'Views'. Relation between ViewModels and Views also will be found depending on ViewModel start words in their names.

All you need is to specify that info in binding provider.
```csharp
var bindingPolicy = new AutoScanViewModelsToViewsBindingInfoProvider("View", "Views");
```

### Warn
All your views must be placed in single namespace

### ServiceCollection registration
##
### Default navigation registration
```csharp
private void AddNavigation(IServiceCollection serviceCollection)
{
    IViewModelsToViewsBindingInfoProvider bindingPolicy;

    bindingPolicy = new HandViewModelsToViewsBindingInfoProvider()
        .Bind<HomeViewModel, HomeView>()
        .Bind<AllSchedulesViewModel, AllSchedulesView>()
        .Bind<CreatingScheduleViewModel, CreatingScheduleView>()
        .Bind<ScheduleViewModel, ScheduleView>();

    //bindingPolicy = new AutoScanViewModelsToViewsBindingInfoProvider("View", "Views");

    //bindingPolicy = new AttributesViewModelsToViewsBindingInfoProvider();

    serviceCollection.AddNavigation(typeof(HomeViewModel).Assembly, typeof(HomeView).Assembly, b =>
    {
        b.UseDefaultNavigation();
        b.BindViewModelsToViews(Current, bindingPolicy);
    });
}
```

### Chained navigation registration
```csharp
serviceCollection.AddNavigation(typeof(HomeViewModel).Assembly, typeof(HomeView).Assembly, b =>
{
    b.UseChainNavigation();
    b.BindViewModelsToViews(Current, bindingPolicy);
});
```

### Registration With EntityContainingViewModelsNavigation
```csharp
serviceCollection.AddNavigation(typeof(HomeViewModel).Assembly, typeof(HomeView).Assembly, b =>
{
    b.UseDefaultNavigation();
    //or b.UseChainNavigation();
    b.AddEntityContainingNavigation(typeof(ScheduleModelToScheduleViewModelNavigationFactory).Assembly);
    b.BindViewModelsToViews(Current, bindingPolicy);
});
```

### Creating Navigation Control

Last thing you need to do is create NavigationBar control and place it in MainWindow. It can look like this:

```xaml
<Grid Width="300">
    <Grid.RowDefinitions>
        <RowDefinition Height="60"/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <Grid Grid.Row="0">
        <StackPanel Orientation="Horizontal">
            <Button Content="&lt;" Width="60" Command="{Binding MoveCommand}"
                    CommandParameter="{x:Static nav:NavigationMoveDirection.Back}"
                    IsEnabled="{Binding CanMoveBack}"/>
            <Button Content="&gt;" Width="60" Command="{Binding MoveCommand}"
                    CommandParameter="{x:Static nav:NavigationMoveDirection.Forward}"
                    IsEnabled="{Binding CanMoveForward}"/>
            <Button Content="Reset" Width="60" Command="{Binding ResetCommand}"/>
        </StackPanel>
    </Grid>
    <Grid Grid.Row="1">
        <ScrollViewer>
            <StackPanel>
                <Button Content="Home" Command="{Binding NavigateCommand}"
                    CommandParameter="{x:Type viewmodels:HomeViewModel}"/>
                <Button Content="All schedules" Command="{Binding NavigateCommand}"
                    CommandParameter="{x:Type viewmodels:AllSchedulesViewModel}"/>
                <Button Content="Create schedule" Command="{Binding NavigateCommand}"
                    CommandParameter="{x:Type viewmodels:CreatingScheduleViewModel}"/>
            </StackPanel>
        </ScrollViewer>
    </Grid>
</Grid>
```

And finally place it in MainWindow and set binding to ```CurrentViewModel``` in ```NavigationViewModel```

```xaml
<Grid Background="#181818">
    <DockPanel>
        <controls:NavigationBar DockPanel.Dock="Left" DataContext="{Binding NavigationViewModel}"/>
        <Border>
            <ContentPresenter Content="{Binding NavigationViewModel.CurrentViewModel}" Grid.Row="1"/>
        </Border>
    </DockPanel>
</Grid>
```

### Now it should work!

### See below!

![ezgif-3-971babd394](https://user-images.githubusercontent.com/73738250/193639808-e082cc91-3ba9-4ddd-a0ee-3266c66fb5c1.gif)

### And see working sample!

![ezgif-3-a776a90eba](https://user-images.githubusercontent.com/73738250/193646204-225cb7c1-67e6-423d-b58e-39e760d672a6.gif)
