namespace ApacheTech.VintageMods.NeverWinter;

[UsedImplicitly]
internal sealed class Program : ModHost
{
    protected override void ConfigureUniversalModServices(IServiceCollection services, ICoreAPI api)
    {
        services.AddFileSystemService(o => o.RegisterSettingsFiles = true);
        services.AddHarmonyPatchingService(o => o.AutoPatchModAssembly = true);
        services.AddNetworkService();
    }
}
