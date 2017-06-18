namespace BrettMStory.Unity2D.Attributes {

    using System;

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
}