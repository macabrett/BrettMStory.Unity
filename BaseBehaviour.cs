namespace BrettMStory.Unity2D {

    using BrettMStory.Unity2D.Attributes;
    using BrettMStory.Unity2D.Extensions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
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
            this.AttachComponents();
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

        private void AttachComponents() {
            var type = this.GetType();
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            var properties = type.GetProperties(flags);

            foreach (var property in properties) {
                if (property.CanWrite && property.PropertyType.IsSubclassOf(typeof(Component)) && property.GetSetMethod() != null) {
                    var attributes = property.GetCustomAttributes(typeof(AttachComponentAttribute), true);

                    if (attributes.FirstOrDefault() is AttachComponentAttribute attachAttribute) {
                        var component = this.GetOrAddComponent(property.PropertyType);
                        property.SetValue(this, component, null);

                        if (component is MonoBehaviour behaviour) {
                            behaviour.enabled = attachAttribute.StartEnabled;
                        }
                    }
                }
            }

            var fields = type.GetFields(flags);
            foreach (var field in fields) {
                if (field.FieldType.IsSubclassOf(typeof(Component))) {
                    var attributes = field.GetCustomAttributes(typeof(AttachComponentAttribute), true);
                    if (attributes.FirstOrDefault() is AttachComponentAttribute attachAttribute) {
                        var component = this.GetOrAddComponent(field.FieldType);
                        Debug.Log(component);
                        field.SetValue(this, component);
                        if (component is MonoBehaviour behaviour) {
                            behaviour.enabled = attachAttribute.StartEnabled;
                        }
                    }
                }
            }
        }

        private T GetAttribute<T>() where T : Attribute {
            return (T)Attribute.GetCustomAttribute(this.GetType(), typeof(T));
        }

        private IEnumerable<T> GetAttributes<T>() where T : Attribute {
            return Attribute.GetCustomAttributes(this.GetType(), typeof(T)).Cast<T>();
        }
    }
}