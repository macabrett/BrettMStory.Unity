namespace BrettMStory.Unity {

    using System;
    using UnityEngine;

    /// <summary>
    /// Event args for camera adjusted.
    /// </summary>
    public class ScreenSizeChangedEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of ScreenSizeChangedEventArgs.
        /// </summary>
        public ScreenSizeChangedEventArgs() {
        }

        /// <summary>
        /// Gets the new height.
        /// </summary>
        public int Height {
            get {
                return Screen.height;
            }
        }

        /// <summary>
        /// Gets the new width.
        /// </summary>
        public int Width {
            get {
                return Screen.width;
            }
        }

        /// <summary>
        /// Gets the new world height.
        /// </summary>
        public float WorldHeight { get; set; }

        /// <summary>
        /// Gets the new world width.
        /// </summary>
        public float WorldWidth { get; set; }
    }
}