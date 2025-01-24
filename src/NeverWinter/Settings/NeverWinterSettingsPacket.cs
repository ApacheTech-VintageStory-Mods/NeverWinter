namespace ApacheTech.VintageMods.NeverWinter.Settings;

/// <summary>
///     Represents the settings packet, including seasonal and temperature configurations.
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public sealed class NeverWinterSettingsPacket : FeatureSettings
{
    /// <summary>
    ///     Specifies an override for the current season.
    /// </summary>
    public string SeasonOverride { get; init; }

    /// <summary>
    ///     Specifies a season to exclude from consideration.
    /// </summary>
    public string ExcludeSeason { get; init; }

    /// <summary>
    ///     Specifies the minimum temperature setting.
    /// </summary>
    public float MinTemperature { get; init; }

    /// <summary>
    ///     Specifies the maximum temperature setting.
    /// </summary>
    public float MaxTemperature { get; init; }
}