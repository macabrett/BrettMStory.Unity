namespace BrettMStory.Unity {

    using System;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Extensiosn to colliders.
    /// </summary>
    public static class ColliderExtensions {

        /// <summary>
        /// Does a Physics.Overlap from a SphereCollider.
        /// </summary>
        /// <param name="sphereCollider">The sphere collider.</param>
        /// <param name="layerName">The layer name for layer mask.</param>
        /// <returns>Any colliders overlapped.</returns>
        public static Collider[] ToOverlappedColliders(this SphereCollider sphereCollider, string layerName) {
            return Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius, 1 << LayerMask.NameToLayer(layerName));
        }

        /// <summary>
        /// Gets current collisions' component with a sphere collider.
        /// </summary>
        /// <typeparam name="T">The component to get out of it.</typeparam>
        /// <param name="sphereCollider">The sphere collider.</param>
        /// <param name="layerName">The layer name for layer mask.</param>
        /// <returns>Overlapped components.</returns>
        public static T[] ToOverlappedComponents<T>(this SphereCollider sphereCollider, string layerName) where T : Component {
            var colliders = sphereCollider.ToOverlappedColliders(layerName);
            return colliders.Select(x => (T)x.GetComponent(typeof(T))).ToArray();
        }

        /// <summary>
        /// Gets current collisions' component with a sphere collider.
        /// </summary>
        /// <param name="sphereCollider">The sphere collider.</param>
        /// <param name="type">The type.</param>
        /// <param name="layerName">The layer name for layer mask.</param>
        /// <returns>
        /// Overlapped components.
        /// </returns>
        public static object[] ToOverlappedComponents(this SphereCollider sphereCollider, Type type, string layerName) {
            var colliders = sphereCollider.ToOverlappedColliders(layerName);
            return colliders.Select(x => x.GetComponent(type)).ToArray();
        }

        /// <summary>
        /// Gets current collisions' game object with a sphere collider.
        /// </summary>
        /// <param name="sphereCollider">The sphere collider.</param>
        /// <param name="layerName">The layer name for layer mask.</param>
        /// <returns>
        /// Overlapped game objects.
        /// </returns>
        public static GameObject[] ToOverlappedGameObjects(this SphereCollider sphereCollider, string layerName) {
            var colliders = sphereCollider.ToOverlappedColliders(layerName);
            return colliders.Select(x => x.gameObject).ToArray();
        }
    }
}