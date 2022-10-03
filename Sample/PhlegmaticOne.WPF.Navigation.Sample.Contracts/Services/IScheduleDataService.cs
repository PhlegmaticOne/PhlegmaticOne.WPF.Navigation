using PhlegmaticOne.WPF.Navigation.Sample.Contracts.Results;
using PhlegmaticOne.WPF.Navigation.Sample.Models;

namespace PhlegmaticOne.WPF.Navigation.Sample.Contracts.Services;

public interface IScheduleDataService
{
    SelectResult<ScheduleModel> GetSchedule(Guid id);
    SelectResult<ScheduleModel> GetAll();
    AddResult Add(ScheduleModel schedule);
    UpdateResult Update(ScheduleModel schedule);
    DeleteResult Delete(ScheduleModel schedule);
}