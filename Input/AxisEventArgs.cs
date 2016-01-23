namespace BrettMStory.Unity {

    using System;

    /// <summary>
    /// Event arguments for axis.
    /// </summary>
    public class AxisEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of AxisEventArgs.
        /// </summary>
        /// <param name="axisValue"></param>
        /// <param name="rawAxisValue"></param>
        public AxisEventArgs(float axisValue, float rawAxisValue) {
            this.AxisValue = axisValue;
            this.RawAxisValue = rawAxisValue;
        }

        /// <summary>
        /// Gets or sets the axis value (-1.0f > value > 1.0f).
        /// </summary>
        public float AxisValue { get; set; }

        /// <summary>
        /// Gets or sets the raw axis value (-1, 0, or 1).
        /// </summary>
        public float RawAxisValue { get; set; }
    }
}