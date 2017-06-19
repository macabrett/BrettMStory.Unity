using System;
using System.Reflection;
using UnityEngine;

namespace BrettMStory.Unity2D.Extensions {

    /// <summary>
    /// Extension methods for components.
    /// </summary>
    public static class ComponentExtensions {

        /// <summary>
        /// Copies the component to the destination game object, as a new component.
        /// </summary>
        /// <typeparam name="T">Must be of type <see cref="Component"/>.</typeparam>
        /// <param name="component">The component.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The copied component.</returns>
        public static T CopyComponentTo<T>(this T component, GameObject destination) where T : Component {
            var copiedComponent = destination.AddComponent<T>();

            var type = component.GetType();
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            var properties = type.GetProperties(flags);
            foreach (var property in properties) {
                if (property.CanWrite && property.GetSetMethod() != null && property.GetGetMethod() != null) {
                    property.SetValue(copiedComponent, property.GetValue(component, null), null);
                }
            }

            var fields = type.GetFields(flags);
            foreach (var field in fields) {
                field.SetValue(copiedComponent, field.GetValue(component));
            }

            return copiedComponent;
        }

        /// <summary>
        /// Gets or add a behaviour.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component">The component.</param>
        /// <returns>The behaviour that was retrieved or added.</returns>
        public static T GetOrAddBehaviour<T>(this Component component) where T : MonoBehaviour {
            var foundBehaviour = component.GetComponent<T>();
            if (foundBehaviour == null) {
                return component.gameObject.AddComponent<T>();
            }

            return foundBehaviour;
        }

        /// <summary>
        /// Gets or adds a component.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="type">The type.</param>
        /// <returns>The component that was retrieved or added</returns>
        public static Component GetOrAddComponent(this Component component, Type type) {
            var foundComponent = component.GetComponent(type);
            if (foundComponent == null) {
                return component.gameObject.AddComponent(type);
            }

            return foundComponent;
        }
    }
}