namespace BrettMStory.Unity {

    /// <summary>
    /// Extension methods for integers.
    /// </summary>
    public static class IntegerExtensions {

        /// <summary>
        /// Constrains the integer, wrapping back to 0 if the max value is passed.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxValue">The INCLUSIVE maximum value.</param>
        /// <returns></returns>
        public static int ConstrainWithWrapping(this int value, int maxValue) {
            return value % (maxValue + 1);
        }
    }
}