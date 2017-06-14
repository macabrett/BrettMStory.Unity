namespace BrettMStory.Unity2D {

    using System;
    using UnityEngine;

    /// <summary>
    /// A class which handles simple mouse input (button presses).
    /// </summary>
    public class SimpleMouse : MonoBehaviour {

        /// <summary>
        /// An instance of SimpleMouse.
        /// </summary>
        private static SimpleMouse _instance;

        /// <summary>
        /// Event handler for [Left Mouse Button Down].
        /// </summary>
        public event EventHandler<MouseInputEventArgs> LeftMouseButtonDown;

        /// <summary>
        /// Event handler for [Left Mouse Button Held].
        /// </summary>
        public event EventHandler<MouseInputEventArgs> LeftMouseButtonHeld;

        /// <summary>
        /// Event Handler for [Left Mouse Button Up].
        /// </summary>
        public event EventHandler<MouseInputEventArgs> LeftMouseButtonUp;

        /// <summary>
        /// The singleton instance of SimpleMouse.
        /// </summary>
        public static SimpleMouse Instance {
            get {
                if (SimpleMouse._instance == null) {
                    var simpleTouchGameObject = new GameObject("Simple Mouse");
                    SimpleMouse._instance = simpleTouchGameObject.AddComponent<SimpleMouse>();
                }

                return SimpleMouse._instance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not an instance exists.
        /// </summary>
        public static bool InstanceExists {
            get {
                return SimpleMouse._instance != null;
            }
        }

        /// <summary>
        /// The update call.
        /// </summary>
        protected void Update() {
            if (Input.GetMouseButtonDown(0)) {
                this.LeftMouseButtonDown.SafeInvoke(this, new MouseInputEventArgs(Input.mousePosition));
            }
            else if (Input.GetMouseButtonUp(0)) {
                this.LeftMouseButtonUp.SafeInvoke(this, new MouseInputEventArgs(Input.mousePosition));
            }
            else if (Input.GetMouseButton(0)) {
                this.LeftMouseButtonHeld.SafeInvoke(this, new MouseInputEventArgs(Input.mousePosition));
            }
        }
    }
}