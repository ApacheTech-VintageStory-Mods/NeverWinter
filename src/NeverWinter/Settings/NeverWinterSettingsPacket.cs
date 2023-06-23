namespace ApacheTech.VintageMods.NeverWinter.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public sealed class NeverWinterSettingsPacket : FeatureSettings
{
    public string SeasonOverride { get; init; }

    public string ExcludeSeason { get; init; }

    public float MinTemperature { get; init; }

    public float MaxTemperature { get; init; }
}