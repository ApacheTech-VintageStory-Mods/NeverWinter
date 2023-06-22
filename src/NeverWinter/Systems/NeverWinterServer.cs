namespace ApacheTech.VintageMods.NeverWinter.Systems;

[UsedImplicitly]
internal sealed class NeverWinterServer : ServerModSystem, IServerServiceRegistrar
{
    public void ConfigureServerModServices(IServiceCollection services, ICoreServerAPI sapi)
    {
        services.AddFeatureWorldSettings<NeverWinterSettings>();
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        var command = new NeverWinterServerCommand(api);
        command.Generate();
    }
}