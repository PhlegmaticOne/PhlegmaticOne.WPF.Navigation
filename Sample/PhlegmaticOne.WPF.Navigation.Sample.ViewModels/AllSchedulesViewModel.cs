using PhlegmaticOne.WPF.Core.Commands;
using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Sample.Contracts.Services;
using PhlegmaticOne.WPF.Navigation.Sample.Models;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Attributes;
using System.Collections.ObjectModel;

namespace PhlegmaticOne.WPF.Navigation.Sample.ViewModels;

[HasView("AllSchedulesView")]
public class AllSchedulesViewModel : ApplicationBaseViewModel
{
	private readonly IScheduleDataService _scheduleDataService;
	private readonly IEntityContainingViewModelsNavigationService _entityContainingViewModelsNavigationService;

	public AllSchedulesViewModel(IScheduleDataService scheduleDataService, 
		IEntityContainingViewModelsNavigationService entityContainingViewModelsNavigationService)
	{
		_scheduleDataService = scheduleDataService;
		_entityContainingViewModelsNavigationService = entityContainingViewModelsNavigationService;
		Schedules = new();

		SelectSchedulesCommand = RelayCommandFactory.CreateEmptyCommand(SelectSchedules);

		NavigateToScheduleCommand = RelayCommandFactory
			.CreateRequiredParameterAsyncCommand<ScheduleModel>(NavigateToSchedule);

		SelectSchedules();
	}
	public ObservableCollection<ScheduleModel> Schedules { get; }
	public IRelayCommand SelectSchedulesCommand { get; }
	public IRelayCommand NavigateToScheduleCommand { get; }
	private async Task NavigateToSchedule(ScheduleModel scheduleModel)
	{
		await _entityContainingViewModelsNavigationService
			.From<ScheduleModel, ScheduleModel>()
			.NavigateAsync<ScheduleViewModel>(scheduleModel);
	}
	private void SelectSchedules()
	{
		var selectResult = _scheduleDataService.GetAll();

		if (selectResult.IsOk)
		{
			Schedules.Clear();
            foreach (var schedule in selectResult.Result)
            {
                Schedules.Add(schedule);
            }
        }
	}
}