namespace BrettMStory.Unity2D {

    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A pool of game objects.
    /// </summary>
    /// <typeparam name="T">The monobehaviour to return.</typeparam>
    public class GameObjectPool<T> where T : MonoBehaviour {

        /// <summary>
        /// Called [on get].
        /// </summary>
        private readonly Action<T> _onGet;

        /// <summary>
        /// Called [on release].
        /// </summary>
        private readonly Action<T> _onRelease;

        /// <summary>
        /// The prefab to instantiate based on.
        /// </summary>
        private readonly GameObject _prefab;

        /// <summary>
        /// The stack to pull behaviours from.
        /// </summary>
        private readonly Stack<T> _stack = new Stack<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectPool{T}"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="startingSize">Size of the starting.</param>
        public GameObjectPool(GameObject prefab, int startingSize) {
            var behaviour = prefab.GetComponent<T>();

            if (behaviour == null) {
                Debug.LogErrorFormat("Prefab for GameObjectPool<{0}> does not contain MonoBehaviour of type '{0}'", typeof(T));
            }

            this._prefab = prefab;
            this._onGet = this.DefaultOnGet;
            this._onRelease = this.DefaultOnRelease;
        }

        /// <summary>
        /// Instantiates a new instance of [GameObjectPool].
        /// </summary>
        /// <param name="prefab">The prefab to instantiate game objects.</param>
        /// <param name="startingSize">The starting size.</param>
        /// <param name="getFunction">
        /// A function to be called on a game object when it is gotten from the pool.
        /// </param>
        /// <param name="releaseFunction">
        /// A function to be called on a game object when it is released back to the pool.
        /// </param>
        public GameObjectPool(GameObject prefab, int startingSize, Action<T> getFunction, Action<T> releaseFunction)
            : this(prefab, startingSize) {
            this._onGet = getFunction;
            this._onRelease = releaseFunction;
        }

        /// <summary>
        /// Gets the next object from the pool.
        /// </summary>
        /// <returns>The next object from the pool.</returns>
        public T GetObject() {
            T poolBehaviour = null;

            if (this._stack.Count > 0) {
                poolBehaviour = this._stack.Pop();
            }
            else {
                poolBehaviour = this.Instantiate();
            }

            if (poolBehaviour != null && this._onGet != null) {
                this._onGet(poolBehaviour);
            }

            return poolBehaviour;
        }

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="poolBehaviour">The pool behaviour.</param>
        public void ReleaseObject(T poolBehaviour) {
            if (poolBehaviour != null && this._onRelease != null) {
                this._onRelease(poolBehaviour);
            }

            this._stack.Push(poolBehaviour);
        }

        /// <summary>
        /// The default method to be called when getting a pool object.
        /// </summary>
        /// <param name="poolBehaviour">The pool behaviour.</param>
        private void DefaultOnGet(T poolBehaviour) {
            poolBehaviour.gameObject.SetActive(true);
        }

        /// <summary>
        /// The default method to be called when getting a pool object.
        /// </summary>
        /// <param name="poolBehaviour">The pool behaviour.</param>
        private void DefaultOnRelease(T poolBehaviour) {
            poolBehaviour.gameObject.SetActive(false);
        }

        /// <summary>
        /// Instantiates a new game object.
        /// </summary>
        /// <returns>The instantiated game object.</returns>
        private T Instantiate() {
            var poolObject = GameObject.Instantiate(this._prefab) as GameObject;
            return poolObject.GetComponent<T>();
        }

        /// <summary>
        /// Preloads
        /// </summary>
        /// <param name="number"></param>
        private void Preload(int number) {
            for (var i = 0; i < number; i++) {
                this.ReleaseObject(this.Instantiate());
            }
        }
    }

    /// <summary>
    /// A pool of game objects.
    /// </summary>
    public class GameObjectPool {

        /// <summary>
        /// Called [on get].
        /// </summary>
        private readonly Action<GameObject> _onGet;

        /// <summary>
        /// Called [on release].
        /// </summary>
        private readonly Action<GameObject> _onRelease;

        /// <summary>
        /// The prefab to pull from the pool.
        /// </summary>
        private readonly GameObject _prefab;

        /// <summary>
        /// The stack to pull objects from.
        /// </summary>
        private readonly Stack<GameObject> _stack = new Stack<GameObject>();

        /// <summary>
        /// Instantiates a new instance of [GameObjectPool].
        /// </summary>
        /// <param name="prefab">The prefab to instantiate game objects.</param>
        /// <param name="startingSize">The starting size.</param>
        public GameObjectPool(GameObject prefab, int startingSize) {
            this._prefab = prefab;
            this.Preload(startingSize);
            this._onGet = this.DefaultOnGet;
            this._onRelease = this.DefaultOnRelease;
        }

        /// <summary>
        /// Instantiates a new instance of [GameObjectPool].
        /// </summary>
        /// <param name="prefab">The prefab to instantiate game objects.</param>
        /// <param name="startingSize">The starting size.</param>
        /// <param name="getFunction">
        /// A function to be called on a game object when it is gotten from the pool.
        /// </param>
        /// <param name="releaseFunction">
        /// A function to be called on a game object when it is released back to the pool.
        /// </param>
        public GameObjectPool(GameObject prefab, int startingSize, Action<GameObject> getFunction, Action<GameObject> releaseFunction)
            : this(prefab, startingSize) {
            this._onGet = getFunction;
            this._onRelease = releaseFunction;
        }

        /// <summary>
        /// Gets the next object from the pool.
        /// </summary>
        /// <returns>The next object from the pool.</returns>
        public GameObject GetObject() {
            GameObject poolObject = null;

            if (this._stack.Count > 0) {
                poolObject = this._stack.Pop();
            }
            else {
                poolObject = this.Instantiate();
            }

            if (poolObject != null && this._onGet != null) {
                this._onGet(poolObject);
            }

            return poolObject;
        }

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="poolObject">The object to place back in the pool.</param>
        public void ReleaseObject(GameObject poolObject) {
            if (poolObject != null && this._onRelease != null) {
                this._onRelease(poolObject);
            }

            this._stack.Push(poolObject);
        }

        /// <summary>
        /// The default method to be called when getting a pool object.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        private void DefaultOnGet(GameObject poolObject) {
            poolObject.SetActive(true);
        }

        /// <summary>
        /// The default method to be called when getting a pool object.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        private void DefaultOnRelease(GameObject poolObject) {
            poolObject.SetActive(false);
        }

        /// <summary>
        /// Instantiates a new game object.
        /// </summary>
        /// <returns>The instantiated game object.</returns>
        private GameObject Instantiate() {
            var poolObject = GameObject.Instantiate(this._prefab) as GameObject;
            return poolObject;
        }

        /// <summary>
        /// Preloads
        /// </summary>
        /// <param name="number"></param>
        private void Preload(int number) {
            for (var i = 0; i < number; i++) {
                this.ReleaseObject(this.Instantiate());
            }
        }
    }
}