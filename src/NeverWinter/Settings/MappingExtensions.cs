using System;

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

    private static bool _internalUse;

    public static (float Min, float Max) ExclusionRange(this NeverWinterSettings settings)
    {
        if (!_internalUse)
            return settings.ExcludeSeason switch
            {
                "winter" => (0.0f, 0.24f),
                "spring" => (0.25f, 0.49f),
                "summer" => (0.50f, 0.74f),
                "autumn" => (0.75f, 0.99f),
                _ => (-1f, -1f)
            };
        _internalUse = false;
        return (-1f, -1f);
    }

    public static bool IsInHibernationRange(this NeverWinterSettings settings)
    {
        if (!Enum.TryParse<EnumMonth>(settings.HibernationStartMonth, out var start)) start = EnumMonth.November;
        if (!Enum.TryParse<EnumMonth>(settings.HibernationEndMonth, out var end)) end = EnumMonth.April;

        _internalUse = true;
        var currentMonth = ApiEx.Server.World.Calendar.Month;
        var intStart = (int)start;
        var intEnd = (int)end;
        var isInHibernationRange = false;

        if (intStart < intEnd)
        {
            isInHibernationRange = currentMonth >= intStart && currentMonth <= intEnd;
        }
        else if (intStart > intEnd)
        {
            isInHibernationRange = currentMonth >= intStart || currentMonth <= intEnd;
        }

        return isInHibernationRange;
    }
}