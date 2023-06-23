// ReSharper disable InconsistentNaming
using ApacheTech.VintageMods.NeverWinter.Settings;

namespace ApacheTech.VintageMods.NeverWinter.Patches;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
[HarmonySidedPatch(EnumAppSide.Universal)]
[SettingsConsumer(EnumAppSide.Universal)]
internal sealed class NeverWinterPatches : WorldSettingsConsumer<NeverWinterSettings>
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ModTemperature), "updateTemperature")]
    internal static void Harmony_ModTemperature_updateTemperature_Postfix(ref ClimateCondition climate)
    {
        climate.Temperature = GameMath.Clamp(climate.Temperature, Settings.MinTemperature, Settings.MaxTemperature);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameCalendar), nameof(GameCalendar.YearRel), MethodType.Getter)]
    internal static void Harmony_GameCalendar_YearRel_Getter_Postfix(ref float __result)
    {
        var (min, max) = Settings.ExclusionRange();
        __result = __result.InverseClamp(min, max);
    }
}