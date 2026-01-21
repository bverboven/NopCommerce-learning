using Nop.Core.Domain.Localization;
using Nop.Core.Domain.SupportRequests;
using Nop.Core.Events;
using Nop.Services.Events;
using Nop.Services.Messages;

namespace Nop.Services.SupportRequests;

public class EventConsumer(IWorkflowMessageService workflowMessageService, LocalizationSettings localizationSettings)
    : IConsumer<EntityInsertedEvent<SupportRequest>>
{
    public async Task HandleEventAsync(EntityInsertedEvent<SupportRequest> eventMessage)
    {
        await workflowMessageService.SendSupportRequestStoreOwnerNotificationMessageAsync(eventMessage.Entity, localizationSettings.DefaultAdminLanguageId);
    }
}
