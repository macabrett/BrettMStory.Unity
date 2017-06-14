namespace BrettMStory.Unity2D {

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
        /// A value indicating whether or not this behaviour is busy.
        /// </summary>
        private bool _isBusy = false;

        /// <summary>
        /// Gets the gameo object attached to this behaviour.
        /// </summary>
        public GameObject GameObject {
            get {
                return this.gameObject;
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
        public Vector2 Position {
            get {
                return this.Transform.position;
            }

            set {
                this.Transform.position = new Vector3(value.x, value.y, 0f);
            }
        }

        /// <summary>
        /// Gets or sets the rotation (along the z-axis)
        /// </summary>
        public float Rotation {
            get {
                return this.Transform.eulerAngles.z;
            }

            set {
                this.Transform.eulerAngles = new Vector3(1f, 1f, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public Vector2 Scale {
            get {
                return this.Transform.localScale.ToVector2();
            }

            set {
                this.Transform.localScale = new Vector3(value.x, value.y, 1f);
            }
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public string Tag {
            get {
                return this.GameObject.tag;
            }

            set {
                this.GameObject.tag = value;
            }
        }

        /// <summary>
        /// Gets the transform of the game object.
        /// </summary>
        public Transform Transform {
            get {
                return this.transform;
            }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        protected virtual void Awake() {
            this.HandleAttachComponentAttributes();
            this.HandleAttachComponentToFieldAttributes();
            this.HandleTagGameObjectAttribute();
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
        /// Handles attaching components and optionally assigning them to properties, if any
        /// AttachBehaviour attributes exist.
        /// </summary>
        private void HandleAttachComponentAttributes() {
            var attachAttributes = this.GetAttributes<AttachComponentAttribute>().ToArray();

            for (var i = 0; i < attachAttributes.Length; i++) {
                var attribute = attachAttributes[i];
                if (!attribute.ComponentType.IsSubclassOf(typeof(Component))) {
                    throw new NotSupportedException($"The AttachComponent attribute can only be used to attach Components. Type '{attribute.ComponentType.Name}' is not a Component.");
                }

                var component = this.GetOrAddComponent(attribute.ComponentType);

                if (string.IsNullOrEmpty(attribute.PropertyName)) {
                    return;
                }

                var propertyInfo = this.GetType().GetProperty(attribute.PropertyName);
                if (propertyInfo == null) {
                    throw new NotSupportedException($"The AttachComponent attribute could not assign to property with name '{attribute.PropertyName}' on class '{this.GetType().Name}', because it does not exist.");
                }
                else if (!propertyInfo.CanWrite) {
                    throw new NotSupportedException($"The AttachComponent attribute could not assign to property with name '{attribute.PropertyName}' on class '{this.GetType().Name}', because it is a read only property.");
                }
                else if (propertyInfo.PropertyType != attribute.ComponentType) {
                    throw new NotSupportedException($"The AttachComponent attribute could not assign to property with name '{attribute.PropertyName}' on class '{this.GetType().Name}', because the property is not of the same type.");
                }

                propertyInfo.SetValue(this, component, null);

                if (component is MonoBehaviour behaviour) {
                    behaviour.enabled = attribute.StartEnabled;
                }
            }
        }

        /// <summary>
        /// Handles attaching components and assigning them to fields, if any AttachComponent
        /// attributes exist.
        /// </summary>
        private void HandleAttachComponentToFieldAttributes() {
            var attachAttributes = this.GetAttributes<AttachComponentToFieldAttribute>().ToArray();

            for (var i = 0; i < attachAttributes.Length; i++) {
                var attribute = attachAttributes[i];
                if (!attribute.ComponentType.IsSubclassOf(typeof(Component))) {
                    throw new NotSupportedException($"The AttachComponentToField attribute can only be used to attach Components. Type '{attribute.ComponentType.Name}' is not a Component.");
                }

                var component = this.GetOrAddComponent(attribute.ComponentType);

                if (string.IsNullOrEmpty(attribute.FieldName)) {
                    return;
                }

                var fieldInfo = this.GetType().GetField(attribute.FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (fieldInfo == null) {
                    throw new NotSupportedException($"The AttachComponentToField attribute could not assign to field with name '{attribute.FieldName}' on class '{this.GetType().Name}', because it does not exist.");
                }
                else if (fieldInfo.IsInitOnly) {
                    throw new NotSupportedException($"The AttachComponentToField attribute could not assign to field with name '{attribute.FieldName}' on class '{this.GetType().Name}', because it is a read only field.");
                }
                else if (fieldInfo.FieldType != attribute.ComponentType) {
                    throw new NotSupportedException($"The AttachComponentToField attribute could not assign to field with name '{attribute.FieldName}' on class '{this.GetType().Name}', because the field is not of the same type.");
                }

                fieldInfo.SetValue(this, component);

                if (component is MonoBehaviour behaviour) {
                    behaviour.enabled = attribute.StartEnabled;
                }
            }
        }

        /// <summary>
        /// Handles the tag game object attribute.
        /// </summary>
        private void HandleTagGameObjectAttribute() {
            var attachAttributes = this.GetAttributes<TagGameObject>().ToArray();

            if (attachAttributes.Length > 0) {
                var attribute = attachAttributes[0];
                this.Tag = attribute.Tag;
            }
        }

        /// <summary>
        /// An attribute for adding components to a BaseBehaviour and optionally assigning them to a property.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public sealed class AttachComponentAttribute : Attribute {

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentAttribute"/> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            public AttachComponentAttribute(Type componentType) : this(componentType, string.Empty) {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentAttribute"/> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            /// <param name="propertyName">Name of the property to set this behaviour to.</param>
            public AttachComponentAttribute(Type componentType, string propertyName) : this(componentType, propertyName, true) {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentAttribute"/> class.
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
            /// <value>The type of the component.</value>
            public Type ComponentType { get; private set; }

            /// <summary>
            /// Gets the name of the property to .
            /// </summary>
            /// <value>The name of the property.</value>
            public string PropertyName { get; private set; }

            /// <summary>
            /// Gets a value indicating whether [start enabled].
            /// </summary>
            /// <value><c>true</c> if [start enabled]; otherwise, <c>false</c>.</value>
            public bool StartEnabled { get; private set; }
        }

        /// <summary>
        /// An attribute for attaching components to a BaseBehaviour and assigning them to a field.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public sealed class AttachComponentToFieldAttribute : Attribute {

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentToFieldAttribute"/> class.
            /// </summary>
            /// <param name="componentType">Type of the component.</param>
            /// <param name="fieldName">Name of the field.</param>
            public AttachComponentToFieldAttribute(Type componentType, string fieldName) : this(componentType, fieldName, true) {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AttachComponentToFieldAttribute"/> class.
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
            /// <value>The type of the component.</value>
            public Type ComponentType { get; private set; }

            /// <summary>
            /// Gets the name of the field.
            /// </summary>
            /// <value>The name of the field.</value>
            public string FieldName { get; private set; }

            /// <summary>
            /// Gets a value indicating whether [start enabled].
            /// </summary>
            /// <value><c>true</c> if [start enabled]; otherwise, <c>false</c>.</value>
            public bool StartEnabled { get; private set; }
        }

        /// <summary>
        /// Attribute for tagging a game object.
        /// </summary>
        /// <seealso cref="System.Attribute"/>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public sealed class TagGameObject : Attribute {

            /// <summary>
            /// Initializes a new instance of the <see cref="TagGameObject"/> class.
            /// </summary>
            /// <param name="tag">The tag.</param>
            public TagGameObject(string tag) {
                this.Tag = tag;
            }

            /// <summary>
            /// Gets the tag.
            /// </summary>
            /// <value>The tag.</value>
            public string Tag { get; private set; }
        }
    }
}