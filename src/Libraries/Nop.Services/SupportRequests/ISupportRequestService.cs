using Nop.Core;
using Nop.Core.Domain.SupportRequests;

namespace Nop.Services.SupportRequests;

public interface ISupportRequestService
{
    public Task InsertSupportRequestAsync(SupportRequest supportRequest);
    public Task UpdateSupportRequestAsync(SupportRequest supportRequest);
    public Task DeleteSupportRequestAsync(SupportRequest supportRequest);
    public Task<SupportRequest> GetSupportRequestByIdAsync(int supportRequestId);
    public Task<IList<SupportRequest>> GetAllSupportRequestsByCustomerIdAsync(int customerId);
    public Task<IPagedList<SupportRequest>> GetAllSupportRequestsAsync(int customerId = 0, DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
        DateTime? updatedFromUtc = null, DateTime? updatedToUtc = null, string searchText = null, int storeId = 0,
        int pageIndex = 0, int pageSize = int.MaxValue);
}
