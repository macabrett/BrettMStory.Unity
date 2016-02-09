namespace BrettMStory.Unity {

    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A pool of game objects.
    /// </summary>
    public class GameObjectPool {

        /// <summary>
        /// The on get method.
        /// </summary>
        private readonly Action<GameObject> _onGet;

        /// <summary>
        /// The on release method.
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
        /// The max size of the stack.
        /// </summary>
        private int _maxSize = 1;

        /// <summary>
        /// The total number of objects, in the stack or in the wild.
        /// </summary>
        private int _totalObjects = 0;

        /// <summary>
        /// Instantiates a new instance of [GameObjectPool].
        /// </summary>
        /// <param name="prefab">The prefab to instantiate game objects.</param>
        /// <param name="startingSize">The starting size.</param>
        /// <param name="maxSize">The max size.</param>
        public GameObjectPool(GameObject prefab, int startingSize, int maxSize) {
            this._prefab = prefab;
            this._maxSize = maxSize;
            this.Preload(startingSize);
            this._onGet = this.DefaultOnGet;
            this._onRelease = this.DefaultOnRelease;
        }

        /// <summary>
        /// Instantiates a new instance of [GameObjectPool].
        /// </summary>
        /// <param name="prefab">The prefab to instantiate game objects.</param>
        /// <param name="startingSize">The starting size.</param>
        /// <param name="maxSize">The max size.</param>
        /// <param name="getFunction">A function to be called on a game object when it is gotten from the pool.</param>
        /// <param name="releaseFunction">A function to be called on a game object when it is released back to the pool.</param>
        public GameObjectPool(GameObject prefab, int startingSize, int maxSize, Action<GameObject> getFunction, Action<GameObject> releaseFunction) {
            this._prefab = prefab;
            this._maxSize = maxSize;
            this.Preload(startingSize);
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
            else if (this._totalObjects < this._maxSize) {
                poolObject = this.Instantiate();
            }

            if (poolObject != null && this._onGet != null)
                this._onGet(poolObject);

            return poolObject;
        }

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="poolObject">The object to place back in the pool.</param>
        public void ReleaseObject(GameObject poolObject) {
            if (poolObject != null && this._onRelease != null)
                this._onRelease(poolObject);

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
            this._totalObjects++;
            return poolObject;
        }

        /// <summary>
        /// Preloads
        /// </summary>
        /// <param name="number"></param>
        private void Preload(int number) {
            for (int i = 0; i < number; i++) {
                this.ReleaseObject(this.Instantiate());
            }
        }
    }
}