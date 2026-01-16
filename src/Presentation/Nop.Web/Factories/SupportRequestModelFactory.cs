using Nop.Core.Domain.SupportRequests;
using Nop.Services.Helpers;
using Nop.Web.Models.SupportRequests;

namespace Nop.Web.Factories;

public class SupportRequestModelFactory(IDateTimeHelper dateTimeHelper)
{
    public virtual async Task PrepareSupportRequestModelAsync(SupportRequest item, SupportRequestModel model)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Id = item.Id;
        model.MessageText = item.MessageText;
        model.ReplyText = item.ReplyText;
        model.Rating = item.Rating;

        model.CreatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.CreatedOnUtc, DateTimeKind.Utc);
        if (item.UpdatedOnUtc.HasValue)
        {
            model.UpdatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.UpdatedOnUtc.Value, DateTimeKind.Utc);
        }
    }

    public virtual async Task PrepareSupportRequestListModelAsync(IList<SupportRequest> items, IList<SupportRequestModel> models)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        foreach (var item in items)
        {
            var model = new SupportRequestModel();
            await PrepareSupportRequestModelAsync(item, model);
            models.Add(model);
        }
    }

    public virtual async Task PrepareEditSupportRequestModelAsync(SupportRequest item, EditSupportRequestModel model)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Id = item.Id;
        model.MessageText = item.MessageText;
        model.ReplyText = item.ReplyText;
        model.Rating = item.Rating;
        model.CreatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.CreatedOnUtc, DateTimeKind.Utc);

        if (item.UpdatedOnUtc.HasValue)
        {
            model.UpdatedOn = await dateTimeHelper.ConvertToUserTimeAsync(item.UpdatedOnUtc.Value, DateTimeKind.Utc);
        }
    }
}
