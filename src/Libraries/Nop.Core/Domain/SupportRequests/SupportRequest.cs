namespace Nop.Core.Domain.SupportRequests;

public class SupportRequest : BaseEntity
{
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public string MessageText { get; set; }
    public string ReplyText { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
