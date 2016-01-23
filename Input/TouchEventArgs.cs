namespace BrettMStory.Unity {

    using System;
    using UnityEngine;

    /// <summary>
    /// The touch event args.
    /// </summary>
    public class TouchEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of TouchEventArgs.
        /// </summary>
        public TouchEventArgs() {
        }

        /// <summary>
        /// Initializes a new instance of TouchEventArgs.
        /// </summary>
        /// <param name="touch">The touch.</param>
        public TouchEventArgs(Touch touch) {
            this.Touch = touch;
        }

        /// <summary>
        /// Gets or sets the touch.
        /// </summary>
        public Touch Touch { get; set; }
    }
}