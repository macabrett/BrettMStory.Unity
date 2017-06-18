namespace BrettMStory.Unity2D {

    using UnityEngine;

    /// <summary>
    /// A static class for vector extensions.
    /// </summary>
    public static class VectorExtensions {

        /// <summary>
        /// Clamps between the minimum and maximum values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>A Vector2 clamped between the two values.</returns>
        public static Vector2 Clamp(this Vector2 value, Vector2 minimum, Vector2 maximum) {
            return new Vector2(Mathf.Clamp(value.x, minimum.x, maximum.x), Mathf.Clamp(value.y, minimum.y, maximum.y));
        }

        /// <summary>
        /// Converts a Vector3 to a Vector2.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>A vector 2.</returns>
        public static Vector2 ToVector2(this Vector3 vector) {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Converts a Vector2 to a Vector3.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>A vector 3.</returns>
        public static Vector3 ToVector3(this Vector2 vector) {
            return new Vector3(vector.x, vector.y, 0f);
        }
    }
}