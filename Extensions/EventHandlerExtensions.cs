namespace BrettMStory.Unity {

    using System;

    /// <summary>
    /// A static class for event handler extensions.
    /// </summary>
    public static class EventHandlerExtensions {

        /// <summary>
        /// Invokes an event thread safely and also checks if null.
        /// </summary>
        /// <typeparam name="T">A type that inherits from EventArgs.</typeparam>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        public static void SafeInvoke<T>(this EventHandler<T> eventHandler, object sender, T e) where T : EventArgs {
            eventHandler?.Invoke(sender, e);
        }

        /// <summary>
        /// Invokes an event thread safely and also checks if null.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="sender">The sender.</param>
        public static void SafeInvoke(this EventHandler eventHandler, object sender) {
            eventHandler?.Invoke(sender, null);
        }
    }
}