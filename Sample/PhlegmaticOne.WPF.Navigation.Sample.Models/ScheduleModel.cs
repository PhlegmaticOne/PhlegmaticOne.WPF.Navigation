using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.Sample.Models;

public class ScheduleModel : EntityBaseViewModel
{
    public DateOnly ScheduleDay { get; set; }
    public TimeOnly ScheduleTimeStart { get; set; }
    public TimeOnly ScheduleTimeEnd { get; set; }
}
