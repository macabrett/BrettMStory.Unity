namespace BrettMStory.Unity {

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// The screen position enum.
    /// </summary>
    public enum ScreenPosition {
        Left,
        Right,
        Center
    }

    /// <summary>
    /// A camera which follows a target.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class HorizontalCamera2D : BaseBehaviour {

        /// <summary>
        /// The screen position.
        /// </summary>
        [SerializeField]
        private ScreenPosition _anchorPoint;

        /// <summary>
        /// The camera.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// Delegate action to follow target.
        /// </summary>
        private Dictionary<ScreenPosition, Action> _followTargetActions = new Dictionary<ScreenPosition, Action>();

        /// <summary>
        /// Half of the world height.
        /// </summary>
        private float _halfWorldHeight;

        /// <summary>
        /// Half of the world width.
        /// </summary>
        private float _halfWorldWidth;

        /// <summary>
        /// The lerp amount.
        /// </summary>
        [SerializeField]
        private float _lerpAmount;

        /// <summary>
        /// The minimum world height.
        /// </summary>
        [SerializeField]
        private float _minimumWorldHeight;

        /// <summary>
        /// The minimum world width.
        /// </summary>
        [SerializeField]
        private float _minimumWorldWidth;

        /// <summary>
        /// The screen height.
        /// </summary>
        private int _screenHeight;

        /// <summary>
        /// The screen width.
        /// </summary>
        private int _screenWidth;

        /// <summary>
        /// The screen height in world units.
        /// </summary>
        private float _screenWorldHeight;

        /// <summary>
        /// The screen width in world units.
        /// </summary>
        private float _screenWorldWidth;

        /// <summary>
        /// The target to follow.
        /// </summary>
        [SerializeField]
        private Transform _target;

        /// <summary>
        /// The offset from the target.
        /// </summary>
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
                return new Vector2(this.Position2D.x - this._halfWorldWidth, this.Position2D.y - this._halfWorldHeight);
            }
        }

        /// <summary>
        /// The bottom right corner of the screen in world position.
        /// </summary>
        public Vector2 BottomRightCorner {
            get {
                return new Vector2(this.Position2D.x + this._halfWorldWidth, this.Position2D.y - this._halfWorldHeight);
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
                return new Vector2(this.Position2D.x - this._halfWorldWidth, this.Position2D.y + this._halfWorldHeight);
            }
        }

        /// <summary>
        /// The top right corner of the screen in world position.
        /// </summary>
        public Vector2 TopRightCorner {
            get {
                return new Vector2(this.Position2D.x + this._halfWorldWidth, this.Position2D.y + this._halfWorldHeight);
            }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        protected virtual void Awake() {
            this._camera = this.GetComponent<Camera>();
            this.StartCoroutine(this.CheckScreenSizeChanged());

            this.SetInitialYPosition();

            this._followTargetActions.Add(ScreenPosition.Left, this.GetLeftFollowAction());
            this._followTargetActions.Add(ScreenPosition.Right, this.GetRightFollowAction());
            this._followTargetActions.Add(ScreenPosition.Center, this.GetCenterFollowAction());
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        protected virtual void Update() {
            if (this._followTargetActions.ContainsKey(this._anchorPoint)) {
                this._followTargetActions[this._anchorPoint]();
            }
        }

        /// <summary>
        /// Adjusts the camera to a new resolution.
        /// </summary>
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

        /// <summary>
        /// Checks if the screen size has changed.
        /// </summary>
        /// <returns>An enumerator.</returns>
        private IEnumerator CheckScreenSizeChanged() {
            while (true) {
                if (Screen.width != this._screenWidth || Screen.height != this._screenHeight) {
                    this.Adjust();
                }

                yield return new WaitForSeconds(0.25f);
            }
        }

        /// <summary>
        /// Gets the follow action for a center anchor point.
        /// </summary>
        /// <returns>The follow action for a center anchor point.</returns>
        private Action GetCenterFollowAction() {
            return () => {
                this.Position2D = Vector2.Lerp(this.Position2D, new Vector2(this._target.position.x, this.Position2D.y) + this._targetOffset, this._lerpAmount * Time.deltaTime);
            };
        }

        /// <summary>
        /// Gets the follow action for a left anchor point.
        /// </summary>
        /// <returns>The follow action for a left anchor point.</returns>
        private Action GetLeftFollowAction() {
            return () => {
                var xPosition = this._target.position.x + this._halfWorldWidth + this._targetOffset.x;
                this.Position2D = Vector2.Lerp(this.Position2D, new Vector2(xPosition, this.Position2D.y), this._lerpAmount * Time.deltaTime);
            };
        }

        /// <summary>
        /// Gets the follow action for a right anchor point.
        /// </summary>
        /// <returns>The follow action for a right anchor point.</returns>
        private Action GetRightFollowAction() {
            return () => {
                var xPosition = this._target.position.x - this._halfWorldWidth + this._targetOffset.x;
                this.Position2D = Vector2.Lerp(this.Position2D, new Vector2(xPosition, this.Position2D.y), this._lerpAmount * Time.deltaTime);
            };
        }

        /// <summary>
        /// Sets the initial Y position.
        /// </summary>
        private void SetInitialYPosition() {
            var yPosition = this._target.position.y + this._halfWorldHeight + this._targetOffset.y;
            this.Position2D = new Vector2(this.Position2D.x, yPosition);
        }
    }
}