using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Core.Domain.Messages;

namespace Nop.Data.Migrations.MyCustomizations;

[NopMigration("2026-01-16 10:30:00", "4.90.0", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class DataMigration(INopDataProvider dataProvider, EmailAccountSettings emailAccountSettings) : Migration
{
    public override void Up()
    {
        var messageTemplateTable = dataProvider.GetTable<MessageTemplate>();

        if (!messageTemplateTable.Any(mt => string.Compare(mt.Name,
                MessageTemplateSystemNames.CUSTOMER_SUPPORT_REQUEST_REPLY_NOTIFICATION,
                StringComparison.OrdinalIgnoreCase) == 0))
        {
            dataProvider.InsertEntity(new MessageTemplate
            {
                Name = MessageTemplateSystemNames.CUSTOMER_SUPPORT_REQUEST_REPLY_NOTIFICATION,
                Subject = "%Store.Name%. Support request has been replied",
                Body = "Hello %Customer.FullName%! The <a href=\"%SupportRequest.Url%\">support request</a> has been replied.",
                EmailAccountId = emailAccountSettings.DefaultEmailAccountId
            });
        }
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}
