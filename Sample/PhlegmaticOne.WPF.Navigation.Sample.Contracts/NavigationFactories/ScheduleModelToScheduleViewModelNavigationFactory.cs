using PhlegmaticOne.WPF.Navigation.Sample.Contracts.Services;
using PhlegmaticOne.WPF.Navigation.Sample.Models;

namespace PhlegmaticOne.WPF.Navigation.Sample.Contracts.NavigationFactories;

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
