using FluentMigrator;
using Nop.Core.Domain.SupportRequests;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Configuration;

namespace Nop.Web.Framework.Migrations.MyCustomizations;

[NopUpdateMigration("2026-01-21 14:00:00", "4.90", UpdateMigrationType.Settings)]
public class SettingsMigration : MigrationBase
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        var settingsService = EngineContext.Current.Resolve<ISettingService>();
        var supportRequestSettings = settingsService.LoadSetting<SupportRequestSettings>();

        if (!settingsService.SettingExists(supportRequestSettings, x => x.ShowCreateSupportRequestLink))
        {
            supportRequestSettings.ShowCreateSupportRequestLink = true;
            settingsService.SaveSetting(supportRequestSettings, x => x.ShowCreateSupportRequestLink);
        }
    }
}
