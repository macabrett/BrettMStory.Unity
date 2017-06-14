namespace BrettMStory.Unity2D {

    using UnityEngine;

    /// <summary>
    /// The type of touch.
    /// </summary>
    internal enum TouchType {
        Up,
        Down,
        Left,
        Right,
        Tap,
        None
    }

    /// <summary>
    /// Touch data.
    /// </summary>
    internal class TouchData {

        /// <summary>
        /// The screen position that the touch starts.
        /// </summary>
        private Vector2 _startPosition;

        /// <summary>
        /// The time when the touch starts.
        /// </summary>
        private float _startTime;

        /// <summary>
        /// Gets the screen position that the touch starts.
        /// </summary>
        internal Vector2 StartPosition {
            get {
                return this._startPosition;
            }
        }

        /// <summary>
        /// Gets the time when the touch starts.
        /// </summary>
        internal float StartTime {
            get {
                return this._startTime;
            }
        }

        /// <summary>
        /// Initializes a touch.
        /// </summary>
        /// <param name="touch">The UnityEngine touch.</param>
        public void Begin(Touch touch) {
            this._startPosition = touch.position;
            this._startTime = Time.time;
        }

        /// <summary>
        /// Processes the end of the input.
        /// </summary>
        /// <param name="touch">The UnityEngine touch.</param>
        public TouchType End(Touch touch) {
            var start = this._startPosition;
            var end = touch.position;
            var yDifference = Mathf.Abs(start.y - end.y);
            var xDifference = Mathf.Abs(start.x - end.x);
            var timeDifference = Time.time - this._startTime;

            if (yDifference > SimpleMobile.Instance.MinimumScreenHeightSwipeDistance && yDifference >= xDifference) {
                if (end.y > start.y) {
                    return TouchType.Up;
                }
                else {
                    return TouchType.Down;
                }
            }
            else if (xDifference > SimpleMobile.Instance.MinimumScreenWidthSwipeDistance && xDifference > yDifference) {
                if (end.x > start.x) {
                    return TouchType.Right;
                }
                else {
                    return TouchType.Left;
                }
            }
            else if (timeDifference < SimpleMobile.Instance.MaximumTapTime) {
                return TouchType.Tap;
            }

            return TouchType.Down;
        }
    }
}