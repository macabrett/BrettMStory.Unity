namespace BrettMStory.Unity {

    using UnityEngine;

    /// <summary>
    /// A set of 2D camera settings.
    /// </summary>
    public struct CameraSettings2D {

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraSettings2D" /> struct.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="screenWorldHeight">Height of the screen in world units.</param>
        /// <param name="leftMostEdge">The left most edge.</param>
        /// <param name="rightMostEdge">The right most edge.</param>
        /// <param name="bottomMostEdge">The bottom most edge.</param>
        /// <param name="topMostEdge">The top most edge.</param>
        public CameraSettings2D(Camera camera, float screenWorldHeight, float leftMostEdge, float rightMostEdge, float bottomMostEdge, float topMostEdge)
            : this(camera, screenWorldHeight, leftMostEdge, rightMostEdge, bottomMostEdge, topMostEdge, Margin.Zero) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraSettings2D" /> struct.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="screenWorldHeight">Height of the screen in world units.</param>
        /// <param name="leftMostEdge">The left most edge.</param>
        /// <param name="rightMostEdge">The right most edge.</param>
        /// <param name="bottomMostEdge">The bottom most edge.</param>
        /// <param name="topMostEdge">The top most edge.</param>
        /// <param name="margin">The margin.</param>
        public CameraSettings2D(Camera camera, float screenWorldHeight, float leftMostEdge, float rightMostEdge, float bottomMostEdge, float topMostEdge, Margin margin) {
            this.ScreenWidth = Screen.width;
            this.ScreenHeight = Screen.height;
            this.Margin = margin;
            this.ScreenWorldHeight = screenWorldHeight + this.Margin.Bottom + this.Margin.Top;
            this.OrthographicSize = this.ScreenWorldHeight * 0.5f;
            this.ScreenWorldWidth = this.ScreenWorldHeight * this.ScreenWidth / this.ScreenHeight;
            var halfScreenWorldWidth = this.ScreenWorldWidth * 0.5f;
            this.MinimumPosition = new Vector2(leftMostEdge + halfScreenWorldWidth - this.Margin.Left, bottomMostEdge + this.OrthographicSize - this.Margin.Bottom);
            this.MaximumPosition = new Vector2(rightMostEdge - halfScreenWorldWidth + this.Margin.Right, topMostEdge - this.OrthographicSize + this.Margin.Top);
        }

        /// <summary>
        /// Gets the margin.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public Margin Margin { get; private set; }

        /// <summary>
        /// Gets the maximum position.
        /// </summary>
        /// <value>
        /// The maximum position.
        /// </value>
        public Vector2 MaximumPosition { get; private set; }

        /// <summary>
        /// Gets the minimum position.
        /// </summary>
        /// <value>
        /// The minimum position.
        /// </value>
        public Vector2 MinimumPosition { get; private set; }

        /// <summary>
        /// Gets the size of the orthographic.
        /// </summary>
        /// <value>
        /// The size of the orthographic.
        /// </value>
        public float OrthographicSize { get; private set; }

        /// <summary>
        /// Gets the height of the screen.
        /// </summary>
        /// <value>
        /// The height of the screen.
        /// </value>
        public int ScreenHeight { get; private set; }

        /// <summary>
        /// Gets the width of the screen.
        /// </summary>
        /// <value>
        /// The width of the screen.
        /// </value>
        public int ScreenWidth { get; private set; }

        /// <summary>
        /// Gets the height of the screen world.
        /// </summary>
        /// <value>
        /// The height of the screen world.
        /// </value>
        public float ScreenWorldHeight { get; private set; }

        /// <summary>
        /// Gets the width of the screen world.
        /// </summary>
        /// <value>
        /// The width of the screen world.
        /// </value>
        public float ScreenWorldWidth { get; private set; }
    }
}