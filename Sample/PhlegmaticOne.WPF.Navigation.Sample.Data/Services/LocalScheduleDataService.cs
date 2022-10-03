using PhlegmaticOne.WPF.Navigation.Sample.Contracts.Results;
using PhlegmaticOne.WPF.Navigation.Sample.Contracts.Services;
using PhlegmaticOne.WPF.Navigation.Sample.Models;

namespace PhlegmaticOne.WPF.Navigation.Sample.Data.Repositories;

public class LocalScheduleDataService : IScheduleDataService
{
    private readonly List<ScheduleModel> _schedules = new();
    public AddResult Add(ScheduleModel schedule)
    {
        schedule.Id = Guid.NewGuid();
        var isOk = Validate(schedule);
        if (isOk)
        {
            _schedules.Add(schedule);
        }
        return new() { IsOk = isOk };
    }

    public DeleteResult Delete(ScheduleModel schedule)
    {
        var isOk = true;
        var existing = _schedules.FirstOrDefault(x => x.Id == schedule.Id);
        if (existing is null)
        {
            isOk = false;
        }
        else
        {
            _schedules.Remove(existing);
        }
        return new() { IsOk = isOk };
    }

    public SelectResult<ScheduleModel> GetAll()
    {
        return new()
        {
            IsOk = true,
            Result = _schedules
        };
    }

    public SelectResult<ScheduleModel> GetSchedule(Guid id)
    {
        var schedules = _schedules.Where(x => x.Id == id).ToList();
        var isOk = schedules.Count == 1;
        return new()
        {
            IsOk = isOk,
            Result = schedules
        };
    }

    public UpdateResult Update(ScheduleModel schedule)
    {
        var isOk = true;
        var existing = _schedules.FirstOrDefault(x => x.Id == schedule.Id);
        if(existing is null || Validate(schedule) == false)
        {
            isOk = false;
        }
        else
        {
            _schedules.Remove(existing);
            _schedules.Add(schedule);
        }
        return new() { IsOk = isOk };
    }

    private bool Validate(ScheduleModel scheduleModel)
    {
        var now = DateTime.Today;
        var today = new DateOnly(now.Year, now.Month, now.Day);

        if(scheduleModel.ScheduleDay < today)
        {
            return false;
        }

        if(scheduleModel.ScheduleTimeStart >= scheduleModel.ScheduleTimeEnd)
        {
            return false;
        }

        return true;
    }
}
