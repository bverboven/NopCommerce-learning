using Nop.Services.ScheduleTasks;

namespace Nop.Services.SupportRequests;

public class DeleteSupportRequestsTask(ISupportRequestService supportRequestService) : IScheduleTask
{
    public virtual async Task ExecuteAsync()
    {
        await supportRequestService.DeleteOldItemsAsync();
    }
}
