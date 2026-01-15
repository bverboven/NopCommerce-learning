using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.SupportRequests;

public record SupportRequestModel : BaseNopEntityModel
{
    [NopResourceDisplayName("Admin.SupportRequests.Fields.Customer")]
    public string CustomerInfo { get; set; }
    public int CustomerId { get; set; }

    [NopResourceDisplayName("Admin.SupportRequests.Fields.StoreName")]
    public string StoreName { get; set; }
    public bool ShowStoreName { get; set; }

    [NopResourceDisplayName("Admin.SupportRequests.Fields.MessageText")]
    public string MessageText { get; set; }
    [NopResourceDisplayName("Admin.SupportRequests.Fields.ReplyText")]
    public string ReplyText { get; set; }

    [NopResourceDisplayName("Admin.SupportRequests.Fields.CreatedOn")]
    public DateTime CreatedOn { get; set; }
    [NopResourceDisplayName("Admin.SupportRequests.Fields.UpdatedOn")]
    public DateTime? UpdatedOn { get; set; }
}