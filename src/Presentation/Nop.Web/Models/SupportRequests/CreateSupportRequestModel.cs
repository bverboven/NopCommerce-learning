using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.SupportRequests;

public record CreateSupportRequestModel : BaseNopEntityModel
{
    [NopResourceDisplayName("SupportRequest.Fields.MessageText")]
    public string MessageText { get; set; }
}