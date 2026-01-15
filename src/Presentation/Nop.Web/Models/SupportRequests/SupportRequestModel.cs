using Nop.Web.Framework.Models;

namespace Nop.Web.Models.SupportRequests;

public record SupportRequestModel : BaseNopEntityModel
{
    public string MessageText { get; set; }
    public string ReplyText { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}