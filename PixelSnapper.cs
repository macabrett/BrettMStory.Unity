namespace BrettMStory.Unity2D {

    using UnityEngine;

    /// <summary>
    /// Snaps pixels!
    /// </summary>
    public class PixelSnapper : BaseBehaviour {
        private Vector2 _actualPosition;
        private Vector2 _pixelPosition;

        [SerializeField]
        private float _pixelsPerUnit = 1f;

        [SerializeField]
        private bool _snapOnUpdate = true;

        private float _unitsPerPixel;

        /// <summary>
        /// Gets or sets the pixels per unit.
        /// </summary>
        /// <value>The pixels per unit.</value>
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
        protected override void Awake() {
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

        private Vector2 GetPixelPosition() {
            return new Vector2(
                Mathf.Round(this._actualPosition.x * this._pixelsPerUnit) * this._unitsPerPixel,
                Mathf.Round(this._actualPosition.y * this._pixelsPerUnit) * this._unitsPerPixel);
        }

        private void LateUpdate() {
            if (!this._snapOnUpdate) {
                return;
            }

            this.SnapToPixelLocation();
        }

        private void SnapToPixelLocation() {
            var newPosition = this.Position;
            var differenceVector = newPosition - this._pixelPosition;
            this._actualPosition += differenceVector;
            this._pixelPosition = this.GetPixelPosition();
            this.Position = this._pixelPosition;
        }
    }
}