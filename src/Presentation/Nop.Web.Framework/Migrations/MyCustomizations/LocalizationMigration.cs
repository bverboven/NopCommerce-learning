using FluentMigrator;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Localization;

namespace Nop.Web.Framework.Migrations.MyCustomizations;

[NopMigration("2026-01-15 12:00:00", NopVersion.FULL_VERSION, UpdateMigrationType.Localization, MigrationProcessType.Update)]
public class LocalizationMigration : Migration
{
    public override void Up()
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
        {
            return;
        }

        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

        localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["PageTitle.Account.SupportRequests"] = "Support requests",
            ["Account.SupportRequests"] = "Support requests",
            ["Account.SupportRequests.Reply"] = "Support team responded to your request",
            ["Account.SupportRequests.IsNotReplied"] = "Is not replied to yet",

            ["Admin.SupportRequests.Fields.Customer"] = "Customer",
            ["Admin.SupportRequests.Fields.Customer.Hint"] = "A Customer who created the support request.",
            ["Admin.SupportRequests.Fields.StoreName"] = "Store",
            ["Admin.SupportRequests.Fields.StoreName.Hint"] = "A store name in which this support request was written.",
            ["Admin.SupportRequests.Fields.MessageText"] = "Message text",
            ["Admin.SupportRequests.Fields.MessageText.Hint"] = "The message text.",
            ["Admin.SupportRequests.Fields.ReplyText"] = "Reply text",
            ["Admin.SupportRequests.Fields.ReplyText.Hint"] = "The reply text",
            ["Admin.SupportRequests.Fields.CreatedOn"] = "Created on",
            ["Admin.SupportRequests.Fields.CreatedOn.Hint"] = "The date/time that the support request was created.",
            ["Admin.SupportRequests.Fields.UpdatedOn"] = "Updated on",
            ["Admin.SupportRequests.Fields.UpdatedOn.Hint"] = "The date/time that the support request was updated.",

            ["Admin.SupportRequests.List.CreatedOnFrom"] = "Created from",
            ["Admin.SupportRequests.List.CreatedOnFrom.Hint"] = "The \"created on from\" date for the search.",
            ["Admin.SupportRequests.List.CreatedOnTo"] = "Created to",
            ["Admin.SupportRequests.List.CreatedOnTo.Hint"] = "The \"created on to\" date for the search.",
            ["Admin.SupportRequests.List.UpdatedOnFrom"] = "Updated from",
            ["Admin.SupportRequests.List.UpdatedOnFrom.Hint"] = "The \"updated on from\" date for the search.",
            ["Admin.SupportRequests.List.UpdatedOnTo"] = "Updated to",
            ["Admin.SupportRequests.List.UpdatedOnTo.Hint"] = "The \"updated on to\" date for the search.",
            ["Admin.SupportRequests.List.CustomerEmail"] = "Customer email",
            ["Admin.SupportRequests.List.CustomerEmail.Hint"] = "The email of the customer who created the support request.",
            ["Admin.SupportRequests.List.SearchText"] = "Search text",
            ["Admin.SupportRequests.List.SearchText.Hint"] = "The text of the support request.",
            ["Admin.SupportRequests.List.SearchStore"] = "Search store",
            ["Admin.SupportRequests.List.SearchStore.Hint"] = "The store where the support request was created.",

            ["Admin.SupportRequests"] = "Support requests",

            ["SupportRequest.Fields.MessageText"] = "Message",
            ["SupportRequest.MessageText.Required"] = "The message should not be empty",
            ["Account.SupportRequest.Add"] = "Create a support request",
            ["SupportRequests.Add.Button"] = "Send",
            ["SupportRequests.CreateSupportRequest"] = "Create",

            ["Plugins.Misc.SupportRequests.Admin.Updated"] = "Support request updated",

        });
    }

    public override void Down()
    {

    }
}
