using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PT
{
    /// <summary>
    /// Tracks the path of on object in the fixed update
    /// </summary>
    public class PTPathTracker : MonoBehaviour
    {
        /// <summary>
        /// How long should the paht be
        /// </summary>
        public int PathSaveFrames = 60;

        /// <summary>
        /// What prefab should be spawned to track the path
        /// </summary>
        public GameObject PathPointPrefab;

        /// <summary>
        /// What transform target are we tracking
        /// </summary>
        public Transform target;

        /// <summary>
        /// Shoudl this start playing on awake
        /// </summary>
        public bool PlayOnAwake;

        private Queue<PathPoint> _currentPath = new Queue<PathPoint>();
        private Transform _path;

        private bool isRunning = false;

        #region API
        public IEnumerable<Vector3> CurrentPath => _currentPath.Select(e => e.position);
        

        public void StartTrack()
        {
            isRunning = true;
        }

        public void StopTrackAndClear()
        {
            isRunning = false;

            while(_currentPath.Count > 0)
            {
                _currentPath.Dequeue().DestroyPoint();
            }
        }
        #endregion

        private void Awake()
        {
            if (PlayOnAwake) StartTrack();
        }

        private void Start()
        {
            GameObject g = new GameObject();
            _path = g.transform;
        }

        private void FixedUpdate()
        {
            if (!isRunning) return;

            Vector3 record = target.position;

            GameObject go = null;

            if (PathPointPrefab != null)
            {
                go = Instantiate(PathPointPrefab);
                go.transform.SetParent(_path.transform, false);
                go.transform.position = record;
            }

            _currentPath.Enqueue(new PathPoint()
            {
                position = target.position,
                viz = go
            });

            while (_currentPath.Count > PathSaveFrames)
            {
                _currentPath.Dequeue().DestroyPoint();
            }
        }

        #region Internal Objects
        public class PathPoint
        {
            public Vector3 position;
            public GameObject viz;

            public void DestroyPoint()
            {
                if (viz != null) Destroy(viz);
            }
        }
        #endregion
    }
}