using PhlegmaticOne.WPF.Core.Commands;
using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Sample.Contracts.Services;
using PhlegmaticOne.WPF.Navigation.Sample.Models;

namespace PhlegmaticOne.WPF.Navigation.Sample.ViewModels;

public class CreatingScheduleViewModel : ApplicationBaseViewModel
{
	private readonly IScheduleDataService _scheduleDataService;

	public CreatingScheduleViewModel(IScheduleDataService scheduleDataService)
	{
		_scheduleDataService = scheduleDataService;

		CreateScheduleCommand = RelayCommandFactory.CreateCommand(CreateSchedule, _ => true); 
	}

	public IRelayCommand CreateScheduleCommand { get; }
	private void CreateSchedule(object? _)
	{
		var startHour = Random.Shared.Next(0, 12);
		var endHour = Random.Shared.Next(startHour, 24);
		var day = Random.Shared.Next(1, 100);

		var schedule = new ScheduleModel
		{
			ScheduleDay = DateOnly.FromDateTime(DateTime.Today.AddDays(day)),
			ScheduleTimeStart = new TimeOnly(startHour, 0, 0),
			ScheduleTimeEnd = new TimeOnly(endHour, 0, 0),
		};

		_scheduleDataService.Add(schedule);
	}
}
