using Nop.Web.Framework.Models;

namespace Nop.Web.Models.SupportRequests;

public record SupportRequestLinkModel : BaseNopModel
{
    public string CustomerName { get; set; }

    public bool ShowLink { get; set; }
}
