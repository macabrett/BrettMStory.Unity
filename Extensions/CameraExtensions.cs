namespace BrettMStory.Unity2D {

    using UnityEngine;

    /// <summary>
    /// Extension methods for cameras.
    /// </summary>
    public static class CameraExtensions {

        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <returns>The mouse position in world space for this camera.</returns>
        public static Vector2 GetMousePosition(this Camera camera) {
            return camera.ScreenToWorldPoint(Input.mousePosition.ToVector2());
        }
    }
}