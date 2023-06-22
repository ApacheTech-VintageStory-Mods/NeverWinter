namespace ApacheTech.VintageMods.NeverWinter;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public sealed class NeverWinterSettings : FeatureSettings
{
    internal static readonly NeverWinterSettings Default = new();

    public string SeasonOverride { get; set; } = "auto";
    public float MinTemperature { get; set; } = -20f;
    public float MaxTemperature { get; set; } = 40f;
}
