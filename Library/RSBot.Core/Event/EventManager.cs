using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSBot.Core.Event;

public class EventManager
{
    private static readonly ConcurrentDictionary<string, List<Delegate>> _listeners = new();
    private static readonly ConcurrentDictionary<(string name, int paramCount), Delegate[]> _handlerCache = new();
    private static readonly object _lockObject = new object();

    /// <summary>
    ///     Registers the event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="handler">The handler.</param>
    public static void SubscribeEvent(string name, Delegate handler)
    {
        if (handler == null)
            return;

        _listeners.AddOrUpdate(
            name,
            _ => new List<Delegate> { handler },
            (_, existing) =>
            {
                lock (_lockObject)
                {
                    existing.Add(handler);
                }
                return existing;
            });

        // Invalidate cache for this event
        InvalidateCache(name);
    }

    /// <summary>
    ///     Registers the event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="handler">The handler.</param>
    public static void SubscribeEvent(string name, Action handler)
    {
        if (handler == null)
            return;

        SubscribeEvent(name, (Delegate)handler);
    }

    /// <summary>
    ///     Unsubscribes from an event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="handler">The handler.</param>
    public static void UnsubscribeEvent(string name, Delegate handler)
    {
        if (handler == null || !_listeners.TryGetValue(name, out var handlers))
            return;

        lock (_lockObject)
        {
            handlers.Remove(handler);
        }

        InvalidateCache(name);
    }

    /// <summary>
    ///     Unsubscribes from an event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="handler">The handler.</param>
    public static void UnsubscribeEvent(string name, Action handler)
    {
        UnsubscribeEvent(name, (Delegate)handler);
    }

    /// <summary>
    ///     Removes all listeners for an event.
    /// </summary>
    /// <param name="name">The event name.</param>
    public static void ClearEvent(string name)
    {
        _listeners.TryRemove(name, out _);
        InvalidateCache(name);
    }

    /// <summary>
    ///     Clears all event listeners.
    /// </summary>
    public static void ClearAll()
    {
        _listeners.Clear();
        _handlerCache.Clear();
    }

    /// <summary>
    ///     Gets the count of listeners for an event.
    /// </summary>
    public static int GetListenerCount(string name)
    {
        return _listeners.TryGetValue(name, out var handlers) ? handlers.Count : 0;
    }

    /// <summary>
    ///     Fires the event.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="parameters">The parameters.</param>
    public static void FireEvent(string name, params object[] parameters)
    {
        try
        {
            var paramCount = parameters?.Length ?? 0;
            var cacheKey = (name, paramCount);

            // Try to get cached handlers, or build if not cached
            if (!_handlerCache.TryGetValue(cacheKey, out var targets))
            {
                if (!_listeners.TryGetValue(name, out var handlers) || handlers.Count == 0)
                    return;

                lock (_lockObject)
                {
                    targets = handlers
                        .Where(h => h.Method.GetParameters().Length == paramCount)
                        .ToArray();
                }

                _handlerCache[cacheKey] = targets;
            }

            if (targets == null || targets.Length == 0)
                return;

            var isNetworkThread = Thread.CurrentThread.Name == "Network.PacketProcessor";

            foreach (var target in targets)
            {
                if (isNetworkThread)
                    Task.Run(() => SafeInvoke(target, parameters));
                else
                    SafeInvoke(target, parameters);
            }
        }
        catch (Exception e)
        {
            Log.Fatal(e);
        }
    }

    /// <summary>
    ///     Fires the event synchronously on the current thread.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="parameters">The parameters.</param>
    public static void FireEventSync(string name, params object[] parameters)
    {
        try
        {
            var paramCount = parameters?.Length ?? 0;
            var cacheKey = (name, paramCount);

            if (!_handlerCache.TryGetValue(cacheKey, out var targets))
            {
                if (!_listeners.TryGetValue(name, out var handlers) || handlers.Count == 0)
                    return;

                lock (_lockObject)
                {
                    targets = handlers
                        .Where(h => h.Method.GetParameters().Length == paramCount)
                        .ToArray();
                }

                _handlerCache[cacheKey] = targets;
            }

            if (targets == null || targets.Length == 0)
                return;

            foreach (var target in targets)
            {
                SafeInvoke(target, parameters);
            }
        }
        catch (Exception e)
        {
            Log.Fatal(e);
        }
    }

    /// <summary>
    ///     Fires the event asynchronously.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="parameters">The parameters.</param>
    public static async Task FireEventAsync(string name, params object[] parameters)
    {
        var tasks = new List<Task>();

        try
        {
            var paramCount = parameters?.Length ?? 0;
            var cacheKey = (name, paramCount);

            if (!_handlerCache.TryGetValue(cacheKey, out var targets))
            {
                if (!_listeners.TryGetValue(name, out var handlers) || handlers.Count == 0)
                    return;

                lock (_lockObject)
                {
                    targets = handlers
                        .Where(h => h.Method.GetParameters().Length == paramCount)
                        .ToArray();
                }

                _handlerCache[cacheKey] = targets;
            }

            if (targets == null || targets.Length == 0)
                return;

            foreach (var target in targets)
            {
                tasks.Add(Task.Run(() => SafeInvoke(target, parameters)));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Log.Fatal(e);
        }
    }

    private static void SafeInvoke(Delegate target, object[] parameters)
    {
        try
        {
            target.DynamicInvoke(parameters);
        }
        catch (Exception ex)
        {
            Log.Warn($"Event handler invocation failed: {ex.Message}");
        }
    }

    private static void InvalidateCache(string name)
    {
        // Remove all cached entries for this event name
        var keysToRemove = _handlerCache.Keys.Where(k => k.name == name).ToList();
        foreach (var key in keysToRemove)
        {
            _handlerCache.TryRemove(key, out _);
        }
    }
}
