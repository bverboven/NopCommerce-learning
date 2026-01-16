using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.SupportRequests;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.SupportRequests;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public class SupportRequestController(ISupportRequestService service, SupportRequestModelFactory factory,
    INotificationService notificationService, ILocalizationService localizationService, IGenericAttributeService genericAttributeService,
    IWorkflowMessageService workflowMessageService) : BaseAdminController
{
    [CheckPermission(StandardPermission.Configuration.MANAGE_SUPPORT_REQUESTS)]
    public virtual async Task<IActionResult> List()
    {
        var model = await factory.PrepareSupportRequestSearchModelAsync(new SupportRequestSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SUPPORT_REQUESTS)]
    public async Task<IActionResult> List(SupportRequestSearchModel searchModel)
    {
        var model = await factory.PrepareSupportRequestListModelAsync(searchModel);

        return Json(model);
    }


    [CheckPermission(StandardPermission.Configuration.MANAGE_SUPPORT_REQUESTS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var item = await service.GetSupportRequestByIdAsync(id);

        if (item == null)
            return RedirectToAction("List");

        var model = factory.PrepareSupportRequestModelAsync(null, item);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SUPPORT_REQUESTS)]
    public virtual async Task<IActionResult> Edit(SupportRequestModel model, bool continueEditing)
    {
        var item = await service.GetSupportRequestByIdAsync(model.Id);
        if (item == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            item.ReplyText = model.ReplyText;
            item.UpdatedOnUtc = DateTime.UtcNow;

            var customerNotifiedOfReplyKey = "CustomerNotifiedOfReply";
            var customerNotifiedOfReply = await genericAttributeService.GetAttributeAsync<bool>(item, customerNotifiedOfReplyKey);
            if (!string.IsNullOrWhiteSpace(item.ReplyText) && !customerNotifiedOfReply)
            {
                // get en-US since NopCustomerDefaults.LanguageIdAttribute is not created yet
                var customerLanguageId = await genericAttributeService.GetAttributeAsync<Customer, int>(item.CustomerId, NopCustomerDefaults.LanguageIdAttribute, item.StoreId, 1);

                var queuedEmailIds = await workflowMessageService.SendSupportRequestReplyCustomerNotificationMessageAsync(item, customerLanguageId);
                if (queuedEmailIds.Any())
                {
                    await genericAttributeService.SaveAttributeAsync(item, customerNotifiedOfReplyKey, true);
                }
            }

            await service.UpdateSupportRequestAsync(item);

            notificationService.SuccessNotification(await localizationService.GetResourceAsync("Admin.SupportRequests.Updated"));

            return continueEditing
                ? RedirectToAction("Edit", new { id = item.Id })
                : RedirectToAction("List");
        }

        model = await factory.PrepareSupportRequestModelAsync(model, item, true);

        return View(model);
    }
}
