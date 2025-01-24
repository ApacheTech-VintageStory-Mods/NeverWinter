using ApacheTech.VintageMods.NeverWinter.Settings;
using Vintagestory.Server;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.NeverWinter.Patches;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
[HarmonyServerSidePatch]
public sealed class NeverWinterServerPatches : WorldSettingsConsumer<NeverWinterSettings>
{
    /// <summary>
    ///     Adds a <see cref="HarmonyPostfix"/> patch to the "PassTimeWhenEmpty" method in the <see cref="ServerConfig"/> class.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ServerConfig), nameof(ServerConfig.PassTimeWhenEmpty), MethodType.Getter)]
    public static void Harmony_ServerConfig_PassTimeWhenEmpty_Getter_Postfix(ref bool __result)
    {
        if (!Settings.HibernationEnabled) return;
        var isInHibernationRange = Settings.IsInHibernationRange();
        if (__result == isInHibernationRange) return;
        __result = isInHibernationRange;
        ApiEx.Server.Server.Config.To<ServerConfig>().Set(nameof(ServerConfig.PassTimeWhenEmpty), isInHibernationRange);
    }
}