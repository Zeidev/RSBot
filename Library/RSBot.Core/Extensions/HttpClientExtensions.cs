using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RSBot.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for HttpClient with built-in error handling.
    /// </summary>
    public static class HttpClientExtensions
    {
        private static readonly HttpClient _defaultClient;

        static HttpClientExtensions()
        {
            _defaultClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        /// <summary>
        /// Gets the default HttpClient instance.
        /// </summary>
        public static HttpClient DefaultClient => _defaultClient;

        /// <summary>
        /// Safely fetches a string from the specified URL, returning null on failure.
        /// </summary>
        public static async Task<string> GetStringSafeAsync(string url)
        {
            try
            {
                return await _defaultClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                Log.Warn($"HTTP GET failed for {url}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Safely fetches bytes from the specified URL, returning null on failure.
        /// </summary>
        public static async Task<byte[]> GetBytesSafeAsync(string url)
        {
            try
            {
                return await _defaultClient.GetByteArrayAsync(url);
            }
            catch (Exception ex)
            {
                Log.Warn($"HTTP GET bytes failed for {url}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Safely sends a GET request, returning null on failure.
        /// </summary>
        public static async Task<HttpResponseMessage> GetSafeAsync(string url)
        {
            try
            {
                return await _defaultClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                Log.Warn($"HTTP GET request failed for {url}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Safely sends a POST request with content, returning null on failure.
        /// </summary>
        public static async Task<HttpResponseMessage> PostSafeAsync(string url, HttpContent content)
        {
            try
            {
                return await _defaultClient.PostAsync(url, content);
            }
            catch (Exception ex)
            {
                Log.Warn($"HTTP POST request failed for {url}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Safely sends a request with timeout, returning null on failure.
        /// </summary>
        public static async Task<HttpResponseMessage> GetWithTimeoutAsync(string url, int timeoutSeconds = 10)
        {
            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return await _defaultClient.GetAsync(url, cts.Token);
            }
            catch (OperationCanceledException)
            {
                Log.Warn($"HTTP GET request timed out for {url}");
                return null;
            }
            catch (Exception ex)
            {
                Log.Warn($"HTTP GET request failed for {url}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Checks if a URL is reachable.
        /// </summary>
        public static async Task<bool> IsReachableAsync(string url)
        {
            try
            {
                using var response = await _defaultClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Downloads a file safely to the specified path.
        /// </summary>
        public static async Task<bool> DownloadFileSafeAsync(string url, string filePath)
        {
            try
            {
                var bytes = await GetBytesSafeAsync(url);
                if (bytes == null)
                    return false;

                var directory = System.IO.Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);

                await System.IO.File.WriteAllBytesAsync(filePath, bytes);
                return true;
            }
            catch (Exception ex)
            {
                Log.Warn($"Failed to download file from {url}: {ex.Message}");
                return false;
            }
        }
    }
}
