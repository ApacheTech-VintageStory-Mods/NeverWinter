using ApacheTech.Common.Extensions.System;

namespace ApacheTech.VintageMods.NeverWinter.Settings;

internal static class MappingExtensions
{
    public static NeverWinterSettingsPacket ToPacket(this NeverWinterSettings settings)
    {
        return new NeverWinterSettingsPacket
        {
            SeasonOverride = settings.SeasonOverride,
            ExcludeSeason = settings.ExcludeSeason,
            MinTemperature = settings.MinTemperature,
            MaxTemperature = settings.MaxTemperature,
        };
    }

    public static void UpdateFromPacket(this NeverWinterSettings settings, NeverWinterSettingsPacket packet)
    {
        settings.SeasonOverride = packet.SeasonOverride;
        settings.ExcludeSeason = packet.ExcludeSeason;
        settings.MinTemperature = packet.MinTemperature;
        settings.MaxTemperature = packet.MaxTemperature;
    }

    public static void Reset(this NeverWinterSettings settings)
    {
        settings.SeasonOverride = NeverWinterSettings.Default.SeasonOverride;
        settings.ExcludeSeason = NeverWinterSettings.Default.ExcludeSeason;
        settings.MinTemperature = NeverWinterSettings.Default.MinTemperature;
        settings.MaxTemperature = NeverWinterSettings.Default.MaxTemperature;
    }

    public static (float Min, float Max) ExclusionRange(this NeverWinterSettings settings)
    {
        return settings.ExcludeSeason switch
        {
            "winter" => (0.0f, 0.24f),
            "spring" => (0.25f, 0.49f),
            "summer" => (0.50f, 0.74f),
            "autumn" => (0.75f, 0.99f),
            _ => (-1f, -1f)
        };
    }
}