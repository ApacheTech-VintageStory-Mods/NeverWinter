using ApacheTech.VintageMods.NeverWinter.Commands;
using ApacheTech.VintageMods.NeverWinter.Settings;

namespace ApacheTech.VintageMods.NeverWinter.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal sealed class NeverWinterServerSystem : ServerModSystem, IServerServiceRegistrar
{
    public void ConfigureServerModServices(IServiceCollection services, ICoreServerAPI sapi)
    {
        services.AddFeatureWorldSettings<NeverWinterSettings>();
        services.AddSingleton<NeverWinterServerCommand>();
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        IOC.Services.Resolve<NeverWinterServerCommand>().GenerateCommand();
    }
}