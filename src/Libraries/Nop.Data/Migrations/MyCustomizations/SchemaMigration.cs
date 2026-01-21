using FluentMigrator;
using Nop.Core.Domain.SupportRequests;
using Nop.Data.Extensions;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations.MyCustomizations;

[NopSchemaMigration("2026-01-20 13:00:00", "SchemaMigration for 4.90.0", MigrationProcessType.NoMatter)]
public class SchemaMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(SupportRequest))).Exists())
        {
            Create.TableFor<SupportRequest>();
        }
        else
        {
            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(SupportRequest))).Column(nameof(SupportRequest.Rating)).Exists())
            {
                Alter.Table(NameCompatibilityManager.GetTableName(typeof(SupportRequest)))
                    .AddColumn(nameof(SupportRequest.Rating)).AsInt32().Nullable();
            }
        }
    }
}
