using System.Linq;

namespace ApacheTech.VintageMods.NeverWinter;

[UsedImplicitly]
internal sealed class Program : ModHost
{
    protected override void ConfigureUniversalModServices(IServiceCollection services, ICoreAPI api)
    {
        services.AddFileSystemService(api, o => o.RegisterSettingsFiles = true);
        services.AddHarmonyPatchingService(api, o => o.AutoPatchModAssembly = true);
        services.AddNetworkService(api);

        // Add all vanilla mod systems.
        foreach (var system in api.ModLoader.Systems.Where(p => p.ShouldLoad(api.Side)))
        {
            services.TryAdd(ServiceDescriptor.Singleton(system.GetType(), system));
        }
    }
}
