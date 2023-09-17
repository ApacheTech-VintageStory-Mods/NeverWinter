namespace ApacheTech.VintageMods.NeverWinter.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public sealed class NeverWinterSettings : FeatureSettings
{
    internal static readonly NeverWinterSettings Default = new();

    public string SeasonOverride { get; set; } = "auto";

    public string ExcludeSeason { get; set; } = "none";

    public float MinTemperature { get; set; } = -100f;

    public float MaxTemperature { get; set; } = 100f;

    public string HibernationStartMonth { get; set; } = "November";

    public string HibernationEndMonth { get; set; } = "April";

    public bool HibernationEnabled { get; set; } = true;
}