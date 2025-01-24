using ApacheTech.VintageMods.NeverWinter.Settings;
using Gantry.Core.Maths.Extensions;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.NeverWinter.Patches;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
[HarmonyUniversalPatch]
public sealed class NeverWinterUniversalPatches : WorldSettingsConsumer<NeverWinterSettings>
{
    /// <summary>
    ///     Adds a <see cref="HarmonyPostfix"/> patch to the "updateTemperature" method in the <see cref="ModTemperature"/> class.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ModTemperature), "updateTemperature")]
    public static void Harmony_ModTemperature_updateTemperature_Postfix(ref ClimateCondition climate)
    {
        climate.Temperature = GameMath.Clamp(climate.Temperature, Settings.MinTemperature, Settings.MaxTemperature);
    }

    /// <summary>
    ///     Adds a <see cref="HarmonyPostfix"/> patch to the "YearRel" method in the <see cref="GameCalendar"/> class.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameCalendar), nameof(GameCalendar.YearRel), MethodType.Getter)]
    public static void Harmony_GameCalendar_YearRel_Getter_Postfix(ref float __result)
    {
        var (min, max) = Settings.ExclusionRange();
        __result = __result.InverseClamp(min, max);
    }
}