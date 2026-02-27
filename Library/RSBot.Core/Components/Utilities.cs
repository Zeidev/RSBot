using System;
using System.Collections.Generic;
using System.Linq;

namespace RSBot.Core.Components
{
    /// <summary>
    /// Provides common utility methods for the RSBot application.
    /// This class contains static helper methods that can be used across the codebase.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Checks if the value is within the specified range (inclusive).
        /// </summary>
        public static bool IsInRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Checks if the value is within the specified range (inclusive).
        /// </summary>
        public static bool IsInRange(this long value, long min, long max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Checks if the value is within the specified range (inclusive).
        /// </summary>
        public static bool IsInRange(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Clamps the value to the specified range.
        /// </summary>
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// Clamps the value to the specified range.
        /// </summary>
        public static long Clamp(long value, long min, long max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// Clamps the value to the specified range.
        /// </summary>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// Converts a percentage value (0-100) to a ratio (0.0-1.0).
        /// </summary>
        public static float PercentToRatio(int percent)
        {
            return Clamp(percent, 0, 100) / 100f;
        }

        /// <summary>
        /// Converts a ratio (0.0-1.0) to a percentage (0-100).
        /// </summary>
        public static int RatioToPercent(float ratio)
        {
            return Clamp((int)(ratio * 100f), 0, 100);
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        public static float Distance(float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates the squared distance between two points (faster than Distance).
        /// </summary>
        public static float DistanceSquared(float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Linear interpolation between two values.
        /// </summary>
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp(t, 0f, 1f);
        }

        /// <summary>
        /// Maps a value from one range to another.
        /// </summary>
        public static float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        /// <summary>
        /// Gets a random value between min and max.
        /// </summary>
        public static int RandomInt(int min, int max)
        {
            return Extensions.RandomExtensions.Random.Next(min, max);
        }

        /// <summary>
        /// Gets a random float between min and max.
        /// </summary>
        public static float RandomFloat(float min, float max)
        {
            return min + (float)Extensions.RandomExtensions.Random.NextDouble() * (max - min);
        }

        /// <summary>
        /// Checks if a number is even.
        /// </summary>
        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Checks if a number is odd.
        /// </summary>
        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        /// <summary>
        /// Gets the sign of a number (-1, 0, or 1).
        /// </summary>
        public static int Sign(float value)
        {
            if (value > 0) return 1;
            if (value < 0) return -1;
            return 0;
        }

        /// <summary>
        /// Swaps two values.
        /// </summary>
        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Gets the enum value or default if not found.
        /// </summary>
        public static TEnum ParseEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(value, out var result))
                return result;
            
            return defaultValue;
        }

        /// <summary>
        /// Safely gets an item from a list by index, returns default if index is out of range.
        /// </summary>
        public static T GetSafe<T>(this IList<T> list, int index, T defaultValue = default)
        {
            if (index < 0 || index >= list.Count)
                return defaultValue;
            
            return list[index];
        }

        /// <summary>
        /// Gets the first item that matches the predicate, or default if none found.
        /// </summary>
        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, T defaultValue = default)
        {
            foreach (var item in enumerable)
            {
                if (predicate(item))
                    return item;
            }
            return defaultValue;
        }

        /// <summary>
        /// Calculates the percentage of a value relative to a total.
        /// </summary>
        public static float PercentageOf(int value, int total)
        {
            if (total == 0) return 0;
            return (float)value / total * 100f;
        }

        /// <summary>
        /// Formats a number with thousand separators.
        /// </summary>
        public static string FormatNumber(long number)
        {
            return number.ToString("N0");
        }

        /// <summary>
        /// Formats bytes to human readable string.
        /// </summary>
        public static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Formats a TimeSpan to human readable string.
        /// </summary>
        public static string FormatTime(TimeSpan time)
        {
            if (time.TotalDays >= 1)
                return $"{(int)time.TotalDays}d {time.Hours}h";
            if (time.TotalHours >= 1)
                return $"{(int)time.TotalHours}h {time.Minutes}m";
            if (time.TotalMinutes >= 1)
                return $"{(int)time.TotalMinutes}m {time.Seconds}s";
            
            return $"{time.Seconds}s";
        }
    }
}
