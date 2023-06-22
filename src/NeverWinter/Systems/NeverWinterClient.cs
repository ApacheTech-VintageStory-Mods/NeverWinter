namespace ApacheTech.VintageMods.NeverWinter.Systems;

[UsedImplicitly]
internal sealed class NeverWinterClient : ClientModSystem, IClientServiceRegistrar
{
    private NeverWinterSettings _settings;

    public void ConfigureClientModServices(IServiceCollection services, ICoreClientAPI capi)
    {
        services.AddFeatureWorldSettings<NeverWinterSettings>();
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<NeverWinterSettings>()
            .SetMessageHandler<NeverWinterSettings>(SyncSettingsWithServer);
    }

    private void SyncSettingsWithServer(NeverWinterSettings packet)
    {
        _settings = packet;
        ModSettings.World.Save(packet);
        Capi.World.Calendar.SetSeasonOverride(_settings.SeasonOverride);
    }
}