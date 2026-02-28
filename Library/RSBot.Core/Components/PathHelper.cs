using System.IO;

namespace RSBot.Core.Components
{
    /// <summary>
    /// Provides centralized path constants and helper methods for the application.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Gets the base application path.
        /// </summary>
        public static string BasePath => Kernel.BasePath;

        /// <summary>
        /// Gets the User data directory path.
        /// </summary>
        public static string UserPath => Path.Combine(BasePath, "User");

        /// <summary>
        /// Gets the Data directory path.
        /// </summary>
        public static string DataPath => Path.Combine(BasePath, "Data");

        /// <summary>
        /// Gets the Logs directory path.
        /// </summary>
        public static string LogsPath => Path.Combine(UserPath, "Logs");

        /// <summary>
        /// Gets the Scripts directory path.
        /// </summary>
        public static string ScriptsPath => Path.Combine(DataPath, "Scripts");

        /// <summary>
        /// Gets the Plugins directory path.
        /// </summary>
        public static string PluginsPath => Path.Combine(DataPath, "Plugins");

        /// <summary>
        /// Gets the Botbases directory path.
        /// </summary>
        public static string BotbasesPath => Path.Combine(DataPath, "Botbases");

        /// <summary>
        /// Gets the Exceptions log file path for the current date.
        /// </summary>
        public static string ExceptionsLogPath
        {
            get
            {
                var path = Path.Combine(DataPath, "Logs", "Exceptions");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return Path.Combine(path, $"{System.DateTime.Now:dd-MM-yyyy}.txt");
            }
        }

        /// <summary>
        /// Gets the profile directory path.
        /// </summary>
        public static string ProfilePath(string profileName) =>
            Path.Combine(UserPath, profileName);

        /// <summary>
        /// Gets the profile config file path.
        /// </summary>
        public static string ProfileFile(string profileName) =>
            Path.Combine(UserPath, $"{profileName}.rs");

        /// <summary>
        /// Gets the global profiles config file path.
        /// </summary>
        public static string ProfilesConfigPath => Path.Combine(UserPath, "Profiles.rs");

        /// <summary>
        /// Gets the autologin file path for a profile.
        /// </summary>
        public static string AutoLoginFile(string profileName) =>
            Path.Combine(ProfilePath(profileName), "autologin.data");

        /// <summary>
        /// Gets the town script path.
        /// </summary>
        public static string TownScriptPath(string scriptName) =>
            Path.Combine(ScriptsPath, $"{scriptName}.tscript");

        /// <summary>
        /// Ensures the directory exists, creates if it doesn't.
        /// </summary>
        public static string EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Ensures the profile directory exists.
        /// </summary>
        public static string EnsureProfileDirectory(string profileName)
        {
            return EnsureDirectory(ProfilePath(profileName));
        }

        /// <summary>
        /// Gets the absolute path relative to base path.
        /// </summary>
        public static string GetRelativePath(string relativePath) =>
            Path.Combine(BasePath, relativePath);

        /// <summary>
        /// Checks if a profile directory exists.
        /// </summary>
        public static bool ProfileExists(string profileName) =>
            Directory.Exists(ProfilePath(profileName));

        /// <summary>
        /// Gets all town script files.
        /// </summary>
        public static string[] GetTownScripts()
        {
            if (!Directory.Exists(ScriptsPath))
                return System.Array.Empty<string>();

            return Directory.GetFiles(ScriptsPath, "*.tscript");
        }

        /// <summary>
        /// Gets all profile directories.
        /// </summary>
        public static string[] GetProfiles()
        {
            if (!Directory.Exists(UserPath))
                return System.Array.Empty<string>();

            return Directory.GetDirectories(UserPath);
        }
    }
}
