using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.SupportRequests;

public record SupportRequestSearchModel : BaseSearchModel
{
    [UIHint("DateNullable")]
    public DateTime? CreatedOnFrom { get; set; }
    [UIHint("DateNullable")]
    public DateTime? CreatedOnTo { get; set; }
    [UIHint("DateNullable")]
    public DateTime? UpdatedOnFrom { get; set; }
    [UIHint("DateNullable")]
    public DateTime? UpdatedOnTo { get; set; }

    public string CustomerEmail { get; set; }
    public string SearchText { get; set; }
    public int SearchStoreId { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; } = [];
    public bool HideStoresList { get; set; }
}