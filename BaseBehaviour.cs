namespace BrettMStory.Unity2D {

    using BrettMStory.Unity2D.Attributes;
    using BrettMStory.Unity2D.Extensions;
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

        private T GetAttribute<T>() where T : Attribute {
            return (T)Attribute.GetCustomAttribute(this.GetType(), typeof(T));
        }

        private IEnumerable<T> GetAttributes<T>() where T : Attribute {
            return Attribute.GetCustomAttributes(this.GetType(), typeof(T)).Cast<T>();
        }

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
    }
}