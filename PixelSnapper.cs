namespace BrettMStory.Unity {

    using UnityEngine;

    /// <summary>
    /// Snaps pixels!
    /// </summary>
    public class PixelSnapper : BaseBehaviour {

        /// <summary>
        /// The transform's actual position.
        /// </summary>
        private Vector2 _actualPosition;

        /// <summary>
        /// The transform's pixel position.
        /// </summary>
        private Vector2 _pixelPosition;

        /// <summary>
        /// The number of pixels per unity unit.
        /// </summary>
        [SerializeField]
        private float _pixelsPerUnit = 1f;

        /// <summary>
        /// A value indicating whether or not this should snap on update.
        /// </summary>
        [SerializeField]
        private bool _snapOnUpdate = true;

        /// <summary>
        /// The number of units per pixel (used for faster calculations).
        /// </summary>
        private float _unitsPerPixel;

        /// <summary>
        /// Gets or sets the pixels per unit.
        /// </summary>
        /// <value>
        /// The pixels per unit.
        /// </value>
        public float PixelsPerUnit {
            get {
                return this._pixelsPerUnit;
            }

            set {
                if (value > 0f) {
                    this._pixelsPerUnit = value;
                    this._unitsPerPixel = 1f / this._pixelsPerUnit;
                    this._pixelPosition = this.GetPixelPosition();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this should snap on update.
        /// </summary>
        public bool SnapOnUpdate {
            get {
                return this._snapOnUpdate;
            }

            set {
                this._snapOnUpdate = value;
            }
        }

        /// <summary>
        /// The awake call.
        /// </summary>
        protected void Awake() {
            if (this._pixelsPerUnit <= 0f) {
                this.enabled = false;
            }
            else {
                this._unitsPerPixel = 1f / this._pixelsPerUnit;
                this._actualPosition = this.Position;
                this._pixelPosition = this.GetPixelPosition();

                this.SnapToPixelLocation();
                this.enabled = this._snapOnUpdate;
            }
        }

        /// <summary>
        /// The late update call.
        /// </summary>
        protected void LateUpdate() {
            if (!this._snapOnUpdate) {
                return;
            }

            this.SnapToPixelLocation();
        }

        /// <summary>
        /// Gets pixel position from actual position.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetPixelPosition() {
            return new Vector2(
                Mathf.Round(_actualPosition.x * this._pixelsPerUnit) * this._unitsPerPixel,
                Mathf.Round(_actualPosition.y * this._pixelsPerUnit) * this._unitsPerPixel);
        }

        /// <summary>
        /// Performs the actual snapping.
        /// </summary>
        private void SnapToPixelLocation() {
            var newPosition = this.Position2D;
            var differenceVector = newPosition - this._pixelPosition;
            this._actualPosition += differenceVector;
            this._pixelPosition = this.GetPixelPosition();
            this.Position2D = this._pixelPosition;
        }
    }
}