namespace ApacheTech.VintageMods.NeverWinter.Extensions;

internal static class GameCalendarExtensions
{
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