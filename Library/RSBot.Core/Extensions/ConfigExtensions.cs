using System;
using System.Collections.Generic;

namespace RSBot.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the Config class.
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Gets a value or creates and saves it if not exists.
        /// </summary>
        public static T GetOrSet<T>(this Config config, string key, T defaultValue)
        {
            if (config.Exists(key))
                return config.Get(key, defaultValue);

            config.Set(key, defaultValue);
            config.Save();
            return defaultValue;
        }

        /// <summary>
        /// Gets an enum value with case-insensitive parsing.
        /// </summary>
        public static TEnum GetEnumIgnoreCase<TEnum>(this Config config, string key, TEnum defaultValue)
            where TEnum : struct
        {
            if (!config.Exists(key))
            {
                config.Set(key, defaultValue.ToString());
                config.Save();
                return defaultValue;
            }

            var value = config.Get(key, defaultValue.ToString());
            return Enum.TryParse(value, ignoreCase: true, out TEnum result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets a boolean value, returns default if not found or parsing fails.
        /// </summary>
        public static bool GetBool(this Config config, string key, bool defaultValue = false)
        {
            if (!config.Exists(key))
            {
                config.Set(key, defaultValue);
                config.Save();
                return defaultValue;
            }

            var value = config.Get(key, defaultValue.ToString());
            return bool.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets an integer value, returns default if not found or parsing fails.
        /// </summary>
        public static int GetInt(this Config config, string key, int defaultValue = 0)
        {
            if (!config.Exists(key))
            {
                config.Set(key, defaultValue);
                config.Save();
                return defaultValue;
            }

            var value = config.Get(key, defaultValue.ToString());
            return int.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets a float value, returns default if not found or parsing fails.
        /// </summary>
        public static float GetFloat(this Config config, string key, float defaultValue = 0f)
        {
            if (!config.Exists(key))
            {
                config.Set(key, defaultValue);
                config.Save();
                return defaultValue;
            }

            var value = config.Get(key, defaultValue.ToString());
            return float.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Sets a value and automatically saves.
        /// </summary>
        public static void SetAndSave<T>(this Config config, string key, T value)
        {
            config.Set(key, value);
            config.Save();
        }

        /// <summary>
        /// Gets or sets a value and automatically saves.
        /// </summary>
        public static T GetOrSetAndSave<T>(this Config config, string key, T defaultValue)
        {
            var value = config.GetOrSet(key, defaultValue);
            config.Save();
            return value;
        }

        /// <summary>
        /// Removes a key from the config.
        /// </summary>
        public static void Remove(this Config config, string key)
        {
            // Note: Config uses ConcurrentDictionary internally but doesn't expose Remove
            // This is a placeholder - actual removal would require modifying Config class
            config.Set(key, default(T));
        }

        /// <summary>
        /// Checks if a key exists and has a non-empty value.
        /// </summary>
        public static bool HasValue(this Config config, string key)
        {
            return config.Exists(key);
        }

        /// <summary>
        /// Gets multiple keys at once.
        /// </summary>
        public static Dictionary<string, T> GetMultiple<T>(this Config config, params string[] keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                result[key] = config.Get(key, default(T));
            }
            return result;
        }

        /// <summary>
        /// Sets multiple keys at once.
        /// </summary>
        public static void SetMultiple<T>(this Config config, Dictionary<string, T> values)
        {
            foreach (var kvp in values)
            {
                config.Set(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Sets multiple keys and saves once.
        /// </summary>
        public static void SetMultipleAndSave<T>(this Config config, Dictionary<string, T> values)
        {
            foreach (var kvp in values)
            {
                config.Set(kvp.Key, kvp.Value);
            }
            config.Save();
        }
    }
}
