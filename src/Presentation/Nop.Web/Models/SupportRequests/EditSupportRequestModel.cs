using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.SupportRequests;

public record EditSupportRequestModel : BaseNopEntityModel
{
    [NopResourceDisplayName("SupportRequest.Fields.MessageText")]
    public string MessageText { get; set; }
    [NopResourceDisplayName("SupportRequest.Fields.ReplyText")]
    public string ReplyText { get; set; }
    [NopResourceDisplayName("SupportRequest.Fields.Rating")]
    public int Rating { get; set; }
    [NopResourceDisplayName("SupportRequest.Fields.CreatedOn")]
    public DateTime CreatedOn { get; set; }
    [NopResourceDisplayName("SupportRequest.Fields.UpdatedOn")]
    public DateTime? UpdatedOn { get; set; }
}
