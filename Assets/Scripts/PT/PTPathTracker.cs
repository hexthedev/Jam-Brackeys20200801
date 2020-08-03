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


        [Header("Visuals")]
        public int _gradientCrumbCount = 60;
        

        private List<PathPoint> _currentPath = new List<PathPoint>();
        private Transform _path;

        private bool isRunning = false;

        #region API
        /// <summary>
        /// Returns the current path backwards from the tracked transform
        /// </summary>
        public IEnumerable<Vector3> CurrentPath => _currentPath.Select(e => e.position);
        
        /// <summary>
        /// Start tracking the transform
        /// </summary>
        public void StartTrack()
        {
            isRunning = true;
        }

        /// <summary>
        /// Start tracking the transform
        /// </summary>
        public void StopTrack()
        {
            isRunning = false;
        }

        /// <summary>
        /// Stop tracking the transform and clear the path
        /// </summary>
        public void StopTrackAndClear()
        {
            isRunning = false;

            foreach(PathPoint p in _currentPath)
            {
                p.DestroyPoint();
            }

            _currentPath.Clear();
        }

        /// <summary>
        /// Get the last count steps counting backwards from the tracked transform
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Vector3[] GetLastCrumbs(int count)
        {
            int len = _currentPath.Count < count ? _currentPath.Count : count;
            if (len == 0) return new Vector3[0];

            Vector3[] ret = new Vector3[len];

            int pathIndex = _currentPath.Count-1;
            for(int i = 0; i<ret.Length; i++)
            {
                ret[i] = _currentPath[pathIndex].position;
                pathIndex--;
            }

            return ret;
        }

        /// <summary>
        /// Clears count crumbs from the trail backward from the tracked transform
        /// </summary>
        /// <param name="count"></param>
        public void ClearCrumbs(int count)
        {
            int len = _currentPath.Count < count ? _currentPath.Count : count;
            if (len == 0) return;

            int amountCleared = 0;
            for(int i = _currentPath.Count-1; ; i--)
            {
                _currentPath[i].DestroyPoint();
                amountCleared++;

                if (amountCleared == len) break;
            }

            _currentPath.RemoveRange(_currentPath.Count - amountCleared, len);
        }
        #endregion

        private void Awake()
        {
            if (PlayOnAwake) StartTrack();
        }

        private void Start()
        {
            GameObject g = new GameObject("Path Crumb Trail");
            g.transform.SetParent(transform.parent);
            _path = g.transform;
        }

        private void FixedUpdate()
        {
            // Validate
            if (!isRunning) return;

            // make a record
            Vector3 record = target.position;
            GameObject go = null;

            if (PathPointPrefab != null)
            {
                go = Instantiate(PathPointPrefab);
                go.transform.SetParent(_path.transform, false);
                go.transform.position = record;
            }

            // Add to path
            _currentPath.Add(new PathPoint()
            {
                position = target.position,
                viz = go,
                r = go.GetComponent<SpriteRenderer>()
            });

            // Remove any frames if the path is too long
            while (_currentPath.Count > PathSaveFrames)
            {
                _currentPath[0].DestroyPoint();
                _currentPath.RemoveAt(0);
            }

            // Apply gradient
            List<PathPoint> grad = new List<PathPoint>();

            float mod = 1f / _gradientCrumbCount;

            int totalGradiented = 0;

            for(int i = _currentPath.Count-1; i >= 0; i--)
            {
                if(totalGradiented <= _gradientCrumbCount)
                {
                    _currentPath[i].ChangeGradient(1 - totalGradiented * mod);
                } else
                {
                    _currentPath[i].ChangeGradient(0);
                }

                totalGradiented++;
            }
        }

        #region Internal Objects
        public class PathPoint
        {
            public Vector3 position;
            public GameObject viz;
            public SpriteRenderer r;

            public void DestroyPoint()
            {
                if (viz != null) Destroy(viz);
            }

            public void ChangeGradient(float grad)
            {
                if(viz != null)
                {
                    Color cur = r.color;
                    r.color = new Color(cur.r, cur.g, cur.b, grad);
                }
            }
        }
        #endregion
    }
}