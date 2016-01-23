namespace BrettMStory.Unity {

    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A static class for list extensions.
    /// </summary>
    public static class ListExtensions {

        /// <summary>
        /// Gets a random item out of a list.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>A random element from the list.</returns>
        public static T Random<T>(this IList<T> list) {
            var random = new Random();
            return list[random.Next(0, list.Count)];
        }

        /// <summary>
        /// Gets a new, randomized version of the original list.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list to randomize.</param>
        /// <returns>A randomized copy of the original list.</returns>
        public static IList<T> Randomize<T>(this IList<T> list) {
            var newList = new List<T>(list);
            var random = new Random();
            var i = newList.Count - 1;
            while (i > 1) {
                int j = random.Next(i + 1);
                T value = list[j];
                list[j] = list[i];
                list[i] = value;
                i--;
            }

            return newList;
        }

        /// <summary>
        /// Gets a random item out of the list. If the list has one or zero elements, it acts exactly the same as FirstOrDefault.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>A random item out of the list.</returns>
        public static T RandomOrDefault<T>(this IList<T> list) {
            if (list.Count <= 1)
                return list.FirstOrDefault();

            return list.Random();
        }
    }
}