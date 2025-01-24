using System;
using ApacheTech.VintageMods.NeverWinter.Settings;

namespace ApacheTech.VintageMods.NeverWinter.Extensions;

/// <summary>
///     Provides extension methods for mapping and manipulating <see cref="NeverWinterSettings"/> objects.
/// </summary>
internal static class MappingExtensions
{
    /// <summary>
    ///     Converts a <see cref="NeverWinterSettings"/> object to a <see cref="NeverWinterSettingsPacket"/> object.
    /// </summary>
    /// <param name="settings">The settings to convert.</param>
    /// <returns>A <see cref="NeverWinterSettingsPacket"/> representing the converted settings.</returns>
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

    /// <summary>
    ///     Updates the <see cref="NeverWinterSettings"/> object with values from a <see cref="NeverWinterSettingsPacket"/>.
    /// </summary>
    /// <param name="settings">The settings to update.</param>
    /// <param name="packet">The packet containing the updated values.</param>
    public static void UpdateFromPacket(this NeverWinterSettings settings, NeverWinterSettingsPacket packet)
    {
        settings.SeasonOverride = packet.SeasonOverride;
        settings.ExcludeSeason = packet.ExcludeSeason;
        settings.MinTemperature = packet.MinTemperature;
        settings.MaxTemperature = packet.MaxTemperature;
    }

    /// <summary>
    ///     Resets the <see cref="NeverWinterSettings"/> object to its default values.
    /// </summary>
    /// <param name="settings">The settings to reset.</param>
    public static void Reset(this NeverWinterSettings settings)
    {
        settings.SeasonOverride = NeverWinterSettings.Default.SeasonOverride;
        settings.ExcludeSeason = NeverWinterSettings.Default.ExcludeSeason;
        settings.MinTemperature = NeverWinterSettings.Default.MinTemperature;
        settings.MaxTemperature = NeverWinterSettings.Default.MaxTemperature;
    }

    /// <summary>
    ///     Gets the exclusion range for the specified <see cref="NeverWinterSettings"/> object based on the excluded season.
    /// </summary>
    /// <param name="settings">The settings to evaluate.</param>
    /// <returns>
    ///     A tuple containing the minimum and maximum values of the exclusion range. If the season is invalid, returns (-1, -1).
    /// </returns>
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

    /// <summary>
    ///     Determines whether the current month falls within the hibernation range specified in the settings.
    /// </summary>
    /// <param name="settings">The settings to evaluate.</param>
    /// <returns>True if the current month is within the hibernation range; otherwise, false.</returns>
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

    private static bool _internalUse;
}