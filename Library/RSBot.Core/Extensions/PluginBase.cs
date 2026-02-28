using System;
using System.Windows.Forms;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Plugins;

namespace RSBot.Core.Extensions
{
    /// <summary>
    /// Abstract base class for plugins to reduce code duplication.
    /// Provides common implementations for IPlugin interface.
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
        private Control _view;

        #region Abstract Properties

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        public abstract string Author { get; }

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the internal name of the plugin.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the display name of the plugin.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        public abstract string Version { get; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets whether the plugin should display as a tab. Default: true.
        /// </summary>
        public virtual bool DisplayAsTab => true;

        /// <summary>
        /// Gets the tab index. Default: 50.
        /// </summary>
        public virtual int Index => 50;

        /// <summary>
        /// Gets whether the plugin requires being in-game. Default: true.
        /// </summary>
        public virtual bool RequireIngame => true;

        #endregion

        #region Implementation

        /// <summary>
        /// Gets or sets whether the plugin is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the view control for the plugin. Lazy-loaded.
        /// </summary>
        public Control View
        {
            get
            {
                if (_view == null)
                    _view = CreateView();

                return _view;
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Creates and returns the view control for this plugin.
        /// </summary>
        protected abstract Control CreateView();

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Initializes the plugin. Override for custom initialization.
        /// </summary>
        public virtual void Initialize()
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Translates the plugin's UI. Default implementation uses LanguageManager.
        /// </summary>
        public virtual void Translate()
        {
            LanguageManager.Translate(View, Kernel.Language);
        }

        /// <summary>
        /// Called when the character is loaded. Override for custom behavior.
        /// </summary>
        public virtual void OnLoadCharacter()
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Enables the plugin. Default: enables the view.
        /// </summary>
        public virtual void Enable()
        {
            if (View != null)
                View.Enabled = true;

            Enabled = true;
        }

        /// <summary>
        /// Disables the plugin. Default: disables the view.
        /// </summary>
        public virtual void Disable()
        {
            if (View != null)
                View.Enabled = false;

            Enabled = false;
        }

        /// <summary>
        /// Called when the plugin is about to be enabled. Return false to prevent enabling.
        /// </summary>
        public virtual bool CanEnable()
        {
            return true;
        }

        /// <summary>
        /// Called when the plugin is about to be disabled. Return false to prevent disabling.
        /// </summary>
        public virtual bool CanDisable()
        {
            return true;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        protected void Debug(object message) => Log.Debug(message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        protected void Warn(object message) => Log.Warn(message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        protected void Error(object message) => Log.Error(message);

        /// <summary>
        /// Logs a notification message.
        /// </summary>
        protected void Notify(object message) => Log.Notify(message);

        /// <summary>
        /// Logs a status message.
        /// </summary>
        protected void Status(object message) => Log.Status(message);

        /// <summary>
        /// Fires an event with the given name.
        /// </summary>
        protected void FireEvent(string eventName, params object[] parameters)
        {
            EventManager.FireEvent(eventName, parameters);
        }

        /// <summary>
        /// Subscribes to an event.
        /// </summary>
        protected void SubscribeEvent(string eventName, Action handler)
        {
            EventManager.SubscribeEvent(eventName, handler);
        }

        /// <summary>
        /// Gets a config value.
        /// </summary>
        protected T GetConfig<T>(string key, T defaultValue = default)
        {
            return PlayerConfig.Get(key, defaultValue);
        }

        /// <summary>
        /// Sets a config value and saves.
        /// </summary>
        protected void SetConfig<T>(string key, T value)
        {
            PlayerConfig.Set(key, value);
            PlayerConfig.Save();
        }

        #endregion
    }

    /// <summary>
    /// Abstract base class for botbases to reduce code duplication.
    /// </summary>
    public abstract class BotbaseBase : IPlugin
    {
        private Control _view;

        #region Abstract Properties

        /// <summary>
        /// Gets the author of the botbase.
        /// </summary>
        public abstract string Author { get; }

        /// <summary>
        /// Gets the description of the botbase.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the internal name of the botbase.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the display name of the botbase.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets the version of the botbase.
        /// </summary>
        public abstract string Version { get; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets whether the botbase should display as a tab. Default: false.
        /// </summary>
        public virtual bool DisplayAsTab => false;

        /// <summary>
        /// Gets the tab index. Default: 0.
        /// </summary>
        public virtual int Index => 0;

        /// <summary>
        /// Gets whether the botbase requires being in-game. Default: true.
        /// </summary>
        public virtual bool RequireIngame => true;

        #endregion

        #region Implementation

        /// <summary>
        /// Gets or sets whether the botbase is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the view control for the botbase. Lazy-loaded.
        /// </summary>
        public Control View
        {
            get
            {
                if (_view == null)
                    _view = CreateView();

                return _view;
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Creates and returns the view control for this botbase.
        /// </summary>
        protected abstract Control CreateView();

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Initializes the botbase. Override for custom initialization.
        /// </summary>
        public virtual void Initialize()
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Translates the botbase's UI.
        /// </summary>
        public virtual void Translate()
        {
            LanguageManager.Translate(View, Kernel.Language);
        }

        /// <summary>
        /// Called when the character is loaded.
        /// </summary>
        public virtual void OnLoadCharacter()
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Enables the botbase.
        /// </summary>
        public virtual void Enable()
        {
            if (View != null)
                View.Enabled = true;

            Enabled = true;
        }

        /// <summary>
        /// Disables the botbase.
        /// </summary>
        public virtual void Disable()
        {
            if (View != null)
                View.Enabled = false;

            Enabled = false;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        protected void Debug(object message) => Log.Debug(message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        protected void Warn(object message) => Log.Warn(message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        protected void Error(object message) => Log.Error(message);

        /// <summary>
        /// Logs a notification message.
        /// </summary>
        protected void Notify(object message) => Log.Notify(message);

        /// <summary>
        /// Logs a status message.
        /// </summary>
        protected void Status(object message) => Log.Status(message);

        #endregion
    }
}
