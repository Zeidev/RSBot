using System;
using System.Collections.Generic;
using System.Linq;

namespace RSBot.Core.Extensions;

public static class CollectionExtensions
{
    public static void RemoveAll<K, V>(this IDictionary<K, V> dict, Func<K, V, bool> predicate)
    {
        foreach (var key in dict.Keys.ToArray().Where(key => predicate(key, dict[key])))
            dict.Remove(key);
    }

    /// <summary>
    /// Safely gets a value from dictionary, returns default if key not found.
    /// </summary>
    public static V GetOrDefault<K, V>(this IDictionary<K, V> dict, K key, V defaultValue = default)
    {
        return dict.TryGetValue(key, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Gets a value from dictionary or adds a new one if not exists.
    /// </summary>
    public static V GetOrAdd<K, V>(this IDictionary<K, V> dict, K key, Func<V> factory)
    {
        if (dict.TryGetValue(key, out var existing))
            return existing;

        var newValue = factory();
        dict[key] = newValue;
        return newValue;
    }

    /// <summary>
    /// Adds a range of items to the collection.
    /// </summary>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    /// <summary>
    /// Checks if the collection is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection == null || collection.Count == 0;
    }

    /// <summary>
    /// Performs action on each item in the collection.
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}
