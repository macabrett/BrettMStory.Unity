namespace BrettMStory.Unity2D.Attributes {

    using System;

    /// <summary>
    /// An attribute for adding components to a BaseBehaviour and optionally assigning them to a
    /// field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public sealed class AttachComponentAttribute : Attribute {

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachComponentAttribute"/> class.
        /// </summary>
        public AttachComponentAttribute() : this(true) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachComponentAttribute"/> class.
        /// </summary>
        /// <param name="startEnabled">if set to <c>true</c> [start enabled].</param>
        public AttachComponentAttribute(bool startEnabled) {
            this.StartEnabled = startEnabled;
        }

        /// <summary>
        /// Gets a value indicating whether [start enabled].
        /// </summary>
        /// <value><c>true</c> if [start enabled]; otherwise, <c>false</c>.</value>
        public bool StartEnabled { get; }
    }
}