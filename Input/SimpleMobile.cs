namespace BrettMStory.Unity2D {

    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Input controller.
    /// </summary>
    public class SimpleMobile : MonoBehaviour {

        /// <summary>
        /// The singleton instance of InputController;
        /// </summary>
        private static SimpleMobile _instance;

        /// <summary>
        /// The input data.
        /// </summary>
        private readonly Dictionary<int, TouchData> _inputData = new Dictionary<int, TouchData>();

        /// <summary>
        /// The maximum time for a touch to still be considered a tap.
        /// </summary>
        [SerializeField]
        private float _maximumTapTime = 0.5f;

        /// <summary>
        /// The minimum percentage of the screen height that a touch must traverse to be considered a swipe.
        /// </summary>
        [SerializeField]
        private float _minimumScreenHeightSwipePercentage = 0.1f;

        /// <summary>
        /// The minimum percentage of the screen width that a touch must traverse to be considered a swipe.
        /// </summary>
        [SerializeField]
        private float _minimumScreenWidthSwipePercentage = 0.1f;

        /// <summary>
        /// Event handler for [Began Touch].
        /// </summary>
        public event EventHandler<TouchEventArgs> BeganTouch;

        /// <summary>
        /// Event handler for [Ended Touch].
        /// </summary>
        public event EventHandler<TouchEventArgs> EndedTouch;

        /// <summary>
        /// Event handler for [Swiped Down].
        /// </summary>
        public event EventHandler<TouchEventArgs> SwipedDown;

        /// <summary>
        /// Event handler for [Swiped Left].
        /// </summary>
        public event EventHandler<TouchEventArgs> SwipedLeft;

        /// <summary>
        /// Event handler for [Swiped Right].
        /// </summary>
        public event EventHandler<TouchEventArgs> SwipedRight;

        /// <summary>
        /// Event handler for [Swiped Up].
        /// </summary>
        public event EventHandler<TouchEventArgs> SwipedUp;

        /// <summary>
        /// Event handler for [Tap].
        /// </summary>
        public event EventHandler<TouchEventArgs> Tap;

        /// <summary>
        /// The singleton instance of SimpleTouch.
        /// </summary>
        public static SimpleMobile Instance {
            get {
                if (SimpleMobile._instance == null) {
                    var simpleTouchGameObject = new GameObject("Simple Touch");
                    SimpleMobile._instance = simpleTouchGameObject.AddComponent<SimpleMobile>();
                }

                return SimpleMobile._instance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not an instance exists.
        /// </summary>
        public static bool InstanceExists {
            get {
                return SimpleMobile._instance != null;
            }
        }

        /// <summary>
        /// Gets the maximum time for a touch to still be considered a tap.
        /// </summary>
        internal float MaximumTapTime {
            get {
                return this._maximumTapTime;
            }
        }

        /// <summary>
        /// Gets the minimum percentage of the screen height that a touch must traverse to be
        /// considered a swipe.
        /// </summary>
        internal float MinimumScreenHeightSwipeDistance { get; private set; }

        /// <summary>
        /// Gets the minimum percentage of the screen width that a touch must traverse to be
        /// considered a swipe.
        /// </summary>
        internal float MinimumScreenWidthSwipeDistance { get; private set; }

        /// <summary>
        /// The awake call.
        /// </summary>
        protected virtual void Awake() {
            SimpleMobile._instance = this;
            this.MinimumScreenHeightSwipeDistance = Screen.height * this._minimumScreenHeightSwipePercentage;
            this.MinimumScreenWidthSwipeDistance = Screen.width * this._minimumScreenWidthSwipePercentage;
        }

        /// <summary>
        /// The update.
        /// </summary>
        protected void Update() {
            for (var i = 0; i < Input.touchCount; i++) {
                var touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began) {
                    this.HandleTouchBegan(touch);
                }
                else if (touch.phase == TouchPhase.Ended) {
                    this.HandleTouchEnd(touch);
                }
            }
        }

        /// <summary>
        /// Handles a touch beginning.
        /// </summary>
        /// <param name="touch">A UnityEngine touch.</param>
        private void HandleTouchBegan(Touch touch) {
            if (!this._inputData.ContainsKey(touch.fingerId)) {
                this._inputData.Add(touch.fingerId, new TouchData());
            }

            this._inputData[touch.fingerId].Begin(touch);

            this.BeganTouch.SafeInvoke(this, new TouchEventArgs(touch));
        }

        /// <summary>
        /// Handles a touch ending.
        /// </summary>
        /// <param name="touch">A UnityEngine touch.</param>
        private void HandleTouchEnd(Touch touch) {
            if (!this._inputData.ContainsKey(touch.fingerId)) {
                return;
            }

            var touchEventArgs = new TouchEventArgs(touch);
            this.EndedTouch.SafeInvoke(this, touchEventArgs);

            var inputData = this._inputData[touch.fingerId];
            var touchType = inputData.End(touch);

            switch (touchType) {
                case TouchType.Down:
                    SwipedDown.SafeInvoke(this, touchEventArgs);
                    break;

                case TouchType.Left:
                    SwipedLeft.SafeInvoke(this, touchEventArgs);
                    break;

                case TouchType.Right:
                    SwipedRight.SafeInvoke(this, touchEventArgs);
                    break;

                case TouchType.Up:
                    SwipedUp.SafeInvoke(this, touchEventArgs);
                    break;

                case TouchType.Tap:
                    Tap.SafeInvoke(this, touchEventArgs);
                    break;
            }
        }
    }
}