namespace BrettMStory.Unity {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// A base behaviour.
    /// </summary>
    public class BaseBehaviour : MonoBehaviour {

        /// <summary>
        /// The name of the coroutine which sets the monobehaviour as busy.
        /// </summary>
        private const string BusyCoroutineName = "BusyDelay";

        /// <summary>
        /// Backing field for game object caching.
        /// </summary>
        private GameObject _gameObject;

        /// <summary>
        /// A value indicating whether or not this behaviour is busy.
        /// </summary>
        private bool _isBusy = false;

        /// <summary>
        /// Backing field for transform caching.
        /// </summary>
        private Transform _transform;

        /// <summary>
        /// Gets the gameo object attached to this behaviour.
        /// </summary>
        public GameObject GameObject {
            get {
                if (this._gameObject == null) {
                    this._gameObject = this.gameObject;
                }

                return this._gameObject;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this is busy.
        /// </summary>
        public bool IsBusy {
            get {
                return this._isBusy;
            }

            set {
                this._isBusy = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent of this behaviour.
        /// </summary>
        public Transform Parent {
            get {
                return this.Transform.parent;
            }

            set {
                this.Transform.parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position {
            get {
                return this.Transform.position;
            }

            set {
                this.Transform.position = value;
            }
        }

        /// <summary>
        /// Gets or sets the 2D position.
        /// </summary>
        public Vector2 Position2D {
            get {
                return this.Transform.position;
            }

            set {
                this.Transform.position = new Vector3(value.x, value.y, this.Position.z);
            }
        }

        /// <summary>
        /// gets or sets the rotation of the transform.
        /// </summary>
        public Vector3 Rotation {
            get {
                return this.Transform.eulerAngles;
            }

            set {
                this.Transform.eulerAngles = value;
            }
        }

        /// <summary>
        /// Gets or sets the 2D rotation (along the z-axis)
        /// </summary>
        public float Rotation2D {
            get {
                return this.Transform.eulerAngles.z;
            }

            set {
                this.Transform.eulerAngles = new Vector3(1f, 1f, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale of the transform.
        /// </summary>
        public Vector3 Scale {
            get {
                return this.Transform.localScale;
            }

            set {
                this.Transform.localScale = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale2 d.
        /// </summary>
        /// <value>
        /// The scale2 d.
        /// </value>
        public Vector2 Scale2D {
            get {
                return this.Transform.localScale.ToVector2();
            }

            set {
                this.Transform.localScale = new Vector3(value.x, value.y, this.Scale.z);
            }
        }

        /// <summary>
        /// Gets the transform of the game object.
        /// </summary>
        public Transform Transform {
            get {
                if (this._transform == null) {
                    this._transform = this.transform;
                }

                return this._transform;
            }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        protected virtual void Awake() {
            this.HandleAttachComponentAttributes();
            this.HandleAttachComponentToFieldAttributes();
        }

        /// <summary>
        /// Delays this monobehaviour for a set amount of time.
        /// </summary>
        /// <param name="timeToDelay">The time to delay.</param>
        /// <returns>An IEnumerator.</returns>
        protected IEnumerator BusyDelay(float timeToDelay) {
            try {
                this._isBusy = true;
                yield return new WaitForSeconds(timeToDelay);
            }
            finally {
                this._isBusy = false;
            }
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <typeparam name="T">The attribute to get.</typeparam>
        /// <returns>The specified attribute.</returns>
        protected T GetAttribute<T>() where T : Attribute {
            return (T)Attribute.GetCustomAttribute(this.GetType(), typeof(T));
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An enumerable of the specified attribute.</returns>
        protected IEnumerable<T> GetAttributes<T>() where T : Attribute {
            return Attribute.GetCustomAttributes(this.GetType(), typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Gets the or add behaviour.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The behaviour that was retrieved or added.</returns>
        protected T GetOrAddBehaviour<T>() where T : MonoBehaviour {
            return this.GetComponent<T>() ?? this.GameObject.AddComponent<T>();
        }

        /// <summary>
        /// Gets the or adds a component.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The component that was retrieved or added</returns>
        protected Component GetOrAddComponent(Type type) {
            return this.GetComponent(type) ?? this.GameObject.AddComponent(type);
        }

        /// <summary>
        /// Handles attaching components and optionally assigning them to properties, if any AttachBehaviour attributes exist.
        /// </summary>
        private void HandleAttachComponentAttributes() {
            var attachAttributes = this.GetAttributes<AttachComponentAttribute>().ToArray();

            for (int i = 0; i < attachAttributes.Length; i++) {
                var attribute = attachAttributes[i];
                if (!attribute.ComponentType.IsSubclassOf(typeof(Component))) {
                    throw new NotSupportedException(
                        string.Format(
                            "The AttachComponent attribute can only be used to attach Components. Type {0} is not a Component.",
                            attribute.ComponentType.Name));
                }

                var component = this.GetOrAddComponent(attribute.ComponentType);

                if (string.IsNullOrEmpty(attribute.PropertyName)) {
                    return;
                }

                var propertyInfo = this.GetType().GetProperty(attribute.PropertyName);

                if (propertyInfo == null) {
                    throw new NotSupportedException(
                        string.Format(
                            "The AttachComponent attribute could not assign to property with name '{0}' on class '{1}', because it does not exist.",
                            attribute.PropertyName,
                            this.GetType().Name));
                }

                if (!propertyInfo.CanWrite) {
                    throw new NotSupportedException(
                        string.Format(
                            "The AttachComponent attribute could not assign to property with name '{0}' on class '{1}', because it is a read only property.",
                            attribute.PropertyName,
                            this.GetType().Name));
                }

                if (propertyInfo.PropertyType != attribute.ComponentType) {
                    throw new NotSupportedException(
                    string.Format(
                        "The AttachComponent attribute could not assign to property with name '{0}' on class '{1}', because the property is not of the same type.",
                        attribute.PropertyName,
                        this.GetType().Name));
                }

                propertyInfo.SetValue(this, component, null);

                var behaviour = component as MonoBehaviour;
                if (behaviour != null) {
                    behaviour.enabled = attribute.StartEnabled;
                }
            }
        }

        /// <summary>
        /// Handles attaching components and assigning them to fields, if any AttachComponent attributes exist.
        /// </summary>
        private void HandleAttachComponentToFieldAttributes() {
            var attachAttributes = this.GetAttributes<AttachComponentToFieldAttribute>().ToArray();

            for (int i = 0; i < attachAttributes.Length; i++) {
                var attribute = attachAttributes[i];
                if (!attribute.ComponentType.IsSubclassOf(typeof(Component))) {
                    throw new NotSupportedException(
                        string.Format(
                            "The AttachComponentToField attribute can only be used to attach Components. Type {0} is not a Component.",
                            attribute.ComponentType.Name));
                }

                var component = this.GetOrAddComponent(attribute.ComponentType);

                if (string.IsNullOrEmpty(attribute.FieldName)) {
                    return;
                }

                var fieldInfo = this.GetType().GetField(attribute.FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (fieldInfo == null) {
                    throw new NotSupportedException(
                        string.Format(
                            "The AttachComponentToField attribute could not assign to field with name '{0}' on class '{1}', because it does not exist.",
                            attribute.FieldName,
                            this.GetType().Name));
                }

                if (fieldInfo.IsInitOnly) {
                    throw new NotSupportedException(
                        string.Format(
                            "The AttachComponentToField attribute could not assign to field with name '{0}' on class '{1}', because it is a read only field.",
                            attribute.FieldName,
                            this.GetType().Name));
                }

                if (fieldInfo.FieldType != attribute.ComponentType) {
                    throw new NotSupportedException(
                    string.Format(
                        "The AttachComponentToField attribute could not assign to field with name '{0}' on class '{1}', because the field is not of the same type.",
                        attribute.FieldName,
                        this.GetType().Name));
                }

                fieldInfo.SetValue(this, component);

                var behaviour = component as MonoBehaviour;
                if (behaviour != null) {
                    behaviour.enabled = attribute.StartEnabled;
                }
            }
        }

        /// <summary>
        /// An attribute for adding components to a BaseBehaviour and optionally assigning them to a property.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public sealed class AttachComponentAttribute : Attribute {

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentAttribute" /> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            public AttachComponentAttribute(Type componentType) : this(componentType, string.Empty) {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentAttribute" /> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            /// <param name="propertyName">Name of the property to set this behaviour to.</param>
            public AttachComponentAttribute(Type componentType, string propertyName) : this(componentType, propertyName, true) {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentAttribute" /> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="startEnabled">if set to <c>true</c> [start enabled].</param>
            public AttachComponentAttribute(Type componentType, string propertyName, bool startEnabled) {
                this.ComponentType = componentType;
                this.PropertyName = propertyName;
                this.StartEnabled = startEnabled;
            }

            /// <summary>
            /// Gets the type of the component.
            /// </summary>
            /// <value>
            /// The type of the component.
            /// </value>
            public Type ComponentType { get; private set; }

            /// <summary>
            /// Gets the name of the property to .
            /// </summary>
            /// <value>
            /// The name of the property.
            /// </value>
            public string PropertyName { get; private set; }

            /// <summary>
            /// Gets a value indicating whether [start enabled].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [start enabled]; otherwise, <c>false</c>.
            /// </value>
            public bool StartEnabled { get; private set; }
        }

        /// <summary>
        /// An attribute for attaching components to a BaseBehaviour and assigning them to a field.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public sealed class AttachComponentToFieldAttribute : Attribute {

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentToFieldAttribute" /> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            /// <param name="fieldName">Name of the field.</param>
            public AttachComponentToFieldAttribute(Type componentType, string fieldName) : this(componentType, fieldName, true) {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentToFieldAttribute" /> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            /// <param name="fieldName">Name of the field.</param>
            /// <param name="startEnabled">if set to <c>true</c> [start enabled].</param>
            public AttachComponentToFieldAttribute(Type componentType, string fieldName, bool startEnabled) {
                this.ComponentType = componentType;
                this.FieldName = fieldName;
                this.StartEnabled = startEnabled;
            }

            /// <summary>
            /// Gets the type of the component.
            /// </summary>
            /// <value>
            /// The type of the component.
            /// </value>
            public Type ComponentType { get; private set; }

            /// <summary>
            /// Gets the name of the field.
            /// </summary>
            /// <value>
            /// The name of the field.
            /// </value>
            public string FieldName { get; private set; }

            /// <summary>
            /// Gets a value indicating whether [start enabled].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [start enabled]; otherwise, <c>false</c>.
            /// </value>
            public bool StartEnabled { get; private set; }
        }
    }
}