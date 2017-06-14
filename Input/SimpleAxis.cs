namespace BrettMStory.Unity {

    using System;
    using UnityEngine;

    /// <summary>
    /// A class which handles simple axis input (up, down, left, right)
    /// </summary>
    public class SimpleAxis : MonoBehaviour {

        /// <summary>
        /// An instance of SimpleAxis.
        /// </summary>
        private static SimpleAxis _instance;

        /// <summary>
        /// A value indicating whether or not down has been pressed.
        /// </summary>
        private bool _isDownPressed = false;

        /// <summary>
        /// A value indicating whether or not left has been pressed.
        /// </summary>
        private bool _isLeftPressed = false;

        /// <summary>
        /// A value indicating whether or not right has been pressed.
        /// </summary>
        private bool _isRightPressed = false;

        /// <summary>
        /// A value indicating whether or not up has been pressed.
        /// </summary>
        private bool _isUpPressed = false;

        /// <summary>
        /// Event thrown when [Down Pressed].
        /// </summary>
        public event EventHandler<AxisEventArgs> DownPressed;

        /// <summary>
        /// Event thrown when [Down Released].
        /// </summary>
        public event EventHandler<AxisEventArgs> DownReleased;

        /// <summary>
        /// Event thrown when [Down Repeating].
        /// </summary>
        public event EventHandler<AxisEventArgs> DownRepeating;

        /// <summary>
        /// Event thrown when [Left Pressed].
        /// </summary>
        public event EventHandler<AxisEventArgs> LeftPressed;

        /// <summary>
        /// Event thrown when [Left Released].
        /// </summary>
        public event EventHandler<AxisEventArgs> LeftReleased;

        /// <summary>
        /// Event thrown when [Left Repeating].
        /// </summary>
        public event EventHandler<AxisEventArgs> LeftRepeating;

        /// <summary>
        /// Event thrown when [Right Pressed].
        /// </summary>
        public event EventHandler<AxisEventArgs> RightPressed;

        /// <summary>
        /// Event thrown when [Right Released].
        /// </summary>
        public event EventHandler<AxisEventArgs> RightReleased;

        /// <summary>
        /// Event thrown when [Right Repeating].
        /// </summary>
        public event EventHandler<AxisEventArgs> RightRepeating;

        /// <summary>
        /// Event thrown when [Up Pressed].
        /// </summary>
        public event EventHandler<AxisEventArgs> UpPressed;

        /// <summary>
        /// Event thrown when [Up Released].
        /// </summary>
        public event EventHandler<AxisEventArgs> UpReleased;

        /// <summary>
        /// Event thrown when [Up Repeating].
        /// </summary>
        public event EventHandler<AxisEventArgs> UpRepeating;

        /// <summary>
        /// The singleton instance of SimpleAxis.
        /// </summary>
        public static SimpleAxis Instance {
            get {
                if (SimpleAxis._instance == null) {
                    var simpleAxisGameObject = new GameObject("Simple Axis");
                    SimpleAxis._instance = simpleAxisGameObject.AddComponent<SimpleAxis>();
                }

                return SimpleAxis._instance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not an instance exists.
        /// </summary>
        public static bool InstanceExists {
            get {
                return SimpleAxis._instance != null;
            }
        }

        /// <summary>
        /// The awake call.
        /// </summary>
        protected void Awake() {
            if (SimpleAxis._instance == null) {
                SimpleAxis._instance = this;
            }
        }

        /// <summary>
        /// The update call.
        /// </summary>
        protected void Update() {
            this.HandleHorizontal();
            this.HandleVertical();
        }

        /// <summary>
        /// Handles horizontal input.
        /// </summary>
        private void HandleHorizontal() {
            var x = Input.GetAxis("Horizontal");
            var xRaw = Input.GetAxisRaw("Horizontal");

            if (xRaw > 0) {
                if (this._isRightPressed) {
                    this.RightRepeating.SafeInvoke(this, new AxisEventArgs(x, xRaw));
                }
                else {
                    this.RightPressed.SafeInvoke(this, new AxisEventArgs(x, xRaw));
                    this._isRightPressed = true;
                }
            }
            else if (this._isRightPressed) {
                this._isRightPressed = false;
                this.RightReleased.SafeInvoke(this, new AxisEventArgs(x, xRaw));
            }

            if (xRaw < 0) {
                if (this._isLeftPressed) {
                    this.LeftRepeating.SafeInvoke(this, new AxisEventArgs(x, xRaw));
                }
                else {
                    this.LeftPressed.SafeInvoke(this, new AxisEventArgs(x, xRaw));
                    this._isLeftPressed = true;
                }
            }
            else if (this._isLeftPressed) {
                this._isLeftPressed = false;
                this.LeftReleased.SafeInvoke(this, new AxisEventArgs(x, xRaw));
            }
        }

        /// <summary>
        /// Handles vertical input.
        /// </summary>
        private void HandleVertical() {
            var y = Input.GetAxis("Vertical");
            var yRaw = Input.GetAxisRaw("Vertical");

            if (yRaw > 0) {
                if (this._isUpPressed) {
                    this.UpRepeating.SafeInvoke(this, new AxisEventArgs(y, yRaw));
                }
                else {
                    this.UpPressed.SafeInvoke(this, new AxisEventArgs(y, yRaw));
                    this._isUpPressed = true;
                }
            }
            else if (this._isUpPressed) {
                this._isUpPressed = false;
                this.UpReleased.SafeInvoke(this, new AxisEventArgs(y, yRaw));
            }

            if (yRaw < 0) {
                if (this._isDownPressed) {
                    this.DownRepeating.SafeInvoke(this, new AxisEventArgs(y, yRaw));
                }
                else {
                    this.DownPressed.SafeInvoke(this, new AxisEventArgs(y, yRaw));
                    this._isDownPressed = true;
                }
            }
            else if (this._isDownPressed) {
                this._isDownPressed = false;
                this.DownReleased.SafeInvoke(this, new AxisEventArgs(y, yRaw));
            }
        }
    }
}