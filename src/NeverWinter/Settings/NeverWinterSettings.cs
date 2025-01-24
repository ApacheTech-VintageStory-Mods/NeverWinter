namespace ApacheTech.VintageMods.NeverWinter.Settings;

/// <summary>
///     Represents the settings for the NeverWinter mod, including seasonal, temperature, and hibernation configurations.
/// </summary>
/// <remarks>
///     This settings class is serialised using ProtoBuf with implicit fields for all public properties.
///     The default settings are provided through the <see cref="Default"/> property.
/// </remarks>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public sealed class NeverWinterSettings : FeatureSettings<NeverWinterSettings>
{
    /// <summary>
    ///     Gets the default settings for the NeverWinter mod.
    /// </summary>
    internal static NeverWinterSettings Default { get; } = new();

    /// <summary>
    ///     Specifies an override for the current season. Defaults to "auto".
    /// </summary>
    public string SeasonOverride { get; set; } = "auto";

    /// <summary>
    ///     Specifies a season to exclude from consideration. Defaults to "none".
    /// </summary>
    public string ExcludeSeason { get; set; } = "none";

    /// <summary>
    ///     Specifies the minimum temperature setting. Defaults to -100.
    /// </summary>
    public float MinTemperature { get; set; } = -100f;

    /// <summary>
    ///     Specifies the maximum temperature setting. Defaults to 100.
    /// </summary>
    public float MaxTemperature { get; set; } = 100f;

    /// <summary>
    ///     Specifies the month when hibernation starts. Defaults to "November".
    /// </summary>
    public string HibernationStartMonth { get; set; } = "November";

    /// <summary>
    ///     Specifies the month when hibernation ends. Defaults to "April".
    /// </summary>
    public string HibernationEndMonth { get; set; } = "April";

    /// <summary>
    ///     Determines whether hibernation is enabled. Defaults to true.
    /// </summary>
    public bool HibernationEnabled { get; set; } = true;
}