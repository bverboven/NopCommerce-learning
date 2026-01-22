using Nop.Core;
using Nop.Core.Domain.SupportRequests;
using Nop.Data;

namespace Nop.Services.SupportRequests;

public class SupportRequestService(IRepository<SupportRequest> supportRepository) : ISupportRequestService
{
    public Task InsertSupportRequestAsync(SupportRequest supportRequest)
    {
        return supportRepository.InsertAsync(supportRequest);
    }

    public Task UpdateSupportRequestAsync(SupportRequest supportRequest)
    {
        return supportRepository.UpdateAsync(supportRequest);
    }

    public Task DeleteSupportRequestAsync(SupportRequest supportRequest)
    {
        return supportRepository.DeleteAsync(supportRequest);
    }

    public virtual async Task DeleteOldItemsAsync()
    {
        var cutoffDate = DateTime.UtcNow.AddYears(-1);
        var oldItems = await supportRepository.GetAllAsync(query =>
            query.Where(x => x.CreatedOnUtc < cutoffDate));
        await supportRepository.DeleteAsync(oldItems);
    }

    public Task<SupportRequest> GetSupportRequestByIdAsync(int supportRequestId)
    {
        return supportRepository.GetByIdAsync(supportRequestId);
    }

    public async Task<IList<SupportRequest>> GetAllSupportRequestsByCustomerIdAsync(int customerId)
    {
        return await GetAllSupportRequestsAsync(customerId);
    }

    public async Task<IPagedList<SupportRequest>> GetAllSupportRequestsAsync(int customerId = 0, DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
        DateTime? updatedFromUtc = null, DateTime? updatedToUtc = null, string searchText = null, int storeId = 0,
        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        return await supportRepository.GetAllPagedAsync(query =>
        {
            if (customerId != 0)
                query = query.Where(x => x.CustomerId == customerId);

            if (createdFromUtc.HasValue)
                query = query.Where(x => x.CreatedOnUtc >= createdFromUtc.Value);

            if (createdToUtc.HasValue)
                query = query.Where(x => x.CreatedOnUtc <= createdToUtc.Value);

            if (updatedFromUtc.HasValue)
                query = query.Where(x => x.UpdatedOnUtc >= updatedFromUtc.Value);

            if (updatedToUtc.HasValue)
                query = query.Where(x => x.UpdatedOnUtc <= updatedToUtc.Value);

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(x => x.MessageText.Contains(searchText));

            if (storeId != 0)
                query = query.Where(x => x.StoreId == storeId);

            // Sorting
            query = query
                .OrderByDescending(x => x.CreatedOnUtc)
                .ThenBy(x => x.Id);

            return query;
        }, pageIndex, pageSize, includeDeleted: false);
    }
}
