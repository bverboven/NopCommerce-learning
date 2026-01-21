using Nop.Core.Domain.Localization;
using Nop.Core.Domain.SupportRequests;
using Nop.Core.Events;
using Nop.Services.Caching;
using Nop.Services.Events;
using Nop.Services.Messages;

namespace Nop.Services.SupportRequests;

public class EventConsumer(IWorkflowMessageService workflowMessageService, LocalizationSettings localizationSettings)
    : CacheEventConsumer<SupportRequest>, IConsumer<EntityInsertedEvent<SupportRequest>>
{
    public override async Task HandleEventAsync(EntityInsertedEvent<SupportRequest> eventMessage)
    {
        await base.HandleEventAsync(eventMessage);
        await workflowMessageService.SendSupportRequestStoreOwnerNotificationMessageAsync(eventMessage.Entity, localizationSettings.DefaultAdminLanguageId);
    }
}
