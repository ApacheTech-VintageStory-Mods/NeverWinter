using ApacheTech.VintageMods.NeverWinter.Settings;

namespace ApacheTech.VintageMods.NeverWinter.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal sealed class NeverWinterClientSystem : ClientModSystem, IClientServiceRegistrar
{
    private static NeverWinterSettings _settings = NeverWinterSettings.Default;

    public void ConfigureClientModServices(IServiceCollection services, ICoreClientAPI capi)
    {
        services.AddFeatureWorldSettings<NeverWinterSettings>();
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        _settings = ModSettings.World.Feature<NeverWinterSettings>();
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<NeverWinterSettingsPacket>()
            .SetMessageHandler<NeverWinterSettingsPacket>(SyncSettingsWithServer);
    }

    private void SyncSettingsWithServer(NeverWinterSettingsPacket packet)
    {
        _settings.UpdateFromPacket(packet);
        ModSettings.World.Save(_settings);
        Capi.World.Calendar.SetSeasonOverride(_settings.SeasonOverride);
    }
}