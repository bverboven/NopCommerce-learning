using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.SupportRequests;
using Nop.Web.Framework.Components;
using Nop.Web.Models.SupportRequests;

namespace Nop.Web.Components;

public class SupportRequestLinkViewComponent(IWorkContext workContext, SupportRequestSettings settings) : NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new SupportRequestLinkModel();

        var customer = await workContext.GetCurrentCustomerAsync();
        if (customer != null)
        {
            model.CustomerName = customer.FirstName;
        }

        model.ShowLink = settings.ShowCreateSupportRequestLink;

        return View(model);
    }
}
