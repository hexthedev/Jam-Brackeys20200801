using HexCS.Mathematics;
using HexUN.Animation;
using HexUN.Events;
using PathCreation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Rewinds the ball
    /// </summary>
    public class PathInterpolator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Invoked when an inteprolation ends with the number of steps taken")]
        public SingleReliableEvent OnEndDistance;

        [SerializeField]
        [Tooltip("The physics rigidbody being rewound")]
        private Rigidbody2D _rb;

        // id for interp
        private int _interpolationId;

        private VertexPath _path = null;
        private IInterpolationToken<float[]> _interp;
        private GameObject _pathContainer;

        private Vector3 _lastPos;
        private Vector3 _currentPos;

        private bool _isInterpolating = false;

        private float _distance;
        private float _pathLength;
        private float _steps;


        private void Start()
        {
            _interpolationId = InterpolationManager.Instance.GetUniqueId();
            _pathContainer = new GameObject("Path Container"); // make obj to save path objects to
        }

        /// <summary>
        /// Perform a rewind based on the path given
        /// </summary>
        /// <param name="path"></param>
        public void PerformInterpolation(IEnumerable<Vector3> path, float duration, EEasingFunction ease)
        {
            // Stop the simulation
            _rb.simulated = false;

            // Make a bezier path
            _path = new VertexPath(new BezierPath(path), _pathContainer.transform);

            // Initialize Interpolation Info
            _distance = 0;
            _steps = path.Count();
            _pathLength = _path.length;

            // Start an inteprolation over the distance of the given path
            _interp = InterpolationManager.Instance.StartInterpolation(
                _interpolationId,
                duration,
                new SInterpolation()
                {
                    Start = 0,
                    End = _path.length,
                    Ease = ease
                });

            // record position and change transform
            _lastPos = Vector3.negativeInfinity;
            _currentPos = _rb.transform.position;

            _interp.OnInterpolationSubscriber.Subscribe(
                v =>
                {
                    if (_path != null)
                    {
                        _lastPos = _currentPos;

                        _distance = v[0];
                        Vector3 move = _path.GetPointAtDistance(_distance, EndOfPathInstruction.Stop);
                        _rb.transform.position = move;
                        _currentPos = move;
                    }
                }
            );

            _interp.OnInterpolationEndSubscriber.Subscribe(() => StopInterpolating());
            _isInterpolating = true;
        }

        /// <summary>
        /// Stop interpolating
        /// </summary>
        public void StopInterpolating()
        {
            if (!_isInterpolating) return;
            _interp?.Cancel();

            // start simulating again
            _rb.simulated = true;

            if (_lastPos != Vector3.negativeInfinity)
            {
                _rb.velocity = (_currentPos - _lastPos) / Time.deltaTime;
            }

            OnEndDistance.Invoke(_distance/_pathLength * _steps);
            _isInterpolating = false;
        }
    }
}