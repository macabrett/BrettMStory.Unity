namespace BrettMStory.Unity2D.Shaders {

    using System;
    using UnityEngine;

    /// <summary>
    /// The shader behaviour that applies a drop shadow effect to everything seen by a camera.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    public sealed class DropShadowShader : MonoBehaviour {

        [SerializeField]
        private Color _backgroundColor;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private CameraClearFlags _clearFlags;

        [SerializeField]
        private Vector2 _offset;

        private Shader _shader;

        private void Awake() {
            var currentCamera = this.GetComponent<Camera>();

            if (currentCamera == null) {
                throw new NotSupportedException("DropShadowShader can only be applied to a GameObject with a Camera.");
            }

            if (this._camera == null) {
                var newGameObject = new GameObject();
                newGameObject.transform.parent = this.gameObject.transform;
                this._camera = newGameObject.AddComponent<Camera>();
            }

            this._camera.CopyFrom(currentCamera);
            this._camera.transform.localPosition += this._offset.ToVector3();
            this._camera.depth = currentCamera.depth - 1;
            this._camera.backgroundColor = this._backgroundColor;
            this._camera.clearFlags = this._clearFlags;

            this._shader = Shader.Find("Hidden/DropShadow");
        }

        private void Start() {
            this._camera.SetReplacementShader(this._shader, "RenderType");
        }
    }
}