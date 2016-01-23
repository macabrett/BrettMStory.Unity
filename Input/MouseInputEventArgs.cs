namespace BrettMStory.Unity {

    using System;
    using UnityEngine;

    /// <summary>
    /// The mouse input event args.
    /// </summary>
    public class MouseInputEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new input of MouseInputEventArgs.
        /// </summary>
        public MouseInputEventArgs() {
        }

        /// <summary>
        /// Initializes a new instance of MouseInputEventArgs.
        /// </summary>
        /// <param name="clickPosition">The screen position of the click.</param>
        public MouseInputEventArgs(Vector2 clickPosition) {
            this.ClickPosition = clickPosition;
        }

        /// <summary>
        /// Gets or sets the click position.
        /// </summary>
        public Vector2 ClickPosition { get; set; }
    }
}