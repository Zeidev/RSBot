using System;
using System.Threading.Tasks;

namespace RSBot.Core.Extensions
{
    /// <summary>
    /// Provides null-safe extension methods for objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Executes the action if the object is not null.
        /// </summary>
        public static void IfNotNull<T>(this T obj, Action<T> action) where T : class
        {
            if (obj != null)
                action(obj);
        }

        /// <summary>
        /// Executes the async action if the object is not null.
        /// </summary>
        public static async Task IfNotNullAsync<T>(this T obj, Func<T, Task> func) where T : class
        {
            if (obj != null)
                await func(obj);
        }

        /// <summary>
        /// Returns the result of the selector if object is not null, otherwise returns default.
        /// </summary>
        public static TRet IfNotNull<T, TRet>(this T obj, Func<T, TRet> selector, TRet defaultValue = default)
            where T : class
        {
            return obj != null ? selector(obj) : defaultValue;
        }

        /// <summary>
        /// Returns the object if not null, otherwise returns the default value.
        /// </summary>
        public static T OrDefault<T>(this T obj, T defaultValue = default) where T : class
        {
            return obj ?? defaultValue;
        }

        /// <summary>
        /// Performs a safe cast to the specified type.
        /// </summary>
        public static TTarget CastAs<TSource, TTarget>(this TSource obj)
            where TSource : class
            where TTarget : class
        {
            return obj as TTarget;
        }

        /// <summary>
        /// Performs a safe cast with fallback.
        /// </summary>
        public static TTarget CastAsOrDefault<TSource, TTarget>(this TSource obj, TTarget defaultValue = default)
            where TSource : class
            where TTarget : class
        {
            return obj as TTarget ?? defaultValue;
        }

        /// <summary>
        /// Checks if the object is null.
        /// </summary>
        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        /// <summary>
        /// Checks if the object is not null.
        /// </summary>
        public static bool IsNotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }

        /// <summary>
        /// Returns the object or throws an exception if null.
        /// </summary>
        public static T ThrowIfNull<T>(this T obj, string message = "Object is null") where T : class
        {
            return obj ?? throw new NullReferenceException(message);
        }

        /// <summary>
        /// Returns the object or throws an exception if null.
        /// </summary>
        public static T ThrowIfNull<T>(this T obj, Exception exception) where T : class
        {
            return obj ?? throw exception;
        }

        /// <summary>
        /// Performs an action on each item if not null.
        /// </summary>
        public static void Also<T>(this T obj, Action<T> action) where T : class
        {
            if (obj != null)
                action(obj);
        }

        /// <summary>
        /// Returns the object after performing an action (for chaining).
        /// </summary>
        public static T Tap<T>(this T obj, Action<T> action) where T : class
        {
            if (obj != null)
                action(obj);
            return obj;
        }

        /// <summary>
        /// Safely compares two objects for equality.
        /// </summary>
        public static bool EqualsSafe<T>(this T obj, T other)
        {
            if (obj == null && other == null)
                return true;
            if (obj == null || other == null)
                return false;
            return obj.Equals(other);
        }
    }
}
