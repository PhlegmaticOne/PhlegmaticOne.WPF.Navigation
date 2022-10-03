using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.WPF.Navigation.Extensions;
using PhlegmaticOne.WPF.Navigation.Sample.Contracts.NavigationFactories;
using PhlegmaticOne.WPF.Navigation.Sample.Contracts.Services;
using PhlegmaticOne.WPF.Navigation.Sample.Data.Repositories;
using PhlegmaticOne.WPF.Navigation.Sample.ViewModels;
using PhlegmaticOne.WPF.Navigation.Sample.Views;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Base;
using System;
using System.Windows;

namespace PhlegmaticOne.WPF.Navigation.Sample.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = BuildServices();

            var mainWindow = services.GetRequiredService<MainWindow>();

            mainWindow.Show();
        }

        private IServiceProvider BuildServices()
        {
            var serviceCollection = new ServiceCollection();
            AddNavigation(serviceCollection);
            serviceCollection.AddSingleton<IScheduleDataService, LocalScheduleDataService>();
            serviceCollection.AddSingleton<MainWindow>();
            return serviceCollection.BuildServiceProvider();
        }

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
                b.UseChainNavigation();
                b.AddEntityContainingNavigation(typeof(ScheduleModelToScheduleViewModelNavigationFactory).Assembly);
                b.BindViewModelsToViews(Current, bindingPolicy);
            });
        }
    }
}
