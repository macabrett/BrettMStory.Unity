namespace BrettMStory.Unity2D {

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// The screen position enum.
    /// </summary>
    public enum ScreenPosition {

        /// <summary>
        /// Will anchor the screen on the left side.
        /// </summary>
        Left,

        /// <summary>
        /// Will anchor the screen on the right side.
        /// </summary>
        Right,

        /// <summary>
        /// Will anchor the screen in the center.
        /// </summary>
        Center
    }

    /// <summary>
    /// A camera which follows a target.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed class HorizontalCamera : BaseBehaviour {

        [SerializeField]
        private ScreenPosition _anchorPoint;

        private Camera _camera;
        private Dictionary<ScreenPosition, Action> _followTargetActions = new Dictionary<ScreenPosition, Action>();
        private float _halfWorldHeight;
        private float _halfWorldWidth;

        [SerializeField]
        private float _lerpAmount;

        [SerializeField]
        private float _minimumWorldHeight;

        [SerializeField]
        private float _minimumWorldWidth;

        private int _screenHeight;
        private int _screenWidth;
        private float _screenWorldHeight;
        private float _screenWorldWidth;

        [SerializeField]
        private Transform _target;

        [SerializeField]
        private Vector2 _targetOffset;

        /// <summary>
        /// Called when [screen size changed].
        /// </summary>
        public event EventHandler<ScreenSizeChangedEventArgs> ScreenSizeChanged;

        /// <summary>
        /// Gets or sets the anchor point.
        /// </summary>
        public ScreenPosition AnchorPoint {
            get {
                return this._anchorPoint;
            }

            set {
                this._anchorPoint = value;
            }
        }

        /// <summary>
        /// The bottom left corner of the screen in world position.
        /// </summary>
        public Vector2 BottomLeftCorner {
            get {
                return new Vector2(this.Position.x - this._halfWorldWidth, this.Position.y - this._halfWorldHeight);
            }
        }

        /// <summary>
        /// The bottom right corner of the screen in world position.
        /// </summary>
        public Vector2 BottomRightCorner {
            get {
                return new Vector2(this.Position.x + this._halfWorldWidth, this.Position.y - this._halfWorldHeight);
            }
        }

        /// <summary>
        /// Gets or sets the target offset.
        /// </summary>
        public Vector2 TargetOffset {
            get {
                return this._targetOffset;
            }

            set {
                this._targetOffset = value;
                this.SetInitialYPosition();
            }
        }

        /// <summary>
        /// The top left corner of the screen in world position.
        /// </summary>
        public Vector2 TopLeftCorner {
            get {
                return new Vector2(this.Position.x - this._halfWorldWidth, this.Position.y + this._halfWorldHeight);
            }
        }

        /// <summary>
        /// The top right corner of the screen in world position.
        /// </summary>
        public Vector2 TopRightCorner {
            get {
                return new Vector2(this.Position.x + this._halfWorldWidth, this.Position.y + this._halfWorldHeight);
            }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        protected override void Awake() {
            this._camera = this.GetComponent<Camera>();
            this.StartCoroutine(this.CheckScreenSizeChanged());

            this.SetInitialYPosition();

            this._followTargetActions.Add(ScreenPosition.Left, this.GetLeftFollowAction());
            this._followTargetActions.Add(ScreenPosition.Right, this.GetRightFollowAction());
            this._followTargetActions.Add(ScreenPosition.Center, this.GetCenterFollowAction());

            base.Awake();
        }

        private void Adjust() {
            this._camera.orthographicSize = this._minimumWorldHeight * 0.5f;
            var worldWidth = this._camera.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x - this._camera.ScreenToWorldPoint(Vector2.zero).x;

            if (worldWidth < this._minimumWorldWidth) {
                this._camera.orthographicSize *= this._minimumWorldWidth / worldWidth;
            }

            this._screenHeight = Screen.height;
            this._screenWidth = Screen.width;

            this._screenWorldWidth = this._camera.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x - this._camera.ScreenToWorldPoint(Vector2.zero).x;
            this._halfWorldWidth = this._screenWorldWidth / 2f;

            this._screenWorldHeight = this._camera.ScreenToWorldPoint(new Vector2(0f, Screen.height)).y - this._camera.ScreenToWorldPoint(Vector2.zero).y;
            this._halfWorldHeight = this._screenWorldHeight / 2f;

            this.ScreenSizeChanged.SafeInvoke(this, new ScreenSizeChangedEventArgs {
                WorldHeight = this._screenWorldHeight,
                WorldWidth = this._screenWorldWidth
            });

            this.SetInitialYPosition();
        }

        private IEnumerator CheckScreenSizeChanged() {
            while (true) {
                if (Screen.width != this._screenWidth || Screen.height != this._screenHeight) {
                    this.Adjust();
                }

                yield return new WaitForSeconds(0.25f);
            }
        }

        private Action GetCenterFollowAction() {
            return () => {
                this.Position = Vector2.Lerp(this.Position, new Vector2(this._target.position.x, this.Position.y) + this._targetOffset, this._lerpAmount * Time.deltaTime);
            };
        }

        private Action GetLeftFollowAction() {
            return () => {
                var xPosition = this._target.position.x + this._halfWorldWidth + this._targetOffset.x;
                this.Position = Vector2.Lerp(this.Position, new Vector2(xPosition, this.Position.y), this._lerpAmount * Time.deltaTime);
            };
        }

        private Action GetRightFollowAction() {
            return () => {
                var xPosition = this._target.position.x - this._halfWorldWidth + this._targetOffset.x;
                this.Position = Vector2.Lerp(this.Position, new Vector2(xPosition, this.Position.y), this._lerpAmount * Time.deltaTime);
            };
        }

        private void SetInitialYPosition() {
            var yPosition = this._target.position.y + this._halfWorldHeight + this._targetOffset.y;
            this.Position = new Vector2(this.Position.x, yPosition);
        }

        private void Update() {
            if (this._followTargetActions.ContainsKey(this._anchorPoint)) {
                this._followTargetActions[this._anchorPoint]();
            }
        }
    }
}