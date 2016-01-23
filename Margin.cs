namespace BrettMStory.Unity {

    /// <summary>
    /// Represents a margin. Usually for cameras.
    /// </summary>
    public struct Margin {

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="left">The left.</param>
        public Margin(float top, float right, float bottom, float left) {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="vertical">The vertical.</param>
        /// <param name="horizontal">The horizontal.</param>
        public Margin(float vertical, float horizontal) {
            this.Top = vertical;
            this.Bottom = vertical;
            this.Right = horizontal;
            this.Left = horizontal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="margin">The margin.</param>
        public Margin(float margin) {
            this.Top = margin;
            this.Right = margin;
            this.Bottom = margin;
            this.Left = margin;
        }

        /// <summary>
        /// Gets the zero.
        /// </summary>
        /// <value>
        /// The zero.
        /// </value>
        public static Margin Zero { get { return new Margin(0f); } }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>
        /// The bottom.
        /// </value>
        public float Bottom { get; private set; }

        /// <summary>
        /// Gets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public float Left { get; private set; }

        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        public float Right { get; private set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public float Top { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format("Margin: (Top: {0}, Right: {1}, Bottom: {2}, Left: {3})", this.Top, this.Right, this.Bottom, this.Left);
        }
    }
}