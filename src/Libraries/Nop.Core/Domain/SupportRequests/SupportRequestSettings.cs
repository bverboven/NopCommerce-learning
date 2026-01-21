using Nop.Core.Configuration;

namespace Nop.Core.Domain.SupportRequests;

public class SupportRequestSettings : ISettings
{
    public bool ShowCreateSupportRequestLink { get; set; }
}
