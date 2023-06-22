namespace ApacheTech.VintageMods.NeverWinter.Patches;

[HarmonySidedPatch(EnumAppSide.Universal)]
[SettingsConsumer(EnumAppSide.Universal)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal sealed class NeverWinterPatches : WorldSettingsConsumer<NeverWinterSettings>
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ModTemperature), "updateTemperature")]
    internal static void Harmony_ModTemperature_updateTemperature_Postfix(ref ClimateCondition climate) 
        => climate.Temperature = GameMath.Clamp(climate.Temperature, Settings.MinTemperature, Settings.MaxTemperature);
}