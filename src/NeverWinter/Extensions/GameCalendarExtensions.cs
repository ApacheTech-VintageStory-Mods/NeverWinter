namespace ApacheTech.VintageMods.NeverWinter.Extensions;

/// <summary>
///     Provides extension methods for interacting with the game calendar.
/// </summary>
internal static class GameCalendarExtensions
{
    /// <summary>
    ///     Sets a season override for the specified game calendar based on a season name.
    /// </summary>
    /// <param name="calendar">The game calendar to update.</param>
    /// <param name="season">
    ///     The name of the season to override. Accepted values are "spring", "summer", "autumn", or "winter".
    ///     If an invalid season name is provided, the override will be cleared.
    /// </param>
    /// <remarks>
    ///     This method maps the provided season name to a corresponding float value representing the season's progression,
    ///     and applies it as an override in the game calendar.
    /// </remarks>
    internal static void SetSeasonOverride(this IGameCalendar calendar, string season)
        => calendar.SetSeasonOverride(season switch
        {
            "spring" => 0.33f,
            "summer" => 0.6f,
            "autumn" => 0.77f,
            "winter" => 0.05f,
            _ => null
        });
}