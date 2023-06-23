using System;

namespace ApacheTech.VintageMods.NeverWinter.Extensions;

public static class MathsExtensions
{
    public static T InverseClamp<T>(this T value, T minValue, T maxValue) where T : IComparable<T>
    {
        if (value.CompareTo(minValue) < 0 || value.CompareTo(maxValue) > 0) return value;
        return value.CompareTo(minValue) - value.CompareTo(maxValue) > 0 ? maxValue : minValue;
    }
}