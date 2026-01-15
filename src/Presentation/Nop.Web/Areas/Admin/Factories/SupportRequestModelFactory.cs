using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.SupportRequests;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Html;
using Nop.Services.Stores;
using Nop.Services.SupportRequests;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.SupportRequests;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public class SupportRequestModelFactory(CatalogSettings catalogSettings, IBaseAdminModelFactory baseAdminModelFactory, IDateTimeHelper dateTimeHelper,
    ISupportRequestService supportRequestService, ICustomerService customerService, IStoreService storeService, IHtmlFormatter htmlFormatter)
{
    public virtual async Task<SupportRequestSearchModel> PrepareSupportRequestSearchModelAsync(SupportRequestSearchModel searchModel)
    {
        if (searchModel == null)
        {
            throw new ArgumentNullException(nameof(searchModel));
        }

        searchModel.HideStoresList = catalogSettings.IgnoreStoreLimitations
                                     || searchModel.AvailableStores.SelectionIsNotPossible();

        searchModel.SetGridPageSize();

        return searchModel;
    }

    public virtual async Task<SupportRequestListModel> PrepareSupportRequestListModelAsync(SupportRequestSearchModel searchModel)
    {
        if (searchModel == null)
        {
            throw new ArgumentNullException(nameof(searchModel));
        }

        var createdOnFromUtc = searchModel.CreatedOnFrom.HasValue
            ? (DateTime?)dateTimeHelper
                .ConvertToUtcTime(searchModel.CreatedOnFrom.Value, await dateTimeHelper.GetCurrentTimeZoneAsync())
            : null;
        var createdOnToUtc = searchModel.CreatedOnTo.HasValue
            ? (DateTime?)dateTimeHelper
                .ConvertToUtcTime(searchModel.CreatedOnTo.Value, await dateTimeHelper.GetCurrentTimeZoneAsync())
                .AddDays(1)
            : null;
        var updatedOnFromUtc = searchModel.UpdatedOnFrom.HasValue
            ? (DateTime?)dateTimeHelper
                .ConvertToUtcTime(searchModel.UpdatedOnFrom.Value, await dateTimeHelper.GetCurrentTimeZoneAsync())
            : null;
        var updatedOnToUtc = searchModel.UpdatedOnTo.HasValue
            ? (DateTime?)dateTimeHelper
                .ConvertToUtcTime(searchModel.UpdatedOnTo.Value, await dateTimeHelper.GetCurrentTimeZoneAsync())
                .AddDays(1)
            : null;

        IPagedList<SupportRequest> items;

        var searchCustomer = await customerService.GetCustomerByEmailAsync(searchModel.CustomerEmail);

        if (!string.IsNullOrWhiteSpace(searchModel.CustomerEmail) && searchCustomer == null)
        {
            items = new PagedList<SupportRequest>(new List<SupportRequest>(), 0, 0, 0);
        }
        else
        {
            items = await supportRequestService.GetAllSupportRequestsAsync(searchCustomer?.Id ?? 0, createdOnFromUtc, createdOnToUtc, updatedOnFromUtc, updatedOnToUtc,
                searchModel.SearchText, searchModel.SearchStoreId, searchModel.Page - 1, searchModel.PageSize);
        }

        var models = await new SupportRequestListModel().PrepareToGridAsync(searchModel, items, () =>
        {
            return items.SelectAwait(async item =>
            {
                var model = item.ToModel<SupportRequestModel>();
                model.CreatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.CreatedOnUtc, DateTimeKind.Utc);
                if (item.UpdatedOnUtc.HasValue)
                {
                    model.UpdatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.UpdatedOnUtc.Value, DateTimeKind.Utc);
                }

                model.StoreName = (await storeService.GetStoreByIdAsync(item.StoreId))?.Name;
                var customer = await customerService.GetCustomerByIdAsync(item.CustomerId);
                model.CustomerInfo = customer?.Email;

                model.MessageText = htmlFormatter.FormatText(item.MessageText, false, true, false, false, false, false);

                model.ReplyText = htmlFormatter.FormatText(item.ReplyText, false, true, false, false, false, false);

                return model;
            });
        });

        return models;
    }

    public virtual async Task<SupportRequestModel> PrepareSupportRequestModelAsync(SupportRequestModel model, SupportRequest item, bool excludeProperties = false)
    {
        if (item != null)
        {
            var showStoreName = (await storeService.GetAllStoresAsync()).Count > 1;
            var customer = await customerService.GetCustomerByIdAsync(item.CustomerId);

            model ??= new SupportRequestModel
            {
                Id = item.Id,
                ShowStoreName = showStoreName,
                StoreName = showStoreName
                    ? (await storeService.GetStoreByIdAsync(item.StoreId))?.Name
                    : string.Empty,
                CustomerId = item.CustomerId,
                CreatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.CreatedOnUtc, DateTimeKind.Utc)
            };

            if (item.UpdatedOnUtc.HasValue)
            {
                model.UpdatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.UpdatedOnUtc.Value, DateTimeKind.Utc);
            }

            if (!excludeProperties)
            {
                model.ReplyText = item.ReplyText;
            }
        }

        return model;
    }
}
