using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.SupportRequests;
using Nop.Services.Customers;
using Nop.Services.SupportRequests;
using Nop.Web.Factories;
using Nop.Web.Models.SupportRequests;

namespace Nop.Web.Controllers;

public class SupportRequestController(ISupportRequestService service, SupportRequestModelFactory modelFactory,
    ICustomerService customerService, IWorkContext workContext, IStoreContext storeContext) : BasePublicController
{
    public async Task<IActionResult> CustomerSupportRequests()
    {
        var currentCustomer = await workContext.GetCurrentCustomerAsync();

        if (await customerService.IsGuestAsync((currentCustomer)))
        {
            return Challenge();
        }

        var items = await service.GetAllSupportRequestsByCustomerIdAsync(currentCustomer.Id);

        var models = new List<SupportRequestModel>();
        await modelFactory.PrepareSupportRequestListModelAsync(items, models);

        return View(models);
    }

    public virtual async Task<IActionResult> CreateSupportRequest()
    {
        var currentCustomer = await workContext.GetCurrentCustomerAsync();

        if (await customerService.IsGuestAsync((currentCustomer)))
        {
            return Challenge();
        }

        var model = new CreateSupportRequestModel();

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateSupportRequestSend(CreateSupportRequestModel model, bool captchaValid)
    {
        var currentCustomer = await workContext.GetCurrentCustomerAsync();

        if (await customerService.IsGuestAsync((currentCustomer)))
        {
            return Challenge();
        }

        if (ModelState.IsValid)
        {
            var item = new SupportRequest
            {
                MessageText = model.MessageText,
                CreatedOnUtc = DateTime.UtcNow,
                CustomerId = currentCustomer.Id,
                StoreId = (await storeContext.GetCurrentStoreAsync()).Id
            };

            await service.InsertSupportRequestAsync(item);

            return RedirectToAction("CustomerSupportRequests");
        }

        return View("CreateSupportRequest", model);
    }

    public virtual async Task<IActionResult> EditSupportRequest(int id)
    {
        var currentCustomer = await workContext.GetCurrentCustomerAsync();

        if (await customerService.IsGuestAsync((currentCustomer)))
        {
            return Challenge();
        }

        var item = await service.GetSupportRequestByIdAsync(id);

        if (item == null || item.CustomerId != currentCustomer.Id)
            return RedirectToAction("CustomerSupportRequests");

        var model = new EditSupportRequestModel();
        await modelFactory.PrepareEditSupportRequestModelAsync(item, model);

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> EditSupportRequestSend(EditSupportRequestModel model)
    {
        var currentCustomer = await workContext.GetCurrentCustomerAsync();

        if (await customerService.IsGuestAsync((currentCustomer)))
        {
            return Challenge();
        }

        if (ModelState.IsValid)
        {
            var item = await service.GetSupportRequestByIdAsync(model.Id);

            if (item == null || item.CustomerId != currentCustomer.Id)
                return RedirectToAction("CustomerSupportRequests");

            if (!string.IsNullOrWhiteSpace(item.ReplyText) && item.Rating == 0)
            {
                var rating = model.Rating;
                if (rating < 1 || rating > 5)
                {
                    rating = 0;
                }

                item.Rating = rating;

                await service.UpdateSupportRequestAsync(item);

                return RedirectToAction("EditSupportRequest", new { id = item.Id });
            }
        }

        return View("EditSupportRequest", model);
    }
}
