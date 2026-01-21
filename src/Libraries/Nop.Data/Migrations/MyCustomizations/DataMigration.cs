using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Security;

namespace Nop.Data.Migrations.MyCustomizations;

[NopMigration("2026-01-21 17:30:00", "4.90.0", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class DataMigration(INopDataProvider dataProvider, EmailAccountSettings emailAccountSettings) : Migration
{
    public override void Up()
    {
        // New permission
        if (!dataProvider.GetTable<PermissionRecord>().Any(pr =>
                string.Compare(pr.SystemName, "ManageSupportRequests", StringComparison.OrdinalIgnoreCase) == 0))
        {
            var manageSupportRequestsPermission = dataProvider.InsertEntity(new PermissionRecord
            {
                Name = "Admin area. Manage Support Requests",
                SystemName = "ManageSupportRequests",
                Category = "SupportRequests"
            });

            // add it to the Admin role by default
            var adminRole = dataProvider.GetTable<CustomerRole>()
                .First(x => x.IsSystemRole && x.SystemName == NopCustomerDefaults.AdministratorsRoleName);

            dataProvider.InsertEntity(new PermissionRecordCustomerRoleMapping
            {
                CustomerRoleId = adminRole.Id,
                PermissionRecordId = manageSupportRequestsPermission.Id
            });
        }


        // Message Templates
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
                EmailAccountId = emailAccountSettings.DefaultEmailAccountId,
                IsActive = true
            });
        }

        if(!messageTemplateTable.Any(mt => string.Compare(mt.Name,
                MessageTemplateSystemNames.STORE_OWNER_SUPPORT_REQUEST_NOTIFICATION,
                StringComparison.OrdinalIgnoreCase) == 0))
        {
            dataProvider.InsertEntity(new MessageTemplate
            {
                Name = MessageTemplateSystemNames.STORE_OWNER_SUPPORT_REQUEST_NOTIFICATION,
                Subject = "%Store.Name%. New support request submitted",
                Body = "A new <a href=\"%Store.URL%admin/SupportRequest/Edit/%SupportRequest.Id%/>support request</a> has been submitted by %Customer.FullName%. <a href=\"%SupportRequest.Url%\">View the request</a>.",
                EmailAccountId = emailAccountSettings.DefaultEmailAccountId,
                IsActive = true
            });
        }
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}
