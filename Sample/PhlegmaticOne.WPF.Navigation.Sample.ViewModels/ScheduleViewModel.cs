using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Sample.Models;

namespace PhlegmaticOne.WPF.Navigation.Sample.ViewModels;

public class ScheduleViewModel : ApplicationBaseViewModel, IEntityContainingViewModel<ScheduleModel>
{
    public ScheduleModel Entity { get; set; }
}
