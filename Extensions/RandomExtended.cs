namespace BrettMStory.Unity2D {

    using UnityEngine;

    /// <summary>
    /// An extended version of Random.
    /// </summary>
    public static class RandomExtended {

        /// <summary>
        /// Returns a bool at random.
        /// </summary>
        /// <returns>A random boolean value.</returns>
        public static bool Bool() {
            return Random.Range(0, 2) == 1;
        }
    }
}