using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.SupportRequests;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.SupportRequests;

public class SupportRequestBuilder : NopEntityBuilder<SupportRequest>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SupportRequest.CustomerId)).AsInt32().ForeignKey<Customer>()
            .WithColumn(nameof(SupportRequest.StoreId)).AsInt32().ForeignKey<Store>();
    }
}
