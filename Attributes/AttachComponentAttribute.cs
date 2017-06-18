namespace BrettMStory.Unity2D.Attributes {

    using System;

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
}