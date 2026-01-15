using Microsoft.AspNetCore.Mvc;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.SupportRequests;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.SupportRequests;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public class SupportRequestController(ISupportRequestService service, SupportRequestModelFactory factory,
    INotificationService notificationService, ILocalizationService localizationService) : BaseAdminController
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

            await service.UpdateSupportRequestAsync(item);

            notificationService.SuccessNotification(await localizationService.GetResourceAsync("Plugins.Misc.SupportRequests.Admin.Updated"));

            return continueEditing
                ? RedirectToAction("Edit", new { id = item.Id })
                : RedirectToAction("List");
        }

        model = await factory.PrepareSupportRequestModelAsync(model, item, true);

        return View(model);
    }
}
